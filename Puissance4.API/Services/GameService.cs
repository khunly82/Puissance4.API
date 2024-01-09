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
            if (color == Color.Red)
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
                throw new Exception("This game does not exist");
            }
            if(g.YellowPlayer != null && g.RedPlayer != null)
            {
                throw new Exception("This game is already full");
            }
            if(g.YellowPlayer == userId || g.RedPlayer == userId)
            {
                throw new Exception("You're already in this game");
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

        public Game? FindById(string gameId)
        {
            return Games.GetValueOrDefault(gameId);
        }

        public string? FindByPlayer(string userId)
        {
            return Games.FirstOrDefault(g => g.Value.RedPlayer == userId || g.Value.YellowPlayer == userId).Key;
        }

        public Game Play(string userId, string gameId, int col) 
        {
            Game? g = Games.GetValueOrDefault(gameId);
            if(g is null)
            {
                throw new Exception($"{gameId} does not exist");
            }
            Color? color = g.YellowPlayer == userId ? Color.Yellow : g.RedPlayer == userId ? Color.Red : null;
            if(color is null)
            {
                throw new Exception("Vous ne pouvez jouer dans cette partie");
            }
            if(g.Turn != color)
            {
                throw new Exception("Ce n'est pas à vous de jouer");
            }
            int? h = GetFirstAvailable(g.Grid, col);
            if(h is null)
            {
                throw new Exception("La colonne est remplie");
            }
            g.Grid[col, (int)h] = color;
            g.Turn = g.Turn == Color.Yellow ? Color.Red : Color.Yellow;
            return g;

        }

        private int? GetFirstAvailable(Color?[,] grid, int col)
        {
            return Enumerable.Range(0, 5)
                .FirstOrDefault(i => grid[col, i] == null);
        }

        public void Delete(string gameId)
        {
            Games.Remove(gameId);
        }

    }
}
