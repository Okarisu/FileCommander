namespace FileCommander.GUI;

using System;
using System.IO;
using Gtk;
using static IconApp;

public class FunctionController
{
    public static void OnLeftHomeClicked(Object sender, EventArgs e)
    {
        LeftRoot = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(LeftStore, LeftRoot);
    }
    public static void OnRightHomeClicked(Object sender, EventArgs e)
    {
        RightRoot = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(RightStore, RightRoot);
    }
    public static void OnLeftUpClicked(Object sender, EventArgs e)
    {
        LeftRoot = LeftRoot.Parent;
        FillStore(LeftStore, LeftRoot);
        //leftUpButton.Sensitive = (root.FullName == "/" ? false : true);
    }
    public static void OnRightUpClicked(Object sender, EventArgs e)
    {
        RightRoot = RightRoot.Parent;
        FillStore(RightStore, RightRoot);
        //rightUpButton.Sensitive = (root.FullName == "/" ? false : true);
    }
    public static void OnRefreshClicked(Object sender, EventArgs e)
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }

    public static void OnBackClicked(Object sender, EventArgs e)
    {
        //TODO logging historie cest? - Queue<Path>, to bude blivajz
    }

    public static void OnForwardClicked(Object sender, EventArgs e)
    {
        //TODO viz výše
    }

    public static void OnUndoClicked(Object sender, EventArgs e)
    {
        //TODO logging provedených akcí
    }

    public static void OnRedoClicked(Object sender, EventArgs e)
    {
        //TODO viz výše
    }

    public static void OnNewClicked(Object sender, EventArgs e)
    {
        Application.Init();
        new InputDialogWindow("Neco");
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