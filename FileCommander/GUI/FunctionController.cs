using System.ComponentModel;
using static FileCommander.GUI.InputDialogWindow;

namespace FileCommander.GUI;

using System;
using System.IO;
using Gtk;
using static App;

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
        Refresh();
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
        new InputDialogWindow("New folder");

        var path = GetPath();
        NullPath();

        var window = App.GetFocusedWindow();
        var root = window == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), path);
        Directory.CreateDirectory(newDirectoryPath);
        Refresh();
    }

    public static void OnCopyClicked(Object sender, EventArgs e)
    {
        var window = GetFocusedWindow();
        var selection = GetSelectedItems(window);

        //new InputDialogWindow("Copy location");
        //var path = GetPath();
        //NullPath();

        var root = window == 1 ? LeftRoot : RightRoot;
        var contests = Directory.EnumerateFileSystemEntries(root.ToString()).ToList();
        foreach (var item in selection)
        {
            var movable = contests.Find(x => x == item.ToString());
            Console.WriteLine(movable);
            
            /*
            var source = Path.Combine(root.ToString(), item.ToString());
            var destination = Path.Combine(path.ToString(), item.ToString());
            File.Copy(source, destination);*/
        }

        
        Refresh();
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

    private static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}