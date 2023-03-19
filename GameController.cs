using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using MultiSnake.Services.Interfaces;

namespace MultiSnake;

[ApiController]
public class GameController : Controller
{
    private readonly IRedisService _redis;

    public GameController(IRedisService redis)
    {
        _redis = redis;
    }

    [HttpPost]
    public Task Add(string test)
    {
        return _redis.CreateAsync("1234", "Eoo", test);
    }

    [HttpGet]
    public Task<object> Get()
    {
        return _redis.GetAsync("1234", "Eoo");
    }
}