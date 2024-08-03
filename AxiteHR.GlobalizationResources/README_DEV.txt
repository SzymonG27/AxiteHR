-----AxiteHR.GlobalizationResources-----

This project was created to handle globalization and localization in microservices.

.resx files are added to the Resources folder to handle localization

In the main structure of the project there is a powershell file.
It fires every time the project is rebuilt.
It generates key files that can then be used for strongly typed strings.

To implement in new microservice:
	1. Add project reference in microservice to this class library
	2. Create Main .resx file with the main language of app
	3. Create .resx files with another languages like CompanyResources.fr.resx
	4. Add Names and value in every of this files
	5. Rebuild class library
	6. Configure extension for program.cs *1
	7. Use extension in program.cs WebAPI
	8. Add singletons in program.cs *2
	9. That's it. Everything should work.



*1 - Standard configuration:
-----START *1-----
public static WebApplicationBuilder AddGlobalization(this WebApplicationBuilder builder)
{
	builder.Services.AddLocalization();
	builder.Services.Configure<RequestLocalizationOptions>(options =>
	{
		var supportedCultures = GetSupportedCultures();

		options.DefaultRequestCulture = new RequestCulture("en");
		options.SupportedCultures = supportedCultures;
		options.SupportedUICultures = supportedCultures;

		options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
		{
			var lang = context.Request.Headers["Accept-Language"].ToString();
			if (string.IsNullOrEmpty(lang))
			{
				lang = "en";
			}
			return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(lang, lang));
		}));
	});

	return builder;

	static List<CultureInfo> GetSupportedCultures()
	{
		return new List<CultureInfo>
		{
			new CultureInfo("en"),
			new CultureInfo("pl")
		};
	}
}
-----END *1-----


*2 - Standard configuration of singletons: 
-----START *2-----
//Factory
builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();

//Shared resource (for every project feel free to use)
builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();

//New resource for your microservice
builder.Services.AddSingleton<IStringLocalizer<YourNewMicroserviceResources>, StringLocalizer<YourNewMicroserviceResources>>();
-----END *2-----