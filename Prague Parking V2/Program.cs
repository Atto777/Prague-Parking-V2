using Spectre.Console;
class Program
{
    static ParkeringsHus parkeringshus = new ParkeringsHus();
    static void Main()
    {
        VisaMeny();
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

            string val = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Vad vill du göra?[/]")
                    .AddChoices(
                        "Parkera fordon",
                        "Flytta fordon",
                        "Hämta ut fordon",
                        "Sök fordon",
                        "Visa parkeringshus",
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
            else
            {
                AnsiConsole.MarkupLine(
                    $"[green]Du valde:[/] {val}");

                Console.ReadKey();
            }
        }
    }



    static void ParkeraFordon()
    {
        string regnummer = AnsiConsole.Ask<string>(
            "[yellow]Ange registreringsnummer:[/]");

        Bil bil = new Bil(regnummer);

        foreach (ParkeringsPlats plats in parkeringshus.Platser)
        {
            if (plats.LaggTill(bil))
            {
                AnsiConsole.MarkupLine(
                    $"[green]Bil {bil.Registreringsnummer} har parkerats på plats {plats.Platsnummer}![/]");

                return;
            }
        }

        AnsiConsole.MarkupLine("[red]Parkeringen är full![/]");
    }

}