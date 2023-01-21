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
            //this.CommandBindings.Add(new CommandBinding(fieldCellVM.OpenCommand));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!((FieldCellViewModel)DataContext).OpenCommand.CanExecute(null)) return;
            Background = new SolidColorBrush(Colors.WhiteSmoke);
            ((FieldCellViewModel)DataContext).OpenCommand.Execute(null);
        }


        public static CellRect CreateCell(in FieldCell cell, in FieldCellViewModel fieldCellVM)
        {
            Brush cellColor = cell.CellStatus switch
            {
                CellStatus.NotOpened => new SolidColorBrush(Colors.Gray),
                _ => new SolidColorBrush(Colors.White),
            };

            string dispText;
            if (cell.CellStatus == CellStatus.Cleared && cell.NeighborMineNum != 0)
            {
                dispText = cell.NeighborMineNum.ToString();
            }
            else
            {
                dispText = string.Empty;
            }

            var textBlock = new TextBlock
            {
                Text = dispText,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16
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
            gridCell.Children.Add(textBlock);
            return gridCell;
        }
    }
}
