using FileCommander.GUI;

namespace FileCommander;

using Gtk;

static class Program
{
    public static void Main()
    {
        Application.Init();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        // ReSharper disable once ObjectCreationAsStatement
        new App();
        Application.Run();
        
        
    }
}