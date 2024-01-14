
using System.Text;
using FluentAssertions;

namespace Juggor.Core.Siteswap.Patterns;

public class SiteswapConductorTest
{
    [Fact]
    public void Load_returns_pattern_elements()
    {
        string text = @"
%Normal
{ 13,  0 }{  4, 0 }

%Reverse
{  4,  0 }{ 13, 0 }

/[ 3-ball ]

%Normal
3 3-Cascade
51	3 shower

/[ 4-ball ]

%Reverse
4 fountain
71 shower";

        IList<IPatternsElement> patternsElements;

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(text)))
        {
            ms.Position = 0;
            using (var sr = new StreamReader(ms))
            {
                var loader = new PatternLoader();
                patternsElements = loader.Load(sr).SuccessValue;
            }
        }

        var group3Ball = patternsElements[0] as PatternsGroup;
        group3Ball!.Name.Should().Be("3-ball");
        (group3Ball!.Elements[0] as PatternsItem)!.ThrowStyle
            .ToString()
            .Should()
            .Be($"Name=Normal, ThrowCatchPoints={{13, 0}}{{4, 0}}");
        (group3Ball!.Elements[0] as PatternsItem)!.Name.Should().Be("3-Cascade");
        (group3Ball!.Elements[0] as PatternsItem)!.Siteswap.RawSiteswap().Should().Be("3");
        (group3Ball!.Elements[1] as PatternsItem)!.ThrowStyle.Name.Should().Be("Normal");
        (group3Ball!.Elements[1] as PatternsItem)!.Name.Should().Be("3 shower");
        (group3Ball!.Elements[1] as PatternsItem)!.Siteswap.RawSiteswap().Should().Be("51");

        var group4Ball = patternsElements[1] as PatternsGroup;
        group4Ball!.Name.Should().Be("4-ball");
        (group4Ball!.Elements[0] as PatternsItem)!.ThrowStyle.Name.Should().Be("Reverse");
        (group4Ball!.Elements[0] as PatternsItem)!.Name.Should().Be("fountain");
        (group4Ball!.Elements[0] as PatternsItem)!.Siteswap.RawSiteswap().Should().Be("4");
        (group4Ball!.Elements[1] as PatternsItem)!.ThrowStyle
            .ToString()
            .Should()
            .Be($"Name=Reverse, ThrowCatchPoints={{4, 0}}{{13, 0}}");
        (group4Ball!.Elements[1] as PatternsItem)!.Name.Should().Be("shower");
        (group4Ball!.Elements[1] as PatternsItem)!.Siteswap.RawSiteswap().Should().Be("71");
    }

    [Fact]
    public void Load_returns_error_if_siteswap_is_not_jugglable()
    {
        string text = @"
/[ 3-ball ]

34 invalid";

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(text)))
        {
            ms.Position = 0;
            using (var sr = new StreamReader(ms))
            {
                var loader = new PatternLoader();
                string error = loader.Load(sr).ErrorValue;
                error.Should().Be("L4: \"34\" is not jugglable.");
            }
        }
    }

    [Fact]
    public void Load_returns_error_if_siteswap_is_invalid()
    {
        string text = @"
/[ 3-ball ]

$3 invalid";

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(text)))
        {
            ms.Position = 0;
            using (var sr = new StreamReader(ms))
            {
                var loader = new PatternLoader();
                string error = loader.Load(sr).ErrorValue;
                error.Should().Be("L4: \"$3\" is invalid.");
            }
        }
    }
}
