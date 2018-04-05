using UWP.MDI.Samples.MVVM.Customers;
using UWP.MDI.Samples.MVVM.Invoices;

namespace UWP.MDI.Samples.MVVM.Infrastructure
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
        }

        public MainPageViewModel MainPageViewModel => new MainPageViewModel();
        public CustomerViewModel CustomerViewModel => new CustomerViewModel();
        public InvoiceViewModel InvoiceViewModel => new InvoiceViewModel();
    }
}
