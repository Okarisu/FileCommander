namespace FileCommander.GUI;

using Gtk;
using static App;

public class DrawTwinPanel
{
    public static DirectoryInfo DrawPanel(ScrolledWindow window, IconView view, ListStore store, DirectoryInfo root, int focusInEvent)
    {
        window.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(store, root);

        view.SelectionMode = SelectionMode.Multiple;
        view.TextColumn = ColDisplayName;
        view.PixbufColumn = ColPixbuf;
        //view.ItemActivated += (_, args) => root = OnItemActivated(args, root, store);
        view.FocusInEvent += (_, _) => FocusedPanel = focusInEvent;

        window.Add(view);

        return root;
    }
}