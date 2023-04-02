// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.IO;
using System.IO.Compression;
using FileCommander.GUI;
using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;
using Gtk;

namespace FileCommander.core;

using static App;
using static NavigationController;

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
                //Komprimování složky
                var handler = new ProcessHandler(items[0]!.Path, targetPath, true);
                var thread = new Thread(handler.Compress);
                thread.Start();

                //Cyklus zajišťující to, aby GUI nezamrzlo
                while (thread.IsAlive)
                {
                    while (Application.EventsPending())
                        Application.RunIteration();
                }
            }

            RefreshIconViews();
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

            //Vytvoření dočasné složky (GC)
            var tmpDirPath = Path.Combine(promptedTarget.root,
                archiveName.path + "_tmp_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            try
            {
                Directory.CreateDirectory(tmpDirPath);
            }
            
            //Následující catch bloky a výjimky byly generovány GitHub Copilotem.
            //Řádky volání konstruktoru okna s chybovou hláškou jsou mým vlastním dílem.
            catch (PathTooLongException)
            {
                new PromptUserDialogWindow("The specified archive name exceeded the system-defined maximum length.");
                return;
            }
            catch (ArgumentException)
            {
                new PromptUserDialogWindow("Malformed archive name");
                return;
            }
            catch (IOException)
            {
                new PromptUserDialogWindow("Parent directory is read-only.");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                new PromptUserDialogWindow("Access to the path is denied.");
                return;
            }
            catch (NotSupportedException)
            {
                new PromptUserDialogWindow("The specified archive name is in an invalid format.");
                return;
            }
            catch (Exception)
            {
                new PromptUserDialogWindow("Unknown error has occured.");
                return;
            }

            new ProgressDialogWindow("Compressing...");
            foreach (var item in items)
            {
                if (item!.IsDirectory)
                {
                    //Zkopírování složky do dočasné složky (GC)
                    var handler = new ProcessHandler(item.Path, Path.Combine(tmpDirPath, item.Name!), true);
                    var thread = new Thread(handler.Copy);
                    thread.Start();

                    while (thread.IsAlive)
                    {
                        while (Application.EventsPending())
                            Application.RunIteration();
                    }
                }
                else
                {
                    //Zkopírování souboru do dočasné složky (GC)
                    var handler = new ProcessHandler(item.Path, Path.Combine(tmpDirPath, item.Name!), false);
                    var thread = new Thread(handler.Copy);
                    thread.Start();

                    while (thread.IsAlive)
                    {
                        while (Application.EventsPending())
                            Application.RunIteration();
                    }
                }
            }

            //Komprimování dočasné složky (GC)
            var zipHandler = new ProcessHandler(tmpDirPath, archiveTargetPath, false);
            var zipThread = new Thread(zipHandler.Compress);
            zipThread.Start();

            while (zipThread.IsAlive)
            {
                while (Application.EventsPending())
                    Application.RunIteration();
            }

            RefreshIconViews();
        }
        
        //Následující řádek byl generován GitHub Copilotem
        new PromptUserDialogWindow("Compression finished.");
    }
}