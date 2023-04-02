namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;

public class ProgressDialogWindow : Dialog
{
    private static Dialog _dialog;

    /*
     * Dialogs in GTK#: Message dialogs. ZetCode [online]. [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gui/gtksharp/dialogs/
     * Upraveno.
     */
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