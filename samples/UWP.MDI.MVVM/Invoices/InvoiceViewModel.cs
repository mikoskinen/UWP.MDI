using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWP.MDI.Samples.MVVM.Invoices
{
    public class InvoiceViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Invoice> Invoices { get; set; }
        public InvoiceViewModel()
        {
            var invoices = new ObservableCollection<Invoice>();
            var random = new Random();

            for (int i = 0; i < 25; i++)
            {
                var id = i + 1;

                var invoice = new Invoice(id, $"Invoice #{id}", random.Next(-3300, 7500));
                invoices.Add(invoice);
            }

            Invoices = invoices;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}