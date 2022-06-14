using HomeEasy.Domain.Commands.v1.User.AddUser;
using HomeEasy.Domain.MapperProfiles.v1.User;
using Infrastructure.Data;
using Infrastructure.Data.Interfaces;
using Infrastructure.Data.ServiceHandler;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HomeEasyContext>(options =>
    options.UseSqlServer("Server=DESKTOP-4CN7MCF\\SQLEXPRESS;Database=HomeEasy;Trusted_Connection=True;MultipleActiveResultSets=true" ?? throw new InvalidOperationException("Connection string 'HomeEasyContext' not found."), b=>b.MigrationsAssembly("HomeEasy")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(AddUserCommandHandler).Assembly);
builder.Services.AddAutoMapper(typeof(AddUserEntityProfile).Assembly);
builder.Services.AddScoped<IUserService, UserServiceHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
