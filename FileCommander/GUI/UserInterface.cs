namespace FileCommander.GUI;

using Gtk;
using System;

class SharpApp : Window
{
    public SharpApp() : base("File Commander")
    {
        
        SetDefaultSize(720, 512);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };
        
        MenuBar mb = new MenuBar();

        Menu filemenu = new Menu();
        MenuItem file = new MenuItem("File");
        file.Submenu = filemenu;
       
        AccelGroup agr = new AccelGroup();
        AddAccelGroup(agr);

        ImageMenuItem newi = new ImageMenuItem(Stock.New, agr);
        newi.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.n, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        filemenu.Append(newi);

        ImageMenuItem open = new ImageMenuItem(Stock.Open, agr);
        open.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.n, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        filemenu.Append(open);

        SeparatorMenuItem menusep = new SeparatorMenuItem();
        filemenu.Append(menusep);

        ImageMenuItem exit = new ImageMenuItem(Stock.Quit, agr);
        exit.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));

        exit.Activated += OnActivated;
        filemenu.Append(exit);

        mb.Append(file);

        VBox menuvbox = new VBox(false, 2);
        menuvbox.PackStart(mb, false, false, 0);
        menuvbox.PackStart(new Label(), false, false, 0);
        
        Add(menuvbox);
        
        //Toolbar
        Toolbar toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Icons;

        ToolButton newtb = new ToolButton(Stock.New);
        ToolButton opentb = new ToolButton(Stock.Open);
        ToolButton savetb = new ToolButton(Stock.Save);
        SeparatorToolItem toolsep = new SeparatorToolItem();
        ToolButton quittb = new ToolButton(Stock.Quit);

        toolbar.Insert(newtb, 0);
        toolbar.Insert(opentb, 1);
        toolbar.Insert(savetb, 2);
        toolbar.Insert(toolsep, 3);
        toolbar.Insert(quittb, 4);

        quittb.Clicked += OnClicked;
         
        VBox toolvbox = new VBox(false, 2);
        toolvbox.PackStart(toolbar, false, false, 0);

        Add(toolvbox);
        

        ShowAll();
    }

    void OnActivated(object sender, EventArgs args)
    {
        Application.Quit();
    }
    
    void OnClicked(object sender, EventArgs args)
    {
        Application.Quit();
    }
}