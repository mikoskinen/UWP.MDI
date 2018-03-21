using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP.MDI.Samples.Layouts
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
