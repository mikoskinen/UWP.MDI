using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWP.MDI.Samples.MVVM.Invoices
{
    public class Invoice : INotifyPropertyChanged
    {
        public Invoice(int id, string name, double amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}