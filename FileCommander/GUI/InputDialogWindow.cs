namespace FileCommander.GUI;

using Gtk;

public class InputDialogWindow : Dialog
{
    private static string _path { get; set; }

    private static Dialog _dialog { get; set; }

    public InputDialogWindow(string title)
    {
        _dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);

        var box = new VBox();
        _dialog.Add(box);

        var entry = new Entry();
        //TODO přidat validaci vstupu
        //TODO nechat jako delegate, nebo změnit na lambdu?
        _dialog.Response += delegate(object o, ResponseArgs args)
        {
            Console.WriteLine(args.ResponseId.ToString());
            if ((int) args.ResponseId == 1) //OK
            {
                _path = entry.Text;
            }
            else
            {
                NullPath();
            }
        };
        _dialog.ContentArea.PackStart(entry, true, true, 0);
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }

    public static string GetPath() => _path;

    public static void NullPath()
    {
        _path = "";
    }
}