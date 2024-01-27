namespace Juggor.Game.Menu;

public partial class Environment : MenuButton
{
    private const int MenuIdTempo050 = 0;
    private const int MenuIdTempo100 = 1;
    private const int MenuIdTempo200 = 2;
    private const int MenuIdTempo300 = 3;

    private const int MenuIdGravity050 = 10;
    private const int MenuIdGravity100 = 11;
    private const int MenuIdGravity200 = 12;
    private const int MenuIdGravity300 = 13;

    private const int MenuIdMirror = 20;

    [Signal]
    public delegate void OnTempoChangedEventHandler(float tempo);

    [Signal]
    public delegate void OnGravityChangedEventHandler(float gravity);

    [Signal]
    public delegate void OnMirrorChangedEventHandler(bool isMirror);

    public override void _Ready()
    {
        var popup = GetPopup();

        // Tempo Menu
        popup.AddSubmenuItem("Tempo", "Tempo");

        var tempoMenu = new PopupMenu
        {
            Name = "Tempo",
        };

        tempoMenu.AddItem("x 0.5", MenuIdTempo050);
        tempoMenu.AddItem("x 1", MenuIdTempo100);
        tempoMenu.AddItem("x 2", MenuIdTempo200);
        tempoMenu.AddItem("x 3", MenuIdTempo300);
        popup.AddChild(tempoMenu);

        tempoMenu.IdPressed += OnItemPressed;

        // Gravity Menu
        popup.AddSubmenuItem("Gravity", "Gravity");

        var gravityMenu = new PopupMenu
        {
            Name = "Gravity",
        };

        gravityMenu.AddItem("x 0.5", MenuIdGravity050);
        gravityMenu.AddItem("x 1", MenuIdGravity100);
        gravityMenu.AddItem("x 2", MenuIdGravity200);
        gravityMenu.AddItem("x 3", MenuIdGravity300);
        popup.AddChild(gravityMenu);

        gravityMenu.IdPressed += OnItemPressed;

        // Mirror Menu
        popup.AddCheckItem("Mirror", MenuIdMirror);

        popup.IdPressed += OnItemPressed;
    }

    private void OnItemPressed(long id)
    {
        if (id == MenuIdTempo050)
        {
            EmitSignal(SignalName.OnTempoChanged, 0.5);
        }
        else if (id == MenuIdTempo100)
        {
            EmitSignal(SignalName.OnTempoChanged, 1.0);
        }
        else if (id == MenuIdTempo200)
        {
            EmitSignal(SignalName.OnTempoChanged, 2.0);
        }
        else if (id == MenuIdTempo300)
        {
            EmitSignal(SignalName.OnTempoChanged, 3.0);
        }
        else if (id == MenuIdGravity050)
        {
            EmitSignal(SignalName.OnGravityChanged, 0.5);
        }
        else if (id == MenuIdGravity100)
        {
            EmitSignal(SignalName.OnGravityChanged, 1.0);
        }
        else if (id == MenuIdGravity200)
        {
            EmitSignal(SignalName.OnGravityChanged, 2.0);
        }
        else if (id == MenuIdGravity300)
        {
            EmitSignal(SignalName.OnGravityChanged, 3.0);
        }
        else if (id == MenuIdMirror)
        {
            var popup = GetPopup();
            var index = popup.GetItemIndex(MenuIdMirror);
            popup.ToggleItemChecked(index);
            bool isChecked = popup.IsItemChecked(index);
            EmitSignal(SignalName.OnMirrorChanged, isChecked);
        }
    }
}
