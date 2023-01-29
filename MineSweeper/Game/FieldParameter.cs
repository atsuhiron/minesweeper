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
        private const int MAX_SIZE = 100;
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
            if (SizeX <= 0 || SizeX > MAX_SIZE) return false;
            if (SizeY <= 0 || SizeY > MAX_SIZE) return false;
            if (TotalMineNum <= 0 || TotalMineNum > SizeX * SizeY) return false;
            return true;
        }

        public static FieldParameter CreateFromPreset(FieldParameterPreset preset)
        {
            return preset switch
            {
                FieldParameterPreset.Small => new FieldParameter(10, 10, 10),
                FieldParameterPreset.Medium => new FieldParameter(15, 15, 24),
                FieldParameterPreset.Large => new FieldParameter(30, 15, 50),
                _ => new FieldParameter(10, 10, 10),
            };
        }

        public static FieldParameterPreset ParsePresetFromString(string presetStr)
        {
            return presetStr switch
            {
                "Small" => FieldParameterPreset.Small,
                "Medium" => FieldParameterPreset.Medium,
                "Large" => FieldParameterPreset.Large,
                _ => FieldParameterPreset.Small
            };
        }
    }
}
