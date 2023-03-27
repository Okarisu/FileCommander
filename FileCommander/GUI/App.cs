// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

using System.Runtime.CompilerServices;

namespace FileCommander.GUI;


using Controllers;
using Toolbars;
using System;
using System.IO;
using Gtk;

public class App : Window
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
    
    public static Stack<DirectoryInfo> LeftHistory = new();
    public static Stack<DirectoryInfo> LeftHistoryForward = new();
    public static Stack<DirectoryInfo> RightHistory = new();
    public static Stack<DirectoryInfo> RightHistoryForward = new();

    public static int FocusedPanel;

    public static Label LeftRootLabel = new("Current directory: " + LeftRoot);
    public static Label RightRootLabel = new("Current directory: " + RightRoot);

    public App() : base("File Commander")
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
        
        HBox twinPanelToolbox = new HBox();

        HBox leftTwinToolbox = new HBox();
        var leftToolbar = ToolbarLeft.DrawLeftToolbar();
        leftTwinToolbox.PackStart(leftToolbar, false, true, 0);
        var leftDisksList = new HBox();
        leftTwinToolbox.PackStart(leftDisksList, false, true, 0);

        var leftCompactBox = new VBox();
        leftCompactBox.PackStart(leftTwinToolbox, false, true, 0);
        leftCompactBox.PackStart(LeftRootLabel, false, true, 0);

        //Ed
        twinPanelToolbox.PackStart(leftCompactBox, true, true, 0);

        twinPanelToolbox.PackStart(new Separator(Orientation.Vertical), false, false, 0);

        HBox rightTwinToolbox = new HBox();
        var rightPanelBar = ToolbarRight.DrawRightToolbar();
        rightTwinToolbox.PackStart(rightPanelBar, false, true, 0);
        var rightDisksList = new HBox();
        rightTwinToolbox.PackStart(rightDisksList, false, true, 0);

        var rightCompactBox = new VBox();
        rightCompactBox.PackStart(rightTwinToolbox, false, true, 0);
        rightCompactBox.PackStart(RightRootLabel, false, true, 0);

        //Ed
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

        //Přepis
/*
        VBox windowContainer = new VBox(false, 0);
        Add(windowContainer);
        var toolbar = ToolbarMain.DrawToolbar();
        windowContainer.PackStart(toolbar, false, true, 0);
        HBox twinPanels = new HBox();
        windowContainer.PackStart(twinPanels, false, true, 0);
        
        VBox leftTwinPanel = new VBox(false, 0);
        twinPanels.PackStart(leftTwinPanel, false, true, 0);
        twinPanels.PackStart(new Separator(Orientation.Vertical), false, false, 0);
        VBox rightTwinPanel = new VBox();
        twinPanels.PackStart(rightTwinPanel, false, true, 0);
        
        
        VBox leftTwinPanelToolbar = new VBox(false, 0);
        leftTwinPanel.PackStart(leftTwinPanelToolbar, false, true, 0);
        var leftToolbar = new Toolbar();
        leftTwinPanelToolbar.PackStart(leftToolbar, false, true, 0);
        
        leftToolbar.ToolbarStyle = ToolbarStyle.Both;

        var leftHomeButton = new ToolButton(Stock.Home);
        leftToolbar.Insert(leftHomeButton, 0);
        leftHomeButton.Clicked += (sender, args) =>
        {
            LeftRoot = NavigationController.OnHomeClicked(args, LeftStore);
            LeftRootLabel.Text = "Current directory: "+LeftRoot;
        };

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) =>
        {
            LeftRoot = NavigationController.OnUpClicked(LeftRoot, LeftStore);
            LeftRootLabel.Text = "Current directory: "+LeftRoot;
        };
        
        leftTwinPanelToolbar.PackStart(LeftRootLabel, false, true, 0);
        
        leftTwinPanel.PackStart(LeftScrolledWindow, true, true, 0);
        
        
        ShowAll();*/

/*
        var compactTwinBox = new HBox(false, 0);
        windowVerticalBox.PackStart(compactTwinBox, true, true, 0);

        LeftScrolledWindow.Add(LeftIconView);
        RightScrolledWindow.Add(RightIconView);
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

    public static DirectoryInfo OnItemActivated(ItemActivatedArgs a, DirectoryInfo root, ListStore store, Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward)
    {
        store.GetIter(out var iter, a.Path);
        var path = (string) store.GetValue(iter, ColPath);
        var isDir = (bool) store.GetValue(iter, ColIsDirectory);

        if (!isDir)
            return root;

        history.Push(root);
        historyForward.Clear();
        
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

    public static void UpdateRootLabel(Label label, DirectoryInfo root)
    {
        label.Text = "Current directory: " + root;
    }
}