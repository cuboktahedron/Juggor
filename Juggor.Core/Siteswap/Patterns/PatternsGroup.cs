namespace Juggor.Core.Siteswap.Patterns;

public class PatternsGroup : IPatternsElement
{
    private readonly int id;
    private readonly string name;
    private readonly List<IPatternsElement> elements = new();

    public PatternsGroup(string name)
    {
        this.id = PatternsIdCounter.Next();
        this.name = name;
    }

    public PatternsElementType ElementType => PatternsElementType.Group;

    public int Id => id;

    public string Name => name;

    public IReadOnlyList<IPatternsElement> Elements => elements.AsReadOnly();

    public void Add(IPatternsElement element)
    {
        elements.Add(element);
    }
}
