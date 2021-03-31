using NUnit.Framework;

namespace MSR_100_Magnetic_Stripe_ParserTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            DummyTest = new System.Collections.Generic.List<string>()
            {
                "%B4111111111111111^LAST/FIRST^15031019999900888000000?;4111111111111111=150310199999888?",
                "%B5555555555554444^LAST/FIRST^14021019999900888000000?;5555555555554444=14021019999988800000?",
                "%B5168755544412233^PKMMV/UNEMBOXXXX       ^1807111100000000000000111000000?;5168755544412233=18071111000011100000?",
                "%B5350290149345177^FATEHI/SUALEH^16042010000000000000000000000000000567001000?;5350290149345177=16042010000056700100?",
                "%B4815881002861896^YATES/EUGENE L            ^^123^356858      00998000000?",
                "%B5168987654321012^PKMMV/UNEMBOXXXX       ^1807^100000000000000111000000?;5168755544412233=18071111000011100000?",
                "%B4815881002861896^YATES/EUGENE L            ^^^356858      00998000000?",
                "%B5168755544412233^PKMMV/UNEMBOXXXXUNEMBOXXXXUNEMBOXXXXUNEMBOXXXXUNEMBOXXXX^1807111100000000000000111000000?;5168755544412233=18071111000011100000?"
            };

            CorrectPANS = new System.Collections.Generic.List<string>()
            {
                "4111111111111111",
                "5555555555554444",
                "5168755544412233",
                "5350290149345177",
                "4815881002861896",
                "5168987654321012",
                "4815881002861896",
                null
            };

            CorrectNames = new System.Collections.Generic.List<string>()
            {
               "FIRST LAST",
               "FIRST LAST",
               "UNEMBOXXXX PKMMV",
               "SUALEH FATEHI",
               "EUGENE L YATES",
               "UNEMBOXXXX PKMMV",
               "EUGENE L YATES",
               null
            };

            CorrectDates = new System.Collections.Generic.List<System.DateTime>()
            { 
                new System.DateTime(2015,3,1),
                new System.DateTime(2014,2,1),
                new System.DateTime(2018,7,1),
                new System.DateTime(2016,4,1),
                new System.DateTime(),
                new System.DateTime(2018,7,1),
                new System.DateTime(),
                new System.DateTime()
            };

            CorrectServices = new System.Collections.Generic.List<string>()
            {
                "101",
                "019",
                "111",
                "201",
                "123",
                null,
                "356",
                null
            };

        }
        
        public System.Collections.Generic.List<string> DummyTest;
        public System.Collections.Generic.List<string> CorrectPANS;
        public System.Collections.Generic.List<System.DateTime> CorrectDates;
        public System.Collections.Generic.List<string> CorrectNames;
        public System.Collections.Generic.List<string> CorrectServices;

        [Test]
        public void CheckPAN()
        {                
            for (int i = 0; i < DummyTest.Count; i++)
            {
                var result = Repos.MSR100Controller.MSR100Controller.ParseData(DummyTest[i]);
                if (result.Track1.PAN != CorrectPANS[i])
                {
                    Assert.AreEqual(result.Track1.PAN, CorrectPANS[i]);
                }
            }                        
        }

        [Test]
        public void CheckDate()
        {
            for (int i = 0; i < DummyTest.Count; i++)
            {
                var result = Repos.MSR100Controller.MSR100Controller.ParseData(DummyTest[i]);
                if (result.Track1.ExpirationDate != CorrectDates[i])
                {
                    Assert.AreEqual(result.Track1.PAN, CorrectPANS[i]);
                }
            }
        }

        [Test]
        public void CheckNames()
        {
            for (int i = 0; i < DummyTest.Count; i++)
            {
                var result = Repos.MSR100Controller.MSR100Controller.ParseData(DummyTest[i]);
                if (result.Track1.Name != CorrectNames[i])
                {
                    Assert.AreEqual(result.Track1.PAN, CorrectPANS[i]);
                }
            }
        }

        [Test]
        public void CheckServices()
        {
            for (int i = 0; i < DummyTest.Count; i++)
            {
                var result = Repos.MSR100Controller.MSR100Controller.ParseData(DummyTest[i]);
                if (result.Track1.ServiceCode != CorrectServices[i])
                {
                    Assert.AreEqual(result.Track1.PAN, CorrectPANS[i]);
                }
            }
        }
    }
}