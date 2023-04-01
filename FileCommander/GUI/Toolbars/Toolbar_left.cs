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
            root = OnHomeClicked(args, store);
            UpdateRootLabel(LeftRootLabel, root);
        };

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) =>
        {
            root = OnUpClicked(root, store);
            UpdateRootLabel(LeftRootLabel, root);
        };
        
        var leftBackButton = new ToolButton(Stock.GoBack);
        leftToolbar.Insert(leftBackButton, 2);
        leftBackButton.Clicked += (_,_) =>
        {
            var tmpRoot = OnBackClicked(root, LeftHistory, LeftHistoryForward, store);
            root = tmpRoot != null ? tmpRoot : root;
            UpdateRootLabel(LeftRootLabel, root);
        };
        
        var leftForwardButton = new ToolButton(Stock.GoForward);
        leftToolbar.Insert(leftForwardButton, 3);
        leftForwardButton.Clicked += (sender, args) =>
        {
            var tmpRoot = OnForwardClicked(root, LeftHistory, LeftHistoryForward, store);
            root = tmpRoot != null ? tmpRoot : root;
            UpdateRootLabel(LeftRootLabel, root);
        };

        return leftToolbar;
    }
}