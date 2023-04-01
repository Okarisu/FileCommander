using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class DrawMenu
{
    public static MenuBar DrawMenuBar()
    {
        MenuBar menuBar = new MenuBar();
        Menu viewmenu = new Menu();
        MenuItem view = new MenuItem("View");
        view.Submenu = viewmenu;

        CheckMenuItem showHiddenFiles = new CheckMenuItem("Show hidden files");
        viewmenu.Append(showHiddenFiles);
        if (Settings.GetConf("ShowHiddenFiles"))
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
                Settings.SetConf("ShowHiddenFiles", true);
            }
            else
            {
                Settings.SetConf("ShowHiddenFiles", false);
            }

            NavigationController.RefreshIconViews();
        };

        CheckMenuItem showMountedDrives = new CheckMenuItem("Show mounted drives");
        viewmenu.Append(showMountedDrives);
        if (Settings.GetConf("ShowMountedDrives"))
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
                Settings.SetConf("ShowMountedDrives", true);
                App.LeftDiskBar.Show();
                App.RightDiskBar.Show();
            }
            else
            {
                Settings.SetConf("ShowMountedDrives", false);
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
                Settings.SetConf("ShowMountLocation", true);
            }
            else
            {
                Settings.SetConf("ShowMountLocation", false);
            }
        };


        menuBar.Append(view);

        return menuBar;
    }
}