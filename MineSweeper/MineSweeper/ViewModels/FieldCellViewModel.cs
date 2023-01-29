using System;
using System.ComponentModel;
using Game;
using MineSweeper.Commands;

namespace MineSweeper.ViewModels
{
    public class FieldCellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private FieldCell _fieldCell;
        public FieldCell FieldCell
        {
            get { return _fieldCell; }
            set
            {
                _fieldCell = value;
                var propertyName = string.Format("{0}:{1}_{2}", nameof(FieldCell), _fieldCell.PosX, _fieldCell.PosY);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
            }
        }

        private GameScreenViewModel ScreenViewModel { get; init; }

        public DelegateCommand OpenCommand { get; init; }
        public DelegateCommand OpenAroundOfCommand { get; init; }
        public DelegateCommand FlagCommand { get; init; }

        public FieldCellViewModel(FieldCell fieldCell, GameScreenViewModel screenViewModel)
        {
            _fieldCell = fieldCell;
            ScreenViewModel = screenViewModel;
            OpenCommand = new DelegateCommand(OpenAction, CanOpenCommand);
            OpenAroundOfCommand = new DelegateCommand(OpenAroundOfAction, CanOpenAroundOfCommand);
            FlagCommand = new DelegateCommand(FlagAction, CanFlagCommand);
        }

        public FieldCellViewModel(GameScreenViewModel screenViewModel, int posX, int posY)
        {
            _fieldCell = new FieldCell(CellType.Default, posX, posY, 0);
            ScreenViewModel = screenViewModel;
            OpenCommand = new DelegateCommand((_) => { ScreenViewModel.OpenActionOnNotInitialized(posX, posY); });
            OpenAroundOfCommand = new DelegateCommand((_) => { });
            FlagCommand = new DelegateCommand((_) => { });
        }

        public void CallDrawCommand(object? mgrid)
        {
            ScreenViewModel.DrawCommand.Execute(mgrid);
        }

        private bool CanOpenCommand(object? _)
        {
            return FieldCell.CellStatus == CellStatus.Unopened;
        }

        private void OpenAction(object? _)
        {
            ScreenViewModel.OpenAction(FieldCell);
        }

        private bool CanOpenAroundOfCommand(object? _)
        {
            // ほんとはもっと条件あるけど面倒なのと、Game 側で対処しているので、これだけ
            return FieldCell.CellStatus == CellStatus.Cleared;
        }

        private void OpenAroundOfAction(object? _)
        {
            ScreenViewModel.OpenAroundOfAction(FieldCell);
        }

        private bool CanFlagCommand(object? _)
        {
            return FieldCell.CellStatus switch
            {
                CellStatus.Unopened => true,
                CellStatus.Flagged => true,
                CellStatus.Suspicious => false, // とりあえず
                CellStatus.Cleared => false,
                CellStatus.Detonated => false,
                _ => false
            };
        }

        private void FlagAction(object? _)
        {
            ScreenViewModel.FlagAction(FieldCell);
        }
    }
}
