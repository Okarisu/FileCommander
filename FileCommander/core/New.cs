// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global
namespace FileCommander.core;


using GUI;
using static GUI.App;
using static GUI.FunctionController;

public partial class Core
{
    public static void OnNewClicked(object sender, EventArgs e)
    {
        var newFolderName = GetPath("New folder", false);

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), newFolderName.path);
        if (Directory.Exists(newDirectoryPath))
        {
            
            new PromptUserDialogWindow("Folder with this name already exists.");
            return;
        }

        Directory.CreateDirectory(newDirectoryPath);

        Refresh();
    }

}