using System.Numerics;

namespace Juggor.Core.Style;

public class ThrowStyle
{
    public static readonly ThrowStyle Normal = CreateNormal();

    private readonly List<ThrowCatchPoint> throwCatchPoints = new();

    public ThrowStyle(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public IReadOnlyList<ThrowCatchPoint> ThrowCatchPoints => throwCatchPoints.AsReadOnly();

    public override string ToString()
    {
        return $"Name={Name}, ThrowCatchPoints={string.Join(",", throwCatchPoints)}";
    }

    internal void Add(Vector2 catchPt, Vector2 throwPt)
    {
        throwCatchPoints.Add(new ThrowCatchPoint(catchPt, throwPt));
    }

    private static ThrowStyle CreateNormal()
    {
        var style = new ThrowStyle("Normal");
        style.Add(new Vector2(13, 0), new Vector2(4, 0));
        return style;
    }
}
