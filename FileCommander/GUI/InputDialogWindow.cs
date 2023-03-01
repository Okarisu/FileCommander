namespace FileCommander.GUI;

using Gtk;

public class InputDialogWindow : Dialog
{
    public static string path;

    private static Dialog dialog { get; set; }

    public InputDialogWindow()
    {
        /*
        SetDefaultSize(250, 200);
        SetPosition(WindowPosition.Center);
        TypeHint = WindowTypeHint.Dialog;
        BorderWidth = 7;
        if (!Program.IS_RUNNING)
        {
            Application.Quit();
        }*/
        //DeleteEvent += delegate { Application.Quit(); };

        dialog =  new Gtk.Dialog("Select player",
            this, DialogFlags.DestroyWithParent,
            Stock.Cancel, ButtonsType.Cancel, Stock.Ok, ButtonsType.Ok);
        dialog.WidthRequest = 600;
        dialog.HeightRequest = 400;
        

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

        ShowAll();*/ /*

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
        
        /*
        DriveInfo[] drivesAvailable = DriveInfo.GetDrives();
        // Dictionary<string, Button> leftDriveButtons = new Dictionary<string, Button>();
        List<Button> leftDriveButtons = new List<Button>();
        //Dictionary<string, Button> rightDriveButtons = new Dictionary<string, Button>();
        
        foreach (var drive in drivesAvailable)
        {
            if (drive.IsReady)
            {
                Console.WriteLine("label: "+ drive.VolumeLabel + "name: "+ drive.Name);
                leftDriveButtons.Add(new Button(drive.VolumeLabel));
                //rightDriveButtons.Add(drive.VolumeLabel, new Button(drive.VolumeLabel));
            }
        }*/


        //ShowAll();
    }

    public Dialog GetDialog() => dialog;
    
    void OnCreateClicked(object sender, EventArgs args)
    {
        Entry entry = (Entry) sender;
        path = entry.Text;
    }
}