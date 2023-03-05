namespace FileCommander.GUI;

using Gtk;

public class InputDialogWindow : Dialog
{
    public static string _path { get; set; }

    private static Dialog _dialog { get; set; }

    public InputDialogWindow(string title)
    {
        _dialog = new Gtk.Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);

        var box = new VBox();
        _dialog.Add(box);

        var entry = new Entry();
        entry.Changed += OnEntry!;
        _dialog.ContentArea.PackStart(entry, true, true, 0);
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();

    }

    public Dialog GetDialog() => _dialog;

    void OnEntry(object sender, EventArgs args)
    {
        Entry entry = (Entry) sender;
        _path = entry.Text;
    }
}