using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.GlobalClass.Redis.Keys;
using AxiteHR.Integration.Storage.Abstractions;
using AxiteHR.Integration.Storage.Constants;
using AxiteHR.Integration.Storage.Helpers;
using AxiteHR.Services.DocumentAPI.Extensions;
using AxiteHR.Services.DocumentAPI.Helpers;
using AxiteHR.Services.DocumentAPI.Models.Invoice.Dto;
using Microsoft.Extensions.Localization;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RazorLight;
using RazorLight.Razor;
using Serilog;
using System.Globalization;

namespace AxiteHR.Services.DocumentAPI.Services.Invoice.Impl
{
	public class InvoiceGeneratorService : IInvoiceGeneratorService
	{
		private readonly string _chromiumExecutablePath;
		private readonly IStorageFactory _storageFactory;
		private readonly IRedisCacheService _redisCacheService;
		private readonly IStringLocalizerFactory _localizerFactory;

		public InvoiceGeneratorService(IStorageFactory storageFactory,
			IRedisCacheService redisCacheService,
			IStringLocalizerFactory localizerFactory)
		{
			_chromiumExecutablePath = Path.Combine(AppContext.BaseDirectory, "Chromium", "chrome-linux", "chrome");

			if (!File.Exists(_chromiumExecutablePath))
			{
				Log.Error("Chromium not found in path: {0}", _chromiumExecutablePath);
				throw new FileNotFoundException("Chromium not found in path: {0}", _chromiumExecutablePath);
			}

			_storageFactory = storageFactory;
			_redisCacheService = redisCacheService;
			_localizerFactory = localizerFactory;
		}

		public async Task<string> GenerateInvoiceAsync(InvoiceGeneratorDto model)
		{
			var embeddedProject = new EmbeddedRazorProject(typeof(InvoiceGeneratorService).Assembly);

			var engine = new RazorLightEngineBuilder()
				.UseProject(embeddedProject)
				.SetOperatingAssembly(typeof(InvoiceGeneratorService).Assembly)
				.UseMemoryCachingProvider()
				.EnableDebugMode()
				.Build();

			var minioService = _storageFactory.Get(ObjectStorageType.Minio);

			var culture = new CultureInfo(model.Language.ToString());
			CultureInfo.CurrentCulture = culture;
			CultureInfo.CurrentUICulture = culture;

			var localizer = _localizerFactory.Create(typeof(DocumentResources));
			model.Translations = localizer.GetTranslations(TranslationKeysHelper.InvoiceTranslationKeys);

			model.Logo.Base64 = await GetLogoBase64Async(minioService);

			var html = await engine.CompileRenderAsync("AxiteHR.Services.DocumentAPI.Templates.InvoiceTemplate", model);

			var launchOptions = new LaunchOptions
			{
				Headless = true,
				ExecutablePath = _chromiumExecutablePath
			};

			await using var browser = await Puppeteer.LaunchAsync(launchOptions);
			await using var page = await browser.NewPageAsync();

			await page.SetContentAsync(html, new NavigationOptions
			{
				WaitUntil = [WaitUntilNavigation.Load]
			});

			await using var pdfStream = await page.PdfStreamAsync(new PdfOptions
			{
				Format = PaperFormat.A4,
				PrintBackground = true
			});

			return await minioService.UploadAsync(
				pdfStream,
				model.BlobFileName,
				ContentTypeHelper.GetContentTypeFromExtension(model.BlobFileName),
				MinioBuckets.Invoices
			);
		}

		private async Task<string> GetLogoBase64Async(IObjectStorageService minioService)
		{
			var logoBase64FromCache = await _redisCacheService.GetObjectAsync<string>(AssetsRedisKeys.GetBase64LogoWebp);
			if (!string.IsNullOrEmpty(logoBase64FromCache))
			{
				return logoBase64FromCache;
			}

			var logoStream = await minioService.DownloadAsync(MinioAssetsContent.LogoWebp, MinioBuckets.Assets);
			await using var msLogo = new MemoryStream();
			await logoStream.CopyToAsync(msLogo);
			var logoBase64 = Convert.ToBase64String(msLogo.ToArray());
			await _redisCacheService.SetObjectAsync(AssetsRedisKeys.GetBase64LogoWebp, logoBase64, TimeSpan.FromMinutes(5));

			return logoBase64;
		}
	}
}
