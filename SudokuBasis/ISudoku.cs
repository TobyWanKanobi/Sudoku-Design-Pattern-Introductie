namespace SudokuBasis
{
    public interface ISudoku
    {
        void Create();

        short Get(short xCor, short yCor);

        bool Set(short xCor, short yCor, short number, bool canAdept);

        bool IsCompleted();

        bool Read(bool canRead);

        bool Write(bool canWrite);

        void Hint(out short x, out short y, out short value, out int succeeded);

        void Cheat();

    }
}
