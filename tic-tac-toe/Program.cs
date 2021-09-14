using System;
using System.Collections.Generic;
using System.Linq;

namespace tic_tac_toe
{

    abstract class TicTacToeAI
    {
        protected Tile type;
        public TicTacToeAI(Tile type)
        {
            this.type = type;
        }

        public Tile Type()
        {
            return type;
        }

        // The AI gets a copy of the board,
        // and needs to decide on a move,
        // returning the coordinates of
        // its chosen field.
        public abstract (uint, uint) move(Board board);
    }

    class Program
    {
        static uint GetNumRange(uint low, uint high)
        {
            uint choice;

            while (true)
            {
                while (!uint.TryParse(Console.ReadLine(), out choice))
                {
                    Console.Write("Please enter a valid number: ");
                }
                if (choice > (UInt64)high || choice < low)
                {
                    Console.Write("Please enter a number in the range {0} to {1}: ", low, high);
                    continue;
                }
                break;
            }

            return choice;
        }

        static T menu<T>((string, T)[] options)
        {
            if (options.Length == 0)
            {
                throw new System.Exception("Invalid empty Array passed.");
            }

            for (var i = 0u; i < options.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, options[i].Item1);
            }

            Console.Write("Please choose: ");

            return options[GetNumRange(1, (uint)options.Length) - 1].Item2;
        }

        static bool confirmation(string message)
        {
            Console.Write(message);
            var line = Console.ReadLine().ToLower();

            while (!new string[] { "y", "n", "yes", "no" }.Contains(line))
            {
                Console.Write("Please enter one of [y/yes/n/no]: ");
                line = Console.ReadLine();
            }

            if (line == "y" || line == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            var board = new Board();

            Console.WriteLine("Welcome to TicTacToe!");

            Console.WriteLine("Which do you want to play as? ");

            var player_tile = menu(new (string, Tile)[]{
                ("Noughts (0)", Tile.Nought),
                ("Crosses (X)", Tile.Cross),
            });

            var ai_tile = player_tile switch
            {
                Tile.Nought => Tile.Cross,
                Tile.Cross => Tile.Nought,
                _ => throw new System.Exception("bug: unreachable")
            };

            Console.WriteLine("Please choose an AI to play against: ");


            var AI = menu(new (String, TicTacToeAI)[] {
                 ("The Dumb AI, all moves it plays are random", new DumbAI(ai_tile))
            });

            if (!confirmation("Do you want to go first? [y/n]: "))
            {
                board.PlayAI(AI);
            }

            while (true)
            {
                board.Draw();

                while (true)
                {
                    Console.Write("Please enter the X coordinate of your move (1-3): ");
                    var p_x = GetNumRange(1, 3) - 1;

                    Console.Write("Please enter the Y coordinate of your move (1-3): ");
                    var p_y = GetNumRange(1, 3) - 1;

                    if (board[p_x, p_y] != Tile.Unnocupied)
                    {
                        Console.WriteLine("This move has been played before! Please try again.");
                    }
                    else
                    {
                        board.PlayMove(p_x, p_y, player_tile);
                        break;
                    }
                }

                board.PlayAI(AI);

                if (board.DetermineWinner(player_tile))
                {
                    board.Draw();
                    Console.WriteLine();
                    Console.WriteLine("Congratulations, you won!");
                    break;
                }
                else if (board.DetermineWinner(ai_tile))
                {
                    board.Draw();
                    Console.WriteLine();
                    Console.WriteLine("You lose!");
                    break;
                }
            }
        }
    }
}
