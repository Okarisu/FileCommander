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

        var promptedDestinationPath = new PromptArchiveTargetPathDialog("Extract to...").GetTargetPanel();
        if (promptedDestinationPath.cancel) return;

        string root;
        if (promptedDestinationPath.targetHere)
        {
            root = (GetFocusedWindow() == 1 ? LeftRoot : RightRoot).ToString();
        }
        else
        {
            root = (GetFocusedWindow() == 1 ? RightRoot : LeftRoot).ToString();
        }

        
        foreach (var item in items)
        {
            var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
            var filename = cleanFilename[0]; //jméno souboru bez koncovky
            if (cleanFilename.Length > 2) //Případ, kdy je v názvu souboru tečka
            {
                for(var i = 0; i < cleanFilename.Length - 2; i++)
                    filename += "." + cleanFilename[i];
            }

            if (!item!.IsDirectory && item.Name!.EndsWith(".zip"))
            {
                ZipFile.ExtractToDirectory(item.Path, Path.Combine(root, filename));
            }

            Refresh();
        }
    }

}