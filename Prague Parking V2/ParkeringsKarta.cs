
using Spectre.Console;

public class ParkeringsKarta
{
    public static void Visa(ParkeringsHus parkeringshus)
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine(
            "[bold blue]PARKERINGSKARTA[/]\n");

        var karta = new Grid();

        for (int i = 0; i < 10; i++)
        {
            karta.AddColumn();
        }

        for (int i = 0; i < parkeringshus.Platser.Count; i += 10)
        {
            var rad = new List<Panel>();

            for (int j = i; j < i + 10 &&
                j < parkeringshus.Platser.Count; j++)
            {
                ParkeringsPlats plats = parkeringshus.Platser[j];

                Color ramFarg = plats.Fordon.Count == 0
                    ? Color.Blue
                    : Color.Red;

                var ruta = new Panel(
                    new Markup($"[white]P{plats.Platsnummer}[/]"))
                {
                    Border = BoxBorder.Rounded,
                    BorderStyle = new Style(ramFarg),
                    Width = 8,
                    Height = 3
                };

                rad.Add(ruta);
            }

            karta.AddRow(rad.ToArray());
        }

        AnsiConsole.Write(karta);

        AnsiConsole.MarkupLine(
            "\n[blue]Blå[/] = Ledig    [red]Röd[/] = Upptagen");
    }
}
