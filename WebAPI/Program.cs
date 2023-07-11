using Microsoft.Extensions.Options;
using System.Reflection;
using WebAPI.Extensions;
using WebAPI.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddAutoMapper(typeof(MapperProfile));

//Register Application Services and EF Context.
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHanlder();

app.UseStatusCodePages();
app.UseAuthorization();

app.MapControllers();
app.MigrateDatabase();

app.Run();
