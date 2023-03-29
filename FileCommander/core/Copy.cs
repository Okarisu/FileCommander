// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;
using Gtk;
using Microsoft.VisualBasic.FileIO;

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


        //Fukus na levém panelu => přesouvá se do pravého
        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();

        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item.Name!);
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

                    var foldersFound = new DirectoryInfo(destinationPath);
                    int duplicateFolders = 0;
                    foreach (DirectoryInfo dir in foldersFound.GetDirectories())
                    {
                        if (dir.Name.Contains(item.Name!))
                            duplicateFolders++;
                    }

                    childDestinationPath += $" ({duplicateFolders})";

                    while (File.Exists(childDestinationPath))
                    {
                        duplicateFolders++;
                        childDestinationPath += $" ({duplicateFolders})";
                    }

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
                        for (var i = 0; i < cleanFilename.Length - 2; i++)
                            filename += "." + cleanFilename[i];
                    }


                    var filesFound = new DirectoryInfo(destinationPath);
                    int duplicateFiles = 0;
                    foreach (FileInfo file in filesFound.GetFiles())
                    {
                        if (file.Name.Contains(filename))
                            duplicateFiles++;
                    }

                    childDestinationPath =
                        Path.Combine(destinationPath, filename + $" ({duplicateFiles})." + extension);

                    while (File.Exists(childDestinationPath))
                    {
                        duplicateFiles++;
                        childDestinationPath =
                            Path.Combine(destinationPath, filename + $" ({duplicateFiles})." + extension);
                    }
                }
                
                var cpp = new FileHandler(item.Path, childDestinationPath);
                var the = new Thread(cpp.Copy);
                the.Start();

                var win = new ProgressDialogWindow("Files are being copied...");
                while (the.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }

                win.Destroy();
                //File.Copy(item.Path, childDestinationPath);
            }
        }

        
        RefreshIconViews();
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