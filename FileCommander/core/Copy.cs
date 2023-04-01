// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI;
using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;
using Gtk;

namespace FileCommander.core;


using static App;
using static NavigationController;
using static PromptConfirmDialogWindow;

public partial class Core
{
    private const string? PromptCopyKey = "PromptDuplicitFileCopy";


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
        new ProgressDialogWindow("Files are being copied...");

        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item.Name!);
            var promptAskAgain = Settings.GetConf(PromptCopyKey);

            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "Directory with this name already exists.",
                            PromptCopyKey);
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

                var handler = new FileHandler(item.Path, childDestinationPath, true);
                var thread = new Thread(handler.Copy);
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
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "File with this name already exists.",
                            PromptCopyKey);
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

                var handler = new FileHandler(item.Path, childDestinationPath, false);
                var thread = new Thread(handler.Copy);
                thread.Start();

                while (thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }
            }
        }
        RefreshIconViews();
        new PromptUserDialogWindow("Finished copying files.");
    }

}