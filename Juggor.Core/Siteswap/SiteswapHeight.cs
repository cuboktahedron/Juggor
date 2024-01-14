namespace Juggor.Core.Siteswap;

public class SiteswapHeight
{
    private readonly int height;

    private SiteswapHeight(int height)
    {
        this.height = height;
    }

    internal SiteswapHeight(SyncSiteswapHeight syncHeight)
    {
        height = syncHeight.Value;
    }

    public int Value => height;

    internal static bool TyParse(SiteswapParseContext context, out SiteswapHeight? height)
    {
        height = null;

        char c = context.Peek();
        int tempHeight;

        if ('A' <= c && c <= 'Z')
        {
            tempHeight = c - 'A' + 10;
        }
        else if ('0' <= c && c <= '9')
        {
            tempHeight = c - '0';
        }
        else
        {
            return false;
        }

        context.Next();

        height = new SiteswapHeight(tempHeight);
        return true;
    }
}
