namespace FileCommander.GUI;

using static FunctionController;
using System;
using System.IO;
using Gtk;

public class IconApp : Window
{
    const int ColPath = 0;
    const int ColDisplayName = 1;
    const int ColPixbuf = 2;
    const int ColIsDirectory = 3;


    public static DirectoryInfo LeftRoot = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal)),
        RightRoot = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));

    static readonly Gdk.Pixbuf FileIcon = GetIcon(Stock.File), DirIcon = GetIcon(Stock.Open);
    public static ListStore LeftStore, RightStore;


    //TODO ?? Nastavení v menu/tools -> YAML nebo XML https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file
    public IconApp() : base("File Commander")
    {
        SetDefaultSize(1280, 720);
        Maximize();
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        //Vytvoření kontejneru vnitřního obsahu okna
        VBox windowVerticalBox = new VBox(false, 0);
        Add(windowVerticalBox);

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

        var toolMoveButton = new ToolButton(Stock.Remove); //Icon TBA
        toolbar.Insert(toolMoveButton, 8);
        toolMoveButton.Clicked += OnMoveClicked!;

        var toolRenameButton = new ToolButton(Stock.Network); //Icon TBA
        toolbar.Insert(toolRenameButton, 9);
        toolRenameButton.Clicked += OnRenameClicked!;

        var toolDeleteButton = new ToolButton(Stock.Remove); //Icon TBA
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

        //Vytvoření lišty správy adresářových panelů
        HBox twinPanelToolbox = new HBox();
        //TODO list dostupných disků - https://learn.microsoft.com/en-us/dotnet/api/system.io.driveinfo.getdrives?redirectedfrom=MSDN&view=net-7.0#System_IO_DriveInfo_GetDrives

        //LEVÁ LIŠTA
        var leftPanelBar = new Toolbar();
        leftPanelBar.ToolbarStyle = ToolbarStyle.Both;

        var leftHomeButton = new ToolButton(Stock.Home);
        leftPanelBar.Insert(leftHomeButton, -1);
        leftHomeButton.Clicked += delegate(object? sender, EventArgs args)
        {
            LeftRoot = OnHomeClicked(sender, args, LeftRoot, LeftStore);
        };

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftPanelBar.Insert(leftUpButton, -1);
        leftUpButton.Clicked += delegate { LeftRoot = OnUpClicked(LeftRoot, LeftStore); };
        
        var leftToolBackButton = new ToolButton(Stock.GoBack);
        toolbar.Insert(leftToolBackButton, 1);
        leftToolBackButton.Clicked += OnBackClicked!;
        
        var leftToolForwardButton = new ToolButton(Stock.GoForward);
        toolbar.Insert(leftToolForwardButton, 2);
        leftToolForwardButton.Clicked += OnForwardClicked!;
        
        var leftToolUndoButton = new ToolButton(Stock.Undo);
        toolbar.Insert(leftToolUndoButton, 3);
        leftToolUndoButton.Clicked += OnUndoClicked!;
        
        var leftToolRedoButton = new ToolButton(Stock.Redo);
        toolbar.Insert(leftToolRedoButton, 4);
        leftToolRedoButton.Clicked += OnRedoClicked!;
        
        twinPanelToolbox.PackStart(leftPanelBar, true, true, 0);

        //PRAVÁ LIŠTA
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;

        var rightHomeButton = new ToolButton(Stock.Home);
        rightPanelBar.Insert(rightHomeButton, -1);
        rightHomeButton.Clicked += delegate(object? sender, EventArgs args)
        {
            RightRoot = OnHomeClicked(sender, args, RightRoot, RightStore);
        };

        var rightUpButton = new ToolButton(Stock.GoUp);
        rightPanelBar.Insert(rightUpButton, -1);
        rightUpButton.Clicked += delegate { RightRoot = OnUpClicked(RightRoot, RightStore); };
        
        var rightToolBackButton = new ToolButton(Stock.GoBack);
        toolbar.Insert(rightToolBackButton, 1);
        rightToolBackButton.Clicked += OnBackClicked!;
        
        var rightToolForwardButton = new ToolButton(Stock.GoForward);
        toolbar.Insert(rightToolForwardButton, 2);
        rightToolForwardButton.Clicked += OnForwardClicked!;
        
        var rightToolUndoButton = new ToolButton(Stock.Undo);
        toolbar.Insert(rightToolUndoButton, 3);
        rightToolUndoButton.Clicked += OnUndoClicked!;
        
        var rightToolRedoButton = new ToolButton(Stock.Redo);
        toolbar.Insert(rightToolRedoButton, 4);
        rightToolRedoButton.Clicked += OnRedoClicked!;

        
        twinPanelToolbox.PackStart(rightPanelBar, true, true, 0);
        windowVerticalBox.PackStart(twinPanelToolbox, false, true, 0);

        //Levý panel
        ScrolledWindow leftScrolledWindow = new ScrolledWindow();
        leftScrolledWindow.ShadowType = ShadowType.EtchedIn;
        leftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        LeftStore = CreateStore();
        FillStore(LeftStore, LeftRoot);

        IconView leftIconView = new IconView(LeftStore);
        leftIconView.SelectionMode = SelectionMode.Multiple;
        leftIconView.TextColumn = ColDisplayName;
        leftIconView.PixbufColumn = ColPixbuf;
        leftIconView.ItemActivated += delegate(object o, ItemActivatedArgs args)
        {
            LeftRoot = OnItemActivated(args, LeftRoot, LeftStore);
        };

        leftScrolledWindow.Add(leftIconView);
        //leftIconView.GrabFocus();

        //Pravý panel
        ScrolledWindow rightScrolledWindow = new ScrolledWindow();
        rightScrolledWindow.ShadowType = ShadowType.EtchedIn;
        rightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        RightStore = CreateStore();
        FillStore(RightStore, RightRoot);

        IconView rightIconView = new IconView(RightStore);
        rightIconView.SelectionMode = SelectionMode.Multiple;
        rightIconView.TextColumn = ColDisplayName;
        rightIconView.PixbufColumn = ColPixbuf;
        rightIconView.ItemActivated += delegate(object o, ItemActivatedArgs args)
        {
            RightRoot = OnItemActivated(args, RightRoot, RightStore);
        };

        rightScrolledWindow.Add(rightIconView);
        //rightIconView.GrabFocus();

        HBox twinPanelsBox = new HBox(false, 0);
        twinPanelsBox.PackStart(leftScrolledWindow, true, true, 0);
        twinPanelsBox.PackStart(rightScrolledWindow, true, true, 0);

        windowVerticalBox.PackStart(twinPanelsBox, true, true, 0);


        ShowAll();
    }

    private static Gdk.Pixbuf GetIcon(string name)
    {
        return Gtk.IconTheme.Default.LoadIcon(name, 48, (IconLookupFlags) 0);
    }

    private ListStore CreateStore()
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

    private DirectoryInfo OnItemActivated(ItemActivatedArgs a, DirectoryInfo root, ListStore store)
    {
        TreeIter iter;
        store.GetIter(out iter, a.Path);
        string path = (string) store.GetValue(iter, ColPath);
        bool isDir = (bool) store.GetValue(iter, ColIsDirectory);

        if (!isDir)
            return root;

        root = new DirectoryInfo(path);
        FillStore(store, root);
        return root;
    }
}