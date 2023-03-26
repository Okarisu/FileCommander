// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace FileCommander.GUI;

using Toolbars;
using System;
using System.IO;
using Gtk;

public class UI : Window
{
    private const int ColPath = 0;
    public const int ColDisplayName = 1;
    public const int ColPixbuf = 2;
    private const int ColIsDirectory = 3;


    public static DirectoryInfo LeftRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
    public static DirectoryInfo RightRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

    private static readonly Gdk.Pixbuf FileIcon = GetIcon(Stock.File), DirIcon = GetIcon(Stock.Directory);

    public static ListStore LeftStore = CreateStore(), RightStore = CreateStore();

    public static ScrolledWindow LeftScrolledWindow = new();
    public static ScrolledWindow RightScrolledWindow = new();

    public static IconView LeftIconView = new(LeftStore);
    public static IconView RightIconView = new(RightStore);

    public static int FocusedPanel;

    public static Label LeftRootLabel = new("Current directory: " + LeftRoot);
    public static Label RightRootLabel = new("Current directory: " + RightRoot);

    public UI() : base("File Commander")
    {
        SetDefaultSize(1280, 720);
        Maximize();
        SetPosition(WindowPosition.Center);
        DeleteEvent += (_, _) => Application.Quit();

        //Vytvoření kontejneru vnitřního obsahu okna
        VBox windowVerticalBox = new VBox(false, 0);
        Add(windowVerticalBox);

        var toolbar = ToolbarMain.DrawToolbar();
        windowVerticalBox.PackStart(toolbar, false, true, 0);
/*
        var compactTwinBox = new HBox();
        windowVerticalBox.PackStart(compactTwinBox, true, true, 0);

        var leftTwinPanel = new VBox();
        var leftTwinToolbox = TwinToolboxes.TwinToolboxLeft.DrawLeftToolbox();
        leftTwinPanel.PackStart(leftTwinToolbox, true, true, 0);
        leftTwinPanel.PackStart(LeftScrolledWindow, true, true, 0);
        compactTwinBox.PackStart(leftTwinPanel, true, true, 0);

        var rightTwinPanel = new VBox();
        var rightTwinToolbox = TwinToolboxes.TwinToolboxRight.DrawRightToolbox();
        rightTwinPanel.PackStart(rightTwinToolbox, true, true, 0);
        rightTwinPanel.PackStart(RightScrolledWindow, true, true, 0);
        compactTwinBox.PackStart(rightTwinPanel, true, true, 0);

        ShowAll();*/


        
        HBox twinPanelToolbox = new HBox();

        HBox leftTwinToolbox = new HBox();
        var leftToolbar = ToolbarLeft.DrawLeftToolbar();
        leftTwinToolbox.PackStart(leftToolbar, false, true, 0);
        var leftDisksList = new HBox();
        leftTwinToolbox.PackStart(leftDisksList, false, true, 0);

        var leftCompactBox = new VBox();
        leftCompactBox.PackStart(leftTwinToolbox, false, true, 0);
        leftCompactBox.PackStart(LeftRootLabel, false, true, 0);

        twinPanelToolbox.PackStart(leftCompactBox, true, true, 0);

        twinPanelToolbox.PackStart(new Separator(Orientation.Vertical), false, false, 0);

        HBox rightTwinToolbox = new HBox();
        var rightPanelBar = ToolbarRight.DrawRightToolbar();
        rightTwinToolbox.PackStart(rightPanelBar, false, true, 0);
        var rightDisksList = new HBox();
        rightTwinToolbox.PackStart(rightDisksList, false, true, 0);

        var rightCompactBox = new VBox();
        rightCompactBox.PackStart(rightTwinToolbox, false, true, 0);
        rightCompactBox.PackStart(RightRootLabel, false, false, 0);

        twinPanelToolbox.PackStart(rightCompactBox, true, true, 0);

        windowVerticalBox.PackStart(twinPanelToolbox, false, true, 0);

        TwinPanels.LeftTwinPanel.DrawLeftPanel();
        TwinPanels.RightTwinPanel.DrawRightPanel();

        //LeftRoot = DrawTwinPanel.DrawPanel(LeftScrolledWindow, LeftIconView, LeftStore, LeftRoot, 1);
        //RightRoot = DrawTwinPanel.DrawPanel(RightScrolledWindow, RightIconView, RightStore, RightRoot, 2);

        HBox twinPanelsBox = new HBox(false, 0);
        twinPanelsBox.PackStart(LeftScrolledWindow, true, true, 0);
        twinPanelsBox.PackStart(new Separator(Orientation.Vertical), false, false, 0);
        twinPanelsBox.PackStart(RightScrolledWindow, true, true, 0);

        windowVerticalBox.PackStart(twinPanelsBox, true, true, 0);
        ShowAll();
    }


    private static Gdk.Pixbuf GetIcon(string name) => IconTheme.Default.LoadIcon(name, 48, 0);

    private static ListStore CreateStore()
    {
        ListStore store = new ListStore(typeof(string),
            typeof(string), typeof(Gdk.Pixbuf), typeof(bool));

        store.SetSortColumnId(ColDisplayName, SortType.Ascending);

        return store;
    }

    public static void FillStore(ListStore store, DirectoryInfo root)
    {
        store.Clear();

        if (!root.Exists)
        {
            return;
        }

        foreach (DirectoryInfo di in root.GetDirectories())
        {
            if (!di.Name.StartsWith("."))
                store.AppendValues(di.FullName, di.Name, DirIcon, true);
        }

        foreach (FileInfo file in root.GetFiles())
        {
            if (!file.Name.StartsWith("."))
                store.AppendValues(file.FullName, file.Name, FileIcon, false);
        }
    }

    public static DirectoryInfo OnItemActivated(ItemActivatedArgs a, DirectoryInfo root, ListStore store)
    {
        store.GetIter(out var iter, a.Path);
        var path = (string) store.GetValue(iter, ColPath);
        var isDir = (bool) store.GetValue(iter, ColIsDirectory);

        if (!isDir)
            return root;

        root = new DirectoryInfo(path);
        FillStore(store, root);
        return root;
    }

    public static int GetFocusedWindow() => FocusedPanel;

    public static Item[] GetSelectedItems()
    {
        var selection = FocusedPanel == 1 ? LeftIconView.SelectedItems : RightIconView.SelectedItems;

        var store = FocusedPanel == 1 ? LeftStore : RightStore;
        var files = new Item[selection.Length];

        for (var i = 0; i < selection.Length; i += 1)
        {
            store.GetIter(out var treeIterator, selection[i]);
            files[i] = new Item(store.GetValue(treeIterator, ColPath).ToString()!,
                store.GetValue(treeIterator, ColDisplayName).ToString(),
                (bool) store.GetValue(treeIterator, ColIsDirectory));
        }

        return files!;
    }
}