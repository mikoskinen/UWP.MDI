using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWP.MDI.Samples.MVVM.Customers
{
    public class Customer : INotifyPropertyChanged
    {
        public Customer(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}