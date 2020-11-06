using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UNITE.Util.Tests
{
    [TestClass]
    public class OptionTests
    {
        class Chicken
        {
            public string Name { get; set; }
            public bool BeforeEgg { get; set; }
        }

        class Egg
        {
            public string Name { get; set; }
            public bool Scrambled { get; set; }
        }

        [TestMethod]
        public void WhenValueIsSet_ThenSomeIsCalled()
        {
            Option<Chicken> chicken = new Chicken()
            {
                Name = "Test Chicken",
                BeforeEgg = true
            };

            chicken.Finally
            (
                some: c => Assert.IsInstanceOfType(c, typeof(Chicken)),
                none: () => Assert.IsTrue(false)
            );
        }

        [TestMethod]
        public void WhenValueIsNull_ThenNoneIsCalled()
        {
            Option<Chicken> chicken = null;

            chicken.Finally
            (
                some: c  => Assert.IsTrue(false),
                none: () => Assert.IsTrue(true)
            );
        }

        [TestMethod]
        public void WhenThenChangesTheValueToNull_FinallyHandlesFailure()
        {
            Option<Chicken> chicken = new Chicken()
            {
                Name = "Test Chicken",
                BeforeEgg = true
            };

            chicken.Then<Chicken>(c => null)
            .Finally
            (
                some: c  => Assert.IsTrue(false),
                none: () => Assert.IsTrue(true)
            );
        }

        [TestMethod]
        public void WhenThenIsCalledWithADifferentType_TheValueInFinallyIsOfTheNewType()
        {
            Option<Chicken> chicken = new Chicken()
            {
                Name = "Test Chicken",
                BeforeEgg = true
            };

            chicken.Then<Egg>(c =>
            {
                return new Egg()
                {
                    Name = c.Name,
                    Scrambled = c.BeforeEgg
                };
            })
            .Finally
            (
                some: c => Assert.IsInstanceOfType(c, typeof(Egg)),
                none: () => Assert.IsTrue(false)
            );
        }

        [TestMethod]
        public void WhenValueIsSet_MultipleCallsToThenAllSucced()
        {
            Option<Chicken> chicken = new Chicken()
            {
                Name = "A",
                BeforeEgg = true
            };

            chicken.Then<Chicken>(c =>
            {
                c.Name += "B";
                return c;
            })
            .Then<Chicken>(c => {
                c.Name += "C";
                return c;
            })
            .Finally
            (
                some: c => Assert.AreEqual("ABC", c.Name),
                none: () => Assert.IsTrue(false)
            );
        }


        [TestMethod]
        public void WhenValueIsSet_ValueOrThrowReturnsValue()
        {
            Option<Chicken> chicken = new Chicken();

            Chicken clucker = chicken.ValueOrThrow();

            Assert.IsNotNull(clucker);
            Assert.IsInstanceOfType(clucker, typeof(Chicken));
        }

        [TestMethod]
        public void WhenValueIsNull_ValueOrThrowGenerartesAnException()
        {
            const string ERROR = "Test";
            Option<Chicken> chicken = null;

            OptionValueException ex = Assert.ThrowsException<OptionValueException>(() =>
            {
                chicken.ValueOrThrow(ERROR);
            });

            Assert.AreEqual(ERROR, ex.Message);
        }

        [TestMethod]
        public void WhenValueIsSet_TryGetValueReturnsTrueAndValue()
        {
            Option<Chicken> chicken = new Chicken();

            bool got = chicken.TryGetValue(out Chicken eggbert);

            Assert.IsTrue(got);
            Assert.IsInstanceOfType(eggbert, typeof(Chicken));
        }

        [TestMethod]
        public void WhenValueIsSet_TryGetValueReturnsFalseAndNull()
        {
            Option<Chicken> chicken = null;

            bool got = chicken.TryGetValue(out Chicken eggbert);

            Assert.IsFalse(got);
            Assert.IsNull(eggbert);
        }

        [TestMethod]
        public void WhenValueIsSet_SomeFunctionIsCalled()
        {
            Option<Chicken> chicken = new Chicken()
            {
                Name = "Little",
                BeforeEgg = false
            };

            bool result = chicken.Finally<bool>(SomeChicken, NoneChicken);

            Assert.IsTrue(result);

            bool SomeChicken(Chicken arg) => true;

            bool NoneChicken() => false;
        }

        [TestMethod]
        public void WhenValueIsNull_NoneFunctionIsCalled()
        {
            Option<Chicken> chicken = null;

            bool result = chicken.Finally<bool>(SomeChicken, NoneChicken);

            Assert.IsFalse(result);

            bool SomeChicken(Chicken arg) => true;

            bool NoneChicken() => false;
        }
    }
}
