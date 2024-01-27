namespace Juggor.Core.Siteswap;

public class SyncSiteswapFactor
{
    private readonly List<SyncSiteswapFactorFactor> factor2s = new();

    private SyncSiteswapFactor(
        SyncSiteswapFactorFactor leftFactor,
        SyncSiteswapFactorFactor rightFactor)
    {
        factor2s.Add(leftFactor);
        factor2s.Add(rightFactor);
    }

    public SyncSiteswapFactorFactor LeftHand => factor2s[0];

    public SyncSiteswapFactorFactor RightHand => factor2s[1];

    public IReadOnlyList<IReadOnlyList<ThrowingData>> ToThrowings()
    {
        return factor2s
            .Select(factor => factor.ToThrowings())
            .ToList();
    }

    public string RawSiteswap()
    {
        return $"({string.Join(",", factor2s.Select(factor2 => factor2.RawSiteswap()))})";
    }

    internal static bool TryParse(
        SiteswapParseContext context,
        out SyncSiteswapFactor? factor)
    {
        factor = null;

        if (context.Next() != '(')
        {
            return false;
        }

        if (!SyncSiteswapFactorFactor.TryParse(context, out SyncSiteswapFactorFactor? leftFactor))
        {
            return false;
        }

        if (context.Next() != ',')
        {
            return false;
        }

        if (!SyncSiteswapFactorFactor.TryParse(context, out SyncSiteswapFactorFactor? rightFactor))
        {
            return false;
        }

        if (context.Next() != ')')
        {
            return false;
        }

        factor = new SyncSiteswapFactor(leftFactor!, rightFactor!);
        return true;
    }
}
