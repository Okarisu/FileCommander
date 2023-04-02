using Gtk;

namespace FileCommander.GUI;

using static App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class TwinPanels
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
            LeftRoot = OnItemActivated(args, LeftRoot, LeftStore, LeftHistory, LeftHistoryForward);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };
        LeftIconView.FocusInEvent += (_, _) => SetFocusedPanel(1);
        
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
        RightIconView.FocusInEvent += (_, _) => SetFocusedPanel(2);

        RightScrolledWindow.Add(RightIconView);
    }
}