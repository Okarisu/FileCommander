// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global
namespace FileCommander.core;

using GUI;
using static GUI.App;
using static GUI.FunctionController;

public partial class Core
{
    public static void OnRenameClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;
        (string Name, bool Cancel, bool addSuffix) newFilename;
        if (items.Length == 1)
        {
            newFilename = GetPath("Rename to...", false);
        }
        else
        {
            newFilename = GetPath("Rename to...", true);
        }

        if (newFilename.Cancel)
        {
            return;
        }

        var destinationPath = (GetFocusedWindow() == 1 ? LeftRoot : RightRoot).ToString();

        var fileSuffixes = new Queue<int>();
        var folderSuffixes = new Queue<int>();
        if (newFilename.addSuffix)
        {
            for (var i = 1; i <= items.Length; i++)
            {
                fileSuffixes.Enqueue(i);
                folderSuffixes.Enqueue(i);
            }
        }

        foreach (var item in items)
        {
            if (item!.IsDirectory)
            {
                var childDestinationPath = Path.Combine(destinationPath, newFilename.Name);
                if (newFilename.addSuffix)
                {
                    childDestinationPath += "_" + folderSuffixes.Dequeue();
                }

                Directory.Move(item.Path, childDestinationPath);
            }
            else
            {
                var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
                var childDestinationPath = Path.Combine(destinationPath, newFilename.Name + "." +
                    cleanFilename[cleanFilename.Length - 1]);
                if (newFilename.addSuffix)
                {
                    childDestinationPath = Path.Combine(destinationPath,
                        newFilename.Name + "_" + fileSuffixes.Dequeue() + "." +
                        cleanFilename[1]);
                }

                File.Move(item.Path, childDestinationPath);
            }
        }

        Refresh();
    }
}