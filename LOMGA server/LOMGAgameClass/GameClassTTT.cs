using System;

namespace LOMGAgameClass
{
    [Serializable]
    public class GameClassTTT : Game
    {
        public bool turnFlag { get; set; }       // true - X,    false - O       1 - X     2 - O    0 - empty cell
        public int[,] field { get; set; }
        public int columnSize { get; set; }
        public int rowSize { get; set; }
        public bool isWin { get; set; }
        public int forWinCount { get; set; }

        public bool areYouTurningFirst = false;

        public GameClassTTT()
        {
            gameType = 1; // TTT
            isWin = false;
            forWinCount = 3;
            columnSize = 3;
            rowSize = 3;
            field = new int[columnSize, rowSize];
            turnFlag = true;

            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < columnSize; j++)
                    field[i, j] = 0;
        }
        public GameClassTTT(int columnCount, int rowCount)
        {
            gameType = 1; // TTT
            isWin = false;
            forWinCount = 3;
            columnSize = columnCount;
            rowSize = rowCount;
            field = new int[rowSize, columnSize];
            turnFlag = true;

            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < columnSize; j++)
                    field[i, j] = 0;
        }

        public void fieldSizeUpdate()
        {
            field = new int[columnSize,rowSize]; 
        }
        public int turn(int turnColumn, int turnRow)
        {
            try
            {
                if (field[turnRow, turnColumn] == 0)        // cell is empty
                {
                    if (turnFlag)   // if now turn of X
                        field[turnRow, turnColumn] = 1; // set X
                    else            // if now turn of O
                        field[turnRow, turnColumn] = 2; // set O

                    // check for WIN:
                    checkForWin();

                    turnFlag = !turnFlag;
                    return 0;
                }
                else            // if cell is not empty
                    return 1;
            }
            catch (IndexOutOfRangeException e) { return 1; }
        }

        public bool checkForWin()
        {
            int shapesNear = 0;

            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < columnSize; j++)
                {
                    if (field[i, j] != 0)
                        for (int l = 0; l < 4; l++)
                        {
                            for (int k = 0; k < forWinCount; k++)
                            {
                                switch (l)
                                {
                                    case 0:
                                        try
                                        {
                                            if (field[i, j + k] == field[i, j])
                                                shapesNear++;
                                        }
                                        catch (IndexOutOfRangeException e) { }
                                        break;
                                    case 1:
                                        try
                                        {
                                            if (field[i + k, j] == field[i, j])
                                                shapesNear++;
                                        }
                                        catch (IndexOutOfRangeException e) { }
                                        break;
                                    case 2:
                                        try
                                        {
                                            if (field[i + k, j + k] == field[i, j])
                                                shapesNear++;
                                        }
                                        catch (IndexOutOfRangeException e) { }
                                        break;
                                    case 3:
                                        try
                                        {
                                            if (field[i - k, j - k] == field[i, j])
                                                shapesNear++;
                                        }
                                        catch (IndexOutOfRangeException e) { }
                                        break;
                                }

                                if (shapesNear == forWinCount)
                                {
                                    isWin = true;
                                    return true;
                                }
                            }
                            shapesNear = 0;
                        }
                }
            return false;
        }
    }
}
