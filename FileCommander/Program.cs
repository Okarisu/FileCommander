using Gtk;
 using static FileCommander.GUI.IconApp; 

class Program
{
 public static void Main()
 {
  Application.Init();
  new FileCommander.GUI.DialogApp();
  Application.Run();

 }
}