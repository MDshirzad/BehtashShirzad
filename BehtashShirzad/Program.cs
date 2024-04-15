
 
using BehtashShirzad.Model.Context;
using BehtashShirzad.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbCommiter>();

var secrete = Infrastructure.GenerateSecretKey(256);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(Infrastructure.SecreteKeyJWT),
       ValidateIssuer = false,
       ValidateAudience = false,
       ClockSkew = TimeSpan.Zero // Optional: prevent clock skew issues
   };
});

// Add authorization services
builder.Services.AddAuthorization();

var app = builder.Build();
 
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DbCommiter>();
    dbContext.Database.Migrate();
}

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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
