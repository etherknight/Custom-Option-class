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
            Option<Chicken> test = new Chicken()
            {
                Name = "Eggbert"
            };
            Option<Chicken> test2 = null;

            string name = string.Empty;
            name = test.Finally<string>(SomeChicken, NoChicken);
            Console.WriteLine($"My name is {name}");

            name = test2.Finally<string>(SomeChicken, NoChicken);
            Console.WriteLine($"My name is {name}");
        }

        private static string NoChicken()
        {
            return "The Nameless Chicken";
        }

        private static string SomeChicken(Chicken chicken)
        {
            return chicken.Name;
        }
    }

    public class Chicken
    {
        public string Name { get; set; }
        public bool BeforeEgg { get; set; }
    }
}
