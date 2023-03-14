using Microsoft.AspNetCore.SignalR;
using MultiSnake.Hubs;
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

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");
app.MapHub<RoomHub>("/socket"); 

app.Run();

//172.20.10.3