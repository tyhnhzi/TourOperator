using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using TourOperatorWeb.Data;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000"; 
builder.WebHost.UseUrls($"http://*:{port}");
// Add services to the container.
builder.Services.AddRazorPages();

// DbContext ket noi den SQLite (file cuc bo)
var chuoiKetNoi = builder.Configuration.GetConnectionString("ChuoiKetNoiCoSoDuLieu");
builder.Services.AddDbContext<UngDungDbContext>(options =>
    options.UseSqlite(chuoiKetNoi));

// Xac thuc cookie don gian
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/DangNhap";
        options.AccessDeniedPath = "/DangNhap";
    });

var app = builder.Build();

// Tao CSDL neu chua ton tai va ap dung seed du lieu
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UngDungDbContext>();
    db.Database.EnsureCreated();
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
