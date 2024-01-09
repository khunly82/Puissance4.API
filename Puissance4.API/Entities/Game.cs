using Puissance4.API.Enums;

namespace Puissance4.API.Entities
{
    public class Game
    {
        public Color?[,] Grid { get; set; } = new Color?[7,6];
        public string RedPlayer { get; set; } = null!;
        public string YellowPlayer { get; set; } = null!;
    }
}
