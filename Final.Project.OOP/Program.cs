using System.Net.Sockets;
using System.Xml.Schema;

namespace Final.Project.OOP
{
    internal class Program
    {
        static void Main(string[] args) { }
    }

    abstract class Delivery
    {
        private string address;
        private string deliveryService;
        protected int usersRating;
        protected static int deliveryNumber = 0;

        public string Address
        {
            get { return address; }
        }
        public string DeliveryService
        {
            get { return deliveryService; }
        }

        public Delivery(string address, string deliveryService)
        {
            this.address = address;
            this.deliveryService = deliveryService;
        }

        /// <summary>
        /// Подтверждает доставку
        /// </summary>
        public virtual void ConfirmDelivery()
        {
            Console.WriteLine("Доставка прошла успешно");
            Delivery.deliveryNumber++;
        }

        /// <summary>
        /// Дает подарок, при количестве заказов 10 и более
        /// </summary>
        public abstract void TryGivePresent();
    }

    class HomeDelivery : Delivery
    {
        private int floor;
        private bool intercom;

        public int Floor
        {
            get { return floor; }
        }
        public bool Intercom
        {
            get { return intercom; }
        }

        public HomeDelivery(int floor, bool intercom, string address, string deliveredService)
            : base(address, deliveredService)
        {
            this.floor = floor;
            this.intercom = intercom;
        }

        /// <summary>
        /// Расчитывает стоимость услуг грузчиков
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public decimal GetLoaders(int floor)
        {
            var priceLoaders = floor * 2;
            return priceLoaders;
        }

        public override void ConfirmDelivery()
        {
            base.ConfirmDelivery();
            Console.WriteLine("Посылка доставлена по адресу: " + this.Address);
            //Считаем оценку пользователя
            usersRating = Random.Shared.Next(20);
            Delivery.deliveryNumber++;
        }

        public override void TryGivePresent()
        {
            if (usersRating >= 10 || (Delivery.deliveryNumber / 100 == 0))
            {
                Console.WriteLine("Выполнить что-то, что отправляет подарок");

            }
        }
    }

    class PickPointDelivery : Delivery
    {
        private int cellNumber;
        private int cellPassword;

        public int CellNumber
        {
            get { return cellNumber; }
        }
        public int CellPassword
        {
            get { return cellPassword; }
            set { cellPassword = value; }
        }

        public PickPointDelivery(int cellNumber, string deliveredService, string address = "Postomat")
            : base(address, deliveredService)
        {
            this.cellNumber = cellNumber;
        }

        public override void ConfirmDelivery()
        {
            base.ConfirmDelivery();
            Console.WriteLine("Посылка доставлена по адресу: " + this.Address + "\nНаходится в ячейке номер " + cellNumber);
            //Считаем оценку пользователя
            usersRating = Random.Shared.Next(20);
            Delivery.deliveryNumber++;
        }

        public override void TryGivePresent()
        {
            if (usersRating >= 15 || (Delivery.deliveryNumber / 100 == 0))
            {
                Console.WriteLine("Выполнить что-то, что отправляет подарок");
            }
        }
    }

    class ShopDelivery : Delivery
    {
        private string shopWorkingHours;

        public string ShopWorkingHours
        {
            get { return shopWorkingHours; }
        }

        public ShopDelivery (string deliveredService, string address = "shop", string shopWorkingHours = "8 - 20")
            : base(address, deliveredService)
        {
            this.shopWorkingHours = shopWorkingHours;
        }

        public override void ConfirmDelivery()
        {
            Console.WriteLine("Доставка в магазин осуществлена");
            //Считаем оценку пользователя
            usersRating = Random.Shared.Next(20);
            Delivery.deliveryNumber++;
        }

        public override void TryGivePresent()
        {
            if (usersRating >= 19 || (Delivery.deliveryNumber / 100 == 0))
            {
                Console.WriteLine("Выполнить что-то, что отправляет подарок");
            }
        }
    }

    class Order<TDelivery, TPayment>
        where TDelivery : Delivery
        where TPayment : Payment
    {
        private TDelivery delivery;

        private TPayment payment;

        private (Item item, int quantity)[] items;

        private int number;

        private string description;

        public int Number
        {
            get { return number; }
        }
        public string Description
        {
            get { return description; }
        }

        public Order(TDelivery delivery, TPayment payment, (Item item, int quantity)[] items, int number, string description)
        {
            this.delivery = delivery;
            this.payment = payment;
            this.items = items;
            this.number = number;
            this.description = description;


        }

        /// <summary>
        /// Показывает адрес доставки
        /// </summary>
        public void DisplayAddress()
        {
            Console.WriteLine(this.delivery.Address);
        }

        /// <summary>
        /// Подтверждает достаточное колличество денег(подтверждает оплату)
        /// </summary>
        /// <returns></returns>
        public bool IsPayed()
        {
            decimal totalSumOrder = 0;

            for(int i = 0; i < this.items.Length; i++)
            {
                var total = this.items[i].item.Price * this.items[i].quantity;
                totalSumOrder += total;
            }
            if (this.payment.Amount - totalSumOrder >= 0)
            {
                return true;
            }
            else { return false; }
        }
    }

    abstract class Payment
    {
        protected string currency;
        private DateTime date;
        private decimal amount;

        public abstract string Currency
        {
            get;
        }
        public DateTime Date
        {
            get { return date; }
        }
        public decimal Amount
        {
            get { return amount; }
        }

        public Payment(string currency, DateTime date, decimal amount)
        {
            this.currency = currency;
            this.date = date;
            this.amount = amount;
        }

        /// <summary>
        /// Показывает тип совершенной оплаты
        /// </summary>
        public abstract void TypeOfPayment();
    }

    class CashPayment : Payment
    {
        private bool receipt;

        public bool Receipt
        {
            get { return receipt; }
        }
        public override string Currency
        {
            get { return currency; }
        }

        public CashPayment( bool receipt, string currency, DateTime date, decimal amount)
            : base(currency, date, amount)
        {
            this.receipt = receipt;
        }

        public override void TypeOfPayment()
        {
            Console.WriteLine("Оплата наличными");
        }
    }

    class CardPayment : Payment
    {
        private string bank;
        private bool terminalReceipt;

        public string Bank
        {
            get { return bank; }
        }
        public bool TerminalReceipt
        {
            get { return terminalReceipt; }
        }
        public override string Currency
        {
            get { return "валюта"; }
        }

        public CardPayment(string bank, bool terminalReceipt, string currency, DateTime date, decimal amount)
            : base(currency, date, amount)
        {
            this.bank = bank;
            this.terminalReceipt = terminalReceipt;
        }

        public override void TypeOfPayment()
        {
            Console.WriteLine("Оплата картой");
        }
    }

    public class Item
    {
        private string name;
        private decimal price;
        private string description;
        private bool isBreakable;
        private int weight;
        private Dimensions dimensions;

        public string Name
        {
            get { return name; }
        }
        public decimal Price
        {
            get { return price; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public bool IsBreakable
        {
            get { return isBreakable; }
        }
        public int Weight
        {
            get { return weight;}
            set { weight = value; }
        }
        public Dimensions Dimensions
        {
            get { return dimensions; }
        }
        public Item(string name, decimal price, string description, bool isBreakable, int weight, Dimensions dimensions)
        {
            this.name = name;
            this.price = price;
            this.description = description;
            this.isBreakable = isBreakable;
            this.dimensions = dimensions;
            this.weight = weight;
        }
    }

    public struct Dimensions
    {
        private int height;
        private int width;
        private int length;

        public int Height
        {
            get { return height; }
        }
        public int Width
        {
            get { return width; }
        }
        public int Length
        {
            get { return length; }
        }
        public Dimensions(int height, int width, int length)
        {
            this.height = height;
            this.width = width;
            this.length = length;
        }
    }

    class Store
    {
        private ItemStorePosition[] itemStorePositions;
        public ItemStorePosition[] ItemStorePositions
        {
            get { return itemStorePositions; }
        }
        public Store(int storePlacesCount)
        {
            itemStorePositions = new ItemStorePosition[storePlacesCount];
        }

        /// <summary>
        /// Добавляет объекты и их колличество в ячейку на складе
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItems(Item item, int quantity)
        {
            for (int i = 0; i < this.itemStorePositions.Length; i++)
            {           
                if (this.itemStorePositions[i] == null)
                {
                    this.itemStorePositions[i] = new ItemStorePosition(item, quantity, Random.Shared.Next().ToString());
                    //break;
                }
            }
        }

        /// <summary>
        /// Удаляет объекты со склада, в зависимости от колличества
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool RemoveItems(string itemName, int? quantity = null)
        {
            if (quantity != null && quantity <= 0)
            {
                Console.Write("Невозможно удалить отрицательное число!");
                return false;
            }
            for (int i = 0; i < this.itemStorePositions.Length; i++)
            {
                if (this.itemStorePositions[i].Item.Name == itemName)
                {
                    if (quantity == null)
                    {
                        this.itemStorePositions[i] = null;
                    }
                    else
                    {
                        this.itemStorePositions[i].Quantity -= quantity.Value;
                        if (itemStorePositions[i].Quantity <= 0)
                        {
                            quantity = -this.itemStorePositions[i].Quantity;
                            itemStorePositions[i] = null;
                        }
                        else return true;
                    }
                }
            }
            return quantity == null || quantity == 0;
        }
        
        /// <summary>
        /// Удаляет все объекты из указанной ячейки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="placeOnStore"></param>
        public void RemoveItemsFromSelectedPlace<T>(string placeOnStore)
        {
            for (int i = 0; i < this.ItemStorePositions.Length; i++)
            {
                if (this.ItemStorePositions[i].PlaceOnStore == placeOnStore)
                {
                    itemStorePositions[i] = null;
                    break;
                }
            }            
        }
    }

    public class ItemStorePosition
    {
        private Item item;
        private int quantity;
        private string placeOnStore;
        public Item Item
        {
            get { return item; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public string PlaceOnStore
        {
            get { return placeOnStore; }
            set { placeOnStore = value; }
        }
        public ItemStorePosition(Item item, int quantity, string placeStore)
        {
            this.item = item;
            this.quantity = quantity;
            this.placeOnStore = placeStore;
        }
    }
}