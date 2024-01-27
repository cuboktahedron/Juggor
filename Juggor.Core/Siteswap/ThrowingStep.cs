namespace Juggor.Core.Siteswap;

public class ThrowingStep
{
    private readonly int cycle;

    private IReadOnlyList<IReadOnlyList<ThrowingData>> hands;

    public ThrowingStep(IReadOnlyList<IReadOnlyList<ThrowingData>> hands, int cycle)
    {
        this.hands = hands;
        this.cycle = cycle;
    }

    public int HandNum => hands.Count;

    public IReadOnlyList<ThrowingData> Hand(int handNo)
    {
        return hands[(handNo + cycle) % HandNum];
    }
}
