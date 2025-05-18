# AxiteHR.GlobalizationResources

This project provides a centralized solution for handling globalization and localization across microservices. It uses `.resx` files and generates strongly-typed keys for seamless string localization.

---

## 📁 Project Structure

- `Resources/` — Contains `.resx` files for localization.
- `GenerateKeys.ps1` — PowerShell script triggered on project rebuild to generate strongly-typed resource keys.

---

## 🚀 How to Integrate in a New Microservice

### 1. Add Project Reference

Add a reference to `AxiteHR.GlobalizationResources` in your microservice project.

### 2. Create Resource Files

Create `.resx` files inside the `Resources` folder:

- Main language file (e.g. `CompanyResources.resx`)
- Additional languages (e.g. `CompanyResources.pl.resx`, `CompanyResources.fr.resx`)

Populate each file with the same keys and localized values.

### 3. Rebuild the Class Library

Rebuilding will run the PowerShell script and generate strongly-typed key files automatically.

### 4. Configure Globalization Extension in `Program.cs`

```csharp
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
```

### 5. Register Singletons in `Program.cs`

```csharp
// Factory
builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();

// Shared resource (for use across all services)
builder.Services.AddSingleton<IStringLocalizer<SharedResources>, StringLocalizer<SharedResources>>();

// Microservice-specific resource
builder.Services.AddSingleton<IStringLocalizer<YourNewMicroserviceResources>, StringLocalizer<YourNewMicroserviceResources>>();
```
