using Juggor.Core.Siteswap;

namespace Juggor.Game;

public partial class AutoPlayer : Node
{
    private long frame;

    private long nextThrowFrame = long.MaxValue;

    private SiteswapConductor? conductor;

    public event EventHandler<StepProgressedEventArgs>? OnStepProgressed;

    /// <summary>
    /// Gets or sets the throw count per minute.
    /// </summary>
    public float Tpm { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        if (conductor == null)
        {
            return;
        }

        frame++;

        if (frame < nextThrowFrame)
        {
            return;
        }

        var step = conductor.Next();
        OnStepProgressed?.Invoke(this, new StepProgressedEventArgs(step));

        int syncFactor = conductor.IsSynchronous ? 2 : 1;
        nextThrowFrame += (long)(60f / Tpm * 60f) * syncFactor;
    }

    public void Play(int tpm, SiteswapConductor conductor)
    {
        frame = 0;
        Tpm = tpm;
        nextThrowFrame = (long)(60f / Tpm * 60f);

        this.conductor = conductor;
    }

    public class StepProgressedEventArgs
    {
        public StepProgressedEventArgs(ThrowingStep throwingStep)
        {
            ThrowingStep = throwingStep;
        }

        public ThrowingStep ThrowingStep { get; }
    }
}
