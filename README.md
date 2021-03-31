# MSR-100-Magnetic-Stripe-Parser
Autoconnect MSR-100 Magnetic Stripe Card Reader Throught RS232 Port and returns CardInfo data on Swipe event.


## Info

https://en.wikipedia.org/wiki/ISO/IEC_7813



## How to use

```
        static void Main(string[] args)
        {
            using (var controller = new MSR100Controller())
            {
                controller.OnCardSwiped += Controller_OnCardSwiped;
                Console.WriteLine("Press any key to continue...");
                Console.WriteLine();
                Console.ReadKey();
            }
        }

        private static void Controller_OnCardSwiped(MagneticCardInfo cardinfo)
        {
            Console.WriteLine("Card Swiped!");
            Console.WriteLine($"PAN: {cardinfo.Track1.PAN}");
            Console.WriteLine($"NN: {cardinfo.Track1.Name}");
            Console.WriteLine($"ED: {cardinfo.Track1.ExpirationDate}");
            Console.WriteLine($"SC: {cardinfo.Track1.ServiceCode}");
            Console.WriteLine($"DD: {cardinfo.Track1.DiscretionaryData}");
        }
        
```

## Contribution

All contribution is welcome