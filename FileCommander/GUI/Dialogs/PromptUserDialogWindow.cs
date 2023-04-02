namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;

public class PromptUserDialogWindow : Dialog
{
    /*
     * Dialogs in GTK#: Message dialogs. ZetCode [online]. [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gui/gtksharp/dialogs/
     * Upraveno.
     */
    public PromptUserDialogWindow(string prompt)
    {
        MessageDialog md = new MessageDialog(this, 
            DialogFlags.DestroyWithParent, MessageType.Info, 
            ButtonsType.Close, prompt);
        md.Run();
        md.Destroy();
    }
}