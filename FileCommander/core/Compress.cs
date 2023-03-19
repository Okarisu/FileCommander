// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;

namespace FileCommander.core;

using GUI;
using static GUI.App;
using static NavigationController;
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

        var promptedTarget = TargetController.GetTargetPanel("Compress");
        if (promptedTarget.cancel) return;

        if (items.Length == 1 && items[0]!.IsDirectory)
        {
            ZipFile.CreateFromDirectory(items[0]!.Path, Path.Combine(promptedTarget.root, items[0]!.Name + ".zip"));
        }
        else
        {
            var archiveName = TargetController.GetTargetPath("Archive name:", false);
            if (archiveName.cancel) return;
            var tmpDirPath = Path.Combine(promptedTarget.root, archiveName.path);
            Directory.CreateDirectory(tmpDirPath);
            
            foreach (var item in items)
            {
                if (item!.IsDirectory)
                {
                    RecursiveCopyDirectory(item.Path, Path.Combine(tmpDirPath, item.Name!));
                }
                else
                {
                    File.Copy(item.Path, Path.Combine(tmpDirPath, item.Name!));
                }
            }

            ZipFile.CreateFromDirectory(tmpDirPath, Path.Combine(promptedTarget.root, archiveName.path + ".zip"));
            Directory.Delete(tmpDirPath, true);
        }
        
        Refresh();
    }
}