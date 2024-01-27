namespace Juggor.Core.Siteswap;

public class SiteswapParseContext
{
    private readonly string pattern;

    private int curIndex;

    public SiteswapParseContext(string pattern)
    {
        this.pattern = pattern.Replace(" ", string.Empty);
    }

    public char Peek()
    {
        if (IsEoc())
        {
            return default;
        }

        return pattern[curIndex];
    }

    public char Next()
    {
        if (IsEoc())
        {
            return default;
        }

        char c = pattern[curIndex];
        curIndex++;

        return c;
    }

    public bool IsEoc()
    {
        return curIndex >= pattern.Length;
    }
}
