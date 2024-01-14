using FluentAssertions;

namespace Juggor.Core.Siteswap;

public class SiteSwapTest
{
    [Fact]
    public void TryParse_succeeded_if_siteswap_is_parsable()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("0"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("3"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("Z"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("345"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext(" 3 4 5 "), out _).Should().BeTrue();
        Siteswap.TryParse(
            new SiteswapParseContext("123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"), out _)
            .Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("[34]20"), out _).Should().BeTrue();

        // sync siteswap
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)(2, 4)"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(2x, 4x)"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(2x, 4x)"), out _).Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2x)"), out _).Should().BeTrue();
    }

    [Fact]
    public void TryParse_failed_if_siteswap_is_not_parsable()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("$"), out _).Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("[]"), out _).Should().BeFalse();

        // sync siteswap
        Siteswap.TryParse(new SiteswapParseContext("(3, 3)"), out _).Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("(4, !)"), out _).Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("([], 4)"), out _).Should().BeFalse();
    }

    [Fact]
    public void IsValid_returns_true_if_siteswap_is_valid()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("0"), out Siteswap? _0);
        _0!.IsValid().Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("[34]20"), out Siteswap? _b43b20);
        _b43b20!.IsValid().Should().BeTrue();

        // sync siteswap
        Siteswap.TryParse(new SiteswapParseContext("(4, 4)"), out Siteswap? _p44p);
        _p44p!.IsValid().Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(4x, 4x)"), out Siteswap? _p4x4xp);
        _p4x4xp!.IsValid().Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(2x, 4)(4, 2x)"), out Siteswap? _p2x4pp42x);
        _p2x4pp42x!.IsValid().Should().BeTrue();
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2x)"), out Siteswap? _p24ppb44xb2xp);
        _p24ppb44xb2xp!.IsValid().Should().BeTrue();
    }

    [Fact]
    public void IsValid_returns_false_if_siteswap_is_invalid()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("223"), out Siteswap? _223);
        _223!.IsValid().Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("54321"), out Siteswap? _54321);
        _54321!.IsValid().Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("[3][2]"), out Siteswap? _b3bb2b);
        _b3bb2b!.IsValid().Should().BeFalse();

        // sync siteswap
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)(2, 2)"), out Siteswap? _p24pp22p);
        _p24pp22p!.IsValid().Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)(4, 2)"), out Siteswap? _p24pp42p);
        _p24pp42p!.IsValid().Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("(4, 4x)"), out Siteswap? _p44xp);
        _p44xp!.IsValid().Should().BeFalse();
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2)"), out Siteswap? _p24ppb44xb2p);
        _p24ppb44xb2p!.IsValid().Should().BeFalse();
    }

    [Fact]
    public void BallNum_returns_ball_num()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("3"), out Siteswap? _3);
        _3!.BallNum().Should().Be(3);
        Siteswap.TryParse(new SiteswapParseContext("12345"), out Siteswap? _12345);
        _12345!.BallNum().Should().Be(3);
        Siteswap.TryParse(new SiteswapParseContext("53"), out Siteswap? _53);
        _53!.BallNum().Should().Be(4);
        Siteswap.TryParse(new SiteswapParseContext("[34]20"), out Siteswap? _b34b20);
        _b34b20!.BallNum().Should().Be(3);

        // sync siteswap
        Siteswap.TryParse(new SiteswapParseContext("(4, 4)"), out Siteswap? _p44p);
        _p44p!.BallNum().Should().Be(4);
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)"), out Siteswap? _p24p);
        _p24p!.BallNum().Should().Be(3);
        Siteswap.TryParse(new SiteswapParseContext("(4x, 4x)"), out Siteswap? _p4x4xp);
        _p4x4xp!.BallNum().Should().Be(4);
        Siteswap.TryParse(new SiteswapParseContext("(2x, 4)(4, 2x)"), out Siteswap? _p2x4pp42x);
        _p2x4pp42x!.BallNum().Should().Be(3);
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2x)"), out Siteswap? _p24ppb44xb2xp);
        _p24ppb44xb2xp!.BallNum().Should().Be(4);
    }

    [Fact]
    public void BallNum_returns_negative_value_if_pattern_is_invalid()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("223"), out Siteswap? _223);
        _223!.BallNum().Should().BeLessThan(0);
        Siteswap.TryParse(new SiteswapParseContext("[3][2]"), out Siteswap? _b3bb2b);
        _b3bb2b!.BallNum().Should().BeLessThan(0);

        // sync siteswap
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)(2, 2)"), out Siteswap? _p24pp22p);
        _p24pp22p!.BallNum().Should().BeLessThan(0);
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)(4, 2)"), out Siteswap? _p24pp42p);
        _p24pp42p!.BallNum().Should().BeLessThan(0);
        Siteswap.TryParse(new SiteswapParseContext("(4, 4x)"), out Siteswap? _p44xp);
        _p44xp!.BallNum().Should().BeLessThan(0);
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2)"), out Siteswap? _p24ppb44xb2p);
        _p24ppb44xb2p!.BallNum().Should().BeLessThan(0);
    }

    [Fact]
    public void GetBallNumsInHands_returns_ball_nums()
    {
        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("0"), out Siteswap? _0);
        var ballNums_0 = _0!.BallNumsInHands();
        ballNums_0.Should().BeEquivalentTo(new List<int>() { 0, 0 });
        Siteswap.TryParse(new SiteswapParseContext("3"), out Siteswap? _3);
        var ballNums_3 = _3!.BallNumsInHands();
        ballNums_3.Should().BeEquivalentTo(new List<int>() { 2, 1 });
        Siteswap.TryParse(new SiteswapParseContext("[34]1"), out Siteswap? _b43b1);
        var ballNums_b43b1 = _b43b1!.BallNumsInHands();
        ballNums_b43b1.Should().BeEquivalentTo(new List<int>() { 3, 1 });

        // async siteswap
        Siteswap.TryParse(new SiteswapParseContext("(2x, 4)(4, 2x)"), out Siteswap? _p2x4pp42x);
        var ballNums_p2x4pp42x = _p2x4pp42x!.BallNumsInHands();
        ballNums_p2x4pp42x.Should().BeEquivalentTo(new List<int>() { 2, 1 });
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2x)"), out Siteswap? _p24ppb44xb2xp);
        var ballNums_p24ppb44xb2xp = _p24ppb44xb2xp!.BallNumsInHands();
        ballNums_p24ppb44xb2xp.Should().BeEquivalentTo(new List<int>() { 2, 2 });
    }
}

