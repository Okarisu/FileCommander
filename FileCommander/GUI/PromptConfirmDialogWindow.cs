namespace FileCommander.GUI;

using Gtk;

public class PromptConfirmDialogWindow : Dialog
{
    private static Dialog _dialog;
    private static bool _isConfirmed = false;
    
    public PromptConfirmDialogWindow(string title, string prompt)
    {

        _dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);
        _dialog.Resizable = false;
        _dialog.DefaultSize = new Gdk.Size(150, 100);

        var requestLabel = new Label(prompt);
        _dialog.ContentArea.PackStart(requestLabel, true, true, 0);
        
        _dialog.Response += delegate(object o, ResponseArgs args)
        {
            if ((int) args.ResponseId == 1) //OK
            {
                _isConfirmed = true;
            }
            else
            {
                //TODO
            }
        };
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }
    
    public static bool IsConfirmed() => _isConfirmed;
}