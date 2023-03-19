// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

using FileCommander.GUI.Controllers;
using FileCommander.GUI.Toolbars;

namespace FileCommander.GUI;



using static NavigationController;
using System;
using System.IO;
using Gtk;

public class App : Window
{
    const int ColPath = 0;
    const int ColDisplayName = 1;
    const int ColPixbuf = 2;
    const int ColIsDirectory = 3;


    public static DirectoryInfo LeftRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
    public static DirectoryInfo RightRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

    private static readonly Gdk.Pixbuf FileIcon = GetIcon(Stock.File), DirIcon = GetIcon(Stock.Open);

    public static ListStore LeftStore = CreateStore(), RightStore = CreateStore();

    private static ScrolledWindow _leftScrolledWindow = new();
    private static ScrolledWindow _rightScrolledWindow = new();

    private static IconView _leftIconView = new(LeftStore);
    private static IconView _rightIconView = new(RightStore);

    private static int _focusedPanel;

    //TODO Nastavení v menu/tools -> YAML nebo XML https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file
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
        windowVerticalBox.PackStart(toolbar, false, false, 0);

        

        #region TwinToolbox

        //Vytvoření lišty správy adresářových panelů
        HBox twinPanelToolbox = new HBox();
        //TODO list dostupných disků - https://learn.microsoft.com/en-us/dotnet/api/system.io.driveinfo.getdrives?redirectedfrom=MSDN&view=net-7.0#System_IO_DriveInfo_GetDrives

        #region LeftTwinBar

        //TODO disk list
        HBox leftTwinToolbox = new HBox();

        //LEVÁ LIŠTA
        Toolbar leftToolbar = ToolbarLeft.DrawLeftToolbar();
        twinPanelToolbox.PackStart(leftToolbar, true, true, 0);

        #endregion

        Toolbar rightPanelBar = ToolbarRight.DrawRightToolbar();
        twinPanelToolbox.PackStart(rightPanelBar, true, true, 0);

        #endregion

        windowVerticalBox.PackStart(twinPanelToolbox, false, true, 0);

        

        #region TwinWindows

        #region LeftIconView

        //Levý panel
        //_leftScrolledWindow.ShadowType = ShadowType.EtchedIn;
        _leftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(LeftStore, LeftRoot);

        _leftIconView.GrabFocus();
        _leftIconView.SelectionMode = SelectionMode.Multiple;
        _leftIconView.TextColumn = ColDisplayName;
        _leftIconView.PixbufColumn = ColPixbuf;
        _leftIconView.ItemActivated += (_, args) => LeftRoot = OnItemActivated(args, LeftRoot, LeftStore);
        _leftIconView.FocusInEvent += (_, _) => _focusedPanel = 1;


        _leftScrolledWindow.Add(_leftIconView);

        #endregion

        #region RightIconView

        //Pravý panel
        //_rightScrolledWindow.ShadowType = ShadowType.EtchedIn;
        _rightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(RightStore, RightRoot);

        _rightIconView.SelectionMode = SelectionMode.Multiple;
        _rightIconView.TextColumn = ColDisplayName;
        _rightIconView.PixbufColumn = ColPixbuf;
        _rightIconView.ItemActivated += (_, args) => RightRoot = OnItemActivated(args, RightRoot, RightStore);
        _rightIconView.FocusInEvent += (_, _) => _focusedPanel = 2;

        _rightScrolledWindow.Add(_rightIconView);

        #endregion

        HBox twinPanelsBox = new HBox(false, 0);
        twinPanelsBox.PackStart(_leftScrolledWindow, true, true, 0);
        twinPanelsBox.PackStart(new Separator(Orientation.Vertical), false, false, 0);
        twinPanelsBox.PackStart(_rightScrolledWindow, true, true, 0);

        #endregion

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

    private static DirectoryInfo OnItemActivated(ItemActivatedArgs a, DirectoryInfo root, ListStore store)
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

    public static int GetFocusedWindow() => _focusedPanel;

    public static Item?[] GetSelectedItems()
    {
        var selection = _focusedPanel == 1 ? _leftIconView.SelectedItems : _rightIconView.SelectedItems;
        if (selection == null) return null;

        var store = _focusedPanel == 1 ? LeftStore : RightStore;
        var files = new Item?[selection.Length];

        for (var i = 0; i < selection.Length; i += 1)
        {
            store.GetIter(out var treeIterator, selection[i]);
            files[i] = new Item(store.GetValue(treeIterator, ColPath).ToString(),
                store.GetValue(treeIterator, ColDisplayName).ToString(),
                (bool) store.GetValue(treeIterator, ColIsDirectory));
        }

        return files;
    }
}