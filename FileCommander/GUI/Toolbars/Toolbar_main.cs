using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using static core.Core;

public class ToolbarMain
{
    public static Toolbar DrawToolbar()
    {
        var toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Both;

        var toolRefreshButton = new ToolButton(Stock.Refresh);
        toolbar.Insert(toolRefreshButton, 0);
        toolRefreshButton.Clicked += NavigationController.OnRefreshClicked!;

        var toolNewButton = new ToolButton(Stock.New);
        toolbar.Insert(toolNewButton, 6);
        toolNewButton.Clicked += OnNewClicked!;

        var toolCopyButton = new ToolButton(Stock.Copy);
        toolbar.Insert(toolCopyButton, 7);
        toolCopyButton.Clicked += OnCopyClicked!;

        var toolMoveButton = new ToolButton(Stock.Dnd); //Icon TBA
        toolbar.Insert(toolMoveButton, 8);
        toolMoveButton.Clicked += OnMoveClicked!;

        var toolRenameButton = new ToolButton(Stock.Edit); //Icon TBA
        toolbar.Insert(toolRenameButton, 9);
        toolRenameButton.Clicked += OnRenameClicked!;

        var toolDeleteButton = new ToolButton(Stock.Delete);
        toolbar.Insert(toolDeleteButton, 10);
        toolDeleteButton.Clicked += OnDeleteClicked!;

        var toolExtractButton = new ToolButton(Stock.Execute); //Icon TBA
        toolbar.Insert(toolExtractButton, 12);
        toolExtractButton.Clicked += OnExtractClicked!;

        var toolCompressButton = new ToolButton(Stock.Execute); //Icon TBA
        toolbar.Insert(toolCompressButton, 12);
        toolCompressButton.Clicked += OnCompressClicked!;

        toolbar.Insert(new SeparatorToolItem(), 5);
        toolbar.Insert(new SeparatorToolItem(), 11);

        return toolbar;

    }
}