using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Controls;
using Game;
using MineSweeper.Commands;
using MineSweeper.Views;
using MineSweeper.Converters;

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

        private bool _isDetonated;
        public bool IsDetonated
        {
            get { return _isDetonated; }
            set
            {
                _isDetonated = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDetonated)));
            }
        }

        private bool _isAllCleared;
        public bool IsAllCleared
        {
            get { return _isAllCleared; }
            set
            {
                _isAllCleared = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAllCleared)));
            }
        }

        private int _hiddenMineNum;
        public int HiddenMineNum
        {
            get { return _hiddenMineNum; }
            set
            {
                _hiddenMineNum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HiddenMineNum)));
            }
        }

        public DelegateCommand DrawCommand { get; init;}
        public DelegateCommand RestartCommand { get; init;}

        private List<List<FieldCellViewModel>> FieldCellVMs { get; set; }

        public GameScreenViewModel()
        {
            _fieldParameter = FieldParameter.CreateFromPreset(FieldParameterPreset.Small);
            _mineField = new Field();
            _isDetonated = false;
            _isAllCleared = false;
            _hiddenMineNum = 0;

            DrawCommand = new DelegateCommand(DrawCommandAction);
            RestartCommand = new DelegateCommand(RestartCommandAction);
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
                FieldCellVMs = GenFieldCellVMField();

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


            if (MineField.IsEnd())
            {
                ShowMessageBox("You win");
            }
        }

        private void InitField(int posX, int posY)
        {
            MineField = new Field(FieldParam, posX, posY);
        }

        private void RestartCommandAction(object? parameter)
        {
            if (parameter is NewGameMenuComplex complex)
            {
                MineField = new Field();
                IsDetonated = false;
                IsAllCleared = false;
                FieldParam = FieldParameter.CreateFromPreset(complex.FieldParameterPreset);
                HiddenMineNum = FieldParam.TotalMineNum;
                DrawCommandAction(complex.MineGrid);
            }
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
            if (st == CellStatus.Detonated)
            {
                IsDetonated = true;
            }
        }

        internal void OpenActionOnNotInitialized(int posX, int posY)
        {
            InitField(posX, posY);
            var st = MineField.Open(posX, posY);
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
            if (st == CellStatus.Detonated)
            {
                IsDetonated = true;
            }
        }

        internal void FlagAction(FieldCell cell)
        {
            switch (cell.CellStatus)
            {
                case CellStatus.Flagged:
                    MineField.SetStatus(cell.PosX, cell.PosY, CellStatus.Unopened);
                    break;
                case CellStatus.Unopened:
                    MineField.SetStatus(cell.PosX, cell.PosY, CellStatus.Flagged);
                    break;
                default:
                    break;
            }
            HiddenMineNum = MineField.HiddenMineNum;
        }

        private List<List<FieldCellViewModel>> GenFieldCellVMField()
        {
            var sizeX = FieldParam.SizeX;
            var sizeY = FieldParam.SizeY;

            var vmField = new List<List<FieldCellViewModel>>();
            if (MineField.IsInitialized)
            {
                foreach (var y in Enumerable.Range(0, sizeY))
                {
                    var line = new List<FieldCellViewModel>();
                    foreach (var x in Enumerable.Range(0, sizeX))
                    {
                        line.Add(new FieldCellViewModel(MineField.Cells[y][x], this));
                    }
                    vmField.Add(line);
                }
            }
            else
            {
                foreach (var y in Enumerable.Range(0, sizeY))
                {
                    var line = new List<FieldCellViewModel>();
                    foreach (var x in Enumerable.Range(0, sizeX))
                    {
                        line.Add(new FieldCellViewModel(this, x, y));
                    }
                    vmField.Add(line);
                }
            }
            
            return vmField;
        }

        private void ShowMessageBox(string msg)
        {
            string caption = "Game over";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            //MessageBoxResult result;

            _ = MessageBox.Show(msg, caption, button, icon, MessageBoxResult.OK);
        }
    }
}
