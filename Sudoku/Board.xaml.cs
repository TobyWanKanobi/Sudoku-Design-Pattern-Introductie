using Microsoft.Practices.ServiceLocation;
using Sudoku.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        private int _squareSize = 40;

        public Board()
        {
            InitializeComponent();

            SquareViewModel[] Items = ServiceLocator.Current.GetInstance<MainViewModel>().Fields;

            int[] verticalSpace = { 3, 6, 12, 15, 21, 24, 30, 33, 39, 42, 48, 51, 57, 60, 66, 69, 75, 78 };
            int[] horizontalSpace = { 27, 54 };

            // Squares
            Label[] squares = new Label[81];

            int yCor = 0;
            int xCor = 0;
            int squareIndex = 0;
            foreach (SquareViewModel square in Items)
            {
                squares[squareIndex] = new Label
                {
                    DataContext = square,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = _squareSize,
                    Width = _squareSize,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    FontSize = 18,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };

                // Create bindings
                Binding numberBinding = new Binding("ValueText") { Source = square };
                Binding backgroundBinding = new Binding("Background") { Source = square };
                Binding fontWeightBinding = new Binding("FontWeight") { Source = square};
                squares[squareIndex].SetBinding(Label.ContentProperty, numberBinding);
                squares[squareIndex].SetBinding(Label.BackgroundProperty, backgroundBinding);
                squares[squareIndex].SetBinding(Label.FontWeightProperty, fontWeightBinding);

                squares[squareIndex].MouseLeftButtonUp += new MouseButtonEventHandler(SelectSquare_Click);

                // Determine margins to create thick borders
                if (horizontalSpace.Contains(squareIndex))
                    yCor += 3;
                if (verticalSpace.Contains(squareIndex))
                    xCor += 3;


                if ((squareIndex + 1) % 9 == 0)
                {
                    squares[squareIndex].Margin = new Thickness(xCor, yCor, 0, 0);
                    yCor = yCor + _squareSize;
                    xCor = 0;
                }
                else
                {
                    squares[squareIndex].Margin = new Thickness(xCor, yCor, 0, 0);
                    xCor = xCor + _squareSize;
                }

                SudokuLines.Children.Add(squares[squareIndex]);
                squareIndex++;
            }
        }

        private void SelectSquare_Click(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().SelectSquareCommand.Execute(((Label)sender).DataContext);
        }
    }
}
