using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Services;
using TodoApi.Dtos;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//make every 4xx code into one format
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var message = string.Join(" ", context.ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors)
                .Select(e => e.ErrorMessage));

            return new BadRequestObjectResult(new ErrorResponse { Message = message });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//handles error code 500
app.UseExceptionHandler(handler =>
{
    handler.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(
            new ErrorResponse { Message = "An unexpected error occurred." });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();  //currently no need

app.MapControllers();

app.Run();
