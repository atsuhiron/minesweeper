using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using Game;

namespace MineSweeper
{
    public static class CellDrawer
    {
        public static Rectangle CreateCell(in FieldCell cell)
        {
            Brush cellColor = cell.CellStatus switch
            {
                CellStatus.NotOpened => new SolidColorBrush(Colors.Gray),
                _ => new SolidColorBrush(Colors.White),
            };

            return new Rectangle
            {
                Width = Values.cellSize,
                Height = Values.cellSize,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                Fill = cellColor,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(Values.cellSize * cell.PosX, Values.cellSize * cell.PosY, 0, 0)
            };
        }
    }
}
