using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP.MDI.Sample
{
    public sealed partial class CustomerForm : UserControl
    {
        public CustomerForm()
        {
            this.InitializeComponent();
        }

        private void DataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            var customers = new ObservableCollection<CustomerData>();

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
                var customer = new CustomerData(id, name.Item1, name.Item2);
                customers.Add(customer);
            }

            this.DataGrid.ItemsSource = customers;
        }
    }

    public class CustomerData
    {
        public CustomerData(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
