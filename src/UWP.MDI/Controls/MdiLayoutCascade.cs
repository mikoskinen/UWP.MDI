using System.Linq;
using Windows.UI.Xaml.Controls;

namespace UWP.MDI.Controls
{
    public class MdiLayoutCascade : MdiLayout
    {
        public bool Resize { get; set; } = true;

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
            MDIChild lastItem = null;
            for (var i = 0; i < items.Count; i++)
            {
                var item = (MDIChild)items[i];
                Canvas.SetLeft(item, i * 30);
                Canvas.SetTop(item, i * 30);

                if (Resize && item.FormBorderStyle != FormBorderStyle.Fixed)
                {
                    item.Width = container.DefaultChildWidth;
                    item.Height = container.DefaultChildHeight;
                }

                lastItem = item;
            }

            lastItem?.Activate();
        }
    }
}