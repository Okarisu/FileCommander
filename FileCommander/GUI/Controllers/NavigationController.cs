// ReSharper disable ObjectCreationAsStatement

using Gtk;

namespace FileCommander.GUI.Controllers;

using static App;

public abstract class NavigationController
{
    public static DirectoryInfo OnHomeClicked(EventArgs e, ListStore store)
    {
        var root = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(store, root);
        return root;
    }

    public static DirectoryInfo OnUpClicked(DirectoryInfo root, ListStore store)
    {
        if (root.Parent == null)
            return root;

        FillStore(store, root.Parent);
        return root.Parent;
    }

    public static void OnRefreshClicked(object sender, EventArgs e)
    {
        Refresh();
    }

    public static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}