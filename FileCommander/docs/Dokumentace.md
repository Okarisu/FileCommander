Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Et harum quidem rerum facilis est et expedita distinctio. Curabitur bibendum justo non orci. Curabitur ligula sapien, pulvinar a vestibulum quis, facilisis vel sapien. Nam sed tellus id magna eleme - 255



# Cíl práce
Cílem práce je vytvořit v programovacím jazyce C# s použitím knihovny GTK multiplatformní  
desktopovou aplikaci pro správu souborů. Mezi funkce programu bude patřit přesouvání a  
kopírování souborů a složek, vytváření nových složek, mazání souborů a složek a komprese a dekomprese archivů.

## Popis programu
Aplikace zobrazuje ve dvou paralelně umístěných panelech obsah dvou adresářů. V těchto oknech uživatel označí soubor či soubory, se kterými si přeje pracovat, a následně na horní nástrojové liště zvolí akci, kterou chce provést. V adresářích se uživatel pohybuje pomocí nástrojové lišty každého panelu.

### Navigace
Program po spuštění zobrazí v obou panelech domovskou složku uživatele. Na platformě Linux je to /home/username, na Windows složka dokumenty. Adresáře uživatel prochází dvojitým kliknutím na složku, kterou si přeje otevřít. Po dvojkliku se panel překreslí a zobrazí obsah vybrané složky.
Každý panel má nad sebou svoji navigační lištu. Ta obsahuje tlačítko Home, které v daném panelu zobrazí domovskou složku, a tlačítko Up, které uživatele přesune o jednu úroveň výše. Dále tlačítka Back a Forward, kterými může uživatel procházet skrze historii navštívených složek dopředu a dozadu. Pokud jsou k počítači připojena přenosná média nebo, pokud má počítač více pevných disků nebo pokud má disk více viditelných oddílů (na linuxu musí být tyto oddíly připojeny), zobrazí se rovněž v této liště. Po kliknutí na tlačítko nesoucí název disku

### Práce se soubory
Pokud uživatel vytváří novou složku, děje se tak v panelu, který byl v tu dobu soustředěný, tedy bylo na něj kliknuto myší naposledy.

Přejmenovává-li složku nebo soubor, je vyzván, aby do dialogového okna, které se objeví, napsal nový název složky nebo souboru. Pokud uživatel přejmenovává více souborů nebo složek najednou, je upozorněn, že k nim bude připojen číselná přípona.
Pokud uživatel složku nebo soubor maže, je vyzván k potvrzení této akce dialogovým oknem. V něm má možnost zaškrtnout "již se nedotazovat" a při příští akci mazání již budou soubory smazány bez potvrzení.

V případě, že uživatel soubory kopíruje nebo přesouvá, cílovým adresářem je adresář zobrazený ve vedlejším panelu, který není soustředěn. Označí-li tedy uživatel soubory nejdříve v levém a následně v pravém panelu, akce se provede pro soubory v pravém panelu a cílovým adresářem bude ten v panelu levém. Pokud již cílový adresář obsahuje soubor(y) se stejným jménem, jako je jméno kopírovaného souboru, je uživatel dotázán, zda si přeje pokračovat. Při potvrzení se ke jménům přikopírovaných souborů připojí do závorky číslo kopie (tedy např. soubor (1).txt). Pokud již složka obsahuje soubory se stejným jménem, jako jsou ty, jež jsou sem přesouvány, program je vynechá a na konci akce uživatele upozorní, že zde nastal problém s duplicitními názvy.

Pokud uživatel vytváří archiv, označí jeden nebo více souborů či složek a klikne na tlačítko Compress. Následně je tázán, zda si přeje soubory komprimovat do adresáře v soustředěném panelu, nebo do toho v nesoustředěném. Akce se pak provádí pro zvolený adresář. Pokud uživatel označil pouze jednu složku, nese výsledný archiv stejný název. Pokud pouze jeden soubor, je vyzván k zadání názvu archivu. Označil-li souborů více, je po zvolení cílového složky vyzván k zadání názvu archivu.

Pokud uživatel jeden nebo více archivů najednou extrahuje, je opět vyzván ke zvolení cílového adresáře, kam se archiv(y) rozbalí. Výsledkem je složka nesoucí stejný název jako název archivu, která obsahuje z něj rozbalené soubory.

