using FileCommander.GUI;

namespace FileCommander;

using Gtk;

static class Program
{
    public static bool IS_RUNNING;
    public static void Main()
    {
        Application.Init();
        var APP = new IconApp();
        Application.Run();

        IS_RUNNING = APP.IsActive;
        
    }
}