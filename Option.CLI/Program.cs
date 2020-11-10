using System;
using System.Collections;
using System.Collections.Generic;
using Options.Util;

namespace Options.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = string.Empty;

            // Test 1:  A sequence of events to generate a chicken
            Option<Egg> test = LayEgg();
            
            name = test.Then<Chicken>(egg => Hatch(egg))
                       .Finally(SomeChicken, NoChicken);  
            Console.WriteLine($"My name is {name}");


            // Test 2: Lets try a failure case. 
            try
            {
                name = LayAnotherEgg()
                            .Then<Egg>(egg => CrackEgg(egg))            // Sets us into an error state.
                            .Then<Chicken>(egg => Hatch(egg))           // Never called because the error drops through to finally. 
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

            // Test 3 : Sometimes we just want the value
            try
            {
                Option<Chicken> test3 = null;  // set to no value for now. 

                // Even though we've set to null above - we've set the value to null. 
                // The option object itself is not null, so we can safely use it. 
                if (false == test3.TryGetValue(out Chicken nugget))
                {
                    Console.WriteLine("No nuggets");
                }

                //We can throw an argument of our choosing. 
                Chicken wing = test3.ValueOrThrow<ArgumentNullException>();

                // Or we can use the standard OptionValueException with our own message.
                Chicken roast = test3.ValueOrThrow("Needs more spuds.");
            }
            catch (OptionValueException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("We got the exception");
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

        private static Chicken Hatch(Egg egg)
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
