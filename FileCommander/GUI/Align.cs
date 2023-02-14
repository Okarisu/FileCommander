namespace FileCommander.GUI;

using Gtk;

class AlignApp : Window
{


    public AlignApp() : base("Alignment")
    {
        SetDefaultSize(260, 150);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        VBox vbox = new VBox(false, 5);
        HBox hbox = new HBox(true, 30);

        Alignment valign = new Alignment(0, 1, 0, 0);
        vbox.PackStart(valign, true, false, 0);

        Button ok = new Button("OK");
        ok.SetSizeRequest(70, 30);
        Button close = new Button("Close");

        hbox.Add(ok);
        hbox.Add(close);

        Alignment halign = new Alignment(1, 0, 0, 0);
        halign.Add(hbox);

        vbox.PackStart(halign, false, false, 3);

        Add(vbox);

        ShowAll();
    }
}