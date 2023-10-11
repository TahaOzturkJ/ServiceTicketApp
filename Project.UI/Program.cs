using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Project.BLL.Validations;
using Project.DAL.Context;
using Project.ENTITY.Models;
using NToastNotify;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Project.BLL.EmailSender.IEmail;
using Project.BLL.EmailSender.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();


builder.Services.AddDbContext<MyContext>();
builder.Services.AddIdentity<User, Project.ENTITY.Models.IdentityRole>()
    .AddEntityFrameworkStores<MyContext>();

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 5000,
    })
    .AddRazorRuntimeCompilation();

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    config.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddMvc();

builder.Services.AddScoped<IValidator<ServiceTicket>, ServiceTicketValidator>();
builder.Services.AddScoped<IValidator<ServiceTicketComment>, ServiceTicketCommentValidator>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "loggeduser";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Auth/Login/Index";
    options.AccessDeniedPath = "/Auth/Error/Index/";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify();

app.UseStatusCodePagesWithReExecute("/Auth/Error/Error404/", "?code={0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{area=Auth}/{controller=Login}/{action=Index}/{id?}"
    );
});

app.MapRazorPages();

app.Run();
