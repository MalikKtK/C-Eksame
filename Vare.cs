public class Vare
{
    public char Varekode { get; set; }
    public int Pris { get; set; }
    public int Antal { get; set; }
    public int Varegruppe { get; set; }
    public bool ErMultipack { get; set; }
    public int MultipackAntal { get; set; }
    public bool ErKampagne { get; set; }
    public int KampagneAntal { get; set; }
    public int KampagnePris { get; set; }
    public bool UdløserPant { get; set; }

    public Vare(char varekode, int pris, int varegruppe, bool erMultipack = false, int multipackAntal = 1,
                bool erKampagne = false, int kampagneAntal = 0, int kampagnePris = 0, bool udløserPant = false)
    {
        Varekode = varekode;
        Pris = pris;
        Antal = 1;
        Varegruppe = varegruppe;
        ErMultipack = erMultipack;
        MultipackAntal = multipackAntal;
        ErKampagne = erKampagne;
        KampagneAntal = kampagneAntal;
        KampagnePris = kampagnePris;
        UdløserPant = udløserPant;
    }
}