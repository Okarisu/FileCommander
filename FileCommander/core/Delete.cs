// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.IO;
using FileCommander.GUI;
using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;
using Gtk;

namespace FileCommander.core;

using static App;
using static NavigationController;
public partial class Core
{
    public static void OnDeleteClicked(object sender, EventArgs e)
    {
        const string? promptDeleteKey = "PromptDelete";
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }
        var promptAskAgain = Settings.GetConf(promptDeleteKey);
        
        if (promptAskAgain)
        {
            new PromptConfirmDialogWindow("Are you sure?", "This action cannot be undone.", promptDeleteKey);
            var consent = PromptConfirmDialogWindow.IsConfirmed();
            if (!consent) return;
        }

        foreach (var item in items)
        {
            if(item.Path.Contains(Directory.GetCurrentDirectory()))
            {
                new PromptUserDialogWindow("Cannot delete system files.");
                continue;
            }
            if (item!.IsDirectory)
            {
                var handler = new FileHandler(item.Path, null, true);
                var thread = new Thread(handler.Delete);
                thread.Start();

                while (thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }

                //Directory.Delete(item.Path, true);
            }
            else
            {
                var handler = new FileHandler(item.Path, null, false);
                var thread = new Thread(handler.Delete);
                thread.Start();

                while (thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }

                //File.Delete(item.Path);
            }
        }

        RefreshIconViews();
        new PromptUserDialogWindow("Finished deleting files.");
    }
}