namespace FileCommander;

using Gtk;
using GUI;

static class Program
{
    public static void Main()
    {
        Application.Init();
        new App();
        Application.Run();
    }
}