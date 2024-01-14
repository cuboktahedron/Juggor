using System.Collections.ObjectModel;
using Juggor.Core.Siteswap.Patterns;

namespace Juggor.Game.Menu;

public partial class Pattern : MenuButton
{
    public event EventHandler<PatternChangedEventArgs>? OnPatternChanged;

    private readonly Dictionary<int, PatternsItem> idToElement = new();

    public override void _Ready()
    {
        GD.Print("Start loading pattern.jgr");

        using var fs = new FileStream(
            "patterns/patterns.jgr", FileMode.Open, System.IO.FileAccess.Read);
        using var sr = new StreamReader(fs);

        var loader = new PatternLoader();
        var result = loader.Load(sr);
        if (result.IsSucceeded)
        {
            CreateMenu(
                GetPopup(),
                new ReadOnlyCollection<IPatternsElement>(result.SuccessValue));
        }
        else
        {
            GD.PushError(result.ErrorValue);
        }
    }

    private void CreateMenu(PopupMenu popupMenu, IReadOnlyList<IPatternsElement> elements)
    {
        foreach (var element in elements)
        {
            if (element is PatternsItem item)
            {
                popupMenu.AddItem($"{item.Siteswap.RawSiteswap()} : {item.Name}", item.Id);
                idToElement[element.Id] = item;
            }
            else if (element is PatternsGroup group)
            {
                var subPopupMenu = new PopupMenu
                {
                    Name = group.Name,
                };

                CreateMenu(subPopupMenu, group.Elements);
                popupMenu.AddChild(subPopupMenu);
                popupMenu.AddSubmenuItem(group.Name, group.Name);
            }
        }

        popupMenu.IdPressed += OnItemPressed;
    }

    private void OnItemPressed(long id)
    {
        if (idToElement.TryGetValue((int)id, out PatternsItem? item))
        {
            OnPatternChanged?.Invoke(this, new PatternChangedEventArgs(item));
        }
    }

    public class PatternChangedEventArgs
    {
        public PatternChangedEventArgs(PatternsItem item)
        {
            this.Item = item;
        }

        public PatternsItem Item { get; }
    }
}
