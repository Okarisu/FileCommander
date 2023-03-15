namespace FileCommander.GUI;
using Gtk;

public class UserPromptDialogWindow : Dialog
{
    public UserPromptDialogWindow(string prompt)
    {
        MessageDialog md = new MessageDialog(this, 
            DialogFlags.DestroyWithParent, MessageType.Info, 
            ButtonsType.Close, prompt);
        md.Run();
        md.Destroy();
    }
}