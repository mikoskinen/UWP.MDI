using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWP.MDI.Samples.MVVM.Infrastructure;

namespace UWP.MDI.Samples.MVVM
{
    public class MainPageViewModel
    {
        public ICommand ShowCustomersCommand { get; set; }
        public ICommand ShowInvoicesCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MainPageViewModel()
        {
            ShowCustomersCommand = new RelayCommand(() =>
            {
                var frm = new Customers.CustomerForm();
                frm.Show();
            });

            ShowInvoicesCommand = new RelayCommand(() =>
            {
                var frm = new Invoices.InvoiceForm();
                frm.Show();
            });

            ExitCommand = new RelayCommand(() =>
            {
                Application.Current.Exit();
            });
        }
    }
}
