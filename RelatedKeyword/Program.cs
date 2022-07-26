using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using RelatedKeyword.Models;
using RelatedKeyword.Services;
using RelatedKeywordLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<NaverSearchAPISettings>(builder.Configuration.GetSection(NaverSearchAPISettings.Key));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<SearchService>();
//데이터베이스 공급자 : https://docs.microsoft.com/ko-kr/ef/core/providers/?tabs=dotnet-core-cli
//UseMySQL by nuget "MySql.EntityFrameworkCore"
builder.Services.AddDbContext<UserContext>(options =>
       options.UseMySQL(builder.Configuration.GetConnectionString("UserContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseAuthentication();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
