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
    

    public IconApp() : base("IconView")
    {
        SetDefaultSize(650, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox(false, 0);
        Add(vbox);

        HBox toolbarHorizontalBox = new HBox(true, 0);
        
        Toolbar leftToolbar = new Toolbar();
        leftToolbar.ToolbarStyle = ToolbarStyle.Icons;

        ToolButton leftButtonNew = new ToolButton(Stock.New);
        ToolButton leftButtonOpen = new ToolButton(Stock.Open);
        ToolButton leftButtonSave = new ToolButton(Stock.Save);
        SeparatorToolItem leftSep = new SeparatorToolItem();
        ToolButton leftButtonQuit = new ToolButton(Stock.Quit);

        leftToolbar.Insert(leftButtonNew, 0);
        leftToolbar.Insert(leftButtonOpen, 1);
        leftToolbar.Insert(leftButtonSave, 2);
        leftToolbar.Insert(leftSep, 3);
        leftToolbar.Insert(leftButtonQuit, 4);

        leftButtonQuit.Clicked += delegate { Application.Quit(); };

        Toolbar rightToolbar = new Toolbar();
        rightToolbar.ToolbarStyle = ToolbarStyle.Icons;

        ToolButton rightButtonNew = new ToolButton(Stock.New);
        ToolButton rightButtonOpen = new ToolButton(Stock.Open);
        ToolButton rightButtonSave = new ToolButton(Stock.Save);
        SeparatorToolItem rightsep = new SeparatorToolItem();
        ToolButton rightButtonQuit = new ToolButton(Stock.Quit);
        //ToolButton unzip = new ToolButton("unzip");

        rightToolbar.Insert(rightButtonNew, 0);
        rightToolbar.Insert(rightButtonOpen, 1);
        rightToolbar.Insert(rightButtonSave, 2);
        rightToolbar.Insert(rightsep, 3);
        rightToolbar.Insert(rightButtonQuit, 4);
         
        rightButtonQuit.Clicked += delegate { Application.Quit(); };

        toolbarHorizontalBox.PackStart(leftToolbar, true, true, 0);
        toolbarHorizontalBox.PackStart(rightToolbar, true, true, 0);
        //HBox toolbox = new HBox(false, 2);
        vbox.PackStart(toolbarHorizontalBox, false, true, 0);

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
        FillStore(LeftStore, leftRoot);

        IconView leftIconView = new IconView(LeftStore);
        leftIconView.SelectionMode = SelectionMode.Multiple;

        leftIconView.TextColumn = COL_DISPLAY_NAME;
        leftIconView.PixbufColumn = COL_PIXBUF;

        leftIconView.ItemActivated += OnLeftItemActivated;
        leftScrolledWindow.Add(leftIconView);
        leftIconView.GrabFocus();
        
        //Right panel
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
        rightIconView.GrabFocus();

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