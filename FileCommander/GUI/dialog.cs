namespace FileCommander.GUI;

using Gtk;
using System;

class DialogApp : Window
{

    Label label;

    public DialogApp() : base("Font Selection Dialog")
    {
        SetDefaultSize(300, 220);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        label = new Label("The only victory over love is flight.");
        Button button = new Button("Select font");
        button.Clicked += OnClicked;

        Fixed fix = new Fixed();
        fix.Put(button, 100, 30);
        fix.Put(label, 30, 90);
        Add(fix);

        ShowAll();
    }


    void OnClicked(object sender, EventArgs args)
    {
        FontSelectionDialog fdia = new FontSelectionDialog("Select font name");
        fdia.Response += delegate(object o, ResponseArgs resp)
        {

            if (resp.ResponseId == ResponseType.Ok)
            {
                Pango.FontDescription fontdesc =
                    Pango.FontDescription.FromString(fdia.FontName);
                label.ModifyFont(fontdesc);
            }
        };

        fdia.Run();
        fdia.Destroy();
    }
}