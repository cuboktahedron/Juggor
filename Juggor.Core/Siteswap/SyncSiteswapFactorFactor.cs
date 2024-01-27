namespace Juggor.Core.Siteswap;

public class SyncSiteswapFactorFactor
{
    private readonly List<ThrowingData> throwings = new();

    private SyncSiteswapFactorFactor(
        params ThrowingData[] throwings)
    {
        this.throwings = throwings.ToList();
    }

    public ThrowingData Throwing => throwings[0];

    public bool IsMultiplex => throwings.Count > 1;

    public IReadOnlyList<ThrowingData> Throwings => throwings;

    public string RawSiteswap()
    {
        if (throwings.Count == 1)
        {
            return throwings[0].ToString("sync");
        }
        else
        {
            return $"[{string.Join(string.Empty, throwings.Select(x => x.ToString("sync")))}]";
        }
    }

    internal static bool TryParse(
        SiteswapParseContext context,
        out SyncSiteswapFactorFactor? ffactor)
    {
        ffactor = null;

        if (context.Peek() == '[')
        {
            context.Next();

            var throwings = new List<ThrowingData>();
            while (SyncSiteswapHeight.TyParse(context, out SyncSiteswapHeight? height))
            {
                bool isCross = false;

                if (context.Peek() == 'x')
                {
                    isCross = true;
                    context.Next();
                }

                throwings.Add(new ThrowingData(height!.Value, isCross));
            }

            if (context.Next() != ']' || !throwings.Any())
            {
                return false;
            }

            ffactor = new SyncSiteswapFactorFactor(throwings.ToArray());
            return true;
        }
        else
        {
            if (SyncSiteswapHeight.TyParse(context, out SyncSiteswapHeight? height))
            {
                bool isCross = false;
                if (context.Peek() == 'x')
                {
                    isCross = true;
                    context.Next();
                }

                ffactor = new SyncSiteswapFactorFactor(
                    new ThrowingData(height!.Value!, isCross));
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    internal IReadOnlyList<ThrowingData> ToThrowings()
    {
        return throwings;
    }
}
