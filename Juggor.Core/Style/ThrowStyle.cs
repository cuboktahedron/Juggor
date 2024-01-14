using System.Numerics;

namespace Juggor.Core.Style;

public class ThrowStyle
{
    public ThrowStyle(string name)
    {
        Name = name;
    }

    public string Name { get; }

    private readonly List<ThrowCatchPoint> throwCatchPoints = new();

    public IReadOnlyList<ThrowCatchPoint> ThrowCatchPoints => throwCatchPoints.AsReadOnly();

    public static readonly ThrowStyle Normal = CreateNormal();

    private static ThrowStyle CreateNormal()
    {
        var style = new ThrowStyle("Normal");
        style.Add(new Vector2(13, 0), new Vector2(4, 0));
        return style;
    }

    internal void Add(Vector2 catchPt, Vector2 throwPt)
    {
        throwCatchPoints.Add(new ThrowCatchPoint(catchPt, throwPt));
    }

    public override string ToString()
    {
        return $"Name={Name}, ThrowCatchPoints={string.Join(",", throwCatchPoints)}";
    }
}
