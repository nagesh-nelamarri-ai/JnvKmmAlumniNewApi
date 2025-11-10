using JnvKmmAlumniApi.Data;
using JnvKmmAlumniApi.Interfaces;
using JnvKmmAlumniApi.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // your Angular app
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>(); // Dapper connection context

// Add repository
builder.Services.AddScoped<MemberRepository>();
//builder.Services.AddScoped<EventsRepository>();
builder.Services.AddTransient<IEventsRepository, EventsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAngularApp");
//app.UseCors("CorsPolicy");
app.UseAuthorization();

// Enable static files middleware
app.UseStaticFiles(); // Serves files from wwwroot by default

// If your ProfileImages folder is outside wwwroot, map it explicitly
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "ProfileImages")),
    RequestPath = "/ProfileImages"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "EventFiles")),
    RequestPath = "/EventFiles"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "EventFiles")),
    RequestPath = "/EventFiles"
});

app.MapControllers();

app.Run();
