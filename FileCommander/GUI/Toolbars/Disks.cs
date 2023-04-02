using Gtk;
namespace FileCommander.GUI.Toolbars;

using System;
using System.Collections.Generic;
using System.IO;
using static App;
using System.Runtime.InteropServices;

public abstract class Disks
{
    private static (bool available, string location) GetMountLocation()
    {
        var mountLocation = Settings.GetConfStr("DefaultLinuxDriveMountLocation");
        if (mountLocation.Contains("{UserName}"))
        {
            mountLocation = mountLocation.Replace("{UserName}", Environment.UserName);
        }

        return (Directory.Exists(mountLocation), Directory.Exists(mountLocation) ? mountLocation : "");
    }

    public static Toolbar DrawDiskBar(Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward,
        DirectoryInfo root, ListStore store, Label rootLabel)
    {
        var diskBar = new Toolbar();
        var diskButtons = new List<ToolButton>();
        diskBar.ToolbarStyle = ToolbarStyle.Both;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            var mount = GetMountLocation();
            if (!mount.available)
            {
                return diskBar;
            }
            foreach (var mnt in new DirectoryInfo(mount.location).GetDirectories())
            {
                var diskButton = new ToolButton(new Image(Stock.Harddisk, IconSize.SmallToolbar), mnt.Name);
                diskButton.Clicked += (_, _) =>
                {
                    history.Push(root);
                    historyForward.Clear();
                    root = new DirectoryInfo(mnt.FullName);
                    FillStore(store, root);
                    UpdateRootLabel(rootLabel, root);
                };
                diskButtons.Add(diskButton);
            }
        }
        else
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if(!drive.IsReady)
                    continue;
                
                var diskButton = new ToolButton(new Image(Stock.Harddisk, IconSize.SmallToolbar), drive.Name);
                diskButton.Clicked += (_, _) =>
                {
                    history.Push(root);
                    historyForward.Clear();
                    root = new DirectoryInfo(drive.Name);
                    FillStore(store, root);
                    UpdateRootLabel(rootLabel, root);
                };
                diskButtons.Add(diskButton);
            }
        }

        for (var i = 0; i < diskButtons.Count; i++)
        {
            diskBar.Insert(diskButtons[i], i);
        }

        return diskBar;
    }
}