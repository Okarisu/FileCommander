namespace FileCommander.GUI;

using System;
using System.IO;
using Gtk;
using static IconApp;

public class FunctionController
{
    public static void OnLeftHomeClicked(Object sender, EventArgs e)
    {
        //Console.WriteLine(sender.GetHashCode());
        LeftRoot = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(LeftStore, LeftRoot);
        //upButton.Sensitive = true;
    }

    public static void OnRightHomeClicked(Object sender, EventArgs e)
    {
        //Console.WriteLine(sender.GetHashCode());
        RightRoot = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(RightStore, RightRoot);
        //upButton.Sensitive = true;
    }


    public static void OnRefreshClicked(Object sender, EventArgs e)
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }

    public static void OnBackClicked(Object sender, EventArgs e)
    {
        //TODO logging historie cest?
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
        Entry entry = new Entry();
        entry.Changed += delegate{Entry entry = (Entry) sender;
            Console.WriteLine(entry.Text);
            ;};
        
        Fixed fix = new Fixed();
        fix.Put(entry, 60, 100);
        //TODO asi nová třída pro dialokno?
        
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