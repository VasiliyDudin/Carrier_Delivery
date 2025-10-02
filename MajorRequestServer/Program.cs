using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MajorRequestServer.Database;
using MajorRequestServer.Models;
using MajorRequestServer.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<RequestContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));
builder.Services.AddControllers();
builder.Services.AddScoped<BaseRepository<Status>>();
builder.Services.AddScoped<BaseRepository<Courier>>();
builder.Services.AddScoped<BaseRepository<Request>>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MajorRequestServer", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MajorRequestServer V1");
    });
}

app.Run();