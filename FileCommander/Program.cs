using FileCommander.GUI;

namespace FileCommander;

using Gtk;

static class Program
{
    public static bool IS_RUNNING;
    public static void Main()
    {
        Application.Init();
        var APP = new App();
        Application.Run();

        IS_RUNNING = APP.IsActive;
        
    }
}