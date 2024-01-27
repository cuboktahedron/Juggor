namespace Juggor.Core.Siteswap;

public abstract class Siteswap
{
    protected Siteswap()
    {
    }

    public abstract bool IsSynchronous { get; }

    public static bool TryParse(SiteswapParseContext context, out Siteswap? siteswap)
    {
        siteswap = null;

        // <siteswap>     ::= <async_factor>+ | <sync_factor>+
        // <async_factor> ::= <height> | "[" <height>+ "]"
        // <height>       ::= "0".."9" | "A".."z"
        // <sync_factor>> ::= "(" <sync_factor_factor> "," <sync_factor_factor> ")"
        // <sync_factor_factor>
        //                ::= <sync_height>x? | "[" (<sync_height>x?)+ "]"
        // <sync_height>  ::= "2" | "4" .."8" | "A" | "C" .. "Y"
        if (context.Peek() == '(')
        {
            if (SyncSiteswap.TryParse(context, out SyncSiteswap? syncSiteswap))
            {
                siteswap = syncSiteswap;
                return true;
            }
        }
        else
        {
            if (AsyncSiteswap.TryParse(context, out AsyncSiteswap? asyncSiteswap))
            {
                siteswap = asyncSiteswap;
                return true;
            }
        }

        return false;
    }

    public abstract bool IsValid();

    public abstract int BallNum();

    public abstract IReadOnlyList<IReadOnlyList<IReadOnlyList<ThrowingData>>> ToThrowings();

    public abstract string RawSiteswap();

    public abstract IReadOnlyList<int> BallNumsInHands();
}
