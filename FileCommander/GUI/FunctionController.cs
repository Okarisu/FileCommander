// ReSharper disable ObjectCreationAsStatement

using System.Globalization;

namespace FileCommander.GUI;

using System;
using System.IO;
using System.IO.Compression;
using Gtk;
using static App;
using static InputPathDialogWindow;

public abstract class FunctionController
{
    #region Navigation

    public static DirectoryInfo OnHomeClicked(EventArgs e, ListStore store)
    {
        var root = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(store, root);
        return root;
    }

    public static DirectoryInfo OnUpClicked(DirectoryInfo root, ListStore store)
    {
        if (root.Parent == null)
            return root;

        FillStore(store, root.Parent);
        return root.Parent;
    }

    public static void OnRefreshClicked(object sender, EventArgs e)
    {
        Refresh();
    }

    public static void OnBackClicked(object sender, EventArgs e)
    {
        //TODO logging historie cest? - Queue<Path>, to bude blivajz
    }

    public static void OnForwardClicked(object sender, EventArgs e)
    {
        //viz výše
    }

    public static void OnUndoClicked(object sender, EventArgs e)
    {
        //TODO logging provedených akcí - command pattern
        //https://stackoverflow.com/questions/3448943/best-design-pattern-for-undo-feature
    }

    public static void OnRedoClicked(object sender, EventArgs e)
    {
        //viz výše
    }

    #endregion

    #region Functions

    public static void OnNewClicked(object sender, EventArgs e)
    {
        var newFolderName = GetPath("New folder");

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), newFolderName.path);
        Directory.CreateDirectory(newDirectoryPath);
        Refresh();
    }

    public static void OnCopyClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
            return;
        }

        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString(); //Prohození cílové složky

        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);

            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                    childDestinationPath += String.Join("_moved_", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                RecursiveCopyDirectory(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                    childDestinationPath += String.Join("_moved_", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                File.Copy(item.Path, childDestinationPath);
            }
        }

        Refresh();
        new UserPromptDialogWindow("Finished copying files.");
    }

    /*
     * MICROSOFT. How to: Copy directories. Microsoft: Microsoft Learn [online]. [cit. 2023-03-11].
     * Dostupné z: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories.
     * Upraveno.
     */

    private static void RecursiveCopyDirectory(string sourceDir, string destinationDir)
    {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            RecursiveCopyDirectory(subDir.FullName, newDestinationDir);
        }
    }

    public static void OnMoveClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
            return;
        }

        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();


        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);
            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                    childDestinationPath += String.Join("_copy_", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                Directory.Move(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                    childDestinationPath += String.Join("_copy_", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                File.Move(item.Path, childDestinationPath);
            }
        }

        Refresh();
        new UserPromptDialogWindow("Finished moving files.");
    }

    public static void OnDeleteClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
            return;
        }

        new PromptConfirmDialogWindow("Are you sure?");
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
        new UserPromptDialogWindow("Finished deleting files.");
    }

    public static void OnRenameClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
            return;
        }

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;
        var newFilename = GetPath("Rename to...");

        var destinationPath = Path.Combine(root.ToString(), newFilename.path);

        if (newFilename.cancel)
        {
            return;
        }

        foreach (var item in items)
        {
            if (item!.IsDirectory)
            {
                Directory.Move(item.Path, Path.Combine(destinationPath, item.Name!));
            }
            else
            {
                File.Move(item.Path, Path.Combine(destinationPath, item.Name!));
            }
        }

        Refresh();
    }

    public static void OnExtractClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
            return;
        }

        var destinationPath = GetPath("Extract to...");

        if (destinationPath.cancel) return;

        if (!Directory.Exists(destinationPath.path))
        {
            Directory.CreateDirectory(destinationPath.path);
        }

        foreach (var item in items)
        {
            if (!item!.IsDirectory && item.Name!.EndsWith(".zip"))
            {
                ZipFile.ExtractToDirectory(item.Path, destinationPath.path);
            }

            Refresh();
        }
    }

    public static void OnCompressClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
            return;
        }

        var destinationPath = GetPath("Compress to...");

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

    #endregion

    private static (string path, bool cancel) GetPath(string dialogTitle)
    {
        new InputPathDialogWindow(dialogTitle);
        var path = InputPathDialogWindow.GetPath();
        NullPath();

        return (path.path, path.cancel);
    }

    private static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}