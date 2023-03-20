// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;

namespace FileCommander.core;

using GUI;
using static GUI.App;
using static NavigationController;
using System.IO.Compression;

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
        var duplicityArchiveFilesOccured = false;
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
                    duplicityArchiveFilesOccured = true;
                    continue;
                }

                ZipFile.ExtractToDirectory(item.Path, targetDirectoryPath);
            }
            else
            {
                notArchiveFilesOccured = true;
            }
        }

        if (notArchiveFilesOccured)
            new PromptUserDialogWindow("Several files were not a .zip archive.");
        if (duplicityArchiveFilesOccured)
            new PromptUserDialogWindow("Several directories with the same name already exist.");
        Refresh();
    }
}