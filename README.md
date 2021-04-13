# MSR-100-Magnetic-Stripe-Parser

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/7a172146d87745a58cd66826c2d61b0e)](https://www.codacy.com/gh/jarmyo/MSR-100-Magnetic-Stripe-Parser/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=jarmyo/MSR-100-Magnetic-Stripe-Parser&amp;utm_campaign=Badge_Grade)
[![CodeFactor](https://www.codefactor.io/repository/github/jarmyo/msr-100-magnetic-stripe-parser/badge)](https://www.codefactor.io/repository/github/jarmyo/msr-100-magnetic-stripe-parser)
![dotnet](https://github.com/jarmyo/MSR-100-Magnetic-Stripe-Parser/actions/workflows/dotnet.yml/badge.svg)

Autoconnect MSR-100 Magnetic Stripe Card Reader Throught RS232 Port and returns MagneticCardInfo class with data on Swipe event.

This Reader:

![Reader Device](/docs/msr100A.jpg "Reader Device")

## ISO 7813 For financial transaction cards

https://en.wikipedia.org/wiki/ISO/IEC_7811

## How to use

Instance a `MSR100Controller` object, attach a custom method to `OnCardSwiped` event.

Constructor has two parameters:

-   `PortName` is a **optional** string parameter that will override the port name, default is COM1
-   `baudRate` is a **optional** integer parameter that will override the baud rate, default is 9600 

Class has one public event:

-   `OnCardSwiped` is event that will fire every time a card is swiped on the device, return an object of type `MagneticCardInfo`

## Use Example

```csharp
    static void Main(string[] args)
    {
            using (var controller = new MSR100Controller())
            {
                    controller.OnCardSwiped += Controller_OnCardSwiped;
                    Console.WriteLine("Press any key to continue...");
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

## Contributing

-   **Bug Reporting**: You can contribute opening [issues](https://github.com/jarmyo/MSR-100-Magnetic-Stripe-Parser/issues).
-   **Bug Fixing**: Fix it and help others and write some [tests](https://github.com/jarmyo/MSR-100-Magnetic-Stripe-Parser/tree/main/MSR-100-Magnetic-Stripe-ParserTests) to make sure that everything are working propertly.
-   **Improving**: Open an [issue](https://github.com/jarmyo/MSR-100-Magnetic-Stripe-Parser/issues) and lets discuss it.

![Reader Device](/docs/msr100B.jpg "Reader Device")
