using System.Runtime.InteropServices.ComTypes;
using Gtk;

namespace FileCommander.GUI.Dialogs;

public class PromptTargetPanelDialog : Dialog
{
    public const int ResponseIdOk = 1;
    private const int ResponseIdYes = 4;
    private static Dialog _dialog { get; set; }
    private static bool _targetHere { get; set; }
    private static bool _cancel = false;

    public PromptTargetPanelDialog(string operation)
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