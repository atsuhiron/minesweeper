namespace Game
{
    public enum FieldParameterPreset
    {
        Small,
        Medium,
        Large
    }

    public class FieldParameter
    {
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int TotalMineNum { get; set; }

        public FieldParameter() { }

        public FieldParameter(int sizeX, int sizeY, int totalMineNum)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            TotalMineNum = totalMineNum;
        }

        public bool IsValid()
        {
            return SizeX > 0 && SizeY > 0 && TotalMineNum > 0;
        }

        public static FieldParameter CreateFromPreset(FieldParameterPreset preset)
        {
            return preset switch
            {
                FieldParameterPreset.Small => new FieldParameter(10, 10, 10),
                FieldParameterPreset.Medium => new FieldParameter(15, 15, 24),
                FieldParameterPreset.Large => new FieldParameter(15, 30, 50),
                _ => new FieldParameter(10, 10, 10),
            };
        }
    }
}
