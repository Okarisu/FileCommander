using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using static App;
using static NavigationController;
public abstract class ToolbarLeft
{
    public static Toolbar DrawLeftToolbar()
    {
        var leftToolbar = new Toolbar();
        leftToolbar.ToolbarStyle = ToolbarStyle.Both;

        var leftHomeButton = new ToolButton(Stock.Home);
        leftToolbar.Insert(leftHomeButton, 0);
        leftHomeButton.Clicked += (sender, args) =>
        {
            LeftRoot = OnHomeClicked(args, store);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) =>
        {
            LeftRoot = OnUpClicked(LeftRoot, store);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };
        
        var leftBackButton = new ToolButton(Stock.GoBack);
        leftToolbar.Insert(leftBackButton, 2);
        leftBackButton.Clicked += (_,_) =>
        {
            var tmpRoot = OnBackClicked(LeftRoot, LeftHistory, LeftHistoryForward, store);
            LeftRoot = tmpRoot != null ? tmpRoot : LeftRoot;
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };
        
        var leftForwardButton = new ToolButton(Stock.GoForward);
        leftToolbar.Insert(leftForwardButton, 3);
        leftForwardButton.Clicked += (sender, args) =>
        {
            var tmpRoot = OnForwardClicked(LeftRoot, LeftHistory, LeftHistoryForward, store);
            LeftRoot = tmpRoot != null ? tmpRoot : LeftRoot;
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };

        return leftToolbar;
    }
}