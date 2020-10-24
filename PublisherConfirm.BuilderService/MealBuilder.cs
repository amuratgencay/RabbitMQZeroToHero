using Newtonsoft.Json;
using PublisherConfirm.BuilderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Util.RabbitMQUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace PublisherConfirm.BuilderService
{
    public class MealBuilder : IDisposable
    {
        private List<string> _actions = new List<string>();
        private PublisherBase _queue;

        public MealBuilder()
        {
            _queue = PublisherFactory.Create(
                PublisherWithoutQueueNameTypes.BasicWithConfirms);
        }

        private Meal PrepareVegMeal()
        {
            Meal meal = new Meal();
            meal.addItem(GetMealItem<VegBurger>("burger", "veg"));
            meal.addItem(GetMealItem<Coke>("drink", "coke"));
            return meal;
        }


        private Meal PrepareNonVegMeal()
        {
            Meal meal = new Meal();

            meal.addItem(GetMealItem<ChickenBurger>("burger", "chicken"));
            meal.addItem(GetMealItem<Pepsi>("drink", "pepsi"));
            return meal;
        }

        private T GetMealItem<T>(string type, string name)
        {
            string response = _queue.BasicPublish(type, name);
            return JsonConvert.DeserializeObject<T>(response);
        }


        public void Add(string request)
        {
            _actions.Add(request);
        }

        public void Show()
        {
            int vegCount = _actions.Count(x => x.Equals("veg"));
            int nonVegCount = _actions.Count(x => x.Equals("nonveg"));
            if (vegCount > 0)
                Console.WriteLine("Veg Meal({0})", vegCount);
            if (nonVegCount > 0)
                Console.WriteLine("Non Veg Meal({0})", nonVegCount);
        }

        public void CompleteOrder()
        {
            bool vegMealAdded = false;
            bool nonVegMealAdded = false;
            int vegCount = _actions.Count(x => x.Equals("veg"));
            int nonVegCount = _actions.Count(x => x.Equals("nonveg"));

            List<Meal> mealList = new List<Meal>();
            for (int i = 0; i < _actions.Count; i++)
            {
                Meal m = null;
                if (_actions[i] == "veg")
                {
                    m = PrepareVegMeal();

                    if (!vegMealAdded)
                    {
                        Console.WriteLine("Veg Meal({0})", vegCount);
                        m.ShowItems();
                        vegMealAdded = true;
                    }
                }
                else
                {
                    m = PrepareNonVegMeal();
                    if (!nonVegMealAdded)
                    {
                        Console.WriteLine("Non Veg Meal({0})", nonVegCount);
                        m.ShowItems();
                        nonVegMealAdded = true;
                    }
                }
                mealList.Add(m);

            }
            Console.WriteLine("Total: {0}$", mealList.Select(m => m.getCost()).Aggregate((x, y) => x + y));
            _actions.Clear();
        }

        public void Dispose()
        {
            _queue.Dispose();
        }
    }
}
