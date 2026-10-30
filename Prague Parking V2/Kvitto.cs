
using Spectre.Console;

public class Kvitto
{
    public static void Visa(Fordon fordon, int platsnummer,
        DateTime sluttid, TimeSpan tid, int kostnad)
    {
        var tabell = new Table();

        tabell.Border(TableBorder.Rounded);
        tabell.BorderColor(Color.Yellow);

        tabell.AddColumn("[blue]Information[/]");
        tabell.AddColumn("[blue]Värde[/]");

        tabell.AddRow("[blue]Registreringsnummer[/]",
            $"[blue]{fordon.Registreringsnummer}[/]");

        tabell.AddRow("[blue]Fordonstyp[/]",
            $"[blue]{fordon.GetType().Name}[/]");

        tabell.AddRow("[blue]Parkeringsplats[/]",
            $"[blue]P{platsnummer}[/]");

        tabell.AddRow("[blue]Ankomsttid[/]",
            $"[blue]{fordon.Ankomsttid:HH:mm}[/]");

        tabell.AddRow("[blue]Uthämtningstid[/]",
            $"[blue]{sluttid:HH:mm}[/]");

        tabell.AddRow("[blue]Parkerad tid[/]",
            $"[blue]{tid.TotalMinutes:F0} minuter[/]");

        tabell.AddRow("[blue]Att betala[/]",
            $"[blue]{kostnad} CZK[/]");

        AnsiConsole.Write(tabell);
    }
}
