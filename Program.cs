using System.Reflection;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.OpenApi.Models;
using MultiSnake.Hubs;
using MultiSnake.Services.implementations;
using MultiSnake.Services.Interfaces;
using MultiSnake.Structs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(c =>
{
    c.AddDefaultPolicy(policyBuilder => policyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("172.20.10.3:5001", "http://localhost:5500")
        .AllowCredentials());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();

app.UseRouting();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/abc", () => "Hello World!");
app.MapHub<RoomHub>("/socket");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapSwagger();
});

app.Run();

//172.20.10.3