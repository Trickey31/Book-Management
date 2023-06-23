using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Models;
using QLThuVien.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("QlthuVienLtwebContext");
builder.Services.AddDbContext<QlthuVienLtwebContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = "Google"; // scheme sẽ được sử dụng khi gọi Challenge() cho Google authentication
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // scheme sẽ được sử dụng khi đăng nhập thành công
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = builder.Configuration["Authentication:Google:CallbackPath"];
    options.SaveTokens = true;
    options.ClaimActions.MapJsonKey("Picture", "picture", "url"); // để có thể gọi Claim.Picture
})
.AddFacebook("Facebook", options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:ClientId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
    options.CallbackPath = builder.Configuration["Authentication:Facebook:CallbackPath"];
    options.SaveTokens = true;
    options.ClaimActions.MapJsonKey("Picture", "picture.data.url");
});

builder.Services.AddScoped<ILoaiSpRepository,LoaiSpRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();


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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseStaticFiles(); // Cho phép truy cập các file tĩnh trong thư mục wwwroot

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
