// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI;
using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;
using Gtk;
using System;
using System.IO;

namespace FileCommander.core;

using static App;
using static NavigationController;

public partial class Core
{
    public static void OnMoveClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();
        var duplicateFilesOccured = false;

        foreach (var item in items)
        {
            if (item.Path.Contains(Directory.GetCurrentDirectory()))
            {
                new PromptUserDialogWindow("Cannot move system files.");
                continue;
            }

            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);
            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                {
                    duplicateFilesOccured = true;
                    continue;
                }

                var handler = new FileHandler(item.Path, childDestinationPath, true);
                var thread = new Thread(handler.Move);
                thread.Start();

                while (thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }
            }
            else
            {
                if (File.Exists(childDestinationPath))
                {
                    duplicateFilesOccured = true;
                    continue;
                }

                var handler = new FileHandler(item.Path, childDestinationPath, false);
                var thread = new Thread(handler.Move);
                thread.Start();

                while (thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }
            }
        }


        RefreshIconViews();
        if (duplicateFilesOccured)
            new PromptUserDialogWindow("File(s) with the same name already exists.");
        new PromptUserDialogWindow("Finished moving files.");
    }
}