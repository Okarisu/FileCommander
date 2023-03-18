// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global
namespace FileCommander.core;

using GUI;
using static GUI.App;
using static GUI.FunctionController;
public partial class Core
{
    public static void OnDeleteClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        new PromptConfirmDialogWindow("Are you sure?", "This action cannot be undone.", "PromptDeletion");
        var consent = PromptConfirmDialogWindow.IsConfirmed();
        if (!consent) return;

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

        Refresh();
        new PromptUserDialogWindow("Finished deleting files.");
    }
}