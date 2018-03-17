using UWP.MDI;
using UWP.MDI.Controls;

namespace Windows.UI.Xaml.Controls
{
    public static class Extension
    {
        public static void Show(this UserControl form)
        {
            var mdiContainer = GetContainer(form);
            mdiContainer.Show(form);
        }


        private static MDIContainer GetContainer(UserControl form)
        {
            var applicationWindow = Window.Current.Content;

            return Helpers.FindDescendant<MDIContainer>(applicationWindow);
        }
    }
}