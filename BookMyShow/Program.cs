using BookMyShow.Controllers;
using BookMyShow.Data;
using BookMyShow.Interfaces;
using BookMyShow.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITheatreOwnersController, TheatreOwnersController>();
builder.Services.AddScoped<IMoviesController, MoviesController>();
builder.Services.AddScoped<IReviewsController, ReviewsController>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<BookMyShowDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MigrateDb();

app.Run();
