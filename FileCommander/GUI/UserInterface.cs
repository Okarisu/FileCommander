// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace FileCommander.GUI;

using static FunctionController;
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

    //TODO ?? Nastavení v menu/tools -> YAML nebo XML https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file
    public App() : base("File Commander")
    {
        SetDefaultSize(1280, 720);
        Maximize();
        SetPosition(WindowPosition.Center);
        DeleteEvent += (sender, _) => Application.Quit();

        //Vytvoření kontejneru vnitřního obsahu okna
        VBox windowVerticalBox = new VBox(false, 0);
        Add(windowVerticalBox);

        #region Toolbar

        //Vytvoření hlavní nástrojové lišty
        var toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Both;

        var toolRefreshButton = new ToolButton(Stock.Refresh);
        toolbar.Insert(toolRefreshButton, 0);
        toolRefreshButton.Clicked += OnRefreshClicked!;

        var toolNewButton = new ToolButton(Stock.New);
        toolbar.Insert(toolNewButton, 6);
        toolNewButton.Clicked += OnNewClicked!;

        var toolCopyButton = new ToolButton(Stock.Copy);
        toolbar.Insert(toolCopyButton, 7);
        toolCopyButton.Clicked += OnCopyClicked!;

        var toolMoveButton = new ToolButton(Stock.Dnd); //Icon TBA
        toolbar.Insert(toolMoveButton, 8);
        toolMoveButton.Clicked += OnMoveClicked!;

        var toolRenameButton = new ToolButton(Stock.Network); //Icon TBA
        toolbar.Insert(toolRenameButton, 9);
        toolRenameButton.Clicked += OnRenameClicked!;

        var toolDeleteButton = new ToolButton(Stock.Delete);
        toolbar.Insert(toolDeleteButton, 10);
        toolDeleteButton.Clicked += OnDeleteClicked!;

        var toolExtractButton = new ToolButton(Stock.Execute); //Icon TBA
        toolbar.Insert(toolExtractButton, 12);
        toolExtractButton.Clicked += OnExtractClicked!;

        var toolCompressButton = new ToolButton(Stock.Execute); //Icon TBA
        toolbar.Insert(toolCompressButton, 12);
        toolCompressButton.Clicked += OnCompressClicked!;

        toolbar.Insert(new SeparatorToolItem(), 5);
        toolbar.Insert(new SeparatorToolItem(), 11);

        windowVerticalBox.PackStart(toolbar, false, false, 0);

        #endregion

        #region TwinToolbox

        //Vytvoření lišty správy adresářových panelů
        HBox twinPanelToolbox = new HBox();
        //TODO list dostupných disků - https://learn.microsoft.com/en-us/dotnet/api/system.io.driveinfo.getdrives?redirectedfrom=MSDN&view=net-7.0#System_IO_DriveInfo_GetDrives

        #region LeftTwinBar

        HBox leftTwinToolbox = new HBox();

        //LEVÁ LIŠTA
        var leftToolbar = new Toolbar();
        leftToolbar.ToolbarStyle = ToolbarStyle.Both;

        var leftHomeButton = new ToolButton(Stock.Home);
        leftToolbar.Insert(leftHomeButton, 0);
        leftHomeButton.Clicked += (sender, args) => LeftRoot = OnHomeClicked(sender, args, LeftRoot, LeftStore);

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) => LeftRoot = OnUpClicked(LeftRoot, LeftStore);

        leftToolbar.Insert(new SeparatorToolItem(), 2);

        var leftToolBackButton = new ToolButton(Stock.GoBack);
        leftToolbar.Insert(leftToolBackButton, 3);
        leftToolBackButton.Clicked += OnBackClicked!;

        var leftToolForwardButton = new ToolButton(Stock.GoForward);
        leftToolbar.Insert(leftToolForwardButton, 4);
        leftToolForwardButton.Clicked += OnForwardClicked!;

        var leftToolUndoButton = new ToolButton(Stock.Undo);
        leftToolbar.Insert(leftToolUndoButton, 5);
        leftToolUndoButton.Clicked += OnUndoClicked!;

        var leftToolRedoButton = new ToolButton(Stock.Redo);
        leftToolbar.Insert(leftToolRedoButton, 5);
        leftToolRedoButton.Clicked += OnRedoClicked!;


        twinPanelToolbox.PackStart(leftToolbar, true, true, 0);

        #endregion

        #region RightTwinBar

        //PRAVÁ LIŠTA
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;

        var rightHomeButton = new ToolButton(Stock.Home);
        rightPanelBar.Insert(rightHomeButton, 0);
        rightHomeButton.Clicked += (sender, args) => RightRoot = OnHomeClicked(sender, args, RightRoot, RightStore);

        var rightUpButton = new ToolButton(Stock.GoUp);
        rightPanelBar.Insert(rightUpButton, 1);
        rightUpButton.Clicked += (_, _) => RightRoot = OnUpClicked(RightRoot, RightStore);

        rightPanelBar.Insert(new SeparatorToolItem(), 2);

        var rightToolBackButton = new ToolButton(Stock.GoBack);
        rightPanelBar.Insert(rightToolBackButton, 3);
        rightToolBackButton.Clicked += OnBackClicked!;

        var rightToolForwardButton = new ToolButton(Stock.GoForward);
        rightPanelBar.Insert(rightToolForwardButton, 4);
        rightToolForwardButton.Clicked += OnForwardClicked!;

        var rightToolUndoButton = new ToolButton(Stock.Undo);
        rightPanelBar.Insert(rightToolUndoButton, 5);
        rightToolUndoButton.Clicked += OnUndoClicked!;


        var rightToolRedoButton = new ToolButton(Stock.Redo);
        rightPanelBar.Insert(rightToolRedoButton, 6);
        rightToolRedoButton.Clicked += OnRedoClicked!;


        twinPanelToolbox.PackStart(rightPanelBar, true, true, 0);

        #endregion

        windowVerticalBox.PackStart(twinPanelToolbox, false, true, 0);

        #endregion

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
        _leftIconView.FocusInEvent += (o, args) => _focusedPanel = 1;


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
        _rightIconView.ItemActivated += (sender, args) => RightRoot = OnItemActivated(args, RightRoot, RightStore);
        _rightIconView.FocusInEvent += (o, args) => _focusedPanel = 2;

        foreach (var item in LeftStore)
        {
            Console.WriteLine(item.GetHashCode());
        }

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


    public static Gdk.Pixbuf GetIcon(string name) => IconTheme.Default.LoadIcon(name, 48, 0);

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
        TreeIter iter;
        store.GetIter(out iter, a.Path);
        var path = (string) store.GetValue(iter, ColPath);
        var isDir = (bool) store.GetValue(iter, ColIsDirectory);

        if (!isDir)
            return root;

        root = new DirectoryInfo(path);
        FillStore(store, root);
        return root;
    }

    public static int GetFocusedWindow() => _focusedPanel;

    public static TreePath[]? GetSelectedItems(int window)
    {
        return window switch
        {
            1 => _leftIconView.SelectedItems,
            2 => _rightIconView.SelectedItems,
            _ => null
        };
    }
}