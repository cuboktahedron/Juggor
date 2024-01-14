using FluentAssertions;

namespace Juggor.Core.Siteswap;

public class SiteswapConductorTest
{
    [Fact]
    public void Next_returns_next_step_with_async_single_odd()
    {
        Siteswap.TryParse(new SiteswapParseContext("345"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);

        var step = conductor.Next();
        var hand0 = step.Hand(0);
        var hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(3, true));
        hand1.Any().Should().BeFalse();

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0.Any().Should().BeFalse();
        hand1[0].Should().Be(new ThrowingData(4, false));

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(5, true));
        hand1.Any().Should().BeFalse();

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0.Any().Should().BeFalse();
        hand1[0].Should().Be(new ThrowingData(3, true));
    }

    [Fact]
    public void Next_returns_next_step_with_async_single_even()
    {
        Siteswap.TryParse(new SiteswapParseContext("53"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);

        var step = conductor.Next();
        var hand0 = step.Hand(0);
        var hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(5, true));
        hand1.Any().Should().BeFalse();

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0.Any().Should().BeFalse();
        hand1[0].Should().Be(new ThrowingData(3, true));

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(5, true));
        hand1.Any().Should().BeFalse();

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0.Any().Should().BeFalse();
        hand1[0].Should().Be(new ThrowingData(3, true));
    }

    [Fact]
    public void Next_returns_next_step_with_async_multi()
    {
        Siteswap.TryParse(new SiteswapParseContext("23[45]01"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);

        var step = conductor.Next();
        var hand0 = step.Hand(0);
        var hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(2, false));
        hand1.Any().Should().BeFalse();

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0.Any().Should().BeFalse();
        hand1[0].Should().Be(new ThrowingData(3, true));

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(4, false));
        hand0[1].Should().Be(new ThrowingData(5, true));
        hand1.Any().Should().BeFalse();

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0.Any().Should().BeFalse();
        hand1[0].Should().Be(new ThrowingData(0, false));

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(1, true));
        hand1.Any().Should().BeFalse();
    }

    [Fact]
    public void Next_returns_next_step_with_sync_single()
    {
        Siteswap.TryParse(new SiteswapParseContext("(4, 2x)(2x, 4)"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);

        var step = conductor.Next();
        var hand0 = step.Hand(0);
        var hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(4, false));
        hand1[0].Should().Be(new ThrowingData(2, true));

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(2, true));
        hand1[0].Should().Be(new ThrowingData(4, false));
    }

    [Fact]
    public void Next_returns_next_step_with_sync_multi()
    {
        Siteswap.TryParse(new SiteswapParseContext("(2, 4)([44x], 2x)"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);

        var step = conductor.Next();
        var hand0 = step.Hand(0);
        var hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(2, false));
        hand1[0].Should().Be(new ThrowingData(4, false));

        step = conductor.Next();
        hand0 = step.Hand(0);
        hand1 = step.Hand(1);
        hand0[0].Should().Be(new ThrowingData(4, false));
        hand0[1].Should().Be(new ThrowingData(4, true));
        hand1[0].Should().Be(new ThrowingData(2, true));
    }

    [Fact]
    public void IsSynchronous_returns_false_is_siteswap_is_asynchronous()
    {
        Siteswap.TryParse(new SiteswapParseContext("345"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);
        conductor.IsSynchronous.Should().BeFalse();
    }

    [Fact]
    public void IsSynchronous_returns_true_is_siteswap_is_synchronous()
    {
        Siteswap.TryParse(new SiteswapParseContext("(4, 4)"), out Siteswap? ss);
        var conductor = new SiteswapConductor(ss!);
        conductor.IsSynchronous.Should().BeTrue();
    }
}
