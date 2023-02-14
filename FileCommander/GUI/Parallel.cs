namespace FileCommander.GUI;
using Gtk;

class ParallelApp : Window
{
    public ParallelApp() : base("File Commander")
    {
        //Defaults
        SetDefaultSize(720, 512);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };
        
        
        
        
    }
}
