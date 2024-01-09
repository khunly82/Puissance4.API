using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Puissance4.API.DTO;
using Puissance4.API.Services;
using System.Security.Claims;

namespace Puissance4.API.Hubs
{
    public class GameHub(GameService _gameService): Hub
    {
        public void SayHello(string message)
        {
            Clients.Others.SendAsync("OnMessage", message);
        }

        [Authorize]
        public void CreateGame(CreateGameDTO dto)
        {
            try
            {
                string idGame = _gameService.Add(ConnectedUser, dto.SelectedColor);
                Groups.AddToGroupAsync(Context.ConnectionId, idGame);
                Clients.All.SendAsync("OnGames", GameService.Games.Select((kvp) => new GameDTO
                {
                    GameId = kvp.Key,
                    YellowPlayer = kvp.Value.YellowPlayer,
                    RedPlayer = kvp.Value.RedPlayer,
                }));
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("OnError", ex.Message);
                throw;
            }
        }

        private string ConnectedUser
        {
            get => Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
