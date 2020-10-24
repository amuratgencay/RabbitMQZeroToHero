using System;
using Util.RabbitMQUtils;

namespace Topic.ObserverServerApp
{
    class Employee
    {
        private PublisherBase queue;
        private string _firstName;
        private string _lastName;
        private decimal _salary;

        public string Name => $"{_firstName} {_lastName}";
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                Notify("FirstName", _firstName, "employee.firstname", "employee.name");
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                Notify("LastName", _lastName, "employee.lastname", "employee.name");
            }
        }
        public decimal Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                Notify("Salary", _salary, "employee.salary");
            }
        }

        public Employee(PublisherBase queue)
        {
            this.queue = queue;
        }

        public Employee(PublisherBase queue, string firstName, string lastName, decimal salary) : this(queue)
        {
            _firstName = firstName;
            _lastName = lastName;
            _salary = salary;
        }


        private void Notify(string propertyName, object value, params string[] topics)
        {
            Console.WriteLine("{0} changed: {1}", propertyName, value.ToString());
            foreach (var topic in topics)
            {
                queue.BasicPublish(topic, value);
            }
        }
    }
}
