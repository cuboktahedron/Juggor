namespace Juggor.Core.Siteswap;

public class AsyncSiteswap : Siteswap
{
    private readonly List<AsyncSiteswapFactor> factors;

    private AsyncSiteswap(List<AsyncSiteswapFactor> factors)
    {
        this.factors = factors;
    }

    public override bool IsSynchronous => false;

    public override int BallNum()
    {
        if (!IsValid())
        {
            return -1;
        }

        int sum = factors.Sum(x => x.Throwings.Sum(y => y.Height));
        return sum / factors.Count;
    }

    public override bool IsValid()
    {
        if (factors.Count == 0)
        {
            return false;
        }

        var modHeights = factors
            .SelectMany((x, i) => x.Throwings.Select(y => (y.Height + i + 1) % factors.Count))
            .OrderBy(x => x)
            .ToList();
        var modAdds = factors
            .SelectMany((x, i) => Enumerable.Repeat((i + 1) % factors.Count, x.Throwings.Count))
            .OrderBy(x => x)
            .ToList();
        return modHeights.Zip(modAdds).All(x => x.First == x.Second);
    }

    public override IReadOnlyList<IReadOnlyList<IReadOnlyList<ThrowingData>>> ToThrowings()
    {
        return factors.Select((factor, i) =>
        {
            var throwings = new List<IReadOnlyList<ThrowingData>>(
                Enumerable.Repeat(new List<ThrowingData>(), 2))
            {
                [i % 2] = factor.ToThrowings(),
            };

            return throwings;
        }).ToList();
    }

    public override string RawSiteswap()
    {
        return string.Join(string.Empty, factors.Select(factor => factor.RawSiteswap()));
    }

    public override IReadOnlyList<int> BallNumsInHands()
    {
        if (!IsValid())
        {
            throw new InvalidOperationException("Siteswap must be valid.");
        }

        const int maxHeight = 36;

        int restBall = BallNum();
        int handNo = 0;
        var initialBallNums = new List<int>() { 0, 0 };
        var ballNumInHands = new List<int>() { 0, 0 };
        var activeBallss = new List<List<int>>();

        foreach (var ballNumInHand in ballNumInHands)
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
                    var ballNum = activeBallss[i].First();
                    activeBallss[i].RemoveAt(0);
                    ballNumInHands[i] += ballNum;
                    activeBallss[i].Add(0);
                }

                var throwings = factor.Throwings.Where(t => t.Height != 0).ToList();
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

                handNo = (handNo + 1) % ballNumInHands.Count;
            }
        }

        if (loopNum >= 1000)
        {
            throw new InvalidOperationException("Couldn't determine the number of balls in hands.");
        }

        return initialBallNums;
    }

    internal static bool TryParse(SiteswapParseContext context, out AsyncSiteswap? siteswap)
    {
        siteswap = null;

        var factors = new List<AsyncSiteswapFactor>();

        while (!context.IsEoc())
        {
            if (AsyncSiteswapFactor.TryParse(context, out AsyncSiteswapFactor? factor))
            {
                factors.Add(factor!);
            }
            else
            {
                return false;
            }
        }

        siteswap = new AsyncSiteswap(factors);
        return true;
    }
}
