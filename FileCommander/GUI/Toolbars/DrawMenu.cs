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
    
    /*
     * Menus in GTK#: Simple menu. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gtksharp/menus/
     * Značně upraveno. 
     */
    public static MenuBar DrawMenuBar()
    {
        MenuBar menuBar = new MenuBar();
        Menu viewMenu = new Menu();
        MenuItem view = new MenuItem("View");
        view.Submenu = viewMenu;

        CheckMenuItem showHiddenFiles = new CheckMenuItem("Show hidden files");
        viewMenu.Append(showHiddenFiles);
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
        viewMenu.Append(showMountedDrives);
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


        menuBar.Append(view);

        return menuBar;
    }
}