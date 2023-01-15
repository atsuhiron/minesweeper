using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using Game;
using MineSweeper.Commands;
using System.Numerics;
using System.Windows.Media.Media3D;

namespace MineSweeper.ViewModels
{
    public class GameScreenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Field _field;
        public Field MineField
        {
            get { return _field; }
            set
            {
                _field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Field)));
            }
        }

        private FieldParameter _fieldParameter;
        public FieldParameter FieldParam
        {
            get { return _fieldParameter; }
            set
            {
                _fieldParameter = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FieldParameter)));
            }
        }

        public DelegateCommand DrawCommand { get; init;}
        public DelegateCommand OpenCommand { get; init; }
        public DelegateCommand OpenAroundCommand { get; init; }

        public GameScreenViewModel()
        {
            _fieldParameter = FieldParameter.CreateFromPreset(FieldParameterPreset.Small);
            _field = new Field();

            DrawCommand = new DelegateCommand(DrawCommandAction);
            OpenCommand = new DelegateCommand(OpenAction);
            OpenAroundCommand = new DelegateCommand(OpenAroundOfAction);
        }

        private void DrawCommandAction(object? parameter)
        {
            // TODO: 毎回全部描画するのは無駄なので、cell に isUpdated のようなフラグを持たせたい。
            if (parameter is Grid mgrid)
            {
                mgrid.Children.Clear();

                int sizeX;
                int sizeY;

                if (MineField.IsInitialized)
                {
                    sizeX = FieldParam.SizeX;
                    sizeY = FieldParam.SizeY;
                    foreach (var y in Enumerable.Range(0, sizeY))
                    {
                        foreach (var x in Enumerable.Range(0, sizeX))
                        {
                            var cell = CellRect.CreateCell(MineField.Cells[y][x]);
                            mgrid.Children.Add(cell);
                        }
                    }
                }
                else
                {
                    sizeX = FieldParam.SizeX;
                    sizeY = FieldParam.SizeY;
                    foreach (var y in Enumerable.Range(0, sizeY))
                    {
                        foreach (var x in Enumerable.Range(0, sizeX))
                        {
                            var cell = CellRect.CreateCell(new FieldCell(CellType.Plane, x, y, 0));
                            mgrid.Children.Add(cell);
                        }
                    }
                }

                
                mgrid.Width = Values.cellSize * sizeX;
                mgrid.Height = Values.cellSize * sizeY;
            }
        }

        private void OpenAction(object? parameter)
        {
            // TODO: 座標を取得
            var posX = 0;
            var posY = 0;

            var st = MineField.Open(posX, posY);
            // TODO: 起爆した時のメッセージ
        }

        private void OpenAroundOfAction(object? parameter)
        {
            // TODO: 座標を取得
            var posX = 0;
            var posY = 0;

            var st = MineField.OpenAroundOf(posX, posY);
            // TODO: 起爆した時のメッセージ
        }
    }
}
