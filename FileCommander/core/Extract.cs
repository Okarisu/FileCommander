// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global
namespace FileCommander.core;

using GUI;
using static GUI.App;
using static GUI.FunctionController;
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

        var promptedDestinationPath = GetPath("Extract to...", false
        );
        if (promptedDestinationPath.cancel) return;
        var root = (GetFocusedWindow() == 1 ? LeftRoot : RightRoot).ToString();

        var destinationPath = Path.Combine(root, promptedDestinationPath.path);

        if (!Directory.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationPath);
        }

        foreach (var item in items)
        {
            if (!item!.IsDirectory && item.Name!.EndsWith(".zip"))
            {
                ZipFile.ExtractToDirectory(item.Path, destinationPath);
            }

            Refresh();
        }
    }

}