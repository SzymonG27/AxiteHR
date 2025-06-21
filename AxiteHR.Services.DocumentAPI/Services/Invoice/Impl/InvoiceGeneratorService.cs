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

		public InvoiceGeneratorService()
		{
			_chromiumExecutablePath = Path.Combine(AppContext.BaseDirectory, "Chromium", "chrome-win", "chrome.exe");

			if (!File.Exists(_chromiumExecutablePath))
			{
				Log.Error("Chromium not found in path: {0}", _chromiumExecutablePath);
				throw new FileNotFoundException("Chromium not found in path: {0}", _chromiumExecutablePath);
			}
		}

		public async Task<string> GenerateInvoiceAsync(InvoiceGeneratorDto model)
		{
			var engine = new RazorLightEngineBuilder()
				.UseEmbeddedResourcesProject(typeof(InvoiceGeneratorService))
				.UseMemoryCachingProvider()
				.Build();

			var html = await engine.CompileRenderAsync("", model);

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

			var randomFileName = Path.GetRandomFileName().Replace(".tmp", ".pdf");
			var tempPath = Path.Combine(Path.GetTempPath(), randomFileName);
			var pdfPath = Path.ChangeExtension(tempPath, ".pdf");

			await page.PdfAsync(pdfPath, new PdfOptions
			{
				Format = PaperFormat.A4,
				PrintBackground = true
			});

			return pdfPath!;
		}
	}
}
