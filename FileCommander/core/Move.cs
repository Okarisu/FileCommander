// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global
namespace FileCommander.core;

using GUI;
using static GUI.App;
using static GUI.FunctionController;
public partial class Core
{
    public static void OnMoveClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        var destinationPath = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();


        foreach (var item in items)
        {
            var childDestinationPath = Path.Combine(destinationPath, item!.Name!);
            var promptAskAgain = Settings.GetBoolValueSetting("PromptDuplicitFileCopy");
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
        new PromptUserDialogWindow("Finished moving files.");
    }
}