// ReSharper disable ObjectCreationAsStatement

namespace FileCommander.GUI;

using System;
using System.IO;
using Gtk;
using static App;
using static PromptPathInputDialogWindow;

public abstract class FunctionController
{
    #region Navigation

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

    public static void OnBackClicked(object sender, EventArgs e)
    {
        //TODO logging historie cest? - Queue<Path>, to bude blivajz
    }

    public static void OnForwardClicked(object sender, EventArgs e)
    {
        //viz výše
    }

    public static void OnUndoClicked(object sender, EventArgs e)
    {
        //TODO logging provedených akcí - command pattern
        //https://stackoverflow.com/questions/3448943/best-design-pattern-for-undo-feature
    }

    public static void OnRedoClicked(object sender, EventArgs e)
    {
        //viz výše
    }

    #endregion


    public static (string path, bool cancel, bool addSuffix) GetPath(string dialogTitle, bool promptSuffix)
    {
        new PromptPathInputDialogWindow(dialogTitle, promptSuffix);
        var path = PromptPathInputDialogWindow.GetPath();
        NullPath();

        return (path.path, path.cancel, path.addSuffix);
    }

    public static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}