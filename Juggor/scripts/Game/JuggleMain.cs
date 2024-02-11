using Juggor.Core.Siteswap;
using Juggor.Core.Siteswap.Patterns;
using Serilog;
using static Juggor.Game.AutoPlayer;

namespace Juggor.Game;

public partial class JuggleMain : Node
{
    private static readonly ILogger Logger = Log.ForContext<JuggleMain>();

    private readonly List<Hand> hands = new();

    private AutoPlayer autoPlayer = null!;

    public override void _Ready()
    {
        autoPlayer = GetNode<AutoPlayer>("AutoPlayer");
        autoPlayer.OnStepProgressed += ProgressStep;
    }

    public void Setup()
    {
        var juggleArea = GetNode<Area2D>("JuggleArea");
        juggleArea.Gravity = (float)EnvironmentSettings.Settings.Gravity;

        var handPositions = new List<Vector2>()
        {
            new(450, 548),
            new(702, 548),
        };

        var handScene = GD.Load<PackedScene>("res://scenes/Game/Hand.tscn");
        for (int handNo = 0; handNo < 2; handNo++)
        {
            var hand = handScene.Instantiate<Hand>();
            hand.JuggleField = this;
            hand.Position = handPositions[handNo];
            hand.Name = $"Hand{handNo}";
            hands.Add(hand);
            AddChild(hand);
        }
    }

    public void Start(PatternsItem item)
    {
        Logger.Information(EnvironmentSettings.Settings.ToString());
        Logger.Information(item.ToString());

        var ballNums = item.Siteswap.BallNumsInHands();

        for (int i = 0; i < hands.Count; i++)
        {
            int handNo = (i + (EnvironmentSettings.Settings.IsMirror ? 1 : 0)) % 2;
            hands[handNo].InitBalls(ballNums[i]);
        }

        var conductor = new SiteswapConductor(item.Siteswap);
        autoPlayer.Play((int)EnvironmentSettings.Settings.Tpm, conductor);

        foreach (var (hand, i) in hands.Select((item, i) => (item, i)))
        {
            hand.ChangeThrowStyle(item.ThrowStyle, item.Siteswap.IsSynchronous, i == 0);
        }
    }

    private void ProgressStep(object? sender, StepProgressedEventArgs e)
    {
        var step = e.ThrowingStep;

        for (int i = 0; i < step.HandNum; i++)
        {
            foreach (var throwing in step.Hand(i))
            {
                int handNo = (i + (EnvironmentSettings.Settings.IsMirror ? 1 : 0)) % 2;

                if (throwing.IsCross)
                {
                    hands[handNo].Throw(hands[(handNo + 1) % 2], throwing.Height);
                }
                else
                {
                    hands[handNo].Throw(hands[handNo], throwing.Height);
                }
            }
        }
    }
}
