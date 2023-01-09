namespace Game
{
    public enum FieldParameterPreset
    {
        Small,
        Medium,
        Large
    }

    public record FieldParameter
    {
        public int SizeX { get; init; }
        public int SizeY { get; init; }
        public int TotalMineNum { get; init; }

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
