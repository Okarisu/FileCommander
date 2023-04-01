using Gtk;

namespace FileCommander.GUI;

using static App;
public abstract class TwinPanels
{
    
    public static void DrawLeftPanel()
    {
        LeftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(store, root);

        LeftIconView.GrabFocus();
        LeftIconView.SelectionMode = SelectionMode.Multiple;
        LeftIconView.TextColumn = ColDisplayName;
        LeftIconView.PixbufColumn = ColPixbuf;
        LeftIconView.ItemActivated += (_, args) =>
        {
            root = OnItemActivated(args, root, store, LeftHistory, LeftHistoryForward);
            UpdateRootLabel(LeftRootLabel, root);
        };
        LeftIconView.FocusInEvent += (_, _) => FocusedPanel = 1;
        
        LeftScrolledWindow.Add(LeftIconView);
    }
    public static void DrawRightPanel()
    {
        RightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(RightStore, RightRoot);

        RightIconView.SelectionMode = SelectionMode.Multiple;
        RightIconView.TextColumn = ColDisplayName;
        RightIconView.PixbufColumn = ColPixbuf;
        RightIconView.ItemActivated += (_, args) =>
        {
            RightRoot = OnItemActivated(args, RightRoot, RightStore, RightHistory, RightHistoryForward);
            UpdateRootLabel(RightRootLabel, RightRoot);
        };
        RightIconView.FocusInEvent += (_, _) => FocusedPanel = 2;

        RightScrolledWindow.Add(RightIconView);
    }
}