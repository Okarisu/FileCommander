using Object = Atk.Object;

namespace FileCommander.GUI.Toolbars;

using Gtk;
using static App;

public class Disks
{
    private static string _mountLocation;

    public static bool CheckDisksAvailable()
    {
        var mountLocation = FileCommander.Settings.GetConfStr("DefaultLinuxDriveMountLocation");
        if (mountLocation.Contains("{UserName}"))
        {
            mountLocation = mountLocation.Replace("{UserName}", Environment.UserName);
        }

        _mountLocation = mountLocation;
        return Directory.Exists(mountLocation);
    }

    public static Toolbar DrawLeftDiskBar()
    {
        var leftDiskBar = new Toolbar();
        leftDiskBar.ToolbarStyle = ToolbarStyle.Both;
        var leftDiskButtons = new List<ToolButton>();

        foreach (var mnt in new DirectoryInfo(_mountLocation).GetDirectories())
        {
            var diskButton = new ToolButton(new Image(Stock.Harddisk, IconSize.SmallToolbar), mnt.Name);
            diskButton.Clicked += (_, _) =>
            {
                LeftHistory.Push(LeftRoot);
                LeftHistoryForward.Clear();
                LeftRoot = new DirectoryInfo(mnt.FullName);
                FillStore(LeftStore, LeftRoot);
                UpdateRootLabel(LeftRootLabel, LeftRoot);
            };
            leftDiskButtons.Add(diskButton);
        }
        for(var i = 0; i < leftDiskButtons.Count; i++)
        {
            leftDiskBar.Insert(leftDiskButtons[i], i);
        }

        return leftDiskBar;
    }
    public static Toolbar DrawRightDiskBar()
    {
        var rightDiskBar = new Toolbar();
        rightDiskBar.ToolbarStyle = ToolbarStyle.Both;
        var leftDiskButtons = new List<ToolButton>();

        foreach (var mnt in new DirectoryInfo(_mountLocation).GetDirectories())
        {
            var diskButton = new ToolButton(new Image(Stock.Harddisk, IconSize.SmallToolbar), mnt.Name);
            diskButton.Clicked += (_, _) =>
            {
                RightHistory.Push(RightRoot);
                RightHistoryForward.Clear();
                RightRoot = new DirectoryInfo(mnt.FullName);
                FillStore(RightStore, RightRoot);
                UpdateRootLabel(RightRootLabel, RightRoot);
            };
            leftDiskButtons.Add(diskButton);
        }
        
        for(var i = 0; i < leftDiskButtons.Count; i++)
        {
            rightDiskBar.Insert(leftDiskButtons[i], i);
        }

        return rightDiskBar;
    }
}