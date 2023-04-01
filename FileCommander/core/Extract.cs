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
    public static void OnExtractClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }


        var promptedTarget = TargetController.GetTargetPanel("Extract");
        if (promptedTarget.cancel) return;

        var notArchiveFilesOccured = false;
        var duplicateArchiveFilesOccured = false;
        foreach (var item in items)
        {
            var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
            var filename = cleanFilename[0]; //jméno souboru bez koncovky
            if (cleanFilename.Length > 2) //Případ, kdy je v názvu souboru tečka
            {
                for (var i = 0; i < cleanFilename.Length - 2; i++)
                    filename += "." + cleanFilename[i];
            }

            var targetDirectoryPath = Path.Combine(promptedTarget.root, filename);
            if (!item!.IsDirectory && item.Name!.EndsWith(".zip"))
            {
                if (Directory.Exists(targetDirectoryPath))
                {
                    duplicateArchiveFilesOccured = true;
                    continue;
                }
                var _handler = new FileHandler(item.Path, targetDirectoryPath, false);
                var _thread = new Thread(_handler.Extract);
                _thread.Start();

                while (_thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }
            }
            else
            {
                notArchiveFilesOccured = true;
            }
        }

        if (notArchiveFilesOccured)
            new PromptUserDialogWindow("Several files were not a .zip archive.");
        if (duplicateArchiveFilesOccured)
            new PromptUserDialogWindow("Several directories with the same name already exist.");
        RefreshIconViews();
    }
}