using FileCommander.GUI;
using Gtk;

namespace FileCommander;

abstract class Program
{
    public static void Main()
    {
        Application.Init();
        new App();

        Application.Run();
    }
}