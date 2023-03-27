namespace FileCommander.GUI.Toolbars;

using Gtk;
public class DrawMenu
{
    public static MenuBar DrawMenuBar()
    {
        MenuBar menuBar = new MenuBar();
        Menu viewmenu = new Menu();
        MenuItem view = new MenuItem("View");
        view.Submenu = viewmenu;

        CheckMenuItem showHiddenFiles = new CheckMenuItem("Show hidden files");
        viewmenu.Append(showHiddenFiles);
        if(FileCommander.Settings.GetConf("ShowHiddenFiles"))
        {
            showHiddenFiles.Active = true;
        }
        else
        {
            showHiddenFiles.Active = false;
        }
        showHiddenFiles.Toggled += (sender, args) =>
        {
            if (showHiddenFiles.Active)
            {
                FileCommander.Settings.SetConf("ShowHiddenFiles", true);
            }
            else
            {
                FileCommander.Settings.SetConf("ShowHiddenFiles", false);
            }
            Controllers.NavigationController.RefreshIconViews();
        };
        
        CheckMenuItem showMountedDrives = new CheckMenuItem("Show mounted drives");
        viewmenu.Append(showMountedDrives);
        if(FileCommander.Settings.GetConf("ShowMountedDrives"))
        {
            showMountedDrives.Active = true;
        }
        else
        {
            showMountedDrives.Active = false;
        }
        showMountedDrives.Toggled += (sender, args) =>
        {
            if (showMountedDrives.Active)
            {
                FileCommander.Settings.SetConf("ShowMountedDrives", true);
                App.LeftDiskBar.Show();
                App.RightDiskBar.Show();
            }
            else
            {
                FileCommander.Settings.SetConf("ShowMountedDrives", false);
                App.LeftDiskBar.Hide();
                App.RightDiskBar.Hide();
            }
        };
        
        CheckMenuItem showMountLocation = new CheckMenuItem("Show mount location");
        viewmenu.Append(showMountLocation);
        showMountLocation.Toggled += (sender, args) =>
        {
            if (showMountLocation.Active)
            {
                FileCommander.Settings.SetConf("ShowMountLocation", true);
            }
            else
            {
                FileCommander.Settings.SetConf("ShowMountLocation", false);
            }
        };




        menuBar.Append(view);
        
        return menuBar;
    } 
}