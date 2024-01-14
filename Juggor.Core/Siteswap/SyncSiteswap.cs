namespace Juggor.Core.Siteswap;

public class SyncSiteswap : Siteswap
{
    private readonly List<SyncSiteswapFactor> factors;

    private SyncSiteswap(List<SyncSiteswapFactor> factors)
    {
        this.factors = factors;
    }

    public override bool IsSynchronous => true;

    internal static bool TryParse(SiteswapParseContext context, out SyncSiteswap? siteswap)
    {
        siteswap = null;

        var factors = new List<SyncSiteswapFactor>();

        while (!context.IsEoc())
        {
            if (SyncSiteswapFactor.TryParse(context, out SyncSiteswapFactor? factor))
            {
                factors.Add(factor!);
            }
            else
            {
                return false;
            }
        }

        siteswap = new SyncSiteswap(factors);
        return true;

    }

    public override int BallNum()
    {
        if (!IsValid())
        {
            return -1;
        }

        var heightss = factors
            .SelectMany(x => new List<List<int>>() {
                x.LeftHand.Throwings.Select(y => y.Height).ToList(),
                x.RightHand.Throwings.Select(y => y.Height).ToList(),
            }).ToList();
        var sum = heightss.Sum(x => x.Sum());

        return sum / heightss.Count;
    }

    public override bool IsValid()
    {
        if (factors.Count == 0)
        {
            return false;
        }

        var heightss = factors
            .SelectMany(x => new List<List<int>>() {
                x.LeftHand.Throwings.Select(y => y.IsCross ?
                    y.Height + 1 :
                    y.Height).ToList(),
                x.RightHand.Throwings.Select(y => y.IsCross ?
                    y.Height - 1 :
                    y.Height).ToList(),
            }).ToList();

        var modHeights = heightss
            .SelectMany((heights, i) => heights.Select(height => (height + i + 1) % heightss.Count))
            .OrderBy(x => x)
            .ToList();
        var modAdds = heightss
            .SelectMany((heights, i) => Enumerable.Repeat((i + 1) % heightss.Count, heights.Count))
            .OrderBy(x => x)
            .ToList();
        return modHeights.Zip(modAdds).All(x => x.First == x.Second);
    }

    public override IReadOnlyList<IReadOnlyList<IReadOnlyList<ThrowingData>>> ToThrowings()
    {
        return factors.Select(factor => factor.ToThrowings()).ToList();
    }

    public override string RawSiteswap()
    {
        return string.Join("", factors.Select(factor => factor.RawSiteswap()));
    }

    public override IReadOnlyList<int> BallNumsInHands()
    {
        if (!IsValid())
        {
            throw new InvalidOperationException("Siteswap must be valid.");
        }

        const int maxHeight = 36;

        int restBall = BallNum();
        var initialBallNums = new List<int>() { 0, 0 };
        var ballNumInHands = new List<int>() { 0, 0 };
        var activeBallss = new List<List<int>>();

        foreach (var _ in ballNumInHands)
        {
            var activeBalls = new List<int>();
            for (int i = 0; i < maxHeight; i++)
            {
                activeBalls.Add(0);
            }

            activeBallss.Add(activeBalls);
        }

        int loopNum = 0;
        while (loopNum < 1000)
        {
            loopNum++;

            foreach (var factor in factors)
            {
                if (restBall == 0)
                {
                    return initialBallNums;
                }

                for (int i = 0; i < activeBallss.Count; i++)
                {
                    var ballNum1 = activeBallss[i][0];
                    var ballNum2 = activeBallss[i][1];
                    activeBallss[i].RemoveAt(1);
                    activeBallss[i].RemoveAt(0);
                    ballNumInHands[i] += ballNum1 + ballNum2;
                    activeBallss[i].Add(0);
                    activeBallss[i].Add(0);
                }

                foreach (var (throwingss, handNo) in
                     factor.ToThrowings().Select((item, i) => (item, i)))
                {
                    var throwings = throwingss.Where(t => t.Height != 0).ToList();
                    int n = throwings.Count - ballNumInHands[handNo];
                    if (n > 0)
                    {
                        initialBallNums[handNo] += n;
                        restBall -= n;

                        if (restBall < 0)
                        {
                            throw new InvalidOperationException(
                                "The number of rest ball bacame negative. This is probably bug.");
                        }
                    }

                    foreach (var throwing in throwings)
                    {
                        if (throwing.IsCross)
                        {
                            activeBallss[(handNo + 1) % ballNumInHands.Count][throwing.Height - 1]++;
                        }
                        else
                        {
                            activeBallss[handNo][throwing.Height - 1]++;
                        }
                    }
                }
            }
        }

        if (loopNum >= 1000)
        {
            throw new InvalidOperationException("Couldn't determine the number of balls in hands.");
        }

        return initialBallNums;
    }
}
