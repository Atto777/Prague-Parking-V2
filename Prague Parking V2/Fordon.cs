public class Fordon 
{
public string Registreringsnummer { get; set; }
public DateTime Ankomsttid { get; set; }

    public Fordon(string regnummer)
    {
        Registreringsnummer = regnummer;
        Ankomsttid = DateTime.Now;
    }
} 