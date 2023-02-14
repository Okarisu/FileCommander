using FileCommander.GUI;

namespace FileCommander;
using Gtk;

static class Program
{

 public static void Main()
 {
  
  Application.Init();
  IconApp APP = new GUI.IconApp();
  Application.Run();
 }
}