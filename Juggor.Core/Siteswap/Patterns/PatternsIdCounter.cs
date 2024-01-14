namespace Juggor.Core.Siteswap.Patterns;

public class PatternsIdCounter
{
    private static int idCounter = 1;

    public static int Next()
    {
        return idCounter++;
    }
}
