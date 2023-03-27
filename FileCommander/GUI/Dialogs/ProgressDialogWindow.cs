namespace FileCommander.GUI.Dialogs;

using Gtk;

public class ProgressDialogWindow : Dialog
{
    public static Dialog _dialog;

    public ProgressDialogWindow(string prompt)
    {
        _dialog = new MessageDialog(this,
            DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Cancel, prompt);
        _dialog.Resizable = false;
        
        while(Application.EventsPending()) Application.RunIteration();
        _dialog.GrabFocus();

        _dialog.Response += delegate(object _, ResponseArgs args)
        {
            Console.WriteLine(args.ResponseId);
            if ((int) args.ResponseId == 1) //OK
            {
               
            }
            else
            {
                _dialog.Destroy();
            }
        };
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
        
        
    }
}