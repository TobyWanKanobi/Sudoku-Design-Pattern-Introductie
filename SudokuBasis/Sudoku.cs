using Sudoku;
using System;

namespace SudokuBasis
{
    public class Sudoku : ISudoku
    {
        private IGame game;
        private Random  random = new Random();

        public Sudoku()
        {
            game = new Game();
        }

        public void Create()
        {
            game.create();
        }

        public short Get(short x, short y)
        {
            short value;
            game.get(x, y, out value);

            return value;
        }

        public bool IsCompleted()
        {
            int valid;
            for(short x = 1; x < 10; x++)
            {
                for(short y = 1; y < 10; y++)
                {
                    if (Get(x, y) == 0)
                        return false;
                }
            }

            game.isValid(out valid);

            return Convert.ToBoolean(valid);
        }

        public void Hint(out short x, out short y, out short value, out int succeeded)
        {
            game.hint(out x, out y, out value, out succeeded);
        }

        public void Cheat()
        {
            short x, y, value;
            int succeeded, isPlaced;

            do
            {
                game.hint(out x, out y, out value, out succeeded);

                if (Convert.ToBoolean(succeeded))
                    game.set(x, y, value, out isPlaced);

            } while (Convert.ToBoolean(succeeded));


            int count = 0;
            while(count < 2)
            {
                short xCord = Convert.ToInt16(random.Next(1, 10));
                short yCord = Convert.ToInt16(random.Next(1, 10));

                game.set(xCord, yCord, 0, out isPlaced);

                if (Convert.ToBoolean(isPlaced))
                    count++;
            }

        }

        public bool Read(bool canRead)
        {
            throw new NotImplementedException();
        }

        public bool Write(bool canWrite)
        {
            throw new NotImplementedException();
        }

        public bool Set(short xCor, short yCor, short number, bool canAdept)
        {
            int succeeded;

            game.set(xCor, yCor, number, out succeeded);

            return Convert.ToBoolean(succeeded);
        }
    }
}
