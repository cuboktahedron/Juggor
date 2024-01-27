namespace Juggor.Core.Siteswap;

public class SyncSiteswapHeight
{
    private readonly int height;

    private SyncSiteswapHeight(int height)
    {
        this.height = height;
    }

    public int Value => height;

    internal static bool TyParse(SiteswapParseContext context, out SyncSiteswapHeight? height)
    {
        height = null;

        char c = context.Peek();
        int tempHeight;

        if (c >= 'A' && c <= 'Z')
        {
            tempHeight = c - 'A' + 10;
        }
        else if (c >= '0' && c <= '9')
        {
            tempHeight = c - '0';
        }
        else
        {
            return false;
        }

        if (tempHeight % 2 != 0)
        {
            return false;
        }

        context.Next();

        height = new SyncSiteswapHeight(tempHeight);
        return true;
    }
}
