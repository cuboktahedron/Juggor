
namespace Juggor.Game.Menu;

public partial class Environment : MenuButton
{
    [Signal]
    public delegate void OnTempoChangedEventHandler(float tempo);

    [Signal]
    public delegate void OnGravityChangedEventHandler(float gravity);

    const int MENU_ID_TEMPO_0_5 = 0;
    const int MENU_ID_TEMPO_1 = 1;
    const int MENU_ID_TEMPO_2 = 2;
    const int MENU_ID_TEMPO_3 = 3;

    const int MENU_ID_GRAVITY_0_5 = 10;
    const int MENU_ID_GRAVITY_1 = 11;
    const int MENU_ID_GRAVITY_2 = 12;
    const int MENU_ID_GRAVITY_3 = 13;

    public override void _Ready()
    {
        var popup = GetPopup();
        popup.AddSubmenuItem("Tempo", "Tempo");

        var tempoMenu = new PopupMenu
        {
            Name = "Tempo"
        };

        tempoMenu.AddItem("x 0.5", MENU_ID_TEMPO_0_5);
        tempoMenu.AddItem("x 1", MENU_ID_TEMPO_1);
        tempoMenu.AddItem("x 2", MENU_ID_TEMPO_2);
        tempoMenu.AddItem("x 3", MENU_ID_TEMPO_3);
        popup.AddChild(tempoMenu);

        tempoMenu.IdPressed += OnItemPressed;

        popup.AddSubmenuItem("Gravity", "Gravity");

        var gravityMenu = new PopupMenu
        {
            Name = "Gravity"
        };

        gravityMenu.AddItem("x 0.5", MENU_ID_GRAVITY_0_5);
        gravityMenu.AddItem("x 1", MENU_ID_GRAVITY_1);
        gravityMenu.AddItem("x 2", MENU_ID_GRAVITY_2);
        gravityMenu.AddItem("x 3", MENU_ID_GRAVITY_3);
        popup.AddChild(gravityMenu);

        gravityMenu.IdPressed += OnItemPressed;
    }

    private void OnItemPressed(long id)
    {
        if (id == MENU_ID_TEMPO_0_5)
        {
            EmitSignal(SignalName.OnTempoChanged, 0.5);
        }
        else if (id == MENU_ID_TEMPO_1)
        {
            EmitSignal(SignalName.OnTempoChanged, 1.0);
        }
        else if (id == MENU_ID_TEMPO_2)
        {
            EmitSignal(SignalName.OnTempoChanged, 2.0);
        }
        else if (id == MENU_ID_TEMPO_3)
        {
            EmitSignal(SignalName.OnTempoChanged, 3.0);
        }
        else if (id == MENU_ID_GRAVITY_0_5)
        {
            EmitSignal(SignalName.OnGravityChanged, 0.5);
        }
        else if (id == MENU_ID_GRAVITY_1)
        {
            EmitSignal(SignalName.OnGravityChanged, 1.0);
        }
        else if (id == MENU_ID_GRAVITY_2)
        {
            EmitSignal(SignalName.OnGravityChanged, 2.0);
        }
        else if (id == MENU_ID_GRAVITY_3)
        {
            EmitSignal(SignalName.OnGravityChanged, 3.0);
        }
    }
}
