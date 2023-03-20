using MultiSnake.Hubs;
using MultiSnake.Services.implementations;
using MultiSnake.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(c =>
{
    c.AddDefaultPolicy(policyBuilder => policyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        //.WithOrigins("172.20.10.2:5015", "http://localhost:5500")
        .WithOrigins("http://127.0.0.1:5500'", "http://localhost:5500", "http://127.0.0.1:5003'")
        .AllowCredentials());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/abc", () => "Hello World! socket is working");
app.MapHub<GameHub>("/game");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapSwagger();
});

app.Run();

//172.20.10.3