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

    DirectoryInfo LeftRoot = new DirectoryInfo("/");
    DirectoryInfo RightRoot = new DirectoryInfo("/");
    Gdk.Pixbuf dirIcon, fileIcon;
    ListStore LeftStore;
    ListStore RightStore;
    

    public IconApp() : base("IconView")
    {
        SetDefaultSize(650, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox(false, 0);
        Add(vbox);
        
        Toolbar toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Icons;

        ToolButton newtb = new ToolButton(Stock.New);
        ToolButton opentb = new ToolButton(Stock.Open);
        ToolButton savetb = new ToolButton(Stock.Save);
        SeparatorToolItem sep = new SeparatorToolItem();
        ToolButton quittb = new ToolButton(Stock.Quit);

        toolbar.Insert(newtb, 0);
        toolbar.Insert(opentb, 1);
        toolbar.Insert(savetb, 2);
        toolbar.Insert(sep, 3);
        toolbar.Insert(quittb, 4);

        quittb.Clicked += delegate { Application.Quit(); };
         
        //HBox toolbox = new HBox(false, 2);
        vbox.PackStart(toolbar, false, true, 0);

        //toolbox.PackStart(toolbar, false, false, 0);

        /*
        Toolbar toolbar = new Toolbar();
        vbox.PackStart(toolbar, false, false, 0);

        upButton = new ToolButton(Stock.GoUp);
        upButton.IsImportant = true;
        upButton.Sensitive = false;
        toolbar.Insert(upButton, -1);

        ToolButton homeButton = new ToolButton(Stock.Home);
        homeButton.IsImportant = true;
        toolbar.Insert(homeButton, -1);
        */

        fileIcon = GetIcon(Stock.File);
        dirIcon = GetIcon(Stock.Open);
        
        
        
        //Left panel
        ScrolledWindow leftScrolledWindow = new ScrolledWindow();
        leftScrolledWindow.ShadowType = ShadowType.EtchedIn;
        leftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        LeftStore = CreateStore();
        FillStore(LeftStore, LeftRoot);

        IconView LeftIconView = new IconView(LeftStore);
        LeftIconView.SelectionMode = SelectionMode.Multiple;

        LeftIconView.TextColumn = COL_DISPLAY_NAME;
        LeftIconView.PixbufColumn = COL_PIXBUF;

        LeftIconView.ItemActivated += OnLeftItemActivated;
        leftScrolledWindow.Add(LeftIconView);
        LeftIconView.GrabFocus();
        
        //Right panel
        ScrolledWindow rightScrolledWindow = new ScrolledWindow();
        rightScrolledWindow.ShadowType = ShadowType.EtchedIn;
        rightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        RightStore = CreateStore();
        FillStore(RightStore, RightRoot);

        IconView RightIconView = new IconView(RightStore);
        RightIconView.SelectionMode = SelectionMode.Multiple;

        RightIconView.TextColumn = COL_DISPLAY_NAME;
        RightIconView.PixbufColumn = COL_PIXBUF;

        RightIconView.ItemActivated += OnRightItemActivated;
        rightScrolledWindow.Add(RightIconView);
        RightIconView.GrabFocus();

        HBox hbox = new HBox(false, 0);
        hbox.PackStart(leftScrolledWindow, true, true, 0);
        hbox.PackStart(rightScrolledWindow, true, true, 0);
        
        vbox.PackStart(hbox, true, true, 0);
        
        ShowAll();
    }

    Gdk.Pixbuf GetIcon(string name)
    {
        return Gtk.IconTheme.Default.LoadIcon(name, 48, (IconLookupFlags) 0);
    }

    ListStore CreateStore()
    {
        ListStore store = new ListStore(typeof(string), 
            typeof(string), typeof (Gdk.Pixbuf), typeof(bool));

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

        LeftRoot = new DirectoryInfo(path);
        FillStore(LeftStore, LeftRoot);

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

        RightRoot = new DirectoryInfo(path);
        FillStore(RightStore, RightRoot);
    }
}