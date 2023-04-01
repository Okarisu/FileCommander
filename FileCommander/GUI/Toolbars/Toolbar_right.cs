using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using static App;
using static NavigationController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class ToolbarRight
{
    public static Toolbar DrawRightToolbar()
    {
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;

        var rightHomeButton = new ToolButton(Stock.Home);
        rightPanelBar.Insert(rightHomeButton, 0);
        rightHomeButton.Clicked += (_, args) =>
        {
            RightRoot = OnHomeClicked(args, RightStore);
            UpdateRootLabel(RightRootLabel, RightRoot);
        };

        var rightUpButton = new ToolButton(Stock.GoUp);
        rightPanelBar.Insert(rightUpButton, 1);
        rightUpButton.Clicked += (_, _) =>
        {
            RightRoot = OnUpClicked(RightRoot, RightStore);
            UpdateRootLabel(RightRootLabel, RightRoot);
        };

        var rightBackButton = new ToolButton(Stock.GoBack);
        rightPanelBar.Insert(rightBackButton, 2);
        rightBackButton.Clicked += (_, _) =>
        {
            var tmpRoot = OnBackClicked(RightRoot, RightHistory, RightHistoryForward, RightStore);
            RightRoot = tmpRoot != null ? tmpRoot : RightRoot;
            UpdateRootLabel(RightRootLabel, RightRoot);
        };

        var rightForwardButton = new ToolButton(Stock.GoForward);
        rightPanelBar.Insert(rightForwardButton, 3);
        rightForwardButton.Clicked += (_, _) =>
        {
            var tmpRoot = OnForwardClicked(RightRoot, RightHistory, RightHistoryForward, RightStore);
            RightRoot = tmpRoot != null ? tmpRoot : RightRoot;
            UpdateRootLabel(RightRootLabel, RightRoot);
        };

        return rightPanelBar;
    }
}