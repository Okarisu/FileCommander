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

        //Vytvoření nástrojové lišty
        var toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Both;

        var toolRefreshButton = new ToolButton(Stock.Refresh);
        var toolBackButton = new ToolButton(Stock.GoBack);
        var toolForwardButton = new ToolButton(Stock.GoForward);
        var toolUndoButton = new ToolButton(Stock.Undo);
        var toolRedoButton = new ToolButton(Stock.Redo);
        var toolNewButton = new ToolButton(Stock.New);
        var toolCopyButton = new ToolButton(Stock.Copy);
        var toolMoveButton = new ToolButton(Stock.Remove); //Icon TBA
        var toolRenameButton = new ToolButton(Stock.Network); //Icon TBA
        var toolDeleteButton = new ToolButton(Stock.Remove); //Icon TBA
        var toolExtractButton = new ToolButton(Stock.Execute); //Icon TBA
        var toolCompressButton = new ToolButton(Stock.Execute); //Icon TBA

        toolRefreshButton.Clicked += OnRefreshClicked!;
        toolBackButton.Clicked += OnBackClicked!;
        toolForwardButton.Clicked += OnForwardClicked!;
        toolUndoButton.Clicked += OnUndoClicked!;
        toolRedoButton.Clicked += OnRedoClicked!;
        toolNewButton.Clicked += OnNewClicked!;
        toolCopyButton.Clicked += OnCopyClicked!;
        toolMoveButton.Clicked += OnMoveClicked!;
        toolDeleteButton.Clicked += OnDeleteClicked!;
        toolRenameButton.Clicked += OnRenameClicked!;
        toolExtractButton.Clicked += OnExtractClicked!;
        toolCompressButton.Clicked += OnCompressClicked!;

        toolbar.Insert(toolRefreshButton, 0);
        toolbar.Insert(toolBackButton, 1);
        toolbar.Insert(toolForwardButton, 2);
        toolbar.Insert(toolUndoButton, 3);
        toolbar.Insert(toolRedoButton, 4);
        toolbar.Insert(new SeparatorToolItem(), 5);
        toolbar.Insert(toolNewButton, 6);
        toolbar.Insert(toolCopyButton, 7);
        toolbar.Insert(toolMoveButton, 8);
        toolbar.Insert(toolRenameButton, 9);
        toolbar.Insert(toolDeleteButton, 10);
        toolbar.Insert(new SeparatorToolItem(), 11);
        toolbar.Insert(toolExtractButton, 12);
        toolbar.Insert(toolCompressButton, 12);

        windowVerticalBox.PackStart(toolbar, false, false, 0);

        //Vytvoření lišty správy adresářových panelů
        HBox twinPanelToolbox = new HBox();
        //TODO list dostupných disků - https://learn.microsoft.com/en-us/dotnet/api/system.io.driveinfo.getdrives?redirectedfrom=MSDN&view=net-7.0#System_IO_DriveInfo_GetDrives

        //Levá lišta
        var leftPanelBar = new Toolbar();
        leftPanelBar.ToolbarStyle = ToolbarStyle.Both;
        var leftHomeButton = new ToolButton(Stock.Home);
        var leftUpButton = new ToolButton(Stock.GoUp);
        leftPanelBar.Insert(leftHomeButton, -1);
        leftPanelBar.Insert(leftUpButton, -1);

        //Pravá lišta
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;
        var rightHomeButton = new ToolButton(Stock.Home);
        var rightUpButton = new ToolButton(Stock.GoUp);
        rightPanelBar.Insert(rightHomeButton, -1);
        rightPanelBar.Insert(rightUpButton, -1);

        twinPanelToolbox.PackStart(leftPanelBar, true, true, 0);
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
            LeftRoot = OnItemActivated(args, LeftRoot, LeftStore);  };

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
            RightRoot = OnItemActivated(args, RightRoot, RightStore);  };

        rightScrolledWindow.Add(rightIconView);
        //rightIconView.GrabFocus();

        HBox twinPanelsBox = new HBox(false, 0);
        twinPanelsBox.PackStart(leftScrolledWindow, true, true, 0);
        twinPanelsBox.PackStart(rightScrolledWindow, true, true, 0);

        windowVerticalBox.PackStart(twinPanelsBox, true, true, 0);

        leftHomeButton.Clicked += OnLeftHomeClicked!;
        rightHomeButton.Clicked += OnRightHomeClicked!;
        //leftUpButton.Clicked += OnLeftUpClicked!;
        //rightUpButton.Clicked += OnRightUpClicked!;
        leftUpButton.Clicked += delegate { LeftRoot = OnUpCLicked(LeftRoot, LeftStore); };
        rightUpButton.Clicked += delegate { RightRoot = OnUpCLicked(RightRoot, RightStore); };

        ShowAll();
    }

    public static Gdk.Pixbuf GetIcon(string name)
    {
        return Gtk.IconTheme.Default.LoadIcon(name, 48, (IconLookupFlags) 0);
    }

    public static ListStore CreateStore()
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
            Console.WriteLine("Root neexistuje");
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

        Console.WriteLine("here");
    }

    //Asi by bylo fajn pro oba panely použít společnou metodu, ale nepřišel jsem na to, jak to sloučit.
    DirectoryInfo OnItemActivated(ItemActivatedArgs a, DirectoryInfo root, ListStore store)
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

    private static void OnLeftItemActivated(object sender, ItemActivatedArgs a)
    {
        TreeIter iter;
        LeftStore.GetIter(out iter, a.Path);
        string path = (string) LeftStore.GetValue(iter, ColPath);
        bool isDir = (bool) LeftStore.GetValue(iter, ColIsDirectory);

        if (!isDir)
            return;


        LeftRoot = new DirectoryInfo(path);
        FillStore(LeftStore, LeftRoot);
    }

    private static void OnRightItemActivated(object sender, ItemActivatedArgs a)
    {
        TreeIter iter;
        RightStore.GetIter(out iter, a.Path);
        string path = (string) RightStore.GetValue(iter, ColPath);
        bool isDir = (bool) RightStore.GetValue(iter, ColIsDirectory);

        if (!isDir)
        {
            Console.WriteLine("Here");
            return;
        }

        RightRoot = new DirectoryInfo(path);
        FillStore(RightStore, RightRoot);
    }
}