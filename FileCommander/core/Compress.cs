// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

namespace FileCommander.core;

using GUI.Controllers;
using GUI.Dialogs;
using static GUI.App;
using static GUI.Controllers.NavigationController;
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
            var targetPath = Path.Combine(promptedTarget.root, items[0]!.Name + ".zip");
            if (File.Exists(targetPath))
            {
                new PromptUserDialogWindow("Archive with this name already exists.");
            }
            else
            {
                ZipFile.CreateFromDirectory(items[0]!.Path, targetPath);
            }
        }
        else
        {
            var archiveName = TargetController.GetTargetPath("Archive name:", false);
            if (archiveName.cancel) return;
            var archiveTargetPath = Path.Combine(promptedTarget.root, archiveName.path + ".zip");

            if (File.Exists(archiveTargetPath))
            {
                new PromptUserDialogWindow("Archive with this name already exists.");
                return;
            }
            
            var tmpDirPath = Path.Combine(promptedTarget.root, archiveName.path + "_tmp_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds());

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
            ZipFile.CreateFromDirectory(tmpDirPath, archiveTargetPath);
            Directory.Delete(tmpDirPath, true);
        }

        Refresh();
    }
}