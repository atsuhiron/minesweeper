using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MineSweeper.Views
{
    public static class Figures
    {
        private static readonly string _path = "MineSweeper.fig.detonated.png";
        public static Image GetDetonatedImg()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var img =  new Image();
    
            using (var stream = assembly.GetManifestResourceStream(_path))
            {
                img.Source = BitmapFrame.Create(stream);
            }
            return img;
        }
    }
}
