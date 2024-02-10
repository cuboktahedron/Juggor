using System.Diagnostics;
using Juggor.Core.Siteswap;
using Juggor.Core.Siteswap.Patterns;
using Juggor.Core.Style;
using Juggor.Game.Menu;
using Juggor.Serilog;
using Serilog;
using Serilog.Events;
using static Juggor.Game.Menu.Pattern;

namespace Juggor.Game;

public partial class Game : Node2D
{
    private JuggleMain? juggleMain;

    private PatternsItem? patternsItem;

    private Juggor.Game.Menu.Environment? environmentMenu;

    public Game()
    {
        InitLog();
    }

    public override void _Ready()
    {
        var menuBar = GetNode("CanvasLayer/Menu/MenuBar");
        var pattern = menuBar.GetNode<Pattern>("Pattern");
        environmentMenu = menuBar.GetNode<Menu.Environment>("Environment");

        pattern.OnPatternChanged += OnPatternsItemChanged;

        environmentMenu.OnTempoChanged += OnTempoChanged;
        environmentMenu.OnGravityChanged += OnGravityChanged;
        environmentMenu.OnMirrorChanged += OnMirrorChanged;

        Siteswap.TryParse(new SiteswapParseContext("3"), out Siteswap? siteswap);
        patternsItem = new PatternsItem("3 ball cascade", siteswap!);

        ResetJuggleMain();
    }

    private static void InitLog()
    {
#if DEBUG
        LoggingLevelSwitches.JuggorLevelSwitch.MinimumLevel = LogEventLevel.Debug;
        LoggingLevelSwitches.JuggorCoreLevelSwitch.MinimumLevel = LogEventLevel.Debug;
#endif
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Juggor", LoggingLevelSwitches.JuggorLevelSwitch)
            .MinimumLevel.Override("Juggor.Core", LoggingLevelSwitches.JuggorCoreLevelSwitch)
            .WriteTo.Godot()
            .WriteTo.File(
                "logs/log-.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 90)
            .CreateLogger();
    }

    private void OnTempoChanged(float tempo)
    {
        EnvironmentSettings.Settings.TempoRate = tempo;

        ResetJuggleMain();
    }

    private void OnGravityChanged(float gravity)
    {
        EnvironmentSettings.Settings.GravityRate = gravity;

        ResetJuggleMain();
    }

    private void OnPatternsItemChanged(object? sender, PatternChangedEventArgs args)
    {
        Debug.Assert(environmentMenu != null, $"{nameof(environmentMenu)} is required.");

        PatternsItem item = args.Item;
        environmentMenu.ChangePatternItem(item);

        EnvironmentSettings.Settings.TempoRate = item.TempoRate;
        EnvironmentSettings.Settings.GravityRate = item.GravityRate;

        this.patternsItem = args.Item;
        ResetJuggleMain();
    }

    private void OnMirrorChanged(bool isMirror)
    {
        EnvironmentSettings.Settings.IsMirror = isMirror;

        ResetJuggleMain();
    }

    private void ResetJuggleMain()
    {
        if (juggleMain != null)
        {
            RemoveChild(juggleMain);
        }

        var juggle = GD.Load<PackedScene>("res://scenes/Game/JuggleMain.tscn");
        juggleMain = juggle.Instantiate<JuggleMain>();
        AddChild(juggleMain);

        juggleMain.Setup();
        juggleMain.Start(patternsItem!);
    }
}
