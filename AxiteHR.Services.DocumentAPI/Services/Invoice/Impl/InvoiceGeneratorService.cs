using AxiteHR.Integration.Storage.Abstractions;
using AxiteHR.Integration.Storage.Constants;
using AxiteHR.Integration.Storage.Helpers;
using AxiteHR.Services.DocumentAPI.Models.Invoice.Dto;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using RazorLight;
using Serilog;

namespace AxiteHR.Services.DocumentAPI.Services.Invoice.Impl
{
	public class InvoiceGeneratorService : IInvoiceGeneratorService
	{
		private readonly string _chromiumExecutablePath;
		private readonly IStorageFactory _storageFactory;

		public InvoiceGeneratorService(IStorageFactory storageFactory)
		{
			_chromiumExecutablePath = Path.Combine(AppContext.BaseDirectory, "Chromium", "chrome-win", "chrome.exe");

			if (!File.Exists(_chromiumExecutablePath))
			{
				Log.Error("Chromium not found in path: {0}", _chromiumExecutablePath);
				throw new FileNotFoundException("Chromium not found in path: {0}", _chromiumExecutablePath);
			}

			_storageFactory = storageFactory;
		}

		public async Task<string> GenerateInvoiceAsync(InvoiceGeneratorDto model)
		{
			var engine = new RazorLightEngineBuilder()
				.UseEmbeddedResourcesProject(typeof(InvoiceGeneratorService))
				.UseMemoryCachingProvider()
				.Build();

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

			var minioService = _storageFactory.Get(ObjectStorageType.Minio);

			return await minioService.UploadAsync(
				pdfStream,
				model.BlobFileName,
				ContentTypeHelper.GetContentTypeFromExtension(model.BlobFileName),
				MinioBuckets.Invoices
			);
		}
	}
}
