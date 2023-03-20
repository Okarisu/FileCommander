namespace FileCommander;

using Gtk;
using GUI;
using static GUI.Dialogs.ProgressBarDialogWindow;

static class Program
{
    public static void Main()
    {
        
        Application.Init();
        new App();
        Application.Run();
        
    }

    
}