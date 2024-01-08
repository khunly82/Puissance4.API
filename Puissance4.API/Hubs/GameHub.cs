using Microsoft.AspNetCore.SignalR;

namespace Puissance4.API.Hubs
{
    public class GameHub: Hub
    {
        public void SayHello()
        {
            Clients.Others.SendAsync("OnMessage", "Hello");
        }
    }
}
