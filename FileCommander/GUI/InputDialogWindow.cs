namespace FileCommander.GUI;

using Gtk;
using Gdk;

public class InputDialogWindow : Gtk.Window
{
    public static string path;

    public InputDialogWindow(string title) : base(title)
    {
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);
        TypeHint = WindowTypeHint.Dialog;
        BorderWidth = 7;
        DeleteEvent += delegate { Application.Quit(); };

        /*
        Entry inputField = new Entry();
        Button ok = new Button("Create");
        //ok.SetSizeRequest(70, 30);
        Button close = new Button("Cancel");

        ok.Clicked += OnCreateClicked;
        close.Clicked += delegate { Application.Quit(); };

        HBox buttonBox = new HBox(true, 3);
        buttonBox.Add(ok);
        buttonBox.Add(close);

        Alignment halign = new Alignment(1, 0, 0, 0);
        halign.Add(buttonBox);

        Fixed fix = new Fixed();
        fix.Put(inputField, 60, 100);
        fix.Put(new Label("Folder Name"), 60, 40);

        Add(fix);

        ShowAll();*/
        
        VBox vbox = new VBox(false, 5);
        Add(vbox);
        
        
        Button ok = new Button("OK");
        ok.SetSizeRequest(70, 30);
        Button close = new Button("Close");
        
        HBox hbox = new HBox(true, 3);
        hbox.Add(ok);
        hbox.Add(close);
        
        Alignment halign = new Alignment(1, 0, 0, 0);
        halign.Add(hbox);
        
        Alignment valign = new Alignment(0, 1, 0, 0);
        vbox.PackStart(valign, false, false, 0);
        vbox.PackStart(halign, false, false, 3);


        ShowAll();
    }

    void OnCreateClicked(object sender, EventArgs args)
    {
        Entry entry = (Entry) sender;
        path = entry.Text;
    }
}