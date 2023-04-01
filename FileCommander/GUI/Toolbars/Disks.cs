namespace FileCommander.GUI.Toolbars;

using Gtk;
using static App;

public abstract class Disks
{
    private static string GetMountLocation()
    {
        var mountLocation = FileCommander.Settings.GetConfStr("DefaultLinuxDriveMountLocation");
        if (mountLocation.Contains("{UserName}"))
        {
            mountLocation = mountLocation.Replace("{UserName}", Environment.UserName);
        }

        return Directory.Exists(mountLocation) ? mountLocation : "";
    }

    public static Toolbar DrawDiskBar(Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward,
        DirectoryInfo root, ListStore store, Label rootLabel)
    {
        var mountLocation = GetMountLocation();
        var diskBar = new Toolbar();
        diskBar.ToolbarStyle = ToolbarStyle.Both;
        var diskButtons = new List<ToolButton>();

        foreach (var mnt in new DirectoryInfo(mountLocation).GetDirectories())
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

        for (var i = 0; i < diskButtons.Count; i++)
        {
            diskBar.Insert(diskButtons[i], i);
        }

        return diskBar;
    }
}