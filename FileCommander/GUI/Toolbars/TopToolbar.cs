using FileCommander.core;
using FileCommander.GUI.Controllers;
using Gtk;

namespace FileCommander.GUI.Toolbars;

using static Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class TopToolbar
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

        var toolMoveButton = new ToolButton(new Image(Stock.File, IconSize.SmallToolbar), "Move"); 
        toolbar.Insert(toolMoveButton, 8);
        toolMoveButton.Clicked += OnMoveClicked!;

        var toolRenameButton = new ToolButton(new Image(Stock.Edit, IconSize.SmallToolbar), "Rename");
        toolbar.Insert(toolRenameButton, 9);
        toolRenameButton.Clicked += OnRenameClicked!;

        var toolDeleteButton = new ToolButton(Stock.Delete);
        toolbar.Insert(toolDeleteButton, 10);
        toolDeleteButton.Clicked += OnDeleteClicked!;

        var toolExtractButton = new ToolButton(new Image(Stock.GoUp, IconSize.SmallToolbar), "Extract");
        toolbar.Insert(toolExtractButton, 12);
        toolExtractButton.Clicked += OnExtractClicked!;

        var toolCompressButton = new ToolButton(new Image(Stock.GoDown, IconSize.SmallToolbar), "Compress");
        toolbar.Insert(toolCompressButton, 13);
        toolCompressButton.Clicked += OnCompressClicked!;

        toolbar.Insert(new SeparatorToolItem(), 5);
        toolbar.Insert(new SeparatorToolItem(), 11);

        return toolbar;

    }
}