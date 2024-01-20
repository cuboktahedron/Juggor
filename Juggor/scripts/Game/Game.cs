global using Godot;
using Juggor.Core.Siteswap;
using Juggor.Core.Siteswap.Patterns;
using Juggor.Core.Style;
using Juggor.Game.Menu;
using static Juggor.Game.Menu.Pattern;

namespace Juggor.Game;

public partial class Game : Node2D
{
    private JuggleMain? juggleMain;

    private PatternsItem? patternsItem;

    public override void _Ready()
    {
        var menuBar = GetNode("CanvasLayer/Menu/MenuBar");
        var environment = menuBar.GetNode<Menu.Environment>("Environment");
        var pattern = menuBar.GetNode<Pattern>("Pattern");

        environment.OnTempoChanged += OnTempoChanged;
        environment.OnGravityChanged += OnGravityChanged;
        environment.OnMirrorChanged += OnMirrorChanged;

        pattern.OnPatternChanged += OnPatternsItemChanged;

        Siteswap.TryParse(new SiteswapParseContext("3"), out Siteswap? siteswap);
        patternsItem = new PatternsItem("3 ball cascade", siteswap!, ThrowStyle.Normal);

        ResetJuggleMain();
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
