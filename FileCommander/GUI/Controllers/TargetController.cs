using FileCommander.GUI.Dialogs;

namespace FileCommander.GUI.Controllers;

using static PromptPathInputDialogWindow;
using static App;

public class TargetController
{
    public static (string root, bool cancel) GetTargetPanel(string operation)
    {
        var promptedTargetPanel = new PromptTargetPanelDialog(operation).GetTargetPanel();

        string root;
        if (promptedTargetPanel.targetHere)
        {
            root = (GetFocusedWindow() == 1 ? LeftRoot : RightRoot).ToString();
        }
        else
        {
            root = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();
        }


        return (root, promptedTargetPanel.cancel);
    }

    public static (string path, bool cancel, bool addSuffix) GetTargetPath(string dialogTitle, bool promptSuffix)
    {
        (string path, bool cancel, bool addSuffix) path;
        do
        {
            new PromptPathInputDialogWindow(dialogTitle, promptSuffix);
            path = GetPath();
            NullPath();
        } while (path.path == "" && !path.cancel);

        return (path.path, path.cancel, path.addSuffix);
    }
}