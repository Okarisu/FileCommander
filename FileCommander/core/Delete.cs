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
    public static void OnDeleteClicked(object sender, EventArgs e)
    {
        const string promptDkey = "PromptDelete";
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }
        var promptAskAgain = Settings.GetConf(promptDkey);
        
        if (promptAskAgain)
        {
            new PromptConfirmDialogWindow("Are you sure?", "This action cannot be undone.", promptDkey);
            var consent = PromptConfirmDialogWindow.IsConfirmed();
            if (!consent) return;
        }

        foreach (var item in items)
        {
            if (item!.IsDirectory)
            {
                Directory.Delete(item.Path, true);
            }
            else
            {
                File.Delete(item.Path);
            }
        }

        RefreshIconViews();
        new PromptUserDialogWindow("Finished deleting files.");
    }
}