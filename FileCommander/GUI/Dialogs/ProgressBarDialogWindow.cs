namespace FileCommander.GUI.Dialogs;

using Gtk;

public class ProgressBarDialogWindow : Dialog
{
    public static Dialog _dialog;

    public ProgressBarDialogWindow(string prompt)
    {
        _dialog = new MessageDialog(this,
            DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.None, prompt);

        ProgressBar pb = new ProgressBar();
        pb.Pulse();
        _dialog.ContentArea.PackStart(pb, true, true, 0);
        _dialog.GrabFocus();

        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }


    public static void StartCopyBar()
    {
        new ProgressBarDialogWindow("Copying files");
        Thread.Sleep(5000);
        _dialog.Destroy();
    }
    public static void StartMoveBar()
    {
        new ProgressBarDialogWindow("Copying files");
    }
}