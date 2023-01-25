using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using Game;
using MineSweeper.ViewModels;

namespace MineSweeper.Views
{
    public class CellRect : Grid
    {
        private int PosX { get; init; }
        private int PosY { get; init; }

        public CellRect(in FieldCellViewModel fieldCellVM, int posX, int posY) : base()
        {
            PosX = posX;
            PosY = posY;
            DataContext = fieldCellVM;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (DataContext is not FieldCellViewModel fieldCellVM) return;
            Background = new SolidColorBrush(Colors.WhiteSmoke);

            if (!fieldCellVM.OpenCommand.CanExecute(null)) return;
            fieldCellVM.OpenCommand.Execute(null);

            fieldCellVM.CallDrawCommand(base.Parent);
        }


        public static CellRect CreateCell(in FieldCell cell, in FieldCellViewModel fieldCellVM)
        {
            Brush cellColor = cell.CellStatus switch
            {
                CellStatus.Unopened => new SolidColorBrush(Colors.Gray),
                _ => new SolidColorBrush(Colors.White),
            };

            var gridCell = new CellRect(fieldCellVM, cell.PosX, cell.PosY)
            {
                Width = Values.cellSize - 2,
                Height = Values.cellSize - 2,
                Background = cellColor,
                ShowGridLines = true,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(Values.cellSize * cell.PosX, Values.cellSize * cell.PosY, 0, 0),
            };

            if (cell.CellStatus == CellStatus.Cleared && cell.NeighborMineNum != 0)
            {
                string dispText = cell.NeighborMineNum.ToString();
                var textBlock = new TextBlock
                {
                    Text = dispText,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16
                };
                gridCell.Children.Add(textBlock);
            }
            else if (cell.CellStatus == CellStatus.Detonated)
            {
                var img = Figures.GetDetonatedImg();
                img.IsEnabled = true;
                gridCell.Children.Add(img);
            }

            return gridCell;
        }
    }
}
