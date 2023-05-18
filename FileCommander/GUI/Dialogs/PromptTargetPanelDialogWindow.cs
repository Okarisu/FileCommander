namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;

public class PromptTargetPanelDialogWindow : Dialog
{
    public const int ResponseIdOk = 1;
    private const int ResponseIdYes = 4;
    private static Dialog _dialog { get; set; }
    private static bool _targetHere { get; set; }
    private static bool _cancel;

    /*
     * Při tvorbě jsem se inspiroval zde:
     * Dialogs in GTK#: Message dialogs. ZetCode [online]. [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gui/gtksharp/dialogs/
     * Upraveno.
     */
    public PromptTargetPanelDialogWindow(string operation)
    {
        _dialog = new Dialog(operation, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel,
            operation+" aside", ButtonsType.YesNo, operation+" here", ButtonsType.Ok);
        _dialog.Resizable = false;


        _dialog.Response += delegate(object _, ResponseArgs args)
        {
            switch ((int) args.ResponseId)
            {
                case ResponseIdOk:
                    _targetHere = true;
                    break;
                case ResponseIdYes:
                    _targetHere = false;
                    break;
                default:
                    _cancel = true;
                    break;
            }
        };
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }

    public (bool targetHere, bool cancel) GetTargetPanel() => (_targetHere, _cancel);

    public static void NullPrompt()
    {
        _cancel = false;
    }
}