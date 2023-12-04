using System;
using System.Collections.Generic;
using System.Linq;

public delegate void VareListeÆndretHandler(List<Vare> varer);

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

public class Prisberegner
{
       public bool TryGetItemPrice(char varekode, out int price)
    {
        return itemPrices.TryGetValue(varekode, out price);
    }

    public event VareListeÆndretHandler VareListeÆndret;
    private List<Vare> solgteVarer = new List<Vare>();

    // Define a dictionary to map product codes to product groups
    private Dictionary<char, int> productGroupMapping = new Dictionary<char, int>
    {
        { 'A', 1 }, // Map 'A' to product group 1
        { 'B', 2 }, // Map 'B' to product group 2
        { 'C', 3 }, // Map 'C' to product group 3
        { 'D', 4 }, // Map 'D' to product group 4
        { 'E', 5 }, // Map 'E' to product group 5
        { 'F', 6 }, // Map 'F' to product group 6
        { 'G', 7 }, // Map 'G' to product group 7
        { 'H', 8 }, // Map 'H' to product group 8
        { 'I', 9 }, // Map 'I' to product group 9
        { 'J', 1 }, // Map 'J' to product group 1
        { 'K', 2 }, // Map 'K' to product group 2
        { 'L', 3 }, // Map 'L' to product group 3
        { 'M', 4 }, // Map 'M' to product group 4
        { 'N', 5 }, // Map 'N' to product group 5
        { 'O', 6 }, // Map 'O' to product group 6
        { 'P', 7 }, // Map 'P' to product group 7
        { 'Q', 8 }, // Map 'Q' to product group 8
        { 'R', 9 }, // Map 'R' to product group 9
        { 'S', 1 }, // Map 'S' to product group 1
        { 'T', 2 }, // Map 'T' to product group 2
        { 'U', 3 }, // Map 'U' to product group 3
        { 'V', 4 }, // Map 'V' to product group 4
        { 'W', 5 }, // Map 'W' to product group 5
        { 'X', 6 }, // Map 'X' to product group 6
        { 'Y', 7 }, // Map 'Y' to product group 7
        { 'Z', 8 }, // Map 'Z' to product group 8
    };

    // Define a dictionary to map product codes to prices
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

    // Example structure, add to your Prisberegner class
public Dictionary<char, (bool IsMultipack, int MultipackAntal)> multipackDetails = new Dictionary<char, (bool, int)>
{
    { 'A', (true, 2) },
    { 'B', (true, 3) },
    { 'C', (false, 0) },
    { 'D', (true, 4) },
    { 'E', (false, 0) },
    { 'F', (true, 5) },
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


public void TilføjVare(Vare vare)
{
    // Retrieve multipack details
    if (multipackDetails.TryGetValue(vare.Varekode, out var multipackInfo) && multipackInfo.IsMultipack)
    {
        vare.ErMultipack = true;
        vare.MultipackAntal = multipackInfo.MultipackAntal;

        // Adjust the price and quantity for multipack
        vare.Pris = itemPrices[vare.Varekode] * vare.MultipackAntal;
        vare.Antal = vare.MultipackAntal;
    }
    else
    {
        // Set price for non-multipack items
        if (itemPrices.TryGetValue(vare.Varekode, out int itemPrice))
        {
            vare.Pris = itemPrice;
        }
    }

    HandleKampagne(vare);

    // Set product group
    if (productGroupMapping.TryGetValue(vare.Varekode, out int mappedVaregruppe))
    {
        vare.Varegruppe = mappedVaregruppe;
    }

    solgteVarer.Add(vare);
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

    // Create a dictionary to keep track of how many items of each product code have been processed
    var itemCounts = new Dictionary<char, int>();

    foreach (var vare in solgteVarer)
    {
        if (!itemCounts.ContainsKey(vare.Varekode))
        {
            itemCounts[vare.Varekode] = 0;
        }
        itemCounts[vare.Varekode]++;

        if (vare.ErKampagne && kampagneDetails.TryGetValue(vare.Varekode, out var kampagne))
        {
            int count = itemCounts[vare.Varekode];
            if (count % kampagne.KampagneAntal == 0)
            {
                // Apply campaign price for each complete set
                totalPris += kampagne.KampagnePris - (kampagne.KampagneAntal - 1) * itemPrices[vare.Varekode];
            }
            else
            {
                // Regular price for items that don't complete a campaign set
                totalPris += itemPrices[vare.Varekode];
            }
        }
        else
        {
            // Regular pricing for items not part of a campaign
            totalPris += vare.Pris;
        }
    }

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

public class Program
{
    public static void Main()
    {
        Prisberegner billigPrisberegner = new Prisberegner();
        Prisberegner dyrPrisberegner = new Prisberegner();

        billigPrisberegner.VareListeÆndret += (varer) => billigPrisberegner.BeregnTotalPris();
        dyrPrisberegner.VareListeÆndret += (varer) => dyrPrisberegner.BeregnDyrePris();

        Random random = new Random();
        Console.WriteLine("Scan a product (enter product code), type 'end' to finish:");

while (true)
{
    Console.Write("Enter product code: ");
    string input = Console.ReadLine();

    if (input.Equals("end", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

// Inside your while loop in the Main method
if (input.Length == 1 && char.TryParse(input, out char varekode))
{
    if (!billigPrisberegner.TryGetItemPrice(varekode, out int pris))
    {
        Console.WriteLine($"Unknown product code: {varekode}");
        continue;
    }

    // Retrieve multipack and campaign details
    billigPrisberegner.multipackDetails.TryGetValue(varekode, out var multipackInfo);
    billigPrisberegner.kampagneDetails.TryGetValue(varekode, out var kampagneInfo);

    Vare vare = new Vare(varekode, pris, 0, multipackInfo.IsMultipack, multipackInfo.MultipackAntal, kampagneInfo.IsKampagne, kampagneInfo.KampagneAntal, kampagneInfo.KampagnePris, varekode == 'P');

                try
                {
                    billigPrisberegner.TilføjVare(vare);

                    // Only add the item to the "expensive" calculator if it's not a multipack
                if (!vare.ErMultipack)
{
    dyrPrisberegner.TilføjVare(vare);
}
                    Console.WriteLine($"Processing item {varekode}...");
                    System.Threading.Thread.Sleep(500); // Simulate delay for each product
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input, try again.");
            }
        }

        // Calculate deposit after scanning is completed
        billigPrisberegner.BeregnPant();
        dyrPrisberegner.BeregnPant();
    }
}
