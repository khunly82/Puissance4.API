﻿using Puissance4.API.Entities;
using Puissance4.API.Enums;

namespace Puissance4.API.DTO
{
    public class GameDetailsDTO: GameDTO
    {
        public List<List<Color?>> Grid { get; set; }

        public GameDetailsDTO(Game game, string gameId)
        {
            GameId = gameId;
            YellowPlayer = game.YellowPlayer;
            RedPlayer = game.RedPlayer;
            Grid = new List<List<Color?>>();
            for(int x = 0; x < game.Grid.GetLength(0); x++)
            {
                Grid.Add(new List<Color?>());
                for(int y = 0; y < game.Grid.GetLength(1); y++)
                {
                    Grid[x].Add(game.Grid[x, y]);
                }
            }
        }
    }
}
