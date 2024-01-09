using Puissance4.API.Entities;
using Puissance4.API.Enums;

namespace Puissance4.API.Services
{
    public class GameService
    {
        public static Dictionary<string, Game> Games { get; } = new();

        public string Add(string userId, Color color)
        {
            if(Games.Values.Any(g => g.RedPlayer == userId || g.YellowPlayer == userId))
            {
                throw new Exception("Cannot be part of 2 games");
            }

            Game game = new Game();
            if(color == Color.Red)
            {
                game.RedPlayer = userId;
            }
            else
            {
                game.YellowPlayer = userId;
            }
            string idGame = Guid.NewGuid().ToString();
            Games.Add(idGame, game);
            return idGame;
        }

        public void Join(string userId, string gameId)
        {
            Game? g = Games.GetValueOrDefault(gameId);
            if(g is null)
            {
                throw new Exception($"{gameId} is not a game");
            }
            if(g.YellowPlayer != null && g.RedPlayer != null)
            {
                throw new Exception();
            }
            if(g.YellowPlayer == userId || g.RedPlayer == userId)
            {
                throw new Exception();
            }
            if(g.YellowPlayer == null)
            {
                g.YellowPlayer = userId;
            }
            else
            {
                g.RedPlayer = userId;
            }
        }

        public string? FindByPlayer(string userId)
        {
            return Games.FirstOrDefault(g => g.Value.RedPlayer == userId || g.Value.YellowPlayer == userId).Key;
        }

        public void Delete(string gameId)
        {
            Games.Remove(gameId);
        }
    }
}
