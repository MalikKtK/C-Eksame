using System;
using System.Collections.Generic;
using System.Linq;


public delegate void VareListeÆndretHandler(List<Vare> varer);

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
