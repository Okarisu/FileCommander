// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

using System.Runtime.InteropServices;
using FileCommander.GUI.Toolbars;
using Gdk;
using Gtk;
using Window = Gtk.Window;

namespace FileCommander.GUI;

public class App : Window
{
    private const int ColPath = 0;
    public const int ColDisplayName = 1;
    public const int ColPixbuf = 2;
    private const int ColIsDirectory = 3;


    public static DirectoryInfo LeftRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
    public static DirectoryInfo RightRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));


    //private static readonly Gdk.Pixbuf FileIcon = GetIcon(Stock.File), DirIcon = GetIcon(Stock.Directory);
    private static readonly Pixbuf FileIcon = new("icons/file.png"), DirIcon = new("icons/folder.png");

    public static ListStore store = CreateStore(), RightStore = CreateStore();

    public static ScrolledWindow LeftScrolledWindow = new();
    public static ScrolledWindow RightScrolledWindow = new();

    public static IconView LeftIconView = new(store);
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
    
    private static HBox leftTwinPanelHeader = new HBox(true, 0);
    private static HBox rightTwinPanelHeader = new HBox(true, 0);

    public App() : base("File Commander")
    {
        SetDefaultSize(1280, 720);
        Maximize();
        SetPosition(WindowPosition.Center);
        DeleteEvent += (_, _) => Application.Quit();

        VBox windowContainer = new VBox(false, 0);
        Add(windowContainer);
        var mb = DrawMenu.DrawMenuBar();
        windowContainer.PackStart(mb, false, true, 0);
        var toolbar = TopToolbar.DrawToolbar();
        windowContainer.PackStart(toolbar, false, true, 0);

        HBox twinPanelsContainer = new HBox(false, 0);
        windowContainer.PackStart(twinPanelsContainer, true, true, 0);
        VBox leftTwinContainer = new VBox(false, 0);
        twinPanelsContainer.PackStart(leftTwinContainer, true, true, 0);
        twinPanelsContainer.PackStart(new Separator(Orientation.Vertical), false, false, 0);
        VBox rightTwinContainer = new VBox(false, 0);
        twinPanelsContainer.PackStart(rightTwinContainer, true, true, 0);

        TwinPanels.DrawLeftPanel();
        TwinPanels.DrawRightPanel();

        //HBox leftTwinPanelHeader = new HBox(true, 0);
        leftTwinContainer.PackStart(leftTwinPanelHeader, false, true, 0);
        leftTwinPanelHeader.PackStart(ToolbarLeft.DrawLeftToolbar(), false, true, 0);
        LeftRootLabel.LineWrap = true;
        LeftRootLabel.MaxWidthChars = 50;
        leftTwinContainer.PackStart(LeftRootLabel, false, true, 0);
        leftTwinContainer.PackStart(LeftScrolledWindow, true, true, 0);

        //HBox rightTwinPanelHeader = new HBox(true, 0);
        rightTwinContainer.PackStart(rightTwinPanelHeader, false, true, 0);
        rightTwinPanelHeader.PackStart(ToolbarRight.DrawRightToolbar(), false, true, 0);
        RightRootLabel.LineWrap = true;
        RightRootLabel.MaxWidthChars = 50;
        rightTwinContainer.PackStart(RightRootLabel, false, true, 0);
        rightTwinContainer.PackStart(RightScrolledWindow, true, true, 0);

        LeftDiskBar = Disks.DrawDiskBar(LeftHistory, LeftHistoryForward, LeftRoot, store, LeftRootLabel);
        RightDiskBar = Disks.DrawDiskBar(RightHistory, RightHistoryForward, RightRoot, RightStore, RightRootLabel);

        leftTwinPanelHeader.PackStart(LeftDiskBar, false, true, 0);
        rightTwinPanelHeader.PackStart(RightDiskBar, false, true, 0);

        ShowAll();

        if (!Settings.GetConf("ShowMountedDrives"))
        {
            LeftDiskBar.Hide();
            RightDiskBar.Hide();
        }
    }

    public static void UpdateDisks()
    {
        LeftDiskBar = Disks.DrawDiskBar(LeftHistory, LeftHistoryForward, LeftRoot, store, LeftRootLabel);
        RightDiskBar = Disks.DrawDiskBar(RightHistory, RightHistoryForward, RightRoot, RightStore, RightRootLabel);

        leftTwinPanelHeader.PackStart(LeftDiskBar, false, true, 0);
        rightTwinPanelHeader.PackStart(RightDiskBar, false, true, 0);

    }


    private static Pixbuf GetIcon(string name) => IconTheme.Default.LoadIcon(name, 48, 0);

    private static ListStore CreateStore()
    {
        ListStore store = new ListStore(typeof(string),
            typeof(string), typeof(Pixbuf), typeof(bool));

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
            if (Settings.GetConf("ShowHiddenFiles"))
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
            if (Settings.GetConf("ShowHiddenFiles"))
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

        var store = FocusedPanel == 1 ? App.store : RightStore;
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
        string parent, optionalSlash;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            parent = root.Parent?.Name is "/" or "" ? "" : root.Parent?.Name;
            optionalSlash = parent is "" ? "" : "/";
        }
        else
        {
            parent = root.Parent?.Name is "" ? "" : root.Parent?.Name;
            optionalSlash = parent.EndsWith(":\\") || parent == "" ? "" : "\\";
        }

        label.Text = $"Current directory: {parent}{optionalSlash}{root.Name}";
    }
}