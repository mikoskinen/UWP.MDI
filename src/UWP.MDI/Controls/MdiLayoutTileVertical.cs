using System.Linq;
using Windows.UI.Xaml.Controls;

namespace UWP.MDI.Controls
{
    public class MdiLayoutTileVertical : MdiLayout
    {
        public override void RunLayout(MDIContainer container)
        {
            if (container == null)
            {
                return;
            }

            if (!container.Items.Children.Any())
            {
                return;
            }

            var items = container.Items.Children;
            var containerWidth = container.ActualWidth;
            var containerHeight = container.ActualHeight;

            var itemWidth = containerWidth / (double)items.Count;
            var itemHeight = containerHeight;

            for (var i = 0; i < items.Count; i++)
            {
                var item = (MDIChild)items[i];
                Canvas.SetLeft(item, i * itemWidth);
                Canvas.SetTop(item, 0);

                item.Width = itemWidth;
                item.Height = itemHeight;
            }
        }
    }
}