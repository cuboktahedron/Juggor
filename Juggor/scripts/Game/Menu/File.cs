namespace Juggor.Game.Menu;

public partial class File : MenuButton
{
    private const int MenuIdQuit = 0;

    public override void _Ready()
    {
        GetPopup().AddItem("Quit", MenuIdQuit);

        GetPopup().IdPressed += OnItemPressed;
    }

    private void OnItemPressed(long id)
    {
        if (id == MenuIdQuit)
        {
            GetTree().Quit();
        }
    }
}
