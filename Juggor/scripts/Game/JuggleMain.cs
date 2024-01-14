using Juggor.Core.Siteswap;
using Juggor.Core.Siteswap.Patterns;
using static Juggor.Game.AutoPlayer;

namespace Juggor.Game;

public partial class JuggleMain : Node
{
    private readonly List<Hand> hands = new();

    private AutoPlayer autoPlayer = null!;

    public override void _Ready()
    {
        autoPlayer = GetNode<AutoPlayer>("AutoPlayer");
        autoPlayer.OnStepProgressed += ProgressStep;
    }

    private void ProgressStep(object? sender, StepProgressedEventArgs e)
    {
        var step = e.ThrowingStep;
        for (int i = 0; i < step.HandNum; i++)
        {
            foreach (var throwing in step.Hand(i))
            {
                if (throwing.IsCross)
                {
                    hands[i].Throw(hands[(i + 1) % 2], throwing.Height);
                }
                else
                {
                    hands[i].Throw(hands[i], throwing.Height);
                }
            }
        }
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
            hand.juggleField = this;
            hand.Position = handPositions[handNo];
            hand.Name = $"Hand{handNo}";
            hands.Add(hand);
            AddChild(hand);
        }
    }

    public void Start(PatternsItem item)
    {
        GD.Print(EnvironmentSettings.Settings);
        GD.Print(item);

        var ballNums = item.Siteswap.BallNumsInHands();

        foreach (var (hand, ballNum) in hands.Zip(ballNums))
        {
            hand.InitBalls(ballNum);
        }

        var conductor = new SiteswapConductor(item.Siteswap);
        autoPlayer.Play((int)EnvironmentSettings.Settings.Tpm, conductor);

        foreach (var (hand, i) in hands.Select((item, i) => (item, i)))
        {
            hand.ChangeThrowStyle(item.ThrowStyle, item.Siteswap.IsSynchronous, i == 0);
        }
    }

    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev is InputEventKey eventKey)
        {
            if (!eventKey.IsPressed())
            {
                return;
            }

            switch (eventKey.Keycode)
            {
                case Key.Key0:
                    hands[0].Throw(hands[0], 0);
                    break;
                case Key.Key1:
                    hands[0].Throw(hands[1], 1);
                    break;
                case Key.Key2:
                    hands[0].Throw(hands[0], 2);
                    break;
                case Key.Key3:
                    hands[0].Throw(hands[1], 3);
                    break;
                case Key.Key4:
                    hands[0].Throw(hands[0], 4);
                    break;
                case Key.Key5:
                    hands[0].Throw(hands[1], 5);
                    break;
                case Key.Q:
                    hands[1].Throw(hands[0], 1);
                    break;
                case Key.W:
                    hands[1].Throw(hands[0], 2);
                    break;
                case Key.E:
                    hands[1].Throw(hands[1], 3);
                    break;
                case Key.R:
                    hands[1].Throw(hands[0], 4);
                    break;
                case Key.T:
                    hands[1].Throw(hands[1], 5);
                    break;
                case Key.P:
                    hands[1].Throw(hands[0], 0);
                    break;
            }
        }
    }
}
