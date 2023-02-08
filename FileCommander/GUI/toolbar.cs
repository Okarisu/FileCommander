namespace FileCommander.GUI;

using Gtk;
using System;
 
class ToolApp : Window {
 

    public ToolApp() : base("Toolbar")
    {
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };
        
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

        quittb.Clicked += OnClicked;
         
        VBox vbox = new VBox(false, 2);
        vbox.PackStart(toolbar, false, false, 0);
        
        Add(vbox);

        ShowAll();
    }

    void OnClicked(object sender, EventArgs args)
    {
        Application.Quit();
    }

   
}