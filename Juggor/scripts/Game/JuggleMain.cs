using System.Diagnostics;
using Juggor.Core.Siteswap;
using Juggor.Core.Siteswap.Patterns;
using Serilog;
using static Juggor.Game.AutoPlayer;

namespace Juggor.Game;

public partial class JuggleMain : Node
{
    private static readonly ILogger Logger = Log.ForContext<JuggleMain>();

    private readonly List<Hand> hands = new();

    private CameraController? cameraController;

    private AutoPlayer autoPlayer = null!;

    private PatternsItem? patternsItem;

    public override void _Ready()
    {
        autoPlayer = GetNode<AutoPlayer>("AutoPlayer");
        autoPlayer.OnStepProgressed += ProgressStep;
    }

    public void Setup(CameraController cameraController, PatternsItem patternsItem)
    {
        this.cameraController = cameraController;
        this.patternsItem = patternsItem;
        cameraController.OnReverTotHomeView += OnRevertCameraToHomeView;

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

        InitCamera();
    }

    public void Start()
    {
        Debug.Assert(patternsItem != null, $"{nameof(patternsItem)} is required.");

        Logger.Information(EnvironmentSettings.Settings.ToString());

        var ballNums = patternsItem.Siteswap.BallNumsInHands();

        for (int i = 0; i < hands.Count; i++)
        {
            int handNo = (i + (EnvironmentSettings.Settings.IsMirror ? 1 : 0)) % 2;
            hands[handNo].InitBalls(ballNums[i]);
        }

        var conductor = new SiteswapConductor(patternsItem.Siteswap);
        autoPlayer.Play((int)EnvironmentSettings.Settings.Tpm, conductor);

        foreach (var (hand, i) in hands.Select((item, i) => (item, i)))
        {
            hand.ChangeThrowStyle(
                patternsItem.ThrowStyle,
                patternsItem.Siteswap.IsSynchronous,
                i == 0);
        }
    }

    private void OnRevertCameraToHomeView()
    {
        InitCamera();
    }

    private void InitCamera()
    {
        Debug.Assert(cameraController != null, $"{nameof(cameraController)} is required.");
        Debug.Assert(patternsItem != null, $"{nameof(patternsItem)} is required.");

        int maxThrowHeight = patternsItem.Siteswap
            .ToThrowings()
            .SelectMany(x => x.SelectMany(y => y.Select(z => z.Height)))
            .DefaultIfEmpty()
            .Max();
        float timeHMax = (maxThrowHeight - 1f) / 2f / EnvironmentSettings.Settings.TempoRate / 2f;
        int maxHandYMax = (int)patternsItem.ThrowStyle.ThrowCatchPoints
            .Max(x => Math.Max(x.CatchPt.Y, x.ThrowPt.Y));
        maxHandYMax = (int)(maxHandYMax * -EnvironmentSettings.Settings.HandMovingScale.Y);
        float gravity = EnvironmentSettings.Settings.Gravity;
        int maxBallYMax = (int)(0.5f * gravity * timeHMax * timeHMax);
        long marginBottom = 100;
        float heightToView = maxHandYMax + maxBallYMax + marginBottom;

        Logger.Debug($"maxThrowHeight={maxThrowHeight}");
        Logger.Debug($"timeHMax={timeHMax}");
        Logger.Debug($"maxHandYMax={maxHandYMax}");
        Logger.Debug($"maxBallYMax={maxBallYMax}");
        Logger.Debug($"maxHeight={heightToView}");

        cameraController.AdjustCamera(new Vector2(0, heightToView));
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
