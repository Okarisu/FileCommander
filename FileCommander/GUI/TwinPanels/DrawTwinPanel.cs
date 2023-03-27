namespace FileCommander.GUI;

using Gtk;
using static App;

public class TwinPanel
{
    public static void DrawPanel(ScrolledWindow scrolledWindow, IconView iconView, ListStore store, DirectoryInfo root, Label label, Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward, int focus)
    {
        scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        FillStore(store, root);

        iconView.GrabFocus();
        iconView.SelectionMode = SelectionMode.Multiple;
        iconView.TextColumn = ColDisplayName;
        iconView.PixbufColumn = ColPixbuf;
        iconView.ItemActivated += (_, args) =>
        {
            root = OnItemActivated(args, root, store, history, historyForward);
            UpdateRootLabel(label, root);
        };
        iconView.FocusInEvent += (_, _) => FocusedPanel = focus;
        
        scrolledWindow.Add(iconView);
    }
}