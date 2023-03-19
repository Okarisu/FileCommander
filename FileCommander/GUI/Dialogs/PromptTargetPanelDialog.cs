using Gtk;

namespace FileCommander.GUI.Dialogs;

public class PromptTargetPanelDialog : Dialog
{
    private static Dialog _dialog { get; set; }
    private static bool _targetHere { get; set; }
    private static bool _cancel { get; set; }

    public PromptTargetPanelDialog(string operation)
    {
        _dialog = new Dialog(operation, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel,
            operation+" aside", ButtonsType.YesNo, operation+" here", ButtonsType.Ok);
        _dialog.Resizable = false;


        _dialog.Response += delegate(object _, ResponseArgs args)
        {
            switch ((int) args.ResponseId)
            {
                //OK
                case 1:
                    _targetHere = true;
                    break;
                //Yes
                case 2:
                    _targetHere = false;
                    break;
                default:
                    _cancel = true;
                    //_dialog.Destroy();
                    break;
            }
        };
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }

    public (bool targetHere, bool cancel) GetTargetPanel() => (_targetHere, _cancel);

    
}