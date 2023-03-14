namespace FileCommander.GUI;

using Gtk;

public class InputPathDialogWindow : Dialog
{
    private static Dialog _dialog { get; set; }
    private static string _path { get; set; }
    private static bool _cancel { get; set; }

    public InputPathDialogWindow(string title)
    {
        _dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);
        _dialog.Resizable = false;

        //var requestLabel = new Label("Folder name");
        //_dialog.ContentArea.PackStart(requestLabel, true, true, 0);
        var entry = new Entry();
        _dialog.ContentArea.PackStart(entry, true, true, 0);
        
        //TODO přidat validaci vstupu

        _dialog.Response += delegate(object o, ResponseArgs args)
        {
            if ((int) args.ResponseId == 1) //OK
            {
                _path = entry.Text;
            }
            else
            {
                NullPath();
                _cancel = true;
                _dialog.Destroy();
            }
        };
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }

    public static (string path, bool cancel) GetPath() => (_path, _cancel);

    public static void NullPath()
    {
        _path = "";
    }
}