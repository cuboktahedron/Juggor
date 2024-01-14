namespace Juggor.Core.Siteswap;

public class SiteswapConductor
{
    private IReadOnlyList<IReadOnlyList<IReadOnlyList<ThrowingData>>> throwingsss =
        new List<List<List<ThrowingData>>>();

    private readonly Siteswap siteswap;

    public bool IsSynchronous => siteswap.IsSynchronous;

    public SiteswapConductor(Siteswap siteswap)
    {
        if (!siteswap.IsValid())
        {
            throw new ArgumentException("Siteswap must be valid.", nameof(siteswap));
        }

        this.siteswap = siteswap;

        throwingsss = siteswap.ToThrowings();
    }

    int stepNo = -1;

    public ThrowingStep Next()
    {
        stepNo++;

        int diffPerCycle = throwingsss.Count % throwingsss[0].Count;
        int cycleNo = stepNo / throwingsss.Count;
        int totalDiff = cycleNo * diffPerCycle;
        int cycle = totalDiff % throwingsss[0].Count;
        return new ThrowingStep(throwingsss[stepNo % throwingsss.Count], cycle);
    }
}
