namespace FileCommander.GUI.TwinToolboxes;

using Gtk;
using static App;
using static Controllers.NavigationController;

public class TwinToolboxLeft
{
    public static VBox DrawLeftToolbox()
    {
        var leftToolbox = new VBox();

        var leftToolbar = new Toolbar();
        leftToolbox.PackStart(leftToolbar, true, true, 0);

        var leftHomeButton = new ToolButton(Stock.Home);
        leftToolbar.Insert(leftHomeButton, 0);
        leftHomeButton.Clicked += (sender, args) =>
        {
            LeftRoot = OnHomeClicked(args, LeftStore);
            LeftRootLabel.Text = "Current directory: " + LeftRoot;
        };

        var leftUpButton = new ToolButton(Stock.GoUp);
        leftToolbar.Insert(leftUpButton, 1);
        leftUpButton.Clicked += (_, _) =>
        {
            LeftRoot = OnUpClicked(LeftRoot, LeftStore);
            LeftRootLabel.Text = "Current directory: " + LeftRoot;
        };

        //TODO disky

        leftToolbox.PackStart(LeftRootLabel, true, true, 0);
            
        return leftToolbox;
    }
}