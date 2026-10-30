using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Bil), "bil")]
[JsonDerivedType(typeof(MC), "mc")]

public class Fordon
{
    public string Registreringsnummer { get; set; }
    public DateTime Ankomsttid { get; set; }

    public Fordon(string registreringsnummer)
    {
        Registreringsnummer = registreringsnummer;
        Ankomsttid = DateTime.Now;
    }
}
