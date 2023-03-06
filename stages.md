# File Commander

Cílem práce je vytvořit v programovacím jazyce C# desktopovou aplikaci k práci se soubory. 
Program bude schopen zobrazit soubory a složky v adresáři, přesouvat je a kopírovat,
přejmenovávat je a mazat, vytvářet nové složky.
Dále bude možné vytvářet archivy a jiné archivy extrahovat.

## Požadavky

### F1. Kontrola cest
Vždy když uživatel zadá jakoukoliv cestu, která by nutně nemusela existovat, program ověří, že existuje,
případně vyhodí dialogové okno s chybovou hláškou. Pokud by uživatel chtěl provádět více kolidujících akcí najednou,
bude každá následující kolizní akce zrušena.

### F2. Zobrazení obsahu adresáře
Uživatel bude moci otevřít složku a zobrazit její obsah. Zobrazení bude ve dvou paralelních oknech,
v každém z nich bude moci být zobrazena jiná složka.

### F3. Práce se souborem / složkou
Uživatel v liště v horní části okna / pravým tlačítkem kliknutím na soubor vybere akci. Zobrazí se mu dialogové okno
s dotazem na jméno souboru v případě vytváření / přejmenování, nebo na cílové umístění v případě kopírování / přesouvání.
Dialog nebude uživateli napovídat obsah složek, bude tedy nutné zadat úplnou cestu. Defaultně se předvyplní cesta do vedlejšího zobrazeného adresáře.
V případě mazání bude uživatel vyzván k potvrzení akce. Nebude možné soubory přesouvat tažením do druhého okna.

### F4. Práce s archivy
- Uživatel vybere složku, kterou chce zabalit, vybere akci (viz. výše) "Zabalit" a v dialogovém okně zvolí název archivu, popřípadě cílovou cestu archivu.
Archiv bude ve formátu `.zip`.
- Uživatel vybere archiv a akci "Rozbalit". V Dialogovém okně bude možnost zvolit "Extrakt here", nebo zadat cestu pro rozbalení.

### F5. Zobrazení průběhu operace
Během akcí trvajících déle se zobrazí dialogové okno s průběhem operace. Operaci bude možno v tomto okně zrušit.

### N1. Platforma
Program bude vyvíjen primárně pro prostředí GNU/Linux v jazyce C# za použití frameworku .NET Core 7.0 a GtkSharp.
Funkčnost na OS Windows je díky GTK pravděpodobná, ale není primární snahou.

### N2. Nastavení
Bude vyvíjena snaha implementovat uživatelská nastavení, ale program se vzhledem ke svojí povaze obejde i bez nich.


## Případy užití

### UC1. Otevření složky
### UC2. Vytvoření nové složky
### UC3. Zkopírování souboru / složky
### UC4. Přesunutí souboru / složky
### UC5. Přejmenování souboru / složky
### UC6. Smazání souboru / složky
### UC7. Vytvoření archivu ze složky
### UC8. Extrakce archivu


| Požadavky | UC1 | UC2 | UC3 | UC4 | UC5 | UC6 | UC7 | UC8 |
|:---------:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
|    F1     |     |  +  |  +  |  +  |  +  |  +  |  +  |  +  |
|    F2     |  +  |     |     |     |     |     |     |     |
|    F3     |     |  +  |  +  |  +  |  +  |  +  |     |     |
|    F4     |     |     |     |     |     |     |  +  |  +  |
 |    F5     |     |     |  +  |  +  |     |     |  +  |  +  |  

## Milestones
 
### MS1. - 12.2.
- [x] UC1

### MS2. - 2.3.
- [x] UC2
- [ ] UC3
- [ ] UC4
- [ ] UC5
- [ ] UC6
- [ ] UC7
- [ ] UC8

### MS3. - 20.3.
- [ ] Kontrola cest, testing, ať to proboha funguje