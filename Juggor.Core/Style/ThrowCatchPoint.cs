using System.Numerics;

namespace Juggor.Core.Style;

public struct ThrowCatchPoint
{
    public ThrowCatchPoint(Vector2 catchPt, Vector2 throwPt)
    {
        CatchPt = catchPt;
        ThrowPt = throwPt;
    }

    public Vector2 CatchPt { get; private set; }

    public Vector2 ThrowPt { get; private set; }

    public override string ToString()
    {
        return $"{{{CatchPt.X}, {CatchPt.Y}}}{{{ThrowPt.X}, {ThrowPt.Y}}}";
    }
}
