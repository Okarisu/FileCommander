namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;

public class ProgressDialogWindow : Dialog
{
    public static Dialog _dialog;

    public ProgressDialogWindow(string prompt)
    {
        _dialog = new MessageDialog(this,
            DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, prompt);
        _dialog.Resizable = false;
        
        while(Application.EventsPending())
            Application.RunIteration();
        
        _dialog.GrabFocus();
        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
        
        
    }
}