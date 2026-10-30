using Spectre.Console;

using PragueParking.Filhantering;

using System.Text.Json;

class Program
{
    static ParkeringsHus parkeringshus = new ParkeringsHus();
    static Konfiguration config = new Konfiguration();
    static void Main(string[] args)
    {
        try
        {

            if (File.Exists("config.json"))
            {
                string json = File.ReadAllText("config.json");

                config = JsonSerializer.Deserialize<Konfiguration>(json)
                         ?? new Konfiguration();
            }

            if (config.AntalPlatser < 1 ||
                config.MaxBilarPerPlats != 1 ||
                config.MaxMCPerPlats < 1 ||
                config.MaxMCPerPlats > 2 ||
                config.PrisBil < 0 ||
                config.PrisMC < 0 ||
                config.GratisMinuter < 0)
            {
                Console.WriteLine("Felaktiga inställningar i config.json!");
                Console.WriteLine("Kontrollera antal platser, fordon och priser.");
                return;
            }

            parkeringshus = new ParkeringsHus(config.AntalPlatser);

            List<ParkeringsPlats>? sparadePlatser =
                FilHantering.Ladda<List<ParkeringsPlats>>();

            parkeringshus.LaddaPlatser(sparadePlatser);

            VisaMeny();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ett fel uppstod:");
            Console.WriteLine(ex.ToString());

            Console.WriteLine("\nTryck på valfri tangent...");
            Console.ReadKey();
        }
    }

    static void VisaMeny()
    {
        bool körProgram = true;

        while (körProgram)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("PRAGUE PARKING")
                    .Centered()
                    .Color(Color.Blue));


            var val = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Välj ett alternativ:[/]")
                    .HighlightStyle("blue bold")
                    .AddChoices(
                        "Parkera fordon",
                        "Flytta fordon",
                        "Hämta ut fordon",
                        "Sök fordon",
                        "Visa parkeringshus",
                        "Ladda om priser",
                        "Avsluta"
                    )
            );


            if (val == "Avsluta")
            {
                körProgram = false;
            }
            else if (val == "Parkera fordon")
            {
                ParkeraFordon();
                Console.ReadKey();
            }
            else if (val == "Visa parkeringshus")
            {
                ParkeringsKarta.Visa(parkeringshus);
                Console.ReadKey();
            }
            else if (val == "Sök fordon")
            {
                SokFordon();
                Console.ReadKey();
            }
            else if (val == "Flytta fordon")
            {
                FlyttaFordon();
                Console.ReadKey();
            }
            else if (val == "Hämta ut fordon")
            {
                HamtaUtFordon();
                Console.ReadKey();
            }
            else if (val == "Ladda om priser")
            {
                LaddaOmPriser();
                Console.ReadKey();
            }
            else
            {
                AnsiConsole.MarkupLine(
                    $"[yellow]Du valde:[/] {val}");

                Console.ReadKey();
            }
        }
    }


    static void ParkeraFordon()
    {
        string typ = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[blue]Vilket fordon vill du parkera?[/]")
                .HighlightStyle("blue bold")
                .AddChoices("Bil", "MC")
        );

        string regnummer = AnsiConsole.Ask<string>(
            "[blue]Ange registreringsnummer:[/]");

        if (parkeringshus.HittaFordon(regnummer) != null)
        {
            AnsiConsole.MarkupLine(
                "[red]Det finns redan ett fordon med detta registreringsnummer![/]");
            return;
        }

        Fordon nyttFordon;

        if (typ == "Bil")
        {
            nyttFordon = new Bil(regnummer);
        }
        else
        {
            nyttFordon = new MC(regnummer);
        }
        
        ParkeringsPlats? plats =
    parkeringshus.ParkeraFordon(nyttFordon, config.MaxMCPerPlats);


        if (plats != null)
        {
            FilHantering.Spara(parkeringshus.Platser);

            AnsiConsole.MarkupLine(
                $"[yellow]Fordonet parkerades på plats P{plats.Platsnummer}![/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                "[red]Tyvärr finns det ingen ledig plats![/]");
        }
    }

    static void SokFordon()
    {
        string regnummer = AnsiConsole.Ask<string>(
            "[yellow]Ange registreringsnummer att söka efter:[/]");

        Fordon? fordon = parkeringshus.HittaFordon(regnummer);

        if (fordon == null)
        {
            AnsiConsole.MarkupLine("[red]Fordonet hittades inte![/]");
            return;
        }

        var tabell = new Table();
        tabell.Border(TableBorder.Rounded);
        tabell.BorderColor(Color.Yellow);

        tabell.AddColumn("[blue]Information[/]");
        tabell.AddColumn("[blue]Värde[/]");

        tabell.AddRow("[blue]Registreringsnummer[/]",
            $"[blue]{fordon.Registreringsnummer}[/]");

        tabell.AddRow("[blue]Fordonstyp[/]",
            $"[blue]{fordon.GetType().Name}[/]");

        tabell.AddRow("[blue]Ankomsttid[/]",
            $"[blue]{fordon.Ankomsttid:HH:mm}[/]");

        ParkeringsPlats? plats = parkeringshus.HittaPlats(fordon);

        if (plats != null)
        {
            tabell.AddRow("[blue]Parkeringsplats[/]",
                $"[blue]P{plats.Platsnummer}[/]");
        }

        AnsiConsole.Write(tabell);
    }

   
    static void FlyttaFordon()
    {
        string regnummer = AnsiConsole.Ask<string>(
            "[yellow]Ange registreringsnummer:[/]");

        Fordon? fordon = parkeringshus.HittaFordon(regnummer);

        if (fordon == null)
        {
            AnsiConsole.MarkupLine("[red]Fordonet hittades inte![/]");
            return;
        }

        ParkeringsPlats? gammalPlats = parkeringshus.HittaPlats(fordon);

        if (gammalPlats == null)
        {
            AnsiConsole.MarkupLine("[red]Parkeringsplatsen hittades inte![/]");
            return;
        }

        int nyttNummer = AnsiConsole.Ask<int>(
            "[yellow]Vilken plats vill du flytta till?[/]");

        if (parkeringshus.FlyttaFordon(fordon, nyttNummer, config.MaxMCPerPlats))
        {
            FilHantering.Spara(parkeringshus.Platser);

            AnsiConsole.MarkupLine(
                $"[yellow]Fordon {fordon.Registreringsnummer} flyttades från P{gammalPlats.Platsnummer} till P{nyttNummer}![/]");
        }
        else
        {
            AnsiConsole.MarkupLine(
                "[red]Flytten kunde inte genomföras. Kontrollera platsnumret och att platsen är ledig.[/]");
        }
    }

    static void HamtaUtFordon()
    {
        string regnummer = AnsiConsole.Ask<string>(
            "[yellow]Ange registreringsnummer:[/]");

        Fordon? fordon = parkeringshus.HittaFordon(regnummer);

        if (fordon == null)
        {
            AnsiConsole.MarkupLine(
                "[red]Fordonet hittades inte![/]");
            return;
        }

        ParkeringsPlats? plats = parkeringshus.HittaPlats(fordon);

        if (plats == null)
        {
            AnsiConsole.MarkupLine(
                "[red]Parkeringsplatsen hittades inte![/]");
            return;
        }

        DateTime sluttid = DateTime.Now;
        TimeSpan tid = sluttid - fordon.Ankomsttid;

        int kostnad = BeraknaKostnad(fordon, tid);

        Kvitto.Visa(
            fordon,
            plats.Platsnummer,
            sluttid,
            tid,
            kostnad);

        plats.TaBort(fordon);

        FilHantering.Spara(parkeringshus.Platser);

        AnsiConsole.MarkupLine(
            "[yellow]Fordonet har hämtats ut![/]");
    }

    static int BeraknaKostnad(Fordon fordon, TimeSpan tid)
    {
        int pris = fordon is Bil ? config.PrisBil : config.PrisMC;

        if (tid.TotalMinutes <= config.GratisMinuter)
        {
            return 0;
        }

        return (int)Math.Ceiling(tid.TotalHours) * pris;
    }

    static void LaddaOmPriser()
    {
        Console.WriteLine("Läser från: " + Path.GetFullPath("config.json"));
        if (!File.Exists("config.json"))
        {
            AnsiConsole.MarkupLine(
                "[red]Kunde inte hitta config.json![/]");
            return;
        }

        try
        {
            string json = File.ReadAllText("config.json");

            Konfiguration? nyaPriser =
                JsonSerializer.Deserialize<Konfiguration>(json);

            if (nyaPriser == null ||
                nyaPriser.PrisBil < 0 ||
                nyaPriser.PrisMC < 0 ||
                nyaPriser.GratisMinuter < 0)
            {
                AnsiConsole.MarkupLine(
                    "[red]Ogiltiga priser eller gratisminuter![/]");
                return;
            }

            config.PrisBil = nyaPriser.PrisBil;
            config.PrisMC = nyaPriser.PrisMC;
            config.GratisMinuter = nyaPriser.GratisMinuter;

            AnsiConsole.MarkupLine(
                "[green]Priserna har laddats om![/]");

            AnsiConsole.MarkupLine(
                $"[yellow]Bil: {config.PrisBil} CZK/timme[/]");

            AnsiConsole.MarkupLine(
                $"[yellow]MC: {config.PrisMC} CZK/timme[/]");

            AnsiConsole.MarkupLine(
                $"[yellow]Gratis: {config.GratisMinuter} minuter[/]");
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine(
                "[red]Kunde inte läsa config.json. Kontrollera filen![/]");
        }
    }

}