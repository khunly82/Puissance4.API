using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Puissance4.API.DTO;
using Puissance4.API.Entities;
using Puissance4.API.Services;
using System.Security.Claims;

namespace Puissance4.API.Hubs
{
    public class GameHub(GameService _gameService) : Hub
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
                Clients.All.BroadCastGames();
                Clients.Caller.SendAsync("OnGame", _gameService.FindById(idGame));
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("OnError", ex.Message);
            }
        }

        public void Join(string gameId)
        {
            try
            {
                _gameService.Join(ConnectedUser, gameId);
                Clients.All.BroadCastGames();
                Clients.Group(gameId).SendAsync("OnGame", _gameService.FindById(gameId));
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("OnError", ex.Message);
            }
        }



        private string ConnectedUser
        {
            get => Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.BroadCastGames();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string? gameId = _gameService.FindByPlayer(ConnectedUser);
            if (gameId is not null)
            {
                Clients.Group(gameId).SendAsync("Leave");
                _gameService.Delete(gameId);
                Clients.All.BroadCastGames();
            }
            return base.OnDisconnectedAsync(exception);
        }
    }

    public static class ClientProxyExtensions 
    {
        public static void BroadCastGames(this IClientProxy clients)
        {
            clients.SendAsync("OnGames", GameService.Games.Select((kvp) => new GameDTO
             {
                 GameId = kvp.Key,
                 YellowPlayer = kvp.Value.YellowPlayer,
                 RedPlayer = kvp.Value.RedPlayer,
             }));
        }
    }
}
