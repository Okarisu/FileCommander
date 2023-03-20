namespace FileCommander.GUI.TwinPanels;

using Gtk;
using static App;

public class RightTwinPanel
{
    public static void DrawRightPanel()
    {
        RightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(RightStore, RightRoot);

        RightIconView.SelectionMode = SelectionMode.Multiple;
        RightIconView.TextColumn = ColDisplayName;
        RightIconView.PixbufColumn = ColPixbuf;
        RightIconView.ItemActivated += (_, args) =>
        {
            RightRoot = OnItemActivated(args, RightRoot, RightStore);
            RightRootLabel.Text = "Current directory: "+RightRoot;
        };
        RightIconView.FocusInEvent += (_, _) => FocusedPanel = 2;

        RightScrolledWindow.Add(RightIconView);
    }
}