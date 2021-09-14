using System.Linq;

namespace tic_tac_toe
{
    class PerfectAI : TicTacToeAI
    {
        enum Outcomes
        {
            Draw,
            Win,
            Loss,
            ForcedDraw,
            ForcedLoss,
        }

        public PerfectAI(Tile type) : base(type) { }

        Outcomes Mine(Board board)
        {
            var available = board.AsArray().Where(t => t == Tile.Unnocupied).Select((_, i) => ((uint)i % 3, (uint)i / 3)).ToArray();
            // We're assuming there is at least one available move
            if (available.Length == 1)
            {
                var (x, y) = available[1];
                var b = board.SimulateMove(x, y, this.type);

                if (b.DetermineWinner(this.type))
                {
                    return Outcomes.Win;
                }
                else if (b.IsDraw())
                {
                    return Outcomes.Draw;
                }
                else
                {
                    return Outcomes.Loss;
                }
            }
            else
            {
                foreach (var (x, y) in available)
                {

                }
            }
        }

        Outcomes Thine(Board b)
        {

        }

        public override (uint, uint) move(Board board)
        {
            return (0, 0);
        }
    }
}