using System;
using System.Linq;

namespace tic_tac_toe
{
    
    enum Tile
    {
        Nought,
        Cross,
        Unnocupied
    }

    class Board
    {
        // Implementing dimensionality manually,
        // Because md arrays have bad library support.
        private Tile[] state;

        public Board()
        {
            this.state = new Tile[3 * 3];
            Array.Fill(this.state, Tile.Unnocupied);
        }

        public Tile this[uint x, uint y]
        {
            get { return this.state[x + y * 3]; }
            set { this.state[x + y * 3] = value; }
        }

        // Ugly, imperative code.
        public String Render()
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("+-+-+-+");
            for (var y = 0u; y < 3; y++)
            {
                builder.Append('|');
                for (var x = 0u; x < 3; x++)
                {
                    var ch = this[x, y] switch
                    {
                        Tile.Nought => '0',
                        Tile.Cross => 'X',
                        Tile.Unnocupied => ' ',
                        _ => throw new Exception("Non-existent Tile type")
                    };
                    builder.Append(ch);
                    builder.Append('|');
                }
                builder.AppendLine();
                builder.AppendLine("+-+-+-+");
            }
            return builder.ToString();
        }

        public void Draw()
        {
            Console.WriteLine(this.Render());
        }

        public void AIMove(TicTacToeAI ai)
        {
            var (x, y) = ai.move(this);

            if (x > 2 || y > 2)
            {
                throw new System.Exception("AI move out of range");
            }
            
            if (this[x, y] != Tile.Unnocupied)
            {
                throw new System.Exception("Bug: AI's move has already been played");
            }

            this[x, y] = ai.Type();
        }

        // Return whether the given Tile type has won        
        public bool DetermineWinner(Tile type)
        {
            var winning_series = new (uint, uint)[][]{
                // vertical
                new[] { (0u,0u), (0u,1u), (0u,2u) },
                new[] { (1u,0u), (1u,1u), (1u,2u) },
                new[] { (2u,0u), (2u,1u), (2u,2u) },
                // horizontal
                new[] { (0u,0u), (1u,0u), (2u,0u) },
                new[] { (0u,1u), (1u,1u), (2u,1u) },
                new[] { (0u,2u), (1u,2u), (2u,2u) },
                // diagonal
                new[] { (0u,0u), (1u,1u), (2u,2u) },
                new[] { (2u,0u), (1u,1u), (0u,2u) },
            };

            return winning_series.Any(s =>
            {
                return s.All(pos =>
                {
                    var (x, y) = pos;
                    return this[x, y] == type;
                });
            });
        }

        public bool IsFull()
        {
            foreach (var tile in this.state)
            {
                if (tile == Tile.Unnocupied)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsDraw()
        {
            return (!DetermineWinner(Tile.Nought) && !DetermineWinner(Tile.Cross)) && IsFull();
        }
    }
}