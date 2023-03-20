using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using static App;
using static NavigationController;

public class ToolbarRight
{
    public static Toolbar DrawRightToolbar()
    {
        var rightPanelBar = new Toolbar();
        rightPanelBar.ToolbarStyle = ToolbarStyle.Both;

        var rightHomeButton = new ToolButton(Stock.Home);
        rightPanelBar.Insert(rightHomeButton, 0);
        rightHomeButton.Clicked += (sender, args) =>
        {
            RightRoot = OnHomeClicked(args, RightStore);
            RightRootLabel.Text = "Current directory: "+RightRoot;
        };

        var rightUpButton = new ToolButton(Stock.GoUp);
        rightPanelBar.Insert(rightUpButton, 1);
        rightUpButton.Clicked += (_, _) =>
        {
            RightRoot = OnUpClicked(RightRoot, RightStore);
            RightRootLabel.Text = "Current directory: "+RightRoot;
        };

        return rightPanelBar;
    }
}