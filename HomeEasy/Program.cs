﻿using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HomeEasyContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("HomeEasyContext") ??
	throw new InvalidOperationException("Connection string 'HomeEasyContext' not found.")), ServiceLifetime.Singleton);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IJobService, JobService>();
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/Users/Login"; 
	options.LogoutPath = "/Users/Logout"; 
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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
