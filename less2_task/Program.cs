using FluentValidation;
using less2_task.ErrorHandling;
using less2_task.Middlewares;
using less2_task.Models;
using less2_task.Models.Validation;
using less2_task.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString)
);

//SQL Server
//builder.Services.AddDbContext<ApiDbContext>(options =>
//    options.UseSqlServer(connectionString)
//);

builder.Services.AddAutoMapper(c =>
{

}, typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers(o =>
{
    o.Filters.Add<FluentValidationActionFilter>();

});

builder.Services.AddSingleton<IDataProvider<Message>>(c =>
{
    var dataFile = builder
    .Configuration
    .GetRequiredSection("MessageService")
    .GetValue<string>("DataFilePath");

    return new MessageProvider(dataFile);
});

builder.Services.AddScoped<MessageService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<RequestLoggerService>();
builder.Services.AddScoped<UploadImageService>();
builder.Services.AddScoped<SiteConfigService>();
builder.Services.AddSingleton<IDataProvider<RouteStat>>(c => new JsonFileDataProvider<RouteStat>("routes.json"));

builder.Services.AddSingleton<IDataProvider<User>>(c =>
{
    var dataFile = builder
    .Configuration
    .GetRequiredSection("UserService")
    .GetValue<string>("DataFilePath");
    return new JsonFileDataProvider<User>(dataFile);
});
builder.Services.AddScoped<UserService>();

builder.Services.AddSingleton<IDataProvider<Order>>(c =>
{
    var dataFile = builder
    .Configuration
    .GetRequiredSection("OrderService")
    .GetValue<string>("DataFilePath");
    return new JsonFileDataProvider<Order>(dataFile);
});
builder.Services.AddScoped<OrderService>();

builder.Services.AddScoped<CategoryService>();

builder.Services.AddOpenApi();

var app = builder.Build();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<CheckUserMiddleware>();
app.UseMiddleware<CounterMiddleware>();

app.MapControllers();

app.UseStaticFiles();

app.Run();
