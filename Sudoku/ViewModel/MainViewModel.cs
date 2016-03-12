using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections.ObjectModel;
using SudokuBasis;
using System;

namespace Sudoku.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ISudoku _game;

        public SquareViewModel[] Fields { get; set; }

        public short[] Options { get; set; }

        private short _selectedOption;

        public short SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                if (_selectedOption == value)
                    return;
                _selectedOption = value;
                RaisePropertyChanged(() => SelectedOption);
            }
        }

        private SquareViewModel _selectedSquare;

        public SquareViewModel SelectedSquare {

            get {
                return _selectedSquare;
            }

            set
            {
                if (_selectedSquare == value)
                    return;

                if (_selectedSquare != null)
                    _selectedSquare.IsSelected = false;

                _selectedSquare = value;
                _selectedSquare.IsSelected = true;

                RaisePropertyChanged(() => SelectedSquare);
            }
        }

        public ICommand FillInSquareCommand { get; set; }

        public ICommand SelectSquareCommand { get; set; }

        public ICommand CheatCommand { get; set; }

        public ICommand NewGameCommand { get; set; }

        public ICommand HintCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _game = new SudokuBasis.Sudoku();
            SelectSquareCommand = new RelayCommand<SquareViewModel>((s) => { SelectedSquare = s; });
            FillInSquareCommand = new RelayCommand(ExecuteFillInSquareCommand);
            CheatCommand = new RelayCommand(ExecuteCheatCommand);
            NewGameCommand = new RelayCommand(ExecuteNewGameCommand);
            HintCommand = new RelayCommand(ExecuteHintCommand);

            Options = new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Fields = new SquareViewModel[81];

            short i = 0;
            for (short y = 1; y < 10; y++)
            {
                for (short x = 1; x < 10; x++)
                {
                    Fields[i] = new SquareViewModel(x, y);
                    i++;
                }
            }

            ExecuteNewGameCommand();
           
        }

        private void ExecuteFillInSquareCommand()
        {
            // Return if no swuare selected or no option selected
            if (SelectedSquare == null || SelectedOption == 0)
                return;

            // Fill in square
            if(_game.Set(SelectedSquare.X, SelectedSquare.Y, SelectedOption, true))
                SelectedSquare.Value = SelectedOption;

            if (_game.IsCompleted())
                MessageBox.Show("Gefelicteerd met het oplossen van de Sudoku");
        }

        public void ExecuteSaveCommand()
        {
            _game.Write(true);
        }

        public void ExecuteCheatCommand()
        {
            _game.Cheat();
            this.ReloadBoard();
        }

        public void ExecuteNewGameCommand()
        {
            // Create new game in domain
            _game.Create();

            // Set values for viewmodel
            foreach (SquareViewModel sq in Fields)
            {
                var test = _game.Get(sq.X, sq.Y);
                sq.Value = _game.Get(sq.X, sq.Y);
                sq.Adaptable = (_game.Get(sq.X, sq.Y) > 0) ? false : true;
            }
        }

        public void ExecuteHintCommand()
        {
            short x, y, value;
            int succeeded;
            _game.Hint(out x, out y, out value, out succeeded);
            if(Convert.ToBoolean(succeeded))
                MessageBox.Show(String.Format("X: {0} Y: {1}, Waarde: {2}", x, y, value));
        }

        public void ReloadBoard()
        {
            foreach (SquareViewModel sq in Fields)
            {
                sq.Value = _game.Get(sq.X, sq.Y);
            }
        }

    }
}