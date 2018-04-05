using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWP.MDI.Samples.MVVM.Customers
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            var customers = new ObservableCollection<Customer>();

            var names = new List<Tuple<string, string>>
            {
                Tuple.Create("Noel", "Hess"),
                Tuple.Create("Silver", "Holland"),
                Tuple.Create("Rudy", "Phillips"),
                Tuple.Create("Skylar", "Cabrera"),
                Tuple.Create("Blair", "Kirby"),
            };

            for (int i = 0; i < names.Count; i++)
            {
                var id = i + 1;
                var name = names[i];
                var customer = new Customer(id, name.Item1, name.Item2);
                customers.Add(customer);
            }

            Customers = customers;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
