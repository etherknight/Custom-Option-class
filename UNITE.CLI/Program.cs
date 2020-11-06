using System;
using System.Collections;
using System.Collections.Generic;
using UNITE.Util;

namespace UNITE.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = string.Empty;

            // Test 1:  A sequence of events to generate a chicken
            Option<Egg> test = LayEgg();
            
            name = test.Then<Chicken>(egg => HatchAnEgg(egg))
                       .Finally(SomeChicken, NoChicken);  
            Console.WriteLine($"My name is {name}");


            // Test 2: Lets try a failure case. 
            try
            {
                name = LayAnotherEgg()
                            .Then<Egg>(egg => CrackEgg(egg))
                            .Then<Chicken>(egg => HatchAnEgg(egg))
                            .Finally<string>(SomeChicken, NoChicken);
                Console.WriteLine($"My name is {name}");
            }
            catch(Exception e)
            {
                // We should never see "Eggception" from HatchAnEgg() 
                //
                // The Option class will protect us from ever passing the cracked egg
                // into HatchAnEgg();
                Console.WriteLine(e.Message);
            }
        }

        private static string NoChicken()
        {
            return "Scrambled Egg";
        }

        private static string SomeChicken(Chicken chicken)
        {
            return chicken.Name;
        }


        private static Egg LayEgg()
        {
            Egg egg = new Egg();
            return egg;
        }

        // This version of LayEgg returns the Option<Egg>
        // for us.  
        // Now we don't have to assign it in our method 
        // and can just chain calls.
        private static Option<Egg> LayAnotherEgg()
        {
            Option<Egg> egg = new Egg();
            return egg;
        }

        private static Chicken HatchAnEgg(Egg egg)
        {
            if (egg == default(Egg))
            {
                throw new Exception("Eggception");
            }

            Chicken chicken = new Chicken()
            {
                Name = "Eggbert"
            };

            return chicken;
        }
        private static Egg CrackEgg(Egg egg)
        {
            return default(Egg);
        }
    }

    public class Chicken
    {
        public string Name { get; set; }
    }

    public class Egg
    {
    }
}
