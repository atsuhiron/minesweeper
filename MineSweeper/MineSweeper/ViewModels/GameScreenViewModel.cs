using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Game;
using MineSweeper.Commands;

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

        public DelegateCommand OpenCommand { get; init; }
        public DelegateCommand OpenAroundCommand { get; init; }

        public GameScreenViewModel()
        {
            _fieldParameter = FieldParameter.CreateFromPreset(FieldParameterPreset.Small);
            _field = new Field();

            
        }
    }
}
