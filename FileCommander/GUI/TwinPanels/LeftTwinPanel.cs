namespace FileCommander.GUI.TwinPanels;

using Gtk;
using static App;

public class LeftTwinPanel
{
    public static void DrawLeftPanel()
    {
        LeftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(LeftStore, LeftRoot);

        LeftIconView.GrabFocus();
        LeftIconView.SelectionMode = SelectionMode.Multiple;
        LeftIconView.TextColumn = ColDisplayName;
        LeftIconView.PixbufColumn = ColPixbuf;
        LeftIconView.ItemActivated += (_, args) =>
        {
            LeftRoot = OnItemActivated(args, LeftRoot, LeftStore);
            LeftRootLabel.Text = "Current directory: "+LeftRoot;
        };
        LeftIconView.FocusInEvent += (_, _) => FocusedPanel = 1;
        
        LeftScrolledWindow.Add(LeftIconView);
    }
}