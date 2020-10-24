using System;
using System.Collections.Generic;
using System.Text;

namespace PublisherConfirm.BuilderModel
{

    public class Meal
    {
        private List<IMealItem> _items = new List<IMealItem>();

        public void addItem(IMealItem item)
        {
            _items.Add(item);
        }

        public decimal getCost()
        {
            decimal cost = 0.0m;

            foreach (IMealItem item in _items)
            {
                cost += item.Price;
            }
            return cost;
        }

        public void ShowItems()
        {

            foreach (IMealItem item in _items)
            {
                Console.Write ("\tItem : " + item.Name);
                Console.Write(", Packing : " + item.Packing().Pack());
                Console.WriteLine(", Price : " + item.Price + "$");
            }
        }
    }
}
