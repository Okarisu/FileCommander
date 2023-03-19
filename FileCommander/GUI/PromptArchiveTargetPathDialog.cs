using Gtk;

namespace FileCommander.GUI;

public class PromptArchiveTargetPathDialog : Dialog
{
    private static Dialog _dialog { get; set; }
    private static bool _targetHere { get; set; }
    private static bool _cancel { get; set; }

    public PromptArchiveTargetPathDialog(string title)
    {
        _dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel,
            "Extract aside", ButtonsType.YesNo, "Extract here", ButtonsType.Ok);
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