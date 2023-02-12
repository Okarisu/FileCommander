namespace FileCommander;
using Gtk;

static class Program
{
 public static void Main()
 {
  Application.Init();
  new GUI.IconApp();
  Application.Run();

 }
}