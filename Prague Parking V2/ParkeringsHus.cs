public class ParkeringsHus

{
    public List<ParkeringsPlats> Platser { get; set; }

    public ParkeringsHus()
    {
        Platser = new List<ParkeringsPlats>();
        {

            for (int i = 1; i <= 100; i++)
            {
                Platser.Add(new ParkeringsPlats(i));
            }
        }
    }
}