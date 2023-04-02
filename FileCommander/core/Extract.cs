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
        new ProgressDialogWindow("Extracting...");
        foreach (var item in items)
        {
            //Přeskočení souborů, které nejsou archivy (CG)
            if (!item.Name!.EndsWith(".zip") || item.IsDirectory)
            {
                notArchiveFilesOccured = true;
                continue;
            }

            var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
            var filename = cleanFilename[0]; //jméno souboru bez koncovky
            if (cleanFilename.Length > 2) //Případ, kdy je v názvu souboru tečka
            {
                for (var i = 0; i < cleanFilename.Length - 2; i++)
                    filename += "." + cleanFilename[i];
            }

            var targetDirectoryPath = Path.Combine(promptedTarget.root, filename);

            if (Directory.Exists(targetDirectoryPath))
            {
                duplicateArchiveFilesOccured = true;
                continue;
            }

            var handler = new ProcessHandler(item.Path, targetDirectoryPath, false);
            var thread = new Thread(handler.Extract);
            thread.Start();

            while (thread.IsAlive)
            {
                while (Application.EventsPending())
                    Application.RunIteration();
            }

            RefreshIconViews();
        }

        if (notArchiveFilesOccured)
            new PromptUserDialogWindow("Several files were not a .zip archive.");
        if (duplicateArchiveFilesOccured)
            new PromptUserDialogWindow("Several directories with the same name already exist.");
        
        //Následující řádek byl generován GitHub Copilotem
        new PromptUserDialogWindow("Extraction finished.");
    }
}