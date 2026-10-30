# Prague Parking V2

## Beskrivning
Prague Parking V2 är ett parkeringssystem byggt i C# med objektorienterad programmering (OOP).

Programmet hanterar bilar och motorcyklar i ett parkeringshus.

## Funktioner
- Parkera bilar och motorcyklar.
- Två motorcyklar kan dela en parkeringsplats.
- Söka efter fordon via registreringsnummer.
- Flytta fordon mellan parkeringsplatser.
- Hämta ut fordon och beräkna parkeringsavgift.
- Visa parkeringshuset med Spectre.Console.
- Spara och läsa parkeringsdata med JSON.
- Ändra inställningar och ladda om priser.

## Teknik
- C#
- .NET 10
- Visual Studio
- Spectre.Console
- JSON
- Objektorienterad programmering med klasser och arv

## Starta programmet
1. Öppna lösningen i Visual Studio.
2. Kontrollera att projektet Prague Parking V2 är startprojekt.
3. Bygg lösningen.
4. Starta programmet med Ctrl + F5.

Datorn behöver stöd för .NET 10 och projektets NuGet-beroenden måste kunna återställas.

## Konfiguration
Filen `config.json` innehåller inställningar för:

- Antal parkeringsplatser.
- Parkeringspris för bil och MC.
- Antal gratisminuter.
- Maximalt antal bilar och MC per parkeringsplats.

Standardpriset är 20 CZK per påbörjad timme för bil och 10 CZK för MC. Parkering i högst 10 minuter är gratis.

## Filhantering
Parkerade fordon sparas i `parkering.json`. Filen uppdateras när fordon parkeras, flyttas eller hämtas ut.

Filhanteringen ligger i ett separat klassbibliotek som heter `PragueParking.Filhantering`.