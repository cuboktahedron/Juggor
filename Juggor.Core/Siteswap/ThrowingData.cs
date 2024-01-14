namespace Juggor.Core.Siteswap;

public class ThrowingData
{
    public int Height { get; private set; }

    public bool IsCross { get; private set; }

    public ThrowingData(int height, bool isCross)
    {
        Height = height;
        IsCross = isCross;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ThrowingData other = (ThrowingData)obj;
        return Height == other.Height && IsCross == other.IsCross;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Height, IsCross);
    }

    public override string ToString()
    {
        return ToString("sync");
    }

    public string ToString(string format)
    {
        string cross = string.Empty;
        if (format == "sync" && IsCross)
        {
            cross = "x";
        }

        if (Height < 10)
        {
            return $"{Height}{cross}";
        }
        else
        {
            string height = $"{(char)(Height - 10 + 'a')}";
            return $"{height}{cross}";
        }
    }
}
