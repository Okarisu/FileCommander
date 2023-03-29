# Práce se soubory v jazyce CS
## Úvod
Práce se soubory je důležitou, ne-li klíčovou částí každého programu, který nějakým způsobem ukládá nebo načítá data. Ať už jde o nejrůznější textové, zvukové a grafické editory, které načítají data v souborech již dříve uložená a ukládají data nová, nebo o hry, které načítají textury, dialogy, zvukové soubory a ukládají postup hráče, všechny tyto programy musí nějak se soubory pracovat.
Ve své práci se pokusím popsat, jaké jsou možnosti práce se soubory obecně, na které věci se při ní musí dávat pozor a to vše aplikovat na jazyk C#.

### Soubory
Jako soubor označujeme sadu dat uloženou na datovém médiu (disku v počítači, USB disku, DVD), se kterou lze pracovat jako s jedním celkem a která má nějaké jméno. 
Soubor může obsahovat jak pouze jeden typ dat (text, zvuk, obrázek), tak i více typů: Multimediální soubory (kontejnery) zabalují obraz, zvuk a případně i titulky, kancelářské dokumenty mohou krom textu obsahovat například obrázek.
https://cs.wikipedia.org/wiki/Soubor

### Typy souborů
Soubory můžeme rozdělit podle jejich typu na textové a binární. Mezi textové se řadí takové soubory, které můžeme otevřit v jakémkoli textovém editoru a jejich obsah bude uživateli buď naprosto čitelný, nebo v něm alespoň rozezná strukturu souboru a některá klíčová slova. Patří sem textové dokumenty jako txt, markdown nebo tex, zdrojové kódy programů a webových standardů (html, css, json), obrázky formátu svg, titulky nebo konfigurační soubory. I když je většina spustitelných souborů binárních, najsou se i výjimky - například dávkový soubor bat systému MS Windows nebo unixový shell script jsou textové povahy a jakýmkoli textovým editorem je lze otevřít.

Binární soubory oproti textovým vyžadují speciální program pro jejich otevření. Pokud bychom se jejich obsah pokusili zobrazit v textovém editoru, dostaneme pouze surová data, ze kterých nic nevyčteme. Binárními soubory jsou například (multi)mediální soubory obrázků (vyjma svg), videí a zvuku, kancelářské dokumenty (PDF, doc, prezentace ppt), archivy jako zip, rar, ISO obrazy nebo spustitelné soubory jako exe a dll.

### Koncovky souborů
Formát souboru a to, zda je textový, nebo binární, můžeme určit podle jeho koncovky.