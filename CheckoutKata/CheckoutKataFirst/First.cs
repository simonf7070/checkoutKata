using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

//A = 50
//B = 30
//C = 20
//D = 15

//3 x A = 130
//2 x B = 45

//price in euros or pounds
//best offer ie. ABC = 90

namespace CheckoutKata.CheckoutKataFirst
{
    [TestFixture]
    public class CheckoutTotalShould
    {
        [Test]
        public void ZeroWhenNoItemsAdded()
        {
            var checkout = new Checkout();
            Assert.That(checkout.Total(), Is.EqualTo(0));
        }

        [TestCase("A", 50)]
        [TestCase("B", 30)]
        [TestCase("C", 20)]
        [TestCase("D", 15)]
        public void ItemPriceWhenSingleItemScanned(string item, int expectedPrice)
        {
            var checkout = new Checkout();
            checkout.Scan(item);
            Assert.That(checkout.Total(), Is.EqualTo(expectedPrice));
        }

        [Test]
        public void OneHundredWhenTwoAScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(100));
        }

        [Test]
        public void EightyWhenAThenBScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("A");
            checkout.Scan("B");
            Assert.That(checkout.Total(), Is.EqualTo(80));
        }

        [Test]
        public void OneHunderedAndThirtyWhenThreeAScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(130));
        }

        [Test]
        public void OneHunderedAndFiftyWhenTwoAThenBCScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("C");
            Assert.That(checkout.Total(), Is.EqualTo(150));
        }

        [Test]
        public void TwoHunderedAndSixtyWhenSixAScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(260));
        }

        [Test]
        public void OneHunderedAndEightyWhenFourAScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(180));
        }

        [Test]
        public void FortyFiveWhenTwoBScanned()
        {
            var checkout = new Checkout();
            checkout.Scan("B");
            checkout.Scan("B");
            Assert.That(checkout.Total(), Is.EqualTo(45));
        }

    }

    public class Checkout
    {
        private int _subtotal;
        private List<string> _items;

        public Checkout()
        {
            _subtotal = 0;
            _items = new List<string>();
        }

        public int Total()
        {
            var discountA = ApplyDiscount("A", 3, 20);
            var discountB = ApplyDiscount("B", 2, 15);
            return _subtotal - discountA - discountB;
        }

        private int ApplyDiscount(string item, int quantityRequiredForDiscount, int discountAmount)
        {
            var numberOfDiscounts = _items.Count(i => i == item) / quantityRequiredForDiscount;
            return numberOfDiscounts * discountAmount;
        }

        public void Scan(string item)
        {
            switch (item)
            {
                case "A":
                    AddToTotal(50);
                    break;
                case "B":
                    AddToTotal(30);
                    break;
                case "C":
                    AddToTotal(20);
                    break;
                case "D":
                    AddToTotal(15);
                    break;
            }
            _items.Add(item);
        }

        private void AddToTotal(int value)
        {
            _subtotal += value;
        }
    }
}
