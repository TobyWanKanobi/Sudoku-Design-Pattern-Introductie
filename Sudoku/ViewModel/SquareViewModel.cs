using GalaSoft.MvvmLight;
using SudokuBasis;
using System.Windows;
using System.Windows.Media;

namespace Sudoku.ViewModel
{
    public class SquareViewModel : ViewModelBase
    {

        private bool _adaptable;

        public bool Adaptable {
            get {
                return _adaptable;
            }
            set
            {
                if (_adaptable == value)
                    return;

                _adaptable = value;
                RaisePropertyChanged(() => Adaptable);
                RaisePropertyChanged(() => FontWeight);
            }
        }

        public short X { get; set; }

        public short Y { get; set; }

        private short _value;

        public short Value {

            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                _value = value;

                RaisePropertyChanged("Value");
                RaisePropertyChanged("ValueText");
            }
        }

        public string ValueText
        {
            get
            {
                if (_value == 0)
                    return string.Empty;
                else
                    return _value.ToString();
            }
        }

        public Brush Background {

            get {
                return (IsSelected) ? Brushes.Azure : Brushes.Beige;
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                return (Adaptable) ? FontWeights.Normal : FontWeights.Bold;
            }

        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
                RaisePropertyChanged(() => Background);
            }
        }

        public SquareViewModel(short x, short y)
        {
            X = x;
            Y = y;
        }
    }
}
