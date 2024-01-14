namespace Juggor.Game.Menu;

public partial class File : MenuButton
{
    const int MENU_ID_QUIT = 0;

    public override void _Ready()
    {
        GetPopup().AddItem("Quit", MENU_ID_QUIT);

        GetPopup().IdPressed += OnItemPressed;
    }

    private void OnItemPressed(long id)
    {
        if (id == MENU_ID_QUIT)
        {
            GetTree().Quit();
        }
    }
}
