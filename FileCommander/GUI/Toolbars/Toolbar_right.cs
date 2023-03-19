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
        rightHomeButton.Clicked += (sender, args) => RightRoot = OnHomeClicked(args, RightStore);

        var rightUpButton = new ToolButton(Stock.GoUp);
        rightPanelBar.Insert(rightUpButton, 1);
        rightUpButton.Clicked += (_, _) => RightRoot = OnUpClicked(RightRoot, RightStore);

        rightPanelBar.Insert(new SeparatorToolItem(), 2);

        var rightToolBackButton = new ToolButton(Stock.GoBack);
        rightPanelBar.Insert(rightToolBackButton, 3);
        rightToolBackButton.Clicked += OnBackClicked!;

        var rightToolForwardButton = new ToolButton(Stock.GoForward);
        rightPanelBar.Insert(rightToolForwardButton, 4);
        rightToolForwardButton.Clicked += OnForwardClicked!;

        var rightToolUndoButton = new ToolButton(Stock.Undo);
        rightPanelBar.Insert(rightToolUndoButton, 5);
        rightToolUndoButton.Clicked += OnUndoClicked!;


        var rightToolRedoButton = new ToolButton(Stock.Redo);
        rightPanelBar.Insert(rightToolRedoButton, 6);
        rightToolRedoButton.Clicked += OnRedoClicked!;

        return rightPanelBar;
    }
}