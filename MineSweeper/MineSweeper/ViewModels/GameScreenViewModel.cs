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
using MineSweeper.Views;

namespace MineSweeper.ViewModels
{
    public class GameScreenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Field _mineField;
        public Field MineField
        {
            get { return _mineField; }
            set
            {
                _mineField = value;
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

        private List<List<FieldCellViewModel>> FieldCellVMs { get; set; }

        public GameScreenViewModel()
        {
            _fieldParameter = FieldParameter.CreateFromPreset(FieldParameterPreset.Small);
            _mineField = new Field();

            DrawCommand = new DelegateCommand(DrawCommandAction);
            FieldCellVMs = GenFieldCellVMField();
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
                            var cell = CellRect.CreateCell(MineField.Cells[y][x], FieldCellVMs[y][x]);
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
                            var cell = CellRect.CreateCell(new FieldCell(CellType.Plane, x, y, 0), FieldCellVMs[y][x]);
                            mgrid.Children.Add(cell);
                        }
                    }
                }

                
                mgrid.Width = Values.cellSize * sizeX;
                mgrid.Height = Values.cellSize * sizeY;
            }
        }

        private void InitField(int posX, int posY)
        {
            MineField = new Field(FieldParam, posX, posY);
        }

        internal void OpenAction(FieldCell cell)
        {
            int posX = cell.PosX;
            int posY = cell.PosY;
            if (!MineField.IsInitialized)
            {
                InitField(posX, posY);
            }

            var st = MineField.Open(posX, posY);
            // TODO: 起爆した時のメッセージ
        }

        internal void OpenAroundOfAction(FieldCell cell)
        {
            int posX = cell.PosX;
            int posY = cell.PosY;
            if (!MineField.IsInitialized)
            {
                InitField(posX, posY);
            }

            var st = MineField.OpenAroundOf(posX, posY);
            // TODO: 起爆した時のメッセージ
        }

        private List<List<FieldCellViewModel>> GenFieldCellVMField()
        {
            var sizeX = FieldParam.SizeX;
            var sizeY = FieldParam.SizeY;

            var vmField = new List<List<FieldCellViewModel>>();
            foreach (var y in Enumerable.Range(0, sizeY))
            {
                var line = new List<FieldCellViewModel>();
                foreach (var x in Enumerable.Range(0, sizeX))
                {
                    line.Add(new FieldCellViewModel(MineField.Cells[y][x], this));
                }
                vmField.Add(line);
            }
            return vmField;
        }

        //internal void OpenAction(int posX, int posY)
        //{
        //    var st = MineField.Open(posX, posY);
        //    // TODO: 起爆した時のメッセージ
        //}

        //internal void OpenAroundOfAction(int posX, int posY)
        //{
        //    var st = MineField.OpenAroundOf(posX, posY);
        //    // TODO: 起爆した時のメッセージ
        //}
    }
}
