public class Prisberegner
{
    public bool TryGetItemPrice(char varekode, out int price)
    {
        return itemPrices.TryGetValue(varekode, out price);
    }

    public event VareListeÆndretHandler VareListeÆndret;
    private List<Vare> solgteVarer = new List<Vare>();

    // Category table
    private Dictionary<char, int> productGroupMapping = new Dictionary<char, int>
    {

        { 'A', 1 },
        { 'B', 2 },
        { 'C', 3 },
        { 'D', 4 },
        { 'E', 5 },
        { 'F', 6 },
        { 'G', 7 },
        { 'H', 8 },
        { 'I', 9 },
        { 'J', 1 },
        { 'K', 2 },
        { 'L', 3 },
        { 'M', 4 },
        { 'N', 5 },
        { 'O', 6 },
        { 'P', 7 },
        { 'Q', 8 },
        { 'R', 9 },
        { 'S', 1 },
        { 'T', 2 },
        { 'U', 3 },
        { 'V', 4 },
        { 'W', 5 },
        { 'X', 6 },
        { 'Y', 7 },
        { 'Z', 8 }
    };

    // Product price table
    private Dictionary<char, int> itemPrices = new Dictionary<char, int>
{
    { 'A', 100 },
    { 'B', 150 },
    { 'C', 110 },
    { 'D', 120 },
    { 'E', 130 },
    { 'F', 140 },
    { 'G', 150 },
    { 'H', 160 },
    { 'I', 170 },
    { 'J', 180 },
    { 'K', 190 },
    { 'L', 200 },
    { 'M', 210 },
    { 'N', 220 },
    { 'O', 230 },
    { 'P', 240 },
    { 'Q', 250 },
    { 'R', 260 },
    { 'S', 270 },
    { 'T', 280 },
    { 'U', 290 },
    { 'V', 300 },
    { 'W', 310 },
    { 'X', 320 },
    { 'Y', 330 },
    { 'Z', 340 }
};

    // multipack table
    public Dictionary<char, (bool IsMultipack, int MultipackAntal)> multipackDetails = new Dictionary<char, (bool, int)>
{
    { 'A', (true, 2) },
    { 'B', (true, 3) },
    { 'C', (false, 0)},
    { 'D', (true, 4) },
    { 'E', (false, 0) },
    { 'F', (false, 0) },
    { 'G', (true, 2) },
    { 'H', (false, 0) },
    { 'I', (true, 3) },
    { 'J', (false, 0) },
    { 'K', (true, 4) },
    { 'L', (true, 2) },
    { 'M', (false, 0) },
    { 'N', (true, 3) },
    { 'O', (true, 5) },
    { 'P', (false, 0) },
    { 'Q', (true, 4) },
    { 'R', (true, 6) },
    { 'S', (false, 0) },
    { 'T', (true, 3) },
    { 'U', (true, 2) },
    { 'V', (false, 0) },
    { 'W', (true, 5) },
    { 'X', (false, 0) },
    { 'Y', (true, 4) },
    { 'Z', (false, 0) }
};

    // campain table
    public Dictionary<char, (bool IsKampagne, int KampagneAntal, int KampagnePris)> kampagneDetails = new Dictionary<char, (bool, int, int)>
{
    { 'A', (true, 3, 250) },
    { 'B', (true, 2, 130) },
    { 'C', (false, 0, 0) },
    { 'D', (true, 4, 300) },
    { 'E', (false, 0, 0) },
    { 'F', (true, 2, 220) },
    { 'G', (false, 0, 0) },
    { 'H', (true, 3, 180) },
    { 'I', (false, 0, 0) },
    { 'J', (true, 2, 210) },
    { 'K', (false, 0, 0) },
    { 'L', (true, 3, 270) },
    { 'M', (false, 0, 0) },
    { 'N', (true, 2, 200) },
    { 'O', (false, 0, 0) },
    { 'P', (true, 5, 400) },
    { 'Q', (false, 0, 0) },
    { 'R', (true, 3, 350) },
    { 'S', (false, 0, 0) },
    { 'T', (true, 2, 150) },
    { 'U', (false, 0, 0) },
    { 'V', (true, 3, 290) },
    { 'W', (false, 0, 0) },
    { 'X', (true, 4, 310) },
    { 'Y', (false, 0, 0) },
    { 'Z', (true, 2, 240) }
};

    // multipack substitution rules
    public Dictionary<char, (char CorrespondingSingleItem, int Quantity)> multipackMappings = new Dictionary<char, (char, int)>
{
    { 'R', ('F', 6) }, // multipack subsittution
};


    public void TilføjVare(Vare vare)
{
    // Tjek for multipack-mapping først
    if (multipackMappings.TryGetValue(vare.Varekode, out var mapping))
    {
        if (itemPrices.TryGetValue(mapping.CorrespondingSingleItem, out int singleItemPrice))
        {
            // Sæt pris og antal baseret på multipack-mapping
            vare.Varekode = mapping.CorrespondingSingleItem;
            vare.Pris = singleItemPrice * mapping.Quantity;
            vare.Antal = mapping.Quantity;
            vare.ErMultipack = true;
            vare.MultipackAntal = mapping.Quantity;
        }
    }
    else
    {
        // Håndter derefter normale priser, hvis ikke en multipack
        if (itemPrices.TryGetValue(vare.Varekode, out int itemPrice))
        {
            vare.Pris = itemPrice;
            vare.Antal = 1; // Standard antal for en enkelt vare
            vare.ErMultipack = multipackDetails.TryGetValue(vare.Varekode, out var multipackInfo) && multipackInfo.IsMultipack;
            if (vare.ErMultipack)
            {
                vare.MultipackAntal = multipackInfo.MultipackAntal;
            }
        }
    }

    HandleKampagne(vare);

    if (productGroupMapping.TryGetValue(vare.Varekode, out int mappedVaregruppe))
    {
        vare.Varegruppe = mappedVaregruppe;
    }

    solgteVarer.Add(vare); // add to list
    VareListeÆndret?.Invoke(solgteVarer);
}



    private void HandleMultipack(Vare vare)
    {
        if (vare.ErMultipack)
        {
            vare.Pris *= vare.MultipackAntal;
            vare.Antal = vare.MultipackAntal;
        }
    }
    private void HandleKampagne(Vare vare)
    {
        // Just flagging if the item is part of a campaign
        if (kampagneDetails.TryGetValue(vare.Varekode, out var kampagne))
        {
            vare.ErKampagne = true;
            vare.KampagneAntal = kampagne.KampagneAntal;
            vare.KampagnePris = kampagne.KampagnePris;
        }
    }

    public void BeregnTotalPris()
{
    int totalPris = 0;

    // Debug: Udskriv alle varekoder for at se, hvilke varer der er tilføjet
    foreach (var item in solgteVarer)
    {
        Console.WriteLine(item.Varekode + " <-");
    }

    // Opret en dictionary for at holde styr på antallet af hver varekode
    var itemCounts = new Dictionary<char, int>();

    foreach (var vare in solgteVarer)
    {
        // Tæl antallet af hver varekode
        if (!itemCounts.ContainsKey(vare.Varekode))
        {
            itemCounts[vare.Varekode] = 0;
        }
        itemCounts[vare.Varekode]++;

        // Håndter kampagnevarer
        if (vare.ErKampagne && kampagneDetails.TryGetValue(vare.Varekode, out var kampagne))
        {
            int count = itemCounts[vare.Varekode];
            // Hvis antallet af en varekode matcher antallet krævet for en kampagne, anvend kampagneprisen
            if (count % kampagne.KampagneAntal == 0)
            {
                totalPris += kampagne.KampagnePris - (kampagne.KampagneAntal - 1) * vare.Pris;
            }
            else
            {
                // Hvis ikke, brug den normale pris for varen
                totalPris += vare.Pris;
            }
        }
        else
        {
            // Hvis varen ikke er en del af en kampagne, anvend dens normale pris
            totalPris += vare.Pris;
        }
    }

    // Udskriv den samlede pris for alle varer
    Console.WriteLine($"Total price: {totalPris} DKK");
}

    public void BeregnDyrePris()
    {
        var gruppedeVarer = solgteVarer
            .GroupBy(v => v.Varegruppe)
            .OrderBy(g => g.Key)
            .Select(group => new
            {
                Varegruppe = group.Key,
                AntalStyk = group.Sum(v => v.Antal)
            });

        Console.WriteLine("Sold items by product groups:");
        foreach (var gruppe in gruppedeVarer)
        {
            Console.WriteLine($"Product group {gruppe.Varegruppe}: {gruppe.AntalStyk} pcs");
        }
    }

    public void BeregnPant()
    {
        var pantVarer = solgteVarer.Where(v => v.UdløserPant);
        int pantBeløb = pantVarer.Sum(v => v.Antal * 10); // Assuming 10 DKK per unit of deposit
        Console.WriteLine($"Deposit amount: {pantBeløb} DKK");
    }
}
