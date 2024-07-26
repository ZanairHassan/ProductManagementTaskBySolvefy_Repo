using BLL.Interfaces;
using BLL.Services;
using DAL.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtoption =>
{
    var key = builder.Configuration.GetValue<string>("Jwt:SecretKey");
    var KeyIntoBytes=Encoding.UTF8.GetBytes(key);
    jwtoption.SaveToken=true;
    jwtoption.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(KeyIntoBytes),
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer=true

    };
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SolvefyConnectionString"), x => x.MigrationsAssembly("ProductManagement")));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<TokenUtility>();
builder.Services.AddTransient<AsymmetricCryptographyUtility>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Register}/{id?}");

app.Run();
