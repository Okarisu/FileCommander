using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using static App;
using static NavigationController;
public class ToolbarLeft
{
    public static Toolbar DrawLeftToolbar()
    {
        var leftToolbar = new Toolbar();
        leftToolbar.ToolbarStyle = ToolbarStyle.Both;

        
        var leftHomeButton = new ToolButton(Stock.Home);
        leftToolbar.Insert(leftHomeButton, 0);
        leftHomeButton.Clicked += (sender, args) => LeftRoot = OnHomeClicked(args, LeftStore);

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) => LeftRoot = OnUpClicked(LeftRoot, LeftStore);

        leftToolbar.Insert(new SeparatorToolItem(), 2);

        var leftToolBackButton = new ToolButton(Stock.GoBack);
        leftToolbar.Insert(leftToolBackButton, 3);
        leftToolBackButton.Clicked += OnBackClicked!;

        var leftToolForwardButton = new ToolButton(Stock.GoForward);
        leftToolbar.Insert(leftToolForwardButton, 4);
        leftToolForwardButton.Clicked += OnForwardClicked!;

        var leftToolUndoButton = new ToolButton(Stock.Undo);
        leftToolbar.Insert(leftToolUndoButton, 5);
        leftToolUndoButton.Clicked += OnUndoClicked!;

        var leftToolRedoButton = new ToolButton(Stock.Redo);
        leftToolbar.Insert(leftToolRedoButton, 5);
        leftToolRedoButton.Clicked += OnRedoClicked!;

        return leftToolbar;
    }
}