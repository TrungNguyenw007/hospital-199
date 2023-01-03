using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;


using WebClient.Data;
using WebClient.Identity;
using WebClient.Service.PartReviewDetails;
using WebClient.Service.PartReviews;
using WebClient.Service.Parts;
using WebClient.Service.Roles;
using WebClient.Service.Upload;
using WebClient.Service.Users;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//service
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<RoleManagerService, RoleManagerService>();
builder.Services.AddTransient<UserManagerService,UserManagerService>();
builder.Services.AddTransient<PartService,PartService>();
builder.Services.AddTransient<PartReviewService, PartReviewService>();
builder.Services.AddTransient<PartReviewDetailService, PartReviewDetailService>();
builder.Services.AddTransient<UploadService, UploadService>();



builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider , ApiAuthenticationStateProvider >();


builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

// builder.Services.AddServerSideBlazor().AddHubOptions(o =>
// {
//     o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
// });





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();