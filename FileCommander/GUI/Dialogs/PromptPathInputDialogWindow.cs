namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;
public class PromptPathInputDialogWindow : Dialog
{
    private static Dialog _dialog { get; set; }
    private static string _path { get; set; }
    private static bool _cancel { get; set; }

    private static bool _addSuffix { get; set; }

    /*
     * Při tvorbě jsem se inspiroval zde:
     * Dialogs in GTK#: Message dialogs. ZetCode [online]. [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gui/gtksharp/dialogs/
     * Upraveno.
     */
    public PromptPathInputDialogWindow(string title, bool promptSuffix)
    {
        _dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);
        _dialog.Resizable = false;

        if (promptSuffix)
        {
            var requestLabel = new Label("Multiple files selected. Suffix will be added to new files.");
            _dialog.ContentArea.PackStart(requestLabel, true, true, 0);
        }

        var entry = new Entry();
        _dialog.ContentArea.PackStart(entry, true, true, 0);

        _dialog.Response += delegate(object _, ResponseArgs args)
        {
            if ((int) args.ResponseId == 1) //OK
            {
                _path = entry.Text;
                if (promptSuffix)
                    _addSuffix = true;
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

    public static (string path, bool cancel, bool addSuffix) GetPath() => (_path, _cancel, _addSuffix);

    public static void NullPath()
    {
        _path = "";
    }
}