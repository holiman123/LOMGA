using System;
using LOMGAgameClass;

namespace LOMGAgameClassTTTcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            GameClassTTT game = new GameClassTTT(10, 3);
            game.forWinCount = 4;

            Console.WriteLine(game.turn(1,0));

            #region show field
            Console.WriteLine();
            for (int i = 0; i < game.rowSize; i++)
            {
                for (int j = 0; j < game.columnSize; j++)
                {
                    Console.Write(game.field[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(game.isWin);
            #endregion

            Console.WriteLine(game.turn(0, 1));

            #region show field
            Console.WriteLine();
            for (int i = 0; i < game.rowSize; i++)
            {
                for (int j = 0; j < game.columnSize; j++)
                {
                    Console.Write(game.field[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(game.isWin);
            #endregion

            Console.WriteLine(game.turn(4, 5));

            #region show field
            Console.WriteLine();
            for (int i = 0; i < game.rowSize; i++)
            {
                for (int j = 0; j < game.columnSize; j++)
                {
                    Console.Write(game.field[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(game.isWin);
            #endregion

            Console.WriteLine(game.turn(1, 0));

            #region show field
            Console.WriteLine();
            for (int i = 0; i < game.rowSize; i++)
            {
                for (int j = 0; j < game.columnSize; j++)
                {
                    Console.Write(game.field[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(game.isWin);
            #endregion

            Console.ReadKey();
        }
    }
}
