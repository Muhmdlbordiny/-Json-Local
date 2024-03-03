using json_based_localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Runtime;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using json_based_localization.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory,jsonstringlocalizationFactor>();// i need instance on inside App
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(jsonstringlocalizationFactor));
    });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCulture = new[]
    {
        new CultureInfo(name:"en-US"),
        new CultureInfo(name:"ar-EG"),
        new CultureInfo(name:"de-DE"),

    };
    //options.DefaultRequestCulture = new RequestCulture(culture: supportedCulture[0], uiCulture: supportedCulture[0]);
    options.SupportedCultures = supportedCulture;
    options.SupportedUICultures = supportedCulture;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
var supportedCulture = new[] { "en-Us", "ar-EG", "de-DE" };
var localizationOptions = new RequestLocalizationOptions()
   // .SetDefaultCulture(supportedCulture[0])
    .AddSupportedCultures(supportedCulture)
    .AddSupportedUICultures(supportedCulture);
app.UseRequestLocalization(localizationOptions);// create MiddleWare


app.UseAuthorization();
app.UseRequestCulture(); //=>>

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
