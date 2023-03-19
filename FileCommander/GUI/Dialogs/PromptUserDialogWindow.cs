using Gtk;

namespace FileCommander.GUI.Dialogs;

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