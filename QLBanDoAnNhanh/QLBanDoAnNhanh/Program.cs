using QLBanDoAnNhanh.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QlbanDoAnNhanhContext>(options =>
{
    options.UseSqlServer(builder.Configuration["QLBanDoAnNhanh"]);
});//

// Thêm dịch vụ lưu trữ session
builder.Services.AddDistributedMemoryCache(); // Dùng để lưu session vào bộ nhớ tạm thời
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
    options.Cookie.HttpOnly = true; // Cookie chỉ truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cho phép session luôn hoạt động
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
// Kích hoạt session
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SanPhams}/{action=TrangChu}/{id?}");

app.Run();
