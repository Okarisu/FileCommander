using FileCommander.GUI;
using Gtk;

namespace FileCommander;

abstract class Program
{
    /*
     * First steps in GTK#. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-03].
     * Dostupné z: https://zetcode.com/gui/gtksharp/firststeps/
     * Převzato v plném rozsahu.
     */
    public static void Main()
    {
        Application.Init();
        new App();
        Application.Run();
    }
}