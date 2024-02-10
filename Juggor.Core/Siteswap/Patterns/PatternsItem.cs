using Juggor.Core.Style;

namespace Juggor.Core.Siteswap.Patterns;

public class PatternsItem : IPatternsElement
{
    public PatternsItem(string name, Siteswap siteswap)
        : this(
            name,
            siteswap,
            ThrowStyle.Normal,
            new JuggleParameter())
    {
    }

    public PatternsItem(
        string name,
        Siteswap siteswap,
        ThrowStyle throwStyle,
        JuggleParameter juggleParameter)
    {
        Id = PatternsIdCounter.Next();
        Name = name;
        Siteswap = siteswap;
        ThrowStyle = throwStyle;
        TempoRate = juggleParameter.TempoRate;
        GravityRate = juggleParameter.GravityRate;
    }

    public ThrowStyle ThrowStyle { get; set; } = ThrowStyle.Normal;

    public PatternsElementType ElementType => PatternsElementType.Item;

    public int Id { get; init; }

    public string Name { get; init; }

    public Siteswap Siteswap { get; init; }

    public float TempoRate { get; init; }

    public float GravityRate { get; init; }

    public void Add(IPatternsElement item)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        var ss = new string[]
        {
            $"Name={Name}",
            $"siteswap={Siteswap.RawSiteswap()}",
            $"style=({ThrowStyle})",
            $"tempoRate={TempoRate}",
            $"gravityRate={GravityRate}",
        };

        return string.Join(", ", ss);
    }
}
