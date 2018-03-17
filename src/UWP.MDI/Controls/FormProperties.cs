using Windows.UI.Xaml;

namespace UWP.MDI.Controls
{
    public class FormProperties : DependencyObject
    {
        public static readonly DependencyProperty FormStartPositionProperty =
            DependencyProperty.RegisterAttached(
                "FormStartPosition",
                typeof(FormStartPosition),
                typeof(FormProperties),
                new PropertyMetadata(default(FormStartPosition))
            );

        public static readonly DependencyProperty FormBorderStyleProperty = DependencyProperty.RegisterAttached(
            "FormBorderStyle", typeof(FormBorderStyle), typeof(FormProperties),
            new PropertyMetadata(default(FormBorderStyle)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text", typeof(string), typeof(FormProperties),
            new PropertyMetadata(default(string), PropertyChangedCallback));

        public static FormBorderStyle GetFormBorderStyle(DependencyObject element)
        {
            return (FormBorderStyle) element.GetValue(FormBorderStyleProperty);
        }

        public static FormStartPosition GetFormStartPosition(UIElement element)
        {
            return (FormStartPosition) element.GetValue(FormStartPositionProperty);
        }

        public static string GetText(DependencyObject element)
        {
            return (string) element.GetValue(TextProperty);
        }

        public static void SetFormBorderStyle(DependencyObject element, FormBorderStyle value)
        {
            element.SetValue(FormBorderStyleProperty, value);
        }

        public static void SetFormStartPosition(UIElement element, FormStartPosition value)
        {
            element.SetValue(FormStartPositionProperty, value);
        }

        public static void SetText(DependencyObject element, string value)
        {
            element.SetValue(TextProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var parentMdi = Helpers.FindParent<MDIChild>(dependencyObject);

            if (parentMdi == null)
            {
                return;
            }

            parentMdi.Title.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
        }
    }
}