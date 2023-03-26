namespace FileCommander.GUI.Dialogs;

using Gtk;

public class ProgressBarDialogWindow : Dialog
{
    public static Dialog _dialog;

    public ProgressBarDialogWindow(double progress)
    {
        _dialog = new MessageDialog(this,
            DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.None, "copying");
        ProgressBar pb = new ProgressBar();
        pb.Window.ProcessUpdates(true);
        while(Application.EventsPending()) Application.RunIteration();
        pb.Fraction = progress/100;
        pb.Pulse();
        _dialog.ContentArea.PackStart(pb, true, true, 0);
        _dialog.GrabFocus();

        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
        
        
    }
}