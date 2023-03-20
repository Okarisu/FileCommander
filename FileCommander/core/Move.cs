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

        var duplicityFilesOccured = false;

        //Thread thread = new Thread(ProgressBarDialogWindow.StartMoveBar);
        //thread.Start();
        
        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);
            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                {
                    duplicityFilesOccured = true;
                    continue;
                }

                Directory.Move(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                {
                    duplicityFilesOccured = true;
                    continue;
                }

                File.Move(item.Path, childDestinationPath);
            }

        }

        //thread.Interrupt();
        
        Refresh();
        if (duplicityFilesOccured)
            new PromptUserDialogWindow("Several file with the same name already exist.");
        new PromptUserDialogWindow("Finished moving files.");
    }
}