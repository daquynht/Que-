using Microsoft.EntityFrameworkCore;
using Que.Models;
using Que.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuizDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:QuizDbContextConnection"]);
});

builder.Services.AddScoped<IQuizRepository, QuizRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app);
}

app.UseStaticFiles();

app.MapDefaultControllerRoute();

    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();