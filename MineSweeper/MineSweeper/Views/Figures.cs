using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MineSweeper.Views
{
    public static class Figures
    {
        private static readonly string _path_detonated = "MineSweeper.fig.detonated.png";
        private static readonly string _path_flag = "MineSweeper.fig.flag.png";

        public static Image GetDetonatedImg()
        {
            return GetImgCore(_path_detonated);   
        }

        public static Image GetFlagImg()
        {
            return GetImgCore(_path_flag);
        }

        private static Image GetImgCore(string path)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var img = new Image();

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                img.Source = BitmapFrame.Create(stream);
            }
            return img;
        }
    }
}
