namespace FileCommander.GUI;

using System;
using System.IO;
using Gtk;

public class IconApp : Window
{
    const int COL_PATH = 0;
    const int COL_DISPLAY_NAME = 1;
    const int COL_PIXBUF = 2;
    const int COL_IS_DIRECTORY = 3;

    DirectoryInfo leftRoot = new DirectoryInfo("/");
    DirectoryInfo rightRoot = new DirectoryInfo("/");

    Gdk.Pixbuf dirIcon, fileIcon;
    ListStore LeftStore;
    ListStore RightStore;

    //TODO ?? Nastavení v menu/tools -> XML https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file
    //TODO ?? --||-- v YAML? 
    public IconApp() : base("File Commander")
    {
        SetDefaultSize(650, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        //Vytvoření kontejneru vnitřního obsahu okna
        VBox windowVerticalBox = new VBox(false, 0);
        Add(windowVerticalBox);

        //Vytvoření nástrojové lišty
        var toolbar = CreateToolbar();
        windowVerticalBox.PackStart(toolbar, false, false, 0);

        //Vytvoření lišty správy adresářových panelů
        HBox twinPanelToolbox = new HBox();
        //TODO list dostupných disků - https://learn.microsoft.com/en-us/dotnet/api/system.io.driveinfo.getdrives?redirectedfrom=MSDN&view=net-7.0#System_IO_DriveInfo_GetDrives

        //Levá lišta
        var leftPanelBar = new Toolbar();
        leftPanelBar.ToolbarStyle = ToolbarStyle.Both;
        var leftHomeButton = new ToolButton(Stock.Home);
        leftPanelBar.Insert(leftHomeButton, -1);

        //Pravá lišta
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;
        var rightHomeButton = new ToolButton(Stock.Home);
        rightPanelBar.Insert(rightHomeButton, -1);

        twinPanelToolbox.PackStart(leftPanelBar, true, true, 0);
        twinPanelToolbox.PackStart(rightPanelBar, true, true, 0);
        windowVerticalBox.PackStart(twinPanelToolbox, false, true, 0);

        fileIcon = GetIcon(Stock.File);
        dirIcon = GetIcon(Stock.Open);

        //Levý panel
        ScrolledWindow leftScrolledWindow = new ScrolledWindow();
        leftScrolledWindow.ShadowType = ShadowType.EtchedIn;
        leftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        LeftStore = CreateStore();
        FillStore(LeftStore, leftRoot);
        
        IconView leftIconView = new IconView(LeftStore);
        leftIconView.SelectionMode = SelectionMode.Multiple;
        leftIconView.TextColumn = COL_DISPLAY_NAME;
        leftIconView.PixbufColumn = COL_PIXBUF;
        leftIconView.ItemActivated += OnLeftItemActivated;
        
        leftScrolledWindow.Add(leftIconView);
        //leftIconView.GrabFocus();

        //Pravý panel
        ScrolledWindow rightScrolledWindow = new ScrolledWindow();
        rightScrolledWindow.ShadowType = ShadowType.EtchedIn;
        rightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        RightStore = CreateStore();
        FillStore(RightStore, rightRoot);
        
        IconView rightIconView = new IconView(RightStore);
        rightIconView.SelectionMode = SelectionMode.Multiple;
        rightIconView.TextColumn = COL_DISPLAY_NAME;
        rightIconView.PixbufColumn = COL_PIXBUF;
        rightIconView.ItemActivated += OnRightItemActivated;
        
        rightScrolledWindow.Add(rightIconView);
        //rightIconView.GrabFocus();

        HBox twinPanelsBox = new HBox(false, 0);
        twinPanelsBox.PackStart(leftScrolledWindow, true, true, 0);
        twinPanelsBox.PackStart(rightScrolledWindow, true, true, 0);

        windowVerticalBox.PackStart(twinPanelsBox, true, true, 0);

        ShowAll();
    }

    Toolbar CreateToolbar()
    {
        var toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Both;

        var toolRefreshButton = new ToolButton(Stock.Refresh);
        var toolBackButton = new ToolButton(Stock.GoBack);
        var toolForwardButton = new ToolButton(Stock.GoForward);
        var toolUndoButton = new ToolButton(Stock.Undo);
        var toolRedoButton = new ToolButton(Stock.Redo);
        var toolNewButton = new ToolButton(Stock.New);
        var toolCopyButton = new ToolButton(Stock.Copy);
        var toolMoveButton = new ToolButton(Stock.Remove);
        var toolDeleteButton = new ToolButton(Stock.Remove);
        var toolRenameButton = new ToolButton(Stock.Network);

        toolbar.Insert(toolRefreshButton, 0);
        toolbar.Insert(toolBackButton, 1);
        toolbar.Insert(toolForwardButton, 2);
        toolbar.Insert(toolUndoButton, 3);
        toolbar.Insert(toolRedoButton, 4);
        toolbar.Insert(new SeparatorToolItem(), 5);
        toolbar.Insert(toolNewButton, 6);
        toolbar.Insert(toolCopyButton, 7);
        toolbar.Insert(toolMoveButton, 8);
        toolbar.Insert(toolDeleteButton, 9);
        toolbar.Insert(toolRenameButton, 10);

        return toolbar;
    }

    Gdk.Pixbuf GetIcon(string name)
    {
        return Gtk.IconTheme.Default.LoadIcon(name, 48, (IconLookupFlags) 0);
    }

    ListStore CreateStore()
    {
        ListStore store = new ListStore(typeof(string),
            typeof(string), typeof(Gdk.Pixbuf), typeof(bool));

        store.SetSortColumnId(COL_DISPLAY_NAME, SortType.Ascending);

        return store;
    }

    void FillStore(ListStore store, DirectoryInfo root)
    {
        store.Clear();

        if (!root.Exists)
            return;

        foreach (DirectoryInfo di in root.GetDirectories())
        {
            if (!di.Name.StartsWith("."))
                store.AppendValues(di.FullName, di.Name, dirIcon, true);
        }

        foreach (FileInfo file in root.GetFiles())
        {
            if (!file.Name.StartsWith("."))
                store.AppendValues(file.FullName, file.Name, fileIcon, false);
        }
    }

    //Asi by bylo fajn pro oba panely použít společnou metodu, ale nepřišel jsem na to, jak to sloučit.
    void OnItemActivated(DirectoryInfo root, ListStore store, object sender, ItemActivatedArgs a)
    {
        TreeIter iter;
        store.GetIter(out iter, a.Path);
        string path = (string) store.GetValue(iter, COL_PATH);
        bool isDir = (bool) store.GetValue(iter, COL_IS_DIRECTORY);

        if (!isDir)
            return;

        root = new DirectoryInfo(path);
        FillStore(store, root);
    }

    void OnLeftItemActivated(object sender, ItemActivatedArgs a)
    {
        TreeIter iter;
        LeftStore.GetIter(out iter, a.Path);
        string path = (string) LeftStore.GetValue(iter, COL_PATH);
        bool isDir = (bool) LeftStore.GetValue(iter, COL_IS_DIRECTORY);

        if (!isDir)
            return;

        leftRoot = new DirectoryInfo(path);
        FillStore(LeftStore, leftRoot);
    }

    void OnRightItemActivated(object sender, ItemActivatedArgs a)
    {
        TreeIter iter;
        RightStore.GetIter(out iter, a.Path);
        string path = (string) RightStore.GetValue(iter, COL_PATH);
        bool isDir = (bool) RightStore.GetValue(iter, COL_IS_DIRECTORY);

        if (!isDir)
        {
            Console.WriteLine("Here");
            return;
        }

        rightRoot = new DirectoryInfo(path);
        FillStore(RightStore, rightRoot);
    }
}