namespace Juggor.Core.Siteswap;

public class AsyncSiteswapFactor
{
    private readonly List<ThrowingData> throwings = new();

    private AsyncSiteswapFactor(params SiteswapHeight[] heights)
    {
        throwings = heights
            .Select(x => new ThrowingData(x.Value, x.Value % 2 == 1))
            .ToList();
    }

    public ThrowingData Throwing => throwings[0];

    public IReadOnlyList<ThrowingData> Throwings => throwings;

    public bool IsMultiplex => throwings.Count > 1;

    public IReadOnlyList<ThrowingData> ToThrowings()
    {
        return throwings;
    }

    public string RawSiteswap()
    {
        if (throwings.Count == 1)
        {
            return throwings[0].ToString("async");
        }
        else
        {
            return $"[{string.Join(string.Empty, throwings.Select(x => x.ToString("async")))}]";
        }
    }

    internal static bool TryParse(
        SiteswapParseContext context,
        out AsyncSiteswapFactor? factor)
    {
        factor = null;

        if (context.Peek() == '[')
        {
            context.Next();

            var heights = new List<SiteswapHeight>();
            while (SiteswapHeight.TyParse(context, out SiteswapHeight? height))
            {
                heights.Add(height!);
            }

            if (context.Next() != ']' || !heights.Any())
            {
                return false;
            }

            factor = new AsyncSiteswapFactor(heights.ToArray());
            return true;
        }
        else
        {
            if (SiteswapHeight.TyParse(context, out SiteswapHeight? height))
            {
                factor = new AsyncSiteswapFactor(height!);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
