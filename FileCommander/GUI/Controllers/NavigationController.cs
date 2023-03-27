// ReSharper disable ObjectCreationAsStatement

using System.Security.AccessControl;
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

    public static DirectoryInfo OnBackClicked(DirectoryInfo root, Stack<DirectoryInfo> history,
        Stack<DirectoryInfo> historyForward, ListStore store)
    {
        if (history.Count == 0)
            return null;

        historyForward.Push(root);
        FillStore(store, history.Peek());
        return history.Pop();
    }

    public static DirectoryInfo OnForwardClicked(DirectoryInfo root, Stack<DirectoryInfo> history,
        Stack<DirectoryInfo> historyForward, ListStore store)
    {
        if (historyForward.Count == 0)
            return null;

        history.Push(root);
        FillStore(store, historyForward.Peek());
        return historyForward.Pop();
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