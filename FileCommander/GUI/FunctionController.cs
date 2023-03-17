// ReSharper disable ObjectCreationAsStatement

namespace FileCommander.GUI;

using System;
using System.IO;
using System.IO.Compression;
using Gtk;
using static App;
using static PromptPathInputDialogWindow;
using static Settings;

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
        var newFolderName = GetPath("New folder", false);

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), newFolderName.path);
        if (Directory.Exists(newDirectoryPath))
        {
            new UserPromptDialogWindow("Folder with this name already exists.");
            return;
        }
        else
        {
            Directory.CreateDirectory(newDirectoryPath);
        }

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

        //Fucus na levém panelu => přesouvá se do pravého
        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();

        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);
            bool promptAskAgain = GetBoolValueSetting("PromptDuplicitFileCopy");

            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "Directory with this name already exists.",
                            "PromptDuplicitFileCopy");
                        var consent = PromptConfirmDialogWindow.IsConfirmed();
                        if (!consent) continue;
                    }

                    childDestinationPath += "_copy_" + DateTime.Now.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss");
                }

                RecursiveCopyDirectory(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "File with this name already exists.",
                            "PromptDuplicitFileCopy");
                        var consent = PromptConfirmDialogWindow.IsConfirmed();
                        if (!consent) continue;
                    }

                    var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky

                    childDestinationPath = Path.Combine(destinationPath,
                        cleanFilename[0] + "_copy_" + DateTime.Now.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss") + "." +
                        cleanFilename[1]);
                }

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

    private static void RecursiveCopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        var dir = new DirectoryInfo(sourceDirectory);

        // Cache directories before start of copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        Directory.CreateDirectory(destinationDirectory);

        foreach (FileInfo file in dir.GetFiles())
        {
            var targetFilePath = Path.Combine(destinationDirectory, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDirectory, subDir.Name);
            RecursiveCopyDirectory(subDir.FullName, newDestinationDir);
        }
    }

    /** Konec citace **/
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
            bool promptAskAgain = GetBoolValueSetting("PromptDuplicitFileCopy");
            if (item.IsDirectory)
            {
                if (Directory.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "Directory with this name already exists.",
                            "PromptDuplicitMoveCopy");
                        var consent = PromptConfirmDialogWindow.IsConfirmed();
                        if (!consent) continue;
                    }

                    childDestinationPath += "_move_" + DateTime.Now.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss");
                }

                Directory.Move(item.Path, childDestinationPath);
            }
            else
            {
                if (File.Exists(childDestinationPath))
                {
                    if (promptAskAgain)
                    {
                        new PromptConfirmDialogWindow("Are you sure?", "File with this name already exists.",
                            "PromptDuplicitMoveCopy");
                        var consent = PromptConfirmDialogWindow.IsConfirmed();
                        if (!consent) continue;
                    }

                    var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
                    childDestinationPath = Path.Combine(destinationPath,
                        cleanFilename[0] + "_move_" + DateTime.Now.ToString("dd'-'MM'-'yyyy'-'HH'-'mm'-'ss") + "." +
                        cleanFilename[1]);
                }

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
            var childDestinationPath = Path.Combine(destinationPath, newFilename.Name);

            if (item!.IsDirectory)
            {
                if (newFilename.addSuffix)
                {
                    childDestinationPath += "_" + folderSuffixes.Dequeue();
                }

                Directory.Move(item.Path, childDestinationPath);
            }
            else
            {
                if (newFilename.addSuffix)
                {
                    var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
                    childDestinationPath = Path.Combine(destinationPath,
                        cleanFilename[0] + "_" + fileSuffixes.Dequeue() + "." +
                        cleanFilename[1]);
                }

                File.Move(item.Path, childDestinationPath);
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

    public static void OnCompressClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new UserPromptDialogWindow("No files selected.");
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

    #endregion

    private static (string path, bool cancel, bool addSuffix) GetPath(string dialogTitle, bool promptSuffix)
    {
        new PromptPathInputDialogWindow(dialogTitle, promptSuffix);
        var path = PromptPathInputDialogWindow.GetPath();
        NullPath();

        return (path.path, path.cancel, path.addSuffix);
    }

    private static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}