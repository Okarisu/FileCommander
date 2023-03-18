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
    
    public static void OnCompressClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        var destinationPath = GetPath("Compress to...", false);

        foreach (var item in items)
        {
            if (item!.IsDirectory)
            {
                ZipFile.CreateFromDirectory(item.Path, Path.Combine(destinationPath.path, item.Name + ".zip"));
            }
            else
            {
                var tmpDir = item.Path + "_tmp";
                Directory.CreateDirectory(tmpDir);
                File.Copy(item.Path, Path.Combine(tmpDir, item.Name!));
                ZipFile.CreateFromDirectory(tmpDir, Path.Combine(destinationPath.path, item.Name + ".zip"));
                Directory.Delete(tmpDir, true);
            }
        }

        Refresh();
    }
}