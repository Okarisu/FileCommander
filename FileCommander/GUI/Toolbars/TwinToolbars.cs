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

public abstract class TwinToolbars
{
    public static Toolbar DrawLeftToolbar()
    {
        var leftToolbar = new Toolbar();
        leftToolbar.ToolbarStyle = ToolbarStyle.Both;

        var leftHomeButton = new ToolButton(Stock.Home);
        leftToolbar.Insert(leftHomeButton, 0);
        leftHomeButton.Clicked += (sender, args) =>
        {
            LeftRoot = OnHomeClicked(LeftStore);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) =>
        {
            LeftRoot = OnUpClicked(LeftRoot, LeftStore);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };

        var leftBackButton = new ToolButton(Stock.GoBack);
        leftToolbar.Insert(leftBackButton, 2);
        leftBackButton.Clicked += (_, _) =>
        {
            LeftRoot = OnBackClicked(LeftRoot, LeftHistory, LeftHistoryForward, LeftStore);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };

        var leftForwardButton = new ToolButton(Stock.GoForward);
        leftToolbar.Insert(leftForwardButton, 3);
        leftForwardButton.Clicked += (sender, args) =>
        {
            LeftRoot = OnForwardClicked(LeftRoot, LeftHistory, LeftHistoryForward, LeftStore);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };

        return leftToolbar;
    }

    public static Toolbar DrawRightToolbar()
    {
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;

        var rightHomeButton = new ToolButton(Stock.Home);
        rightPanelBar.Insert(rightHomeButton, 0);
        rightHomeButton.Clicked += (_, args) =>
        {
            RightRoot = OnHomeClicked(RightStore);
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
            RightRoot = OnBackClicked(RightRoot, RightHistory, RightHistoryForward, RightStore);
            UpdateRootLabel(RightRootLabel, RightRoot);
        };

        var rightForwardButton = new ToolButton(Stock.GoForward);
        rightPanelBar.Insert(rightForwardButton, 3);
        rightForwardButton.Clicked += (_, _) =>
        {
            RightRoot = OnForwardClicked(RightRoot, RightHistory, RightHistoryForward, RightStore);
            UpdateRootLabel(RightRootLabel, RightRoot);
        };

        return rightPanelBar;
    }
}