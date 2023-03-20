namespace FileCommander.GUI.TwinToolboxes;

using Gtk;
using static App;
using static Controllers.NavigationController;

public class TwinToolboxRight
{
    public static VBox DrawRightToolbox()
    {
        var rightToolbox = new VBox();

        var rightToolbar = new Toolbar();
        rightToolbox.PackStart(rightToolbar, true, true, 0);

        var rightHomeButton = new ToolButton(Stock.Home);
        rightToolbar.Insert(rightHomeButton, 0);
        rightHomeButton.Clicked += (sender, args) =>
        {
            RightRoot = OnHomeClicked(args, RightStore);
            RightRootLabel.Text = "Current directory: " + RightRoot;
        };

        var rightUpButton = new ToolButton(Stock.GoUp);
        rightToolbar.Insert(rightUpButton, 1);
        rightUpButton.Clicked += (_, _) =>
        {
            RightRoot = OnUpClicked(RightRoot, RightStore);
            RightRootLabel.Text = "Current directory: " + RightRoot;
        };

        //TODO disky

        rightToolbox.PackStart(RightRootLabel, true, true, 0);
            
        return rightToolbox;
    }
}