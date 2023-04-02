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
    /*
     * Při tvorbě třídy jsem se inspiroval zde:
     * Toolbars in GTK#: Simple toolbar. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gtksharp/toolbars/
     * Upraveno. 
     */
    public static Toolbar DrawTopToolbar()
    {
        var toolbar = new Toolbar();
        toolbar.ToolbarStyle = ToolbarStyle.Both;

        var toolRefreshButton = new ToolButton(Stock.Refresh);
        toolbar.Insert(toolRefreshButton, 0);
        toolRefreshButton.Clicked += NavigationController.OnRefreshClicked!;

        toolbar.Insert(new SeparatorToolItem(), 1);
        
        var toolNewButton = new ToolButton(Stock.New);
        toolbar.Insert(toolNewButton, 2);
        toolNewButton.Clicked += OnNewClicked!;

        var toolCopyButton = new ToolButton(Stock.Copy);
        toolbar.Insert(toolCopyButton, 3);
        toolCopyButton.Clicked += OnCopyClicked!;

        var toolMoveButton = new ToolButton(new Image(Stock.File, IconSize.SmallToolbar), "Move"); 
        toolbar.Insert(toolMoveButton, 4);
        toolMoveButton.Clicked += OnMoveClicked!;

        var toolRenameButton = new ToolButton(new Image(Stock.Edit, IconSize.SmallToolbar), "Rename");
        toolbar.Insert(toolRenameButton, 5);
        toolRenameButton.Clicked += OnRenameClicked!;

        var toolDeleteButton = new ToolButton(Stock.Delete);
        toolbar.Insert(toolDeleteButton, 6);
        toolDeleteButton.Clicked += OnDeleteClicked!;

        toolbar.Insert(new SeparatorToolItem(), 7);
        
        var toolExtractButton = new ToolButton(new Image(Stock.GoUp, IconSize.SmallToolbar), "Extract");
        toolbar.Insert(toolExtractButton, 8);
        toolExtractButton.Clicked += OnExtractClicked!;

        var toolCompressButton = new ToolButton(new Image(Stock.GoDown, IconSize.SmallToolbar), "Compress");
        toolbar.Insert(toolCompressButton, 9);
        toolCompressButton.Clicked += OnCompressClicked!;


        return toolbar;

    }
}