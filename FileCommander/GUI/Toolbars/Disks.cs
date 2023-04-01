using Gtk;
using Style = Pango.Style;

namespace FileCommander.GUI.Toolbars;

using static App;
using System.Runtime.InteropServices;

public abstract class Disks
{
    private static string GetMountLocation()
    {
        var mountLocation = Settings.GetConfStr("DefaultLinuxDriveMountLocation");
        if (mountLocation.Contains("{UserName}"))
        {
            mountLocation = mountLocation.Replace("{UserName}", Environment.UserName);
        }

        return Directory.Exists(mountLocation) ? mountLocation : "";
    }

    public static Toolbar DrawDiskBar(Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward,
        DirectoryInfo root, ListStore store, Label rootLabel)
    {
        var diskBar = new Toolbar();
        var diskButtons = new List<ToolButton>();
        diskBar.ToolbarStyle = ToolbarStyle.Both;
        DirectoryInfo MNT;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            foreach (var mnt in new DirectoryInfo(GetMountLocation()).GetDirectories())
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
                var diskButton = new ToolButton(new Image(Stock.Harddisk, IconSize.SmallToolbar), drive.VolumeLabel);
                diskButton.Clicked += (_, _) =>
                {
                    history.Push(root);
                    historyForward.Clear();
                    root = new DirectoryInfo(drive.VolumeLabel);
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