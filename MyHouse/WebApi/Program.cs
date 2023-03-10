using System;
using System.IO;
using System.Text;
using Domain.Identity.Roles;
using Domain.Identity.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SqlServ4r.EntityFramework;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
builder.Services.AddApplicationAsync<MyModule>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });
    c.OperationFilter<AddHeaderParameter>();
});

builder.Services.AddDbContext<DreamContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyApp"),b=>b.MigrationsAssembly("WebApi"));
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DreamContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.Configure<IdentityOptions> (options => {
    // set up Password
    options.Password.RequireDigit = false; // Kh??ng b???t ph???i c?? s???
    options.Password.RequireLowercase = false; // Kh??ng b???t ph???i c?? ch??? th?????ng
    options.Password.RequireNonAlphanumeric = false; // Kh??ng b???t k?? t??? ?????c bi???t
    options.Password.RequireUppercase = false; // Kh??ng b???t bu???c ch??? in
    options.Password.RequiredLength = 3; // S??? k?? t??? t???i thi???u c???a password
    options.Password.RequiredUniqueChars = 0; // S??? k?? t??? ri??ng bi???t

    // C???u h??nh Lockout - kh??a user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Kh??a 5 ph??t
    options.Lockout.MaxFailedAccessAttempts = 5; // Th???t b???i 5 l??? th?? kh??a
    options.Lockout.AllowedForNewUsers = true;

    // C???u h??nh v??? User.
    options.User.AllowedUserNameCharacters = // c??c k?? t??? ?????t t??n user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;  // Email l?? duy nh???t
    
    // configure login
    options.SignIn.RequireConfirmedEmail = false;            // C???u h??nh x??c th???c ?????a ch??? email (email ph???i t???n t???i)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // X??c th???c s??? ??i???n tho???i

});

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("EmployeeOnly", policy =>
//         {
//             policy.RequireRole("manager");
//             policy.RequireClaim("manager-level-1");
//         }
//     );
// });





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/StaticFiles"
});



app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();