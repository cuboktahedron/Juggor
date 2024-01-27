namespace Juggor.Core.Siteswap.Patterns;

public interface IPatternsElement
{
    public int Id { get; }

    public PatternsElementType ElementType { get; }

    string Name { get; }
}
