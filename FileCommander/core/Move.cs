// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;

namespace FileCommander.core;

using GUI;
using static GUI.App;
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

        //Thread thread = new Thread(ProgressBarDialogWindow.StartMoveBar);
        //thread.Start();
        
        foreach (var item in items)
        {
            if(item.Path.Contains(Directory.GetCurrentDirectory()))
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

                Directory.Move(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                {
                    duplicateFilesOccured = true;
                    continue;
                }

                File.Move(item.Path, childDestinationPath);
            }

        }

        //thread.Interrupt();
        
        RefreshIconViews();
        if (duplicateFilesOccured)
            new PromptUserDialogWindow("Several file with the same name already exist.");
        new PromptUserDialogWindow("Finished moving files.");
    }
}