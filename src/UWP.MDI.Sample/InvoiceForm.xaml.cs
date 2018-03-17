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
    public sealed partial class InvoiceForm : UserControl
    {
        public InvoiceForm()
        {
            this.InitializeComponent();
        }

        private void DataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            var invoices = new ObservableCollection<InvoiceData>();
            var random = new Random();

            for (int i = 0; i < 25; i++)
            {
                var id = i + 1;

                var invoice = new InvoiceData(id, $"Invoice #{id}", random.Next(-3300, 7500));
                invoices.Add(invoice);
            }

            this.DataGrid.ItemsSource = invoices;
        }
    }

    public class InvoiceData
    {
        public InvoiceData(int id, string name, double amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }
}
