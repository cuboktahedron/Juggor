

namespace Juggor.Game;

public partial class HandPaths : Node2D
{
    private int pathCounter = 0;

    private readonly List<HandPath> paths = new();

    private long times;
    private long maxTimes;

    public void AddPath(HandPath handPath, float tempoRate)
    {
        paths.Add(handPath);
        AddChild(handPath);

        maxTimes = (long)(60f / tempoRate);
    }

    public Vector2 Next()
    {
        if (!paths.Any())
        {
            return default;
        }

        times++;
        paths[pathCounter].ProgressRatio = (float)times / maxTimes;
        var pos = paths[pathCounter].PathFollow.Position;

        if (times >= maxTimes)
        {
            times = 0;
            pathCounter = (pathCounter + 1) % paths.Count;
            paths[pathCounter].ProgressRatio = 0;
        }

        return Position + pos;
    }

    public Vector2 PathPositionAfter(long flyingFrame)
    {
        long tmpTimes = times + flyingFrame;
        int tmpPathCounter = (int)(pathCounter + tmpTimes / maxTimes) % paths.Count;
        tmpTimes %= maxTimes;

        float tmpProgressRatio = paths[tmpPathCounter].ProgressRatio;
        paths[tmpPathCounter].ProgressRatio = (float)tmpTimes / maxTimes;
        var pos = Position + paths[tmpPathCounter].PathFollow.Position;
        paths[tmpPathCounter].ProgressRatio = tmpProgressRatio;

        return pos;
    }

    public void Slide()
    {
        times = 0;
        pathCounter = (pathCounter + 1) % paths.Count;
    }
}
