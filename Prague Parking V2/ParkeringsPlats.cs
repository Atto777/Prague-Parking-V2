public class ParkeringsPlats
{
    public int Platsnummer { get; set; }

    public List<Fordon> Fordon { get; set; }

    public ParkeringsPlats(int nummer)
    {
        Platsnummer = nummer;
        Fordon = new List<Fordon>();
    }


    public bool FinnsPlats(Fordon nyttFordon)
    {
        if (Fordon.Count == 0)
        {
            return true;
        }
        if (nyttFordon is MC && Fordon.Count == 1 && Fordon[0] is MC)
        {
            return true;
        }
        return false;
    }
public bool LaggTill(Fordon nyttFordon)
    {
        if (FinnsPlats(nyttFordon))
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
