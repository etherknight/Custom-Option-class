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
            Option<Chicken> test = HatchAnEgg("Eggbert");
            Option<Chicken> test2 = CrackedEgg();

            string name = string.Empty;
            name = test.Finally<string>(SomeChicken, NoChicken);
            Console.WriteLine($"My name is {name}");

            name = test2.Finally<string>(SomeChicken, NoChicken);
            Console.WriteLine($"My name is {name}");
        }

        private static string NoChicken()
        {
            return "Scrambled Egg";
        }

        private static string SomeChicken(Chicken chicken)
        {
            return chicken.Name;
        }

        private static Option<Chicken> HatchAnEgg(string name)
        {
            Option<Chicken> chicken = new Chicken()
            {
                Name = name,
                BeforeEgg = false
            };

            return chicken;
        }

        private static Option<Chicken> CrackedEgg()
        {
            Option<Chicken> egg = null;
            return egg;
        }
    }

    public class Chicken
    {
        public string Name { get; set; }
        public bool BeforeEgg { get; set; }
    }
}
