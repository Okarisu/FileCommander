# Práce se soubory v jazyce CS
## Úvod
Práce se soubory je důležitou, ne-li klíčovou částí každého programu, který nějakým způsobem ukládá nebo načítá data. Ať už jde o nejrůznější textové, zvukové a grafické editory, které načítají data v souborech již dříve uložená a ukládají data nová, o hry, které načítají textury, dialogy, zvukové soubory a ukládají postup hráče, nebo jakýkoli program ukládající svá nastavení, všechny tyto programy musí nějak se soubory pracovat.
Ve své práci se pokusím popsat, jaké jsou možnosti práce se soubory obecně, na které věci se při ní musí dávat pozor a to vše aplikovat na jazyk C#.

### Soubory
Jako soubor označujeme sadu dat uloženou na datovém médiu (disku v počítači, USB disku, DVD), se kterou lze pracovat jako s jedním celkem a která má nějaké jméno. 
Soubor může obsahovat jak pouze jeden typ dat (text, zvuk, obrázek), tak i více typů: Multimediální soubory (kontejnery) zabalují obraz, zvuk a případně i titulky, kancelářské dokumenty mohou krom textu obsahovat například obrázek.
https://cs.wikipedia.org/wiki/Soubor
Potáček, Jiří. soubor. In: _KTD: Česká terminologická databáze knihovnictví a informační vědy (TDKIV)_ [online]. Praha : Národní knihovna ČR, 2003- [cit. 2023-03-29]. Dostupné z: https://aleph.nkp.cz/F/?func=direct&doc_number=000000018&local_base=KTD.

### Typy souborů
Soubory můžeme rozdělit podle jejich typu na textové a binární. Mezi textové se řadí takové soubory, které můžeme otevřit v jakémkoli textovém editoru a jejich obsah bude uživateli buď naprosto čitelný, nebo v něm alespoň rozezná strukturu souboru a některá klíčová slova. Patří sem textové dokumenty jako txt, markdown nebo tex, zdrojové kódy programů a webových standardů (html, css, json), obrázky formátu svg, titulky nebo konfigurační soubory. I když je většina spustitelných souborů binárních, najsou se i výjimky - například dávkový soubor bat systému MS Windows nebo unixový shell script jsou textové povahy a jakýmkoli textovým editorem je lze otevřít.

Binární soubory oproti textovým vyžadují speciální program pro jejich otevření. Pokud bychom se jejich obsah pokusili zobrazit v textovém editoru, dostaneme pouze surová data, ze kterých nic nevyčteme. Binárními soubory jsou například (multi)mediální soubory obrázků (vyjma svg), videí a zvuku, kancelářské dokumenty (PDF, doc, prezentace ppt), archivy jako zip, rar, ISO obrazy nebo spustitelné soubory jako exe a dll.
https://www.nayuki.io/page/what-are-binary-and-text-files


+ obrázky

### Koncovky souborů
Formát souboru a to, zda je textový, nebo binární, můžeme zpravidla určit podle jeho koncovky. Toho můžeme využít kupříkladu tehdy, chceme-li načíst všechny obrázky v adresáři a zobrazit je v galerii - v tomto případě chceme vyloučit všechny dokumenty a jiné nepodporované soubory, jelikož nemáme kontrolu nad tím, co přesně se v adresáři nalézá. Pokud bychom předpokládali, že jsou v adresáři pouze obrázky, přestože by to tak ve skutečnosti nebylo, program by mohl skončit chybou.

#### Odlišnosti systémů v souvislosti s koncovkami
Zatímco MS Windows koncovky souborů nutně potřebuje k úspěšnému zpracování dat v souboru (tedy zobrazení jeho obsahu nebo jeho přehrání), linuxové systémy formát souboru určují podle jeho hlavičky. Shell script například nemusí mít koncovku .sh, aby ho systém jako spustitelný script rozpoznal, ale musí pak na první řádce obsahovat sekvenci #!/bin/bash. Pokud ani jednu z těchto podmínek nespoňuje, stává se z něj obyčejný nespustitelný soubor.

Neplatí to však u všech typů souborů - pokud například smažeme koncovku certifikátu .pem, systém ho bude mít za soubor programu MATLAB, přestože jeho obsah zůstal nezměněný. (ověřeno lokálním experimentem)

Některé linuxové programy však navrch kontroly hlavičky provádí i kontrolu koncovky souboru, což může vést k neočekávaným chybám a některé soubory nemusí být rozpoznány, přestože jde o správný formát. Příkladem mohl být prohlížeč obrázků Eye of Gnome, který nedokázal obrázek otevřít, pokud neměl správnou koncovku. (V nejnovější verzi programu už tato chyba/funkcionalita není.) V některých případech koncovku souboru vyžaduje samotný systém - při zpracování seznamu softwarových zdrojů čte kernel pouze ze souborů s koncovkou .list, aby zamezil načtení nežádoucích dat.
https://askubuntu.com/questions/803434/do-file-extensions-have-any-purpose-in-linux


# Práce se soubory obecně
Jakoukoli práci se souborem můžeme rozfázovat na 3 dílčí fáze procesu - otevření, čtení nebo zápis a zavření souboru.

## Otevření
Způsob otevření souboru závisí na tom, jak s ním má být nakládáno. Read-only mód se používá, chceme-li ze souboru pouze číst, protože se tím minimalizuje riziko poškození souboru. Program v tomto módu totiž nemá přístup k funkcím, jimiž by do souboru zapisoval. Pokud bychom soubor otevřeli v read-write módu, v němž k těmto funkcím program přístup má, při chybě běhu programu by se do souboru mohla zapsat náhodná data a poškodit jej tak.

Tento mód otevření se používá

Posledním způsobem otevření je write-only mód používaný při zápisu do souboru. Vzhledem k tomu, že nenačítá soubor do RAM, ale rovnou zapisuje na určené místo na disku, je mnohem efektivnější než read-write mód, který soubor do paměti načítá.
Od write-only módu je odvozený append mód, který zápis začíná na konci souboru a text pouze připojuje - předchozí obsah souboru tedy nechává netknutý. Write-only mód