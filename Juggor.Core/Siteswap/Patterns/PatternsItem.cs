using Juggor.Core.Style;

namespace Juggor.Core.Siteswap.Patterns;

public class PatternsItem : IPatternsElement
{
    private readonly int id;

    private readonly string name;

    private readonly Siteswap siteswap;

    public PatternsElementType ElementType => PatternsElementType.Item;

    public int Id => id;

    public string Name => name;

    public Siteswap Siteswap => siteswap;

    public ThrowStyle ThrowStyle { get; set; } = ThrowStyle.Normal;

    public PatternsItem(string name, Siteswap siteswap)
        : this(name, siteswap, ThrowStyle.Normal)
    {
    }

    public PatternsItem(string name, Siteswap siteswap, ThrowStyle throwStyle)
    {
        this.id = PatternsIdCounter.Next();
        this.name = name;
        this.siteswap = siteswap;
        this.ThrowStyle = throwStyle;
    }

    public void Add(IPatternsElement item)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"Name={name}, siteswap={siteswap.RawSiteswap()}, style=({ThrowStyle})";
    }
}
