using System.Linq;
using Windows.UI.Xaml.Controls;

namespace UWP.MDI.Controls
{
    public class MdiLayoutTileHorizontal : MdiLayout
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

            var itemWidth = containerWidth;
            var itemHeight = containerHeight / (double)items.Count;

            for (var i = 0; i < items.Count; i++)
            {
                var item = (MDIChild)items[i];
                Canvas.SetLeft(item, 0);
                Canvas.SetTop(item, i * itemHeight);

                item.Width = itemWidth;
                item.Height = itemHeight;
            }
        }
    }
}