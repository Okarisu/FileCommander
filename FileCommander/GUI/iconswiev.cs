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

    DirectoryInfo root = new DirectoryInfo("/");
    Gdk.Pixbuf dirIcon, fileIcon;
    ListStore LeftStore;
    ToolButton upButton;

    public IconApp() : base("IconView")
    {
        SetDefaultSize(650, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox(false, 0);
        Add(vbox);

        Toolbar toolbar = new Toolbar();
        vbox.PackStart(toolbar, false, false, 0);

        upButton = new ToolButton(Stock.GoUp);
        upButton.IsImportant = true;
        upButton.Sensitive = false;
        toolbar.Insert(upButton, -1);

        ToolButton homeButton = new ToolButton(Stock.Home);
        homeButton.IsImportant = true;
        toolbar.Insert(homeButton, -1);

        fileIcon = GetIcon(Stock.File);
        dirIcon = GetIcon(Stock.Open);

        
        //TODO new HBox - do něj 2 new ScrolledWindow
        //TODO do hlavního VBox přidat HBox

        HBox hbox = new HBox(false, 0);
        
        ScrolledWindow leftScrolledWindow = new ScrolledWindow();
        leftScrolledWindow.ShadowType = ShadowType.EtchedIn;
        leftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        hbox.PackStart(leftScrolledWindow, true, true, 0);

        LeftStore = CreateStore();
        LeftStore.FillStore();

        IconView iconView = new IconView(LeftStore);
        iconView.SelectionMode = SelectionMode.Multiple;

        upButton.Clicked += new EventHandler(OnUpClicked);
        homeButton.Clicked += new EventHandler(OnHomeClicked);

        iconView.TextColumn = COL_DISPLAY_NAME;
        iconView.PixbufColumn = COL_PIXBUF;

        iconView.ItemActivated += OnItemActivated;
        leftScrolledWindow.Add(iconView);
        iconView.GrabFocus();
        
        ShowAll();
    }

    Gdk.Pixbuf GetIcon(string name)
    {
        return Gtk.IconTheme.Default.LoadIcon(name, 48, (IconLookupFlags) 0);
    }

    ListStore CreateStore()
    {
        ListStore store = new ListStore(typeof (string), 
            typeof(string), typeof (Gdk.Pixbuf), typeof(bool));

        store.SetSortColumnId(COL_DISPLAY_NAME, SortType.Ascending);

        return store;
    }

    void FillStore()
    {
        LeftStore.Clear();

        if (!root.Exists)
            return;

        foreach (DirectoryInfo di in root.GetDirectories())
        {
            if (!di.Name.StartsWith("."))
                LeftStore.AppendValues(di.FullName, di.Name, dirIcon, true);
        }
        
        foreach (FileInfo file in root.GetFiles())
        {
            if (!file.Name.StartsWith("."))
                LeftStore.AppendValues(file.FullName, file.Name, fileIcon, false);
        }
    }

    void OnHomeClicked(object sender, EventArgs a)
    {
        root = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore();
        upButton.Sensitive = true;
    }

    void OnItemActivated(object sender, ItemActivatedArgs a)
    {
        TreeIter iter;
        LeftStore.GetIter(out iter, a.Path);
        string path = (string) LeftStore.GetValue(iter, COL_PATH);
        bool isDir = (bool) LeftStore.GetValue(iter, COL_IS_DIRECTORY);

        if (!isDir)
            return;

        root = new DirectoryInfo(path);
        FillStore();

        upButton.Sensitive = true;
    }

    void OnUpClicked(object sender, EventArgs a)
    {
        root = root.Parent;
        FillStore();
        upButton.Sensitive = (root.FullName == "/" ? false : true);
    }
}