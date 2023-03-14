using Microsoft.AspNetCore.SignalR;

namespace MultiSnake.Hubs;

public class RoomHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("GET", "jebac cie");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("ktos się rozłączył");
        return Task.CompletedTask;
    }

    public async Task SendMessage(string token)
    {
        await Clients.Caller.SendAsync("SEND", "tylko do ciebie" + token );
        await Clients.All.SendAsync("SEND" , token + " do wszystkich" + token);
    }
}