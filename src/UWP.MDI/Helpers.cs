using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace UWP.MDI
{
    public sealed class Helpers
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            var parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;

            if (parent != null)
            {
                return parent;
            }

            return FindParent<T>(parentObject);
        }


        public static T FindChildren<T>(DependencyObject startNode) where T : DependencyObject
        {
            if (startNode == null)
            {
                return null;
            }

            var count = VisualTreeHelper.GetChildrenCount(startNode);
            for (var i = 0; i < count; i++)
            {
                var current = VisualTreeHelper.GetChild(startNode, i);
                if (current.GetType() == typeof(T) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    var asType = (T)current;

                    return asType;
                }

                FindChildren<T>(current);
            }

            return null;
        }

        public static T FindDescendant<T>(DependencyObject element)
            where T : DependencyObject
        {
            T retValue = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var type = child as T;
                if (type != null)
                {
                    retValue = type;
                    break;
                }

                retValue = FindDescendant<T>(child);

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }

        public static Size GetCurrentDisplaySize()
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            var t = typeof(DisplayInformation).GetTypeInfo();

            var props = t.DeclaredProperties.Where(x => x.Name.StartsWith("Screen") && x.Name.EndsWith("InRawPixels"))
                .ToArray();
            var w = props.First(x => x.Name.Contains("Width")).GetValue(displayInformation);
            var h = props.First(x => x.Name.Contains("Height")).GetValue(displayInformation);

            var size = new Size(Convert.ToInt32(w), Convert.ToInt32(h));

            switch (displayInformation.CurrentOrientation)
            {
                case DisplayOrientations.Landscape:
                case DisplayOrientations.LandscapeFlipped:
                    size = new Size(Math.Max(size.Width, size.Height), Math.Min(size.Width, size.Height));

                    break;
                case DisplayOrientations.Portrait:
                case DisplayOrientations.PortraitFlipped:
                    size = new Size(Math.Min(size.Width, size.Height), Math.Max(size.Width, size.Height));

                    break;
            }

            return size;
        }
    }

    namespace Windows.UI.Xaml.Controls
    {
    }
}