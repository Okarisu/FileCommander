namespace FileCommander.GUI;

using Gtk;
using Gdk;

public class DialogWindow : FileChooserDialog
{
    public static string path;

    public DialogWindow(string title)
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

        */

        //var fc = new FileChooserDialog("Open...", this, FileChooserAction.Open,"Cancel", ResponseType.Cancel, "Open", ResponseType.Ok);
        
        //fc.Run();
        //fc.Destroy();


            
        ShowAll();
    }

    void OnCreateClicked(object sender, EventArgs args)
    {
        Entry entry = (Entry) sender;
        path = entry.Text;
    }
}