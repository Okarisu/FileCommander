using Gtk;

namespace FileCommander.GUI;

using static App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public abstract class TwinPanels
{
    /*
    * Advanced widgets in GTK#. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-03].
    * Dostupné z: https://zetcode.com/gui/gtksharp/advancedwidgets/
    * Upraveno.
    */

    public static void DrawLeftPanel()
    {
        FillStore(LeftStore, LeftRoot);
        LeftScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic); //Nastavení chování posuvníku
        LeftIconView.GrabFocus(); //Jeden z panelů musí být na začátku zaktivován kvůli předejití chybám
        LeftRootLabel.ModifyFg(StateType.Normal, new Gdk.Color(0, 200, 0)); //Zvýraznění aktivního panelu
        LeftIconView.SelectionMode = SelectionMode.Multiple; //Možnost výběru více souborů
        LeftIconView.TextColumn = ColDisplayName; //Zobrazení jména souboru pod ikonou
        LeftIconView.PixbufColumn = ColPixbuf; //Zobrazení ikony souboru

        //Zpracování dvojkliku na položku
        LeftIconView.ItemActivated += (_, args) =>
        {
            //Funkce musí nový root vracet - při jeho aktualizaci pouze ve funkci by se nezměnil
            LeftRoot = OnItemActivated(args, LeftRoot, LeftStore, LeftHistory, LeftHistoryForward);
            UpdateRootLabel(LeftRootLabel, LeftRoot);
        };
        LeftIconView.FocusInEvent += (_, _) => SetFocusedPanel(1); //Nastavení panelu jako aktivního

        LeftScrolledWindow.Add(LeftIconView); //Přidání widgetu do okna
    }

    public static void DrawRightPanel()
    {
        FillStore(RightStore, RightRoot);
        RightScrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        RightIconView.SelectionMode = SelectionMode.Multiple;
        RightIconView.TextColumn = ColDisplayName;
        RightIconView.PixbufColumn = ColPixbuf;
        RightIconView.ItemActivated += (_, args) =>
        {
            RightRoot = OnItemActivated(args, RightRoot, RightStore, RightHistory, RightHistoryForward);
            UpdateRootLabel(RightRootLabel, RightRoot);
        };
        RightIconView.FocusInEvent += (_, _) => SetFocusedPanel(2);

        RightScrolledWindow.Add(RightIconView);
    }
}