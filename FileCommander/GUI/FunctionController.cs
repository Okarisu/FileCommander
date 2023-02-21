namespace FileCommander.GUI;

using System;
using System.IO;
using Gtk;
using static IconApp;

public class FunctionController
{
    #region Navigation

    public static DirectoryInfo OnHomeClicked(object? sender, EventArgs e, DirectoryInfo root, ListStore store)
    {
        root = new DirectoryInfo(Environment.GetFolderPath(
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
    public static void OnRefreshClicked(Object sender, EventArgs e)
    {
        //Není potřeba znovu zapsat DirInfo do rootu?
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
    public static void OnBackClicked(Object sender, EventArgs e)
    {
        //TODO logging historie cest? - Queue<Path>, to bude blivajz
    }
    public static void OnForwardClicked(Object sender, EventArgs e)
    {
        //viz výše
    }
    public static void OnUndoClicked(Object sender, EventArgs e)
    {
        //TODO logging provedených akcí - command pattern
        //https://stackoverflow.com/questions/3448943/best-design-pattern-for-undo-feature
    }
    public static void OnRedoClicked(Object sender, EventArgs e)
    {
        //viz výše
    }

    #endregion

    public static void OnNewClicked(Object sender, EventArgs e)
    {
        Application.Init();
        new DialogWindow("New Folder");
        Application.Run();


        Console.WriteLine(InputDialogWindow.path);
    }

    public static void OnCopyClicked(Object sender, EventArgs e)
    {
    }

    public static void OnMoveClicked(Object sender, EventArgs e)
    {
    }

    public static void OnDeleteClicked(Object sender, EventArgs e)
    {
    }

    public static void OnRenameClicked(Object sender, EventArgs e)
    {
    }

    public static void OnExtractClicked(Object sender, EventArgs e)
    {
    }

    public static void OnCompressClicked(Object sender, EventArgs e)
    {
    }
}