using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace CheckoutKata.CheckoutKataSecond
{
    [TestFixture]
    public class CheckoutTest
    {
        [Test]
        public void SingleItemATotalForItem()
        {
            var checkout = CreateCheckout();
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(50));
        }

        [Test]
        public void SingleItemBTotalForItem()
        {
            var checkout = CreateCheckout();
            checkout.Scan("B");
            Assert.That(checkout.Total(), Is.EqualTo(30));
        }

        [Test]
        public void TwoItemATotalForItems()
        {
            var checkout = CreateCheckout();
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(100));
        }

        [Test]
        public void ItemAandItemBTotalForItems()
        {
            var checkout = CreateCheckout();
            checkout.Scan("A");
            checkout.Scan("B");
            Assert.That(checkout.Total(), Is.EqualTo(80));
        }

        [Test]
        public void AddThreeAshouldTotal130()
        {
            var checkout = CreateCheckout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(130));
        }

        [Test]
        public void AddFourAshouldTotal180()
        {
            var checkout = CreateCheckout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            Assert.That(checkout.Total(), Is.EqualTo(180));
        }

        [Test]
        public void AddTwoBshouldTotal45()
        {
            var checkout = CreateCheckout();
            checkout.Scan("B");
            checkout.Scan("B");
            Assert.That(checkout.Total(), Is.EqualTo(45));
        }

        [Test]
        public void BigBasketshouldTotal405()
        {
            var checkout = CreateCheckout();
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("C");
            Assert.That(checkout.Total(), Is.EqualTo(405));
        }

        private static Checkout CreateCheckout()
        {
            return new Checkout(new ItemPrice("A", 50), new ItemPrice("B", 30), new ItemPrice("C", 20));
        }
    }

    public class PriceList
    {
        private readonly List<ItemPrice> _inner;

        public PriceList(IEnumerable<ItemPrice> itemPrices)
        {
            _inner = itemPrices.ToList();
        }
        
        public int GetPrice(string item)
        {
            return _inner.First(i => i.Item == item).Price;
        }
    }

    public class ItemPrice
    {
        public string Item { get; private set; }
        public int Price { get; private set; }

        public ItemPrice(string item, int price)
        {
            Item = item;
            Price = price;
        }
    }

    internal class DiscountList : IEnumerable<KeyValuePair<string, Discount>>
    {
        private readonly Dictionary<string, Discount> _inner;

        public DiscountList(Dictionary<string, Discount> discounts)
        {
            _inner = discounts;
        }

        public IEnumerator<KeyValuePair<string, Discount>> GetEnumerator()
        {
            foreach (var discount in _inner)
            {
                yield return new KeyValuePair<string, Discount>(discount.Key, discount.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Checkout
    {
        private int _subTotal;
        private readonly PriceList _priceList;
        private readonly List<string> _itemsScannedList;
        private readonly DiscountList _discountList;

        public Checkout(params ItemPrice[] itemPrice)
        {
            _priceList = new PriceList(itemPrice);
            _itemsScannedList = new List<string>();
            _discountList = new DiscountList(SetupDiscounts());
        }

        public int Total()
        {
            foreach (var discount in _discountList)
            {
                var itemCount = _itemsScannedList.Count(i => i == discount.Key);
                _subTotal -= discount.Value.DiscountToApply(itemCount);
            }
            return _subTotal;
        }

        private static Dictionary<string, Discount> SetupDiscounts()
        {
            var discountDictionary = new Dictionary<string, Discount>();
            discountDictionary.Add("A", new Discount(3, 20));
            discountDictionary.Add("B", new Discount(2, 15));
            return discountDictionary;
        }

        public void Scan(string item)
        {
            _itemsScannedList.Add(item);
            _subTotal += _priceList.GetPrice(item);
        }
    }

    internal class Discount
    {
        public int DiscountQuantity { get; private set; }
        public int DiscountAmount { get; private set; }

        public Discount(int discountQuantity, int discountAmount)
        {
            DiscountQuantity = discountQuantity;
            DiscountAmount = discountAmount;
        }
           
        internal int DiscountToApply(int itemCount)
        {
            int discountsToApply = itemCount/this.DiscountQuantity;
            return discountsToApply * this.DiscountAmount;
        }
    }
}
