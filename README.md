# OISISI-Projekat – Sajam knjiga

Konzolna aplikacija u C# (.NET 10) za vođenje evidencije sajma knjiga: posetioci, autori, knjige, kupovine i izdavači.

## Tim
- Aleksa Nedeljković
- Bogdan Injac

## Struktura projekta
- `Core/Models` – modeli (entiteti)
- `Core/DAO` – klase za rad sa podacima (učitavanje, dodavanje, izmena, brisanje)
- `Core/Utils` – zajednička serijalizacija u fajlove
- `ConsoleApp/Meniji` – konzolni meniji i validacija unosa
- `ConsoleApp/Data` – test podaci (CSV fajlovi)

## Čuvanje podataka
Podaci se čuvaju u tekstualnim fajlovima u folderu `Data`.
Jedna linija = jedan objekat, polja su odvojena znakom `|`, a liste ID-jeva zarezom.
Veze između entiteta čuvaju se preko ID-jeva.

## Ko je šta radio

### Aleksa
- Modeli: `Adresa`, `Autor`, `Knjiga`
- `ISerializable` i `Serializer<T>` (zajednička serijalizacija)
- `BaseDAO<T>`, `AdresaDAO`, `AutorDAO`, `KnjigaDAO`
- Konzola: `AutorMeni` i `KnjigaMeni` (prikaz, dodavanje, izmena, brisanje)
- Test podaci (`adrese.txt`, `autori.txt`, `knjige.txt`)

### Bogdan
- Solution i glavni meni
- Modeli: `Posetilac`, `StatusPosetioca`, `Kupovina`, `Izdavac`
- `PosetilacDAO`, `KupovinaDAO`, `IzdavacDAO`
- Konzola: `PosetilacMeni`, `KupovinaMeni`, `IzdavacMeni`
- `Unos` (validacija unosa sa tastature)

## Pokretanje
1. Klonirati repozitorijum.
2. Otvoriti `SajamKnjiga/SajamKnjiga.sln` u Visual Studiju 2026.
3. Pokrenuti projekat `ConsoleApp` (F5).
