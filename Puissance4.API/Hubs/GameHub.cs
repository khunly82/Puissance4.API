using Microsoft.AspNetCore.SignalR;

namespace Puissance4.API.Hubs
{
    public class GameHub: Hub
    {
        public void SayHello(string message)
        {
            Clients.Others.SendAsync("OnMessage", message);
        }
    }
}
