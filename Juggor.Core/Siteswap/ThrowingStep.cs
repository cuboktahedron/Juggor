namespace Juggor.Core.Siteswap;

public class ThrowingStep
{
    private readonly int cycle;

    public ThrowingStep(IReadOnlyList<IReadOnlyList<ThrowingData>> hands, int cycle)
    {
        this.hands = hands;
        this.cycle = cycle;
    }

    private IReadOnlyList<IReadOnlyList<ThrowingData>> hands;

    public IReadOnlyList<ThrowingData> Hand(int handNo)
    {
        return hands[(handNo + cycle) % HandNum];
    }

    public int HandNum => hands.Count;
}
