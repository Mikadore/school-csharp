using System.Collections.Generic;

namespace tic_tac_toe
{
    // The Dumb AI just chooses a random available field.
    class DumbAI : TicTacToeAI
    {
        public DumbAI(Tile type) : base(type) { }

        public override (uint, uint) move(Board board)
        {
            var available = new List<(uint, uint)>();

            // hate this inflexibility
            for (var y = 0u; y < 3; y++)
            {
                for (var x = 0u; x < 3; x++)
                {
                    if (board[x, y] == Tile.Unnocupied)
                    {
                        available.Add((x, y));
                    }
                }
            }

            if (available.Count == 0)
            {
                throw new System.Exception("Error: Board is full, cannot move");
            }

            return available[new System.Random().Next() % available.Count];
        }
    }
}