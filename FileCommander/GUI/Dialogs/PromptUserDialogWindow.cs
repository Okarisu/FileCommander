namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;

public class PromptUserDialogWindow : Dialog
{
    public PromptUserDialogWindow(string prompt)
    {
        MessageDialog md = new MessageDialog(this, 
            DialogFlags.DestroyWithParent, MessageType.Info, 
            ButtonsType.Close, prompt);
        md.Run();
        md.Destroy();
    }
}