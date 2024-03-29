// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

using System.Runtime.InteropServices;
using FileCommander.GUI.Toolbars;
using Gdk;
using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pango;

namespace FileCommander.GUI;

public class App : Gtk.Window
{
    //Konstanty určující property objektu ListStore, kterou chceme získat
    private const int ColPath = 0; //Cesta k souboru (GC)
    public const int ColDisplayName = 1; //Jméno souboru (GC)
    public const int ColPixbuf = 2; //Ikona souboru (GC)
    private const int ColIsDirectory = 3; //Je soubor složkou? (GC)

    //Aktuální cesta k levému a pravému zobrazenému adresáři - na začátku je nastavena na osobní složku
    public static DirectoryInfo LeftRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
    public static DirectoryInfo RightRoot = new(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

    
    public static ListStore LeftStore = CreateStore(), RightStore = CreateStore(); //Objekty obsahující data o souborech a složkách (GC)
    public static ScrolledWindow LeftScrolledWindow = new(), RightScrolledWindow = new(); //Widgety pro zobrazení posouvatelných panelů
    public static IconView LeftIconView = new(LeftStore), RightIconView = new(RightStore); //Widgety pro zobrazení ikon

    //Zásobníky pro historii adresářů
    public static Stack<DirectoryInfo> LeftHistory = new();
    public static Stack<DirectoryInfo> LeftHistoryForward = new();
    public static Stack<DirectoryInfo> RightHistory = new();
    public static Stack<DirectoryInfo> RightHistoryForward = new();

    //Textová pole zobrazující název aktuálního adresáře
    public static Label LeftRootLabel = new("Current directory: " + LeftRoot);
    public static Label RightRootLabel = new("Current directory: " + RightRoot);

    public static Toolbar LeftDiskBar = new(), RightDiskBar = new();

    //Určuje, který panel je aktivní - nastavována z třídy TwinPanels. 1 = levý panel, 2 = pravý panel
    private static int _focusedPanel;

    //"Hlavičky" panelů, obsahující navigační tlačítka, ikony disků a název adresáře
    private static HBox _leftTwinPanelHeader = new HBox(true, 0);
    private static HBox _rightTwinPanelHeader = new HBox(true, 0);

    /*
     * Informace pro tvorbu konstruktoru jsem čerpal zde:
     * Advanced widgets in GTK#. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-03].
     * Dostupné z: https://zetcode.com/gui/gtksharp/advancedwidgets/
     * Upraveno.
     */
    public App() : base("File Commander")
    {
        //Nastavení vlastností okna
        SetDefaultSize(1280, 720);
        Maximize();
        DeleteEvent += (_, _) => Application.Quit();

        //Nastavení hlavního kontejneru - přidání menu a horní nástrojové lišty
        VBox windowContainer = new VBox(false, 0);
        Add(windowContainer);
        var menuBar = DrawMenu.DrawMenuBar();
        windowContainer.PackStart(menuBar, false, true, 0);
        var toolbar = TopToolbar.DrawTopToolbar();
        windowContainer.PackStart(toolbar, false, true, 0);

        //Nastavení kontejneru pro dvojici panelů (GC)
        HBox twinPanelsContainer = new HBox(false, 0);
        windowContainer.PackStart(twinPanelsContainer, true, true, 0);
        VBox leftTwinContainer = new VBox(false, 0);
        twinPanelsContainer.PackStart(leftTwinContainer, true, true, 0);
        twinPanelsContainer.PackStart(new Separator(Orientation.Vertical), false, false, 0);
        VBox rightTwinContainer = new VBox(false, 0);
        twinPanelsContainer.PackStart(rightTwinContainer, true, true, 0);

        //Vykreslení panelů
        TwinPanels.DrawLeftPanel();
        TwinPanels.DrawRightPanel();

        //Nastavení kontejneru pro panelové lišty
        leftTwinContainer.PackStart(_leftTwinPanelHeader, false, true, 0);
        _leftTwinPanelHeader.PackStart(TwinToolbars.DrawLeftToolbar(), false, true, 0);
        LeftRootLabel.LineWrap = true;
        LeftRootLabel.MaxWidthChars = 50;
        leftTwinContainer.PackStart(LeftRootLabel, false, true, 0);
        leftTwinContainer.PackStart(LeftScrolledWindow, true, true, 0);

        //Následující řádky byly generovány GitHub Copilotem
        rightTwinContainer.PackStart(_rightTwinPanelHeader, false, true, 0);
        _rightTwinPanelHeader.PackStart(TwinToolbars.DrawRightToolbar(), false, true, 0);
        RightRootLabel.LineWrap = true;
        RightRootLabel.MaxWidthChars = 50;
        rightTwinContainer.PackStart(RightRootLabel, false, true, 0);
        rightTwinContainer.PackStart(RightScrolledWindow, true, true, 0);
        //Konec generovaných řádků
        
        
        //Vykreslení lišt disků
        LeftDiskBar = Disks.DrawDiskBar(LeftHistory, LeftHistoryForward, LeftRoot, LeftStore, LeftRootLabel);
        //Následující řádek byl generován GitHub Copilotem
        RightDiskBar = Disks.DrawDiskBar(RightHistory, RightHistoryForward, RightRoot, RightStore, RightRootLabel);

        _leftTwinPanelHeader.PackStart(LeftDiskBar, false, true, 0);
        _rightTwinPanelHeader.PackStart(RightDiskBar, false, true, 0);

        ShowAll();

        //V závislosti na nastavení se zobrazí lišta disků (GC)
        if (Settings.GetConf("ShowMountedDrives")) return;
        LeftDiskBar.Hide();
        RightDiskBar.Hide();
    }

/*
 * Advanced widgets in GTK#: IconView. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-02].
 * Dostupné z: https://zetcode.com/gtksharp/advancedwidgets/
 * Převzato v plném rozsahu.
 */
    private static ListStore CreateStore()
    {
        ListStore store = new ListStore(typeof(string),
            typeof(string), typeof(Pixbuf), typeof(bool));

        store.SetSortColumnId(ColDisplayName, SortType.Ascending);

        return store;
    }
    /* Konec citace */

    /*
    * Advanced widgets in GTK#: IconView. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-02].
    * Dostupné z: https://zetcode.com/gtksharp/advancedwidgets/
    * Upraveno.
    */
    public static void FillStore(ListStore store, DirectoryInfo root)
    {
        Pixbuf fileIcon = new("icons/file.png");
        Pixbuf dirIcon = new("icons/folder.png");
        store.Clear();

        if (!root.Exists)
        {
            return;
        }

        foreach (DirectoryInfo dir in root.GetDirectories())
        {
            if (Settings.GetConf("ShowHiddenFiles"))
            {
                store.AppendValues(dir.FullName, dir.Name, dirIcon, true);
            }
            else
            {
                if (!dir.Name.StartsWith("."))
                    store.AppendValues(dir.FullName, dir.Name, dirIcon, true);
            }
        }

        foreach (FileInfo file in root.GetFiles())
        {
            if (Settings.GetConf("ShowHiddenFiles"))
            {
                store.AppendValues(file.FullName, file.Name, fileIcon, false);
            }
            else
            {
                if (!file.Name.StartsWith("."))
                    store.AppendValues(file.FullName, file.Name, fileIcon, false);
            }
        }
    }
    
    /* Konec citace */

    /*
    * Advanced widgets in GTK#: IconView. ZetCode [online]. 6. 1. 2022 [cit. 2023-04-02].
    * Dostupné z: https://zetcode.com/gtksharp/advancedwidgets/
    * Upraveno.
    */
    public static DirectoryInfo OnItemActivated(ItemActivatedArgs args, DirectoryInfo root, ListStore store,
        Stack<DirectoryInfo> history, Stack<DirectoryInfo> historyForward)
    {
        store.GetIter(out var iter, args.Path);
        var path = (string) store.GetValue(iter, ColPath);
        var isDir = (bool) store.GetValue(iter, ColIsDirectory);

        if (!isDir)
            return root;

        history.Push(root); //Uložení aktuální složky do historie "zpět" (GC)
        historyForward.Clear(); //Při otevření složky se maže historie "dopředu"

        root = new DirectoryInfo(path);
        FillStore(store, root);

        return root;
    }
    /* Konec citace */
    public static int GetFocusedPanel() => _focusedPanel;

    public static void SetFocusedPanel(int panel)
    {
        _focusedPanel = panel;

        //Zvýraznění aktivního panelu
        switch (panel)
        {
            case 1:
                LeftRootLabel.ModifyFg(StateType.Normal, new Gdk.Color(0, 200, 0));
                RightRootLabel.ModifyFg(StateType.Normal, new Gdk.Color(255, 255, 255));
                break;
            case 2:
                RightRootLabel.ModifyFg(StateType.Normal, new Gdk.Color(0, 200, 0));
                LeftRootLabel.ModifyFg(StateType.Normal, new Gdk.Color(255, 255, 255));
                break;
        }
    }

    public static Item[] GetSelectedItems()
    {
        var selection = _focusedPanel == 1 ? LeftIconView.SelectedItems : RightIconView.SelectedItems;

        var store = _focusedPanel == 1 ? LeftStore : RightStore;
        var files = new Item[selection.Length];

        for (var i = 0; i < selection.Length; i += 1)
        {
            store.GetIter(out var treeIterator, selection[i]);
            files[i] = new Item(store.GetValue(treeIterator, ColPath).ToString()!,
                store.GetValue(treeIterator, ColDisplayName).ToString(),
                (bool) store.GetValue(treeIterator, ColIsDirectory));
        }

        return files;
    }

    public static void UpdateRootLabel(Label label, DirectoryInfo root)
    {
        string? parent;
        string optionalSlash;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            parent = root.Parent?.Name is "/" or "" ? "" : root.Parent?.Name;
            optionalSlash = parent is "" ? "" : "/";
        }
        else
        {
            parent = root.Parent?.Name is "" ? "" : root.Parent?.Name;
            optionalSlash = parent != null && (parent.EndsWith(":\\") || parent == "") ? "" : "\\";
        }

        label.Text = $"Current directory: {parent}{optionalSlash}{root.Name}";
    }
}