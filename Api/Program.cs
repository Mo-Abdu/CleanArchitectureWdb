using Api.Middleware;
using Application;
using Application.Behaviors;
using Application.Commands.AuthorCommands;
using Application.Commands.BookCommands;
using Application.Interfaces.RepositoryInterfaces;
using Application.Validators.AuthorValidators;
using Application.Validators.BookValidators;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<IValidator<UpdateBookCommand>, UpdateBookCommandValidator>();
builder.Services.AddTransient<IValidator<AddBookCommand>, AddBookCommandValidator>();
builder.Services.AddTransient<IValidator<AddAuthorCommand>, AddAuthorCommandValidator>();
builder.Services.AddTransient<IValidator<UpdateAuthorCommand>, UpdateAuthorCommandValidator>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
