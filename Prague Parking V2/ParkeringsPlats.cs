public class ParkeringsPlats
{
    public int Platsnummer { get; set; }

    public List<Fordon> Fordon { get; set; }

    public ParkeringsPlats(int platsnummer)
    {
        Platsnummer = platsnummer;
        Fordon = new List<Fordon>();
    }

    public bool FinnsPlats(Fordon nyttFordon, int maxBilar = 1, int maxMC = 2)
    {
        if (Fordon.Count == 0)
        {
            return true;
        }

        if (nyttFordon is MC && Fordon[0] is MC)
        {
            return Fordon.Count < maxMC;
        }

        return false;
    }

    public bool LaggTill(Fordon nyttFordon, int maxMC = 2)
    {
        if (FinnsPlats(nyttFordon, 1, maxMC))
        {
            Fordon.Add(nyttFordon);
            return true;
        }

        return false;
    }

    public bool TaBort(Fordon fordon)
    {
        return Fordon.Remove(fordon);
    }
}
