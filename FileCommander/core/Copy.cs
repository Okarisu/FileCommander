// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;

namespace FileCommander.core;

using GUI;
using static GUI.App;
using static NavigationController;
using static PromptConfirmDialogWindow;

public partial class Core
{
    const string promptCkey = "PromptDuplicitFileCopy";
        public static void OnCopyClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        //TODO copy here or there
        //var destinationPath = GetTargetPath("Compress to...", false, true);
        //Fukus na levém panelu => přesouvá se do pravého
        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();

        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);
            var promptAskAgain = Settings.GetConf(promptCkey);

            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "Directory with this name already exists.",
                            promptCkey);
                        var consent = IsConfirmed();
                        if (!consent) continue;
                    }

                    childDestinationPath += " (copy)";
                }

                RecursiveCopyDirectory(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "File with this name already exists.",
                            promptCkey);
                        var consent = IsConfirmed();
                        if (!consent) continue;
                    }

                    var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
                    var extension = cleanFilename[^1]; //koncovka souboru; ^1 = poslední prvek pole
                    var filename = cleanFilename[0]; //jméno souboru bez koncovky
                    if (cleanFilename.Length > 2) //Případ, kdy je v názvu souboru tečka
                    {
                        for(var i = 0; i < cleanFilename.Length - 2; i++)
                            filename += "." + cleanFilename[i];
                    }

                    childDestinationPath = Path.Combine(destinationPath,
                        filename + " (copy)." +
                        extension);
                }

                File.Copy(item.Path, childDestinationPath);
            }
        }

        Refresh();
        new PromptUserDialogWindow("Finished copying files.");
    }

    /*
     * MICROSOFT. How to: Copy directories. Microsoft: Microsoft Learn [online]. [cit. 2023-03-11].
     * Dostupné z: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories.
     * Upraveno.
     */

    private static void RecursiveCopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        var dir = new DirectoryInfo(sourceDirectory);

        // Cache directories before start of copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        Directory.CreateDirectory(destinationDirectory);

        foreach (FileInfo file in dir.GetFiles())
        {
            var targetFilePath = Path.Combine(destinationDirectory, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDirectory, subDir.Name);
            RecursiveCopyDirectory(subDir.FullName, newDestinationDir);
        }
    }
    
    /* Konec citace */
}