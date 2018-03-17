using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.MDI.Controls
{
    public sealed class MDIContainer : Control
    {
        public static Action<Control, MDIChild, MDIContainer> Configure = ConfigureMdiControl;
        public static Action<Control, MDIChild, MDIContainer> BeforeShow = (control, mdiChild, container) => { };

        public MDIContainer()
        {
            DefaultStyleKey = typeof(MDIContainer);
        }

        public Canvas Items { get; set; }
        public ItemsControl MinimizedItems { get; set; }

        public void Show(Control control)
        {
            var child = CreateMdiControl(control);
            child.Opacity = 0;
            Items.Children.Add(child);

            child.Loaded += ChildOnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Items = GetTemplateChild("Items") as Canvas;
            MinimizedItems = GetTemplateChild("MinimizedItems") as ItemsControl;
        }

        private void ChildOnActivating(object mdiChild, MDIChild mdiChildControl)
        {
            foreach (var item in Items.Children.Where(x => x != mdiChild))
            {
                if (item is MDIChild control)
                {
                    control.Deactivate();
                }
            }
        }

        private void ChildOnClosed(object sender, MDIChild child)
        {
            if (child.FormWindowState == FormWindowState.Minimized)
            {
                MinimizedItems.Items?.Remove(child);
            }
            else
            {
                Items.Children.Remove(child);
            }

            RemoveChildEventHandlers(child);
        }

        private void ChildOnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            var child = (MDIChild) o;
            child.Loaded -= ChildOnLoaded;

            Configure(child.InnerControl, child, this);
            BeforeShow(child.InnerControl, child, this);
            CreateChildEventHandlers(child);

            child.Opacity = 1;
        }

        private void ChildOnRestored(object o, MDIChild child)
        {
            if (child.PreviousFormWindowState != FormWindowState.Minimized)
            {
                return;
            }

            MinimizedItems.Items?.Remove(child);
            Items.Children.Add(child);
        }

        private static void ConfigureMdiControl(Control control, MDIChild child, MDIContainer mdiContainer)
        {
            foreach (var item in mdiContainer.Items.Children)
            {
                if (item is MDIChild mdiChild)
                {
                    mdiChild.Deactivate();
                }
            }

            child.Activate();

            HandleStartPosition(control, child, mdiContainer);
        }

        private void CreateChildEventHandlers(MDIChild child)
        {
            child.Restored += ChildOnRestored;
            child.Closed += ChildOnClosed;
            child.Activating += ChildOnActivating;
            child.MinimizeButton.Click += OnMinimizeButtonOnClick;
            child.Activated += OnChildOnActivated;
        }

        private MDIChild CreateMdiControl(Control control)
        {
            var child = new MDIChild(control)
            {
                Width = double.IsNaN(control.Width) ? 300 : control.Width,
                Height = double.IsNaN(control.Height) ? 300 : control.Height
            };

            return child;
        }

        private void CtrlTabPressed()
        {
            // TODO
        }

        private static void HandleStartPosition(Control control, MDIChild child, MDIContainer mdiContainer)
        {
            double startLeft;
            double startTop;

            var startPosition = FormStartPosition.WindowsDefaultLocation;
            var apStart = control.GetValue(FormProperties.FormStartPositionProperty);

            if (apStart != null)
            {
                startPosition = (FormStartPosition) apStart;
            }

            if (startPosition == FormStartPosition.CenterParent)
            {
                var screenWidth = mdiContainer.ActualWidth;
                var screenHeight = mdiContainer.ActualHeight;
                var windowWidth = child.Width;
                var windowHeight = child.Height;
                startLeft = screenWidth / 2 - windowWidth / 2;
                startTop = screenHeight / 2 - windowHeight / 2;
            }
            else if (startPosition == FormStartPosition.CenterScreen)
            {
                var displaySize = Helpers.GetCurrentDisplaySize();

                var screenWidth = displaySize.Width;
                var screenHeight = displaySize.Height;
                var windowWidth = child.Width;
                var windowHeight = child.Height;
                startLeft = screenWidth / 2 - windowWidth / 2;
                startTop = screenHeight / 2 - windowHeight / 2;
            }
            else
            {
                var screenWidth = mdiContainer.ActualWidth;
                var screenHeight = mdiContainer.ActualHeight;
                var windowWidth = child.Width;
                var windowHeight = child.Height;
                var maxLeft = screenWidth / 2 - windowWidth / 2;
                var maxTop = screenHeight / 2 - windowHeight / 2;

                var rnd = new Random();
                startLeft = rnd.Next(0, (int) maxLeft);
                startTop = rnd.Next(0, (int) maxTop);
            }

            Canvas.SetLeft(child, startLeft);
            Canvas.SetTop(child, startTop);
        }

        private void OnChildOnActivated(object sender, MDIChild mdiChild)
        {
        }

        private void OnMinimizeButtonOnClick(object sender, RoutedEventArgs args)
        {
            var child = Helpers.FindParent<MDIChild>((DependencyObject) sender);
            Items.Children.Remove(child);
            MinimizedItems.Items?.Add(child);
            child.Minimize(this);
        }

        private void RemoveChildEventHandlers(MDIChild child)
        {
            child.Loaded -= ChildOnLoaded;
            child.Restored -= ChildOnRestored;
            child.Activating -= ChildOnActivating;
            child.MinimizeButton.Click -= OnMinimizeButtonOnClick;
            child.Activated -= OnChildOnActivated;
            child.Closed -= ChildOnClosed;
        }
    }
}