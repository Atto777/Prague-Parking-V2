public class ParkeringsHus

{
    public List<ParkeringsPlats> Platser { get; set; }


    public ParkeringsHus(int antalPlatser = 100)
    {
        Platser = new List<ParkeringsPlats>();

        for (int i = 1; i <= antalPlatser; i++)
        {
            Platser.Add(new ParkeringsPlats(i));
        }
    }


    public Fordon? HittaFordon(string regnummer)
    {
        foreach (ParkeringsPlats plats in Platser)
        {
            foreach (Fordon fordon in plats.Fordon)
            {
                if (fordon.Registreringsnummer.Equals(
                    regnummer, StringComparison.OrdinalIgnoreCase))
                {
                    return fordon;
                }
            }
        }

        return null;
    }
    public ParkeringsPlats? HittaPlats(Fordon fordon)
    {
        foreach (ParkeringsPlats plats in Platser)
        {
            if (plats.Fordon.Contains(fordon))
            {
                return plats;
            }
        }

        return null;
    }

    public ParkeringsPlats? ParkeraFordon(Fordon fordon, int maxMC = 2)
    {
        foreach (ParkeringsPlats plats in Platser)
        {
            if (plats.LaggTill(fordon, maxMC))
            {
                return plats;
            }
        }

        return null;
    }


    public void LaddaPlatser(List<ParkeringsPlats>? sparadePlatser)
    {
        if (sparadePlatser == null || sparadePlatser.Count == 0)
            return;

        if (sparadePlatser.Count > Platser.Count)
        {
            Platser = sparadePlatser;
            return;
        }

        for (int i = 0; i < sparadePlatser.Count; i++)
        {
            Platser[i] = sparadePlatser[i];
        }
    }

    public bool FlyttaFordon(Fordon fordon, int nyttNummer, int maxMC = 2)
    {
        ParkeringsPlats? gammalPlats = HittaPlats(fordon);

        if (gammalPlats == null)
        {
            return false;
        }

        if (nyttNummer < 1 || nyttNummer > Platser.Count)
        {
            return false;
        }

        ParkeringsPlats nyPlats = Platser[nyttNummer - 1];

        if (nyPlats == gammalPlats || !nyPlats.FinnsPlats(fordon, 1, maxMC))
        {
            return false;
        }

        gammalPlats.TaBort(fordon);
        nyPlats.LaggTill(fordon);

        return true;
    }

}