namespace FileCommander.GUI;

using static InputDialogWindow;
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

    private static string GetPath(string dialogTitle)
    {
        new InputDialogWindow(dialogTitle);
        var path = InputDialogWindow.GetPath();
        NullPath();
        return path;
    }

    public static void OnNewClicked(object sender, EventArgs e)
    {
        var path = GetPath("New folder");

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), path);
        Directory.CreateDirectory(newDirectoryPath);
        Refresh();
    }

    public static void OnCopyClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.files == null) return;

        var path = GetPath("Copy to...");

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;


        foreach (var item in items.files)
        {
            if (item!.IsDirectory)
            {
                //https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
            }
            else
            {
                File.Copy(item.Path, Path.Combine(path, item.Filename));
            }
        }

        Refresh();
    }

    public static void OnMoveClicked(object sender, EventArgs e)
    {
    }

    public static void OnDeleteClicked(object sender, EventArgs e)
    {
    }

    public static void OnRenameClicked(object sender, EventArgs e)
    {
    }

    public static void OnExtractClicked(object sender, EventArgs e)
    {
    }

    public static void OnCompressClicked(object sender, EventArgs e)
    {
    }

    private static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}