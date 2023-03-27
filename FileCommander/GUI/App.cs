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

    public static Label LeftRootLabel = new("Current directory: " + LeftRoot);
    public static Label RightRootLabel = new("Current directory: " + RightRoot);
    
    public static Toolbar LeftDiskBar = new();
    public static Toolbar RightDiskBar = new();

    public static int FocusedPanel;

    public App() : base("File Commander")
    {
        SetDefaultSize(1280, 720);
        Maximize();
        SetPosition(WindowPosition.Center);
        DeleteEvent += (_, _) => Application.Quit();

        VBox windowContainer = new VBox(false, 0);
        Add(windowContainer);
        var mb = Toolbars.DrawMenu.DrawMenuBar();
        windowContainer.PackStart(mb, false, true, 0);
        var toolbar = ToolbarMain.DrawToolbar();
        windowContainer.PackStart(toolbar, false, true, 0);

        HBox twinPanelsContainer = new HBox(false, 0); //false?
        windowContainer.PackStart(twinPanelsContainer, true, true, 0);
        VBox leftTwinContainer = new VBox(false, 0);
        twinPanelsContainer.PackStart(leftTwinContainer, true, true, 0);
        twinPanelsContainer.PackStart(new Separator(Orientation.Vertical), false, false, 0);
        VBox rightTwinContainer = new VBox(false, 0);
        twinPanelsContainer.PackStart(rightTwinContainer, true, true, 0);

        TwinPanels.LeftTwinPanel.DrawLeftPanel();
        TwinPanels.RightTwinPanel.DrawRightPanel();

        //TwinPanel.DrawPanel(LeftScrolledWindow, LeftIconView, LeftStore, LeftRoot, LeftRootLabel, LeftHistory, LeftHistoryForward, 1);
        //TwinPanel.DrawPanel(RightScrolledWindow, RightIconView, RightStore, RightRoot, RightRootLabel, RightHistory, RightHistoryForward, 2);

        HBox leftTwinPanelHeader = new HBox(true, 0);
        leftTwinContainer.PackStart(leftTwinPanelHeader, false, true, 0);
        leftTwinPanelHeader.PackStart(ToolbarLeft.DrawLeftToolbar(), false, true, 0);
        leftTwinContainer.PackStart(LeftRootLabel, false, true, 0);
        leftTwinContainer.PackStart(LeftScrolledWindow, true, true, 0);

        HBox rightTwinPanelHeader = new HBox(true, 0);
        rightTwinContainer.PackStart(rightTwinPanelHeader, false, true, 0);
        rightTwinPanelHeader.PackStart(ToolbarRight.DrawRightToolbar(), false, true, 0);
        rightTwinContainer.PackStart(RightRootLabel, false, true, 0);
        rightTwinContainer.PackStart(RightScrolledWindow, true, true, 0);

        if (Disks.CheckDisksAvailable())
        {
            LeftDiskBar = Disks.DrawLeftDiskBar();
            RightDiskBar = Disks.DrawRightDiskBar();

            leftTwinPanelHeader.PackStart(LeftDiskBar, false, true, 0);
            rightTwinPanelHeader.PackStart(RightDiskBar, false, true, 0);
        }

        ShowAll();

        if (!FileCommander.Settings.GetConf("ShowMountedDrives"))
        {
            LeftDiskBar.Hide();
            RightDiskBar.Hide();
        }
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
            if (FileCommander.Settings.GetConf("ShowHiddenFiles"))
            {
                store.AppendValues(di.FullName, di.Name, DirIcon, true);
            }
            else
            {
                if (!di.Name.StartsWith("."))
                    store.AppendValues(di.FullName, di.Name, DirIcon, true);
            }
        }

        foreach (FileInfo file in root.GetFiles())
        {
            if (FileCommander.Settings.GetConf("ShowHiddenFiles"))
            {
                store.AppendValues(file.FullName, file.Name, FileIcon, false);
            }
            else
            {
                if (!file.Name.StartsWith("."))
                    store.AppendValues(file.FullName, file.Name, FileIcon, false);
            }
        }
    }

    public static DirectoryInfo OnItemActivated(ItemActivatedArgs a, DirectoryInfo root, ListStore store,
        Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward)
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