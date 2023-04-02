# Cíl práce
Cílem práce je vytvořit v programovacím jazyce C# s použitím knihovny GTK multiplatformní  
desktopovou aplikaci pro správu souborů. Mezi funkce programu bude patřit přesouvání a  
kopírování souborů a složek, vytváření nových složek, mazání souborů a složek a komprese a dekomprese archivů.

## Popis programu
Aplikace zobrazuje ve dvou paralelně umístěných panelech obsah dvou adresářů. V těchto oknech uživatel označí soubor či soubory, se kterými si přeje pracovat, a následně na horní nástrojové liště zvolí akci, kterou chce provést. V adresářích se uživatel pohybuje pomocí nástrojové lišty každého panelu.

### Navigace
Program po spuštění zobrazí v obou panelech domovskou složku uživatele. Na platformě Linux je to /home/username, na Windows složka dokumenty. Adresáře uživatel prochází dvojitým kliknutím na složku, kterou si přeje otevřít. Po dvojkliku se panel překreslí a zobrazí obsah vybrané složky.
Každý panel má nad sebou svoji navigační lištu. Ta obsahuje tlačítko Home, které v daném panelu zobrazí domovskou složku, a tlačítko Up, které uživatele přesune o jednu úroveň výše. Dále tlačítka Back a Forward, kterými může uživatel procházet skrze historii navštívených složek dopředu a dozadu. Pokud jsou k počítači připojena přenosná média, pokud má počítač více pevných disků nebo pokud má disk více viditelných oddílů (na linuxu musí být tyto oddíly připojeny), zobrazí se rovněž v této liště. Po kliknutí na tlačítko nesoucí název disku se uživateli zobrazí jeho obsah a může ho procházet stejně jako ostatní adresáře.
Na liště je rovněž zobrazena zkrácená cesta k aktuální složce - název aktuální složky a složky o úroveň výše.

## Preference
V menu pod horní lištou celého okna si může uživatel v položce View zvolit, zda chce zobrazovat skryté soubory (začínající tečkou) a připnuté disky. Volbu provede zaškrtnutím nebo odškrtnutím příslušné položky. 

### Práce se soubory
Pokud uživatel vytváří novou složku, děje se tak v panelu, který byl v tu dobu soustředěný, tedy bylo na něj kliknuto myší naposledy.

Přejmenovává-li složku nebo soubor, je vyzván, aby do dialogového okna, které se objeví, napsal nový název složky nebo souboru. Pokud uživatel přejmenovává více souborů nebo složek najednou, je upozorněn, že k nim bude připojen číselná přípona.
Pokud uživatel složku nebo soubor maže, je vyzván k potvrzení této akce dialogovým oknem. V něm má možnost zaškrtnout "již se nedotazovat" a při příští akci mazání již budou soubory smazány bez potvrzení.

V případě, že uživatel soubory kopíruje nebo přesouvá, cílovým adresářem je adresář zobrazený ve vedlejším panelu, který není soustředěn. Označí-li tedy uživatel soubory nejdříve v levém a následně v pravém panelu, akce se provede pro soubory v pravém panelu a cílovým adresářem bude ten v panelu levém. Pokud již cílový adresář obsahuje soubor(y) se stejným jménem, jako je jméno kopírovaného souboru, je uživatel dotázán, zda si přeje pokračovat. Při potvrzení se ke jménům přikopírovaných souborů připojí do závorky číslo kopie (tedy např. soubor (1).txt). Pokud již složka obsahuje soubory se stejným jménem, jako jsou ty, jež jsou sem přesouvány, program je vynechá a na konci akce uživatele upozorní, že zde nastal problém s duplicitními názvy.

Pokud uživatel vytváří archiv, označí jeden nebo více souborů či složek a klikne na tlačítko Compress. Následně je tázán, zda si přeje soubory komprimovat do adresáře v soustředěném panelu, nebo do toho v nesoustředěném. Akce se pak provádí pro zvolený adresář. Pokud uživatel označil pouze jednu složku, nese výsledný archiv stejný název. Pokud pouze jeden soubor, je vyzván k zadání názvu archivu. Označil-li souborů více, je po zvolení cílového složky vyzván k zadání názvu archivu.

Pokud uživatel jeden nebo více archivů najednou extrahuje, je opět vyzván ke zvolení cílového adresáře, kam se archiv(y) rozbalí. Výsledkem je složka nesoucí stejný název jako název archivu, která obsahuje z něj rozbalené soubory.

## Řešení aplikace
Aplikace vzhledem ke své původně jediné zamýšlené platformě (linux) používá pro vykreslování uživatelského rozhranní knihovnu GTK (GNOME toolkit).

### Struktura programu
Program je rozdělen do dvou hlavních částí - GUI, která vykresluje veškeré grafické rozhranní a stará se o navigaci, a core, která obsahuje všechny funkce pro správu souborů.
Část GUI se dále dělí na kontrolery, které řeší navigaci a cílové adresáře, část zahrnující veškerá dialogová okna a část vykreslující nástrojové lišty. Obsahuje také hlavní třídu App, která je vstupním bodem celého rozhranní a volá všechny podsložky části GUI.

## Vstupní třída a samostatné třídy
#### Program
Abstraktní třída Program obsahuje jedinou metodu Main(), která na 3 řádcích inicializuje aplikaci, volá konstruktor App a následně aplikaci spouští.

#### Item
Třída Item slouží k vytvoření objektu Item, kterým jsou všechny soubory a složky zobrazené v panelech. Ve fieldech konstruktoru Item se definuje cesta k souboru/složce, jméno, které bude později zobrazeno spolu s ikonou, a zda je objekt složkou (tj. lze ho rozkliknout), nebo souborem.

#### Settings
Abstraktní třída Settings slouží k načítání a ukládání uživatelských nastavení. Používá knihovnu YamlDotNet a preference ukládá do souboru config.yaml jako hodnoty true/false, které knihovna sama převádí na typ bool. Výjimku tvoří položka DefaultLinuxDriveMountLocation, která je typu string. Metody pro čtení a zápis do souboru používají návrhový vzor try/catch pro případ, že by například konfigurační soubor chyběl, nebo kdyby došlo k jiné chybě.

Třída ukládá a načítá uživatelské preference ohledně dotazování při mazání a kopírování souborů, zobrazování připojených disků a skrytých souborů a v případě linuxového systému i cestu ke složce, kam jsou připojovány disky.

Všechny funkce této třídy nejdříve za pomoci objektu StreamReader deserializují konfigurační soubor, což znamená, že ho rozloží na jednotlivé objekty a ty následně uloží do proměnné typu Dictionary.

Funkce GetConf(Str) vrací konfiguraci jako bool nebo string. V argumentu se jim předá klíč, jehož hodnotu poté v Dictionary najdou a buď rovnou vrátí, nebo ji převedou na typ string a a vrací výsledek porovnání se stringem "true".

Funkce SetConf nejdříve deserializuje obsah konfiguračního souboru a následně v Dictionary přepíše hodnotu pod zadaným klíčem na novou hodnotu. Dictionary poté serializuje nazpět a pomocí instance StreamWriter konfiguraci uloží do souboru.

## Část core
Tato část obsahuje všechny funkce, které se soubory manipulují. Sdružil jsem je do jedné třídy Core, přičemž každá funkce je implementována ve vlastním souboru v parciální třídě. Přišlo mi zbytečné, aby měla každá funkce svoji třídu, ale zároveň jsem je potřeboval kvůli přehlednosti kódu rozčlenit do jednotlivých souborů, a použití parciální třídy mi to umožnilo. 

## Třída ProcessHandler
Funkce, u kterých hrozí, že jejich procesy zaberou déle času, se spouští v odlišném vláknu, než je vlákno hlavního procesu. K tomu slouží třída ProcessHandler, jejíž konstruktor nastaví do privátních proměnných zdrojovou a cílovou cestu akce a informaci, zda je objekt složkou. Akce, která má být se souborem provedena, pak probíhá ve  vlastním vláknu jako metoda beroucí své argumenty z těchto privátních proměnných.

V každé funkci, která svůj proces ve vláknu spouští, je navíc smyčka čekající na skončení procesu. Ta zajišťuje, že bude okno Gtk obnovováno a zamezuje tak zamrznutí programu.

Tento způsob zpracování byl původně plánován kvůli zobrazení okna s informacemi o průběhu operace, ale protože jsem během vývoje narazil na problémy (stále zaseklý progress bar, neukončitelné okno nebo vlákno), se kterými si nevěděl rady ani nikdo z lidí, se kterými jsem problém konzultoval, tato funkconalita programu tedy implementována není. Myslím si však, že je dobré proces, u kterého je větší riziko chyby, oddělit od hlavního procesu programu, a tak jsem tyto procesy do vláken umístil všechny, i když by se to mohlo bez funkce zmíněného informačního okna zdát zbytečné. 

### New()
Funkce vytvářející novou složku je nejjednodušší funkcí z celé části core. Po zpracování uživatelského vstupu kontroluje, zda už v adresáři není složka se stejným názvem, a následně ji v try/catch bloku zkouší vytvořit. Tento blok se snaží zachytit výjimky, ke kterým by mohlo při zadávání dojít, jako prázdné jméno složky, přesažení limitu délky názvu (na linuxu 255 znaků), zakázaných znaků v názvu (specifické pro Windows) nebo pokus o vytvoření složky v adresáři, kam uživatel nemá přístup. Poslední výjiku se mi povedlo zdárně otestovat, program v root adresáři systému opravdu složku nevytvoří a vrátí uživateli chybovou hlášku.
Funkce na konci zavolá RefreshIconViews(), čímž obnoví zobrazení obsahu obou panelů.

### Způsob fungování Copy a Move
Tyto funkce mají slopečný základ a liší se jen částmi specifickými pro danou operaci. Informace o označených souborech se zjišťují funkcí GetSelectedItems z třídy App a následně se kontroluje, zda jsou vůbec nějaké soubory vybrány. V tomto případě uživateli zobrazí okno s upozorněním. Inforrmace o označených souborech se ukládají do pole items jako objekty typu Item. 
Cílový adresář se zjišťuje pomocí funkce GetFocusedWindow, již popisuji níže a která vrací int o hodnotě 1 pro levý a 2 pro pravý panel. Pokud je soustředěný panel levý, cílovým adresářem bude ten vpravo, a naopak.
Následně funkce iteruje skrze pole s uloženými označenými soubory. V každém běhu smyčky se deklaruje proměnná cílové cesty, skládající se z adresy cílové složky a názvu zpracovávaného souboru.
Každá iterace probíhá podle toho, zda je objekt dané iterace soubor, nebo složka, kvůli odlišnosti práce s těmito objekty. K tomuto rozlišení slouží field objektu isDirectory.
Po určení způsobu, jakým má být s objektem Item nakládáno, zkontrolují obě funkce, zda již v cílovém adresáři objekt se stejným názvem není. Při kopírování lze tyto soubory kopírovat s přidanou příponou, přesouvání tyto soubory přeskakuje. Následně je v novém vlákně spuštěn proces, který zvolenou akci pro daný objekt provede, po jeho ukončení se obnoví zobrazení souborů v panelech a probíhá další kolo smyčky.
Po skončení iterace vyskočí dialogové okno, ohlašující konec operace.

#### Copy()
V této funkci si program kromě průběhu popsaného výše navíc v každém běhu smyčky načte uživatelské preference ohledně kopírování duplicitních souborů, a to funkcí GetConf s argumentem PromptDuplicityFileCopy. Vrátí-li tato funkce true, dotazuje se při výskytu duplicitního objektu, zda má objekt kopírovat a připojit mu číselnou příponu, či ho má přeskočit. Takto můžeme rozlišit jednotlivé případy a pro každý objekt zvolit zvlášť. Pokud však uživatel zaškrtne políčko "Don't ask again", program si tuto preferenci uloží do konfiguračního souboru a již se dotazovat nebude, a to ani při nové instanci programu. Soubory bude odteď automaticky kopírovat s přidanou příponou. Pokud si uživatel přeje dotazování obnovit, musí v konfiguračním souboru změnit hodnotu klíče PromptDuplicityFileCopy z false na true. Tento způsob řešení vychází z mých zkušeností s kopírováním na různých operačních systémech. 
Následuje blok kódu pro přejmenování objektu v případě výskytu duplicit. V případě složek program iteruje skrze všechny nalezené složky v cílovém adresáři a kontroluje, zda neobsahují název kopírované složky. Pokud nějaké najde, uloží si jejich počet a toto číslo pak připojí k názvu kopírované složky. Následně zkontroluje, zda již neexistuje kopie složky se stejnou příponou, a pokud ano, k názvu přidá ještě jednu příponu. Toto ošetření je zde pro případ, že bychom kopírovali složku, která již jednu příponu má.
V případě souborů je tento postup stejný, ale předchází mu rozdělení názvu souboru na jméno a koncovku, aby mezi ně mohla být vsunuta přípona. Název je rozdělen do pole stringů funkcí String.Split s argumentem tečky. Poslední prvek pole, tj. koncovka, a první prvek pole se uloží do oddělených proměnných a pokud je pole delší než 2 (což znamená, že je v názvu tečka), ke stringu s prvním prvkem se postupně připojí všechny další prvky v poli, vyjma toho posledního (koncovky). Následně probíhá stejný proces kontroly názvů a přípon, jako u složek, pouze se navíc připojuje koncovka.
Funkce kopírování se spouští v odděleném vláknu, objekt ProcessHandler se vytváří s parametry zdrojové a cílové cesty a hodnoty isDirectory. Pokud je objekt složka, je hodnota true, pokud ne, false. Funkce Copy třídy ProcessHandler pak kopíruje buď soubor pomocí metody File.Copy(), nebo rekurzivně kopíruje celou složku.
V této rekurzivní funkci nejdříve vytvoří cílovou složku, zkopíruje do ní všechny soubory a poté volá sama sebe pro zkopírování dalších vnořených složek. Takto postupuje, dokud nejsou zkopírovány všechny soubory ve složce obsažené.

### Move()
Funkce Move() krom částí společných s funkcí Copy() kontroluje výskyt duplicitních objektů a pokud nějaké najde, nenahrazuje je, ale přeskakuje. Je to z důvodu mé osobní preference, protože se tím podle mne zvyšuje riziko nenávratného smazání souborů omylem. Po skončení akce je uživatel informován, že došlo k výskytu duplicit a že byly přeskočeny. Funkce také kontroluje, zda přesouvaný objekt neleží na cestě systémové složky programu, protože by při přesunutí jeho souborů došlo k chybě. 
Proces přesunu je opět spuštěn v novém vlákně s argumenty zdrojové a cílové cesty a hodnty isDirectory. Platforma .NET implementuje funkci přesouvání jak pro složky, tak pro soubory, a tím pádem není potřeba psát rekurzivní funkci pro přesun složek.

### Delete()
Funkce Delete() se před začátkem operace uživatele dotáže, zda si je skutečně mazáním vybraných souborů jistý. Metoda File/Directory.Delete volá interní metodu FileSystem.RemoveFile/Directory, která neumožňuje obnovení odstraněných souborů, a proto je operace mazání nevratná. Předpokládá se, že si je po potvrzení operace uživatel jistý, které soubory k odstranění zvolil, a proto se funkce dotazuje jen jednou na začátku, nikoli před smazáním každého jednotlivého souboru. Funkce opět načítá uživatelské preference, tentokrát pod klíčem PromptDelete, a uživatel má možnost zaškrtnou možnost "již se nedotazovat". Dotazování může být opět obnoveno upravením klíče v konfiguračním souboru - přepsáním hodnoty PromptDelete z false na true.
Funkce, stejně jako Move(), kontroluje, zda není mazán systémový adresář programu či jeho soubory. V takovém případě tyto soubory přeskakuje a zobrazuje okno s chybovou hláškou. Proces však dále běží v novém vlákně.
Funkce Delete() třídy ProcessHandler nepotřebuje cílovou cestu, jako tento parametr se předává hodnota null. Podle toho, jaká je konstruktoru ProcessHandler předána hodnota isDirectory, se spouští buď metoda Directory.Delete() s parametrem pro rekurzivní odstranění složky jako true, nebo metoda File.Delete().

### Rename()
Funkce Rename() se po získání označených souborů a kontrole, zda je označen alespoň jeden soubor, uživatele dotáže na nové jméno souboru či složky. Pokud je objektů označených více, je uživatel upozorněn, že ke jménu bude přidávána číselná přípona. Tento dotaz probíhá ve funkci GetTargetPath(), která volá kostruktor dialogového okna PromptPathInputDialogWindow a vrací do tuple objektu newFilename nové jméno, signál pro zrušení operace a signál pro přidání přípony. Je-li flag Cancel pravdivý, je operace zrušena. 

Vzhledem k tomu, že přejmenování objektu je vlastně jeho přesunutí do stejného adresáře pod jiným jménem, je třeba určit jeho cílovou složku. To provádí opět funkce GetFocusedWindow, která ji nastavuje na adresář, jež je soustředěný.

Pro případ, že mají být objektům přidávány přípony, se vytvoří dva objekty typu Queue - jeden pro přípony složek a jeden pro soubory. Následně se do těchto front zařadí stejný počet přípon zvyšujících se o 1, jako je počet přejmenovávaných objektů.

Funkce následně iteruje skrze pole označených objektů, přičemž pokaždé kontroluje, zda uživatel nepřejmenovává systémové soubory programu. V takovém případě vyhazuje informační okno a v pozadí pokračuje v operaci. Funkce se opět dělí na část pro složky a část pro soubory. Pokud mají být připojovány přípony, za jméno objektu je přidána přípona jejím vyřazením z příslušné fronty dle typu objektu. U souborů je pak od původního názvu souboru oddělena koncovka, vytvořeno nové jméno s příponou a koncovkou a soubor přejmenován (tj. přesunut).

Vzhledem k tomu, že objekty zůstávají na svém místě na disku, nejde o tak náročnou operaci, jako je jejich přesouvání do jiného adresáře, a proto jsem se rozhodl funkce File/Directory.Move nevolat z nových vláken. Musel jsem však ošetřit možné výjimky způsobené špatným formátem jména či nedostatečnými právy uživatele k úpravě adresáře, stejně jako u funkce New() pomocí try/catch bloku.

### Compress()
Tato funkce se na začátku uživatele dotáže, zda si přeje označené položky komprimovat do adresáře v soustředěném panelu, nebo do vedlejšího. Následně, pokud je označen pouze jedna položka, která je složkou, se tato složka komprimuje do archivu formátu .zip se stejným názvem, jako měla původní složka, a to v novém vláknu. Pokud již archiv s tímto názvem existuje, uživateli se zobrazí chybová hláška a akce se neprovede.
Pokud je označen soubor či více položek, je uživatel dotázán na jméno archivu. Pokud již archiv s tímto názvem existuje, uživateli se zobrazí chybová hláška a akce se neprovede. 
Platforma .NET umožňuje vytvářet archivy pouze ze složek, což znamená, že musí být vybrané soubory zkopírovány do dočasné složky. Aby se zamezilo konfliktu s již existujícími složkami, je tato dočasná složka pojmenována ve formátu název archivu_tmp_čas vytvoření proměnné názvu složky v unix timestampu. Šance, že bude adresář obsahovat složku s tímto názvem, je velmi malá. Následuje pokus vytvořit složku v try/catch bloku, který ošetřuje výjimky jako délka názvu archivu/složky či práva k úpravě adresáře. Poté jsou všechny označené soubory v novém vlákně překopírovány do dočasné složky a ta je opět v novém vlákně zkomprimována do archivu, načež je smazána. Jménem archivu je jméno zadané uživatelem. Po skončení akce je o tom uživatel informován a zobrazené soubory v obou panelech jsou obnoveny.

### Extract()

















## Programovací prostředky

### Použité knihovny
- GtkSharp - https://github.com/GtkSharp/GtkSharp
- YamlDotNet - https://github.com/aaubry/YamlDotNet

### Vývojové prostředí
Pro vývoj programu na linuxu jsem použil JetBrains Rider ver. 2022.3.2 a jeho doplněk ReSharper pro formátování kódu a upozorňování na konvence názvů proměnných. Pro debugování na Windows jsem použil Visual Studio 2022. Při vývoji jsem používal doplněk GitHub Autopilot, a to zejména kvůli zjednodušení opakovaného volání konstruktorů s velkým počtem argumentů nebo importování tříd v hlavičkách souborů. Také jsem s jeho pomocí psal některé vysvětlující komentáře v kódu - tyto komentáře jsou na konci označeny písmeny GC (generováno copilotem). Všechny podstatnější či rozsáhlejší části kódu, které doplněk vygeneroval, jsou v kódu viditelně označeny komentářem //Generováno GitHub Copilotem. Rovněž jsem použil balíček PackageRestore (https://www.nuget.org/packages/PackageRestore) kvůli pár problémům s odstraněním knihoven.

## Spuštění
