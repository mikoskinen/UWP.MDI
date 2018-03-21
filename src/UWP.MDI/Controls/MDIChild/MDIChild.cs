using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace UWP.MDI.Controls
{
    [TemplateVisualState(Name = "Default", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Active", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Deactivated", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Minimized", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Maximized", GroupName = "CommonStates")]
    public sealed class MDIChild : Control
    {
        private readonly Control _innerControl;
        private Panel _mainContainer;
        private Point _previousLocation;
        private Size _previousSize;
        private const double MinimumHeight = 90;
        private const double MinimumWidth = 24;

        public MDIChild(Control innerControl) : this()
        {
            _innerControl = innerControl;
        }

        public MDIChild()
        {
            DefaultStyleKey = typeof(MDIChild);
        }

        public FormWindowState PreviousFormWindowState { get; set; }
        public FormWindowState FormWindowState { get; set; }

        public Thumb DragThumb { get; private set; }
        public Thumb ResizeLeft { get; private set; }
        public Thumb ResizeTop { get; private set; }
        public Thumb ResizeRight { get; private set; }
        public Thumb ResizeBottom { get; private set; }
        public Thumb ResizeTopLeft { get; private set; }
        public Thumb ResizeTopRight { get; private set; }
        public Thumb ResizeBottomRight { get; private set; }
        public Thumb ResizeBottomLeft { get; private set; }
        public TextBlock Title { get; private set; }
        public ContentControl MDIContent { get; private set; }
        public Button RestoreButton { get; private set; }
        public Button MaximizeButton { get; private set; }
        public Button CloseButton { get; private set; }
        public Button MinimizeButton { get; private set; }

        private MDIContainer MDIParent
        {
            get
            {
                var result = Helpers.FindParent<MDIContainer>(this);

                return result;
            }
        }

        public Control InnerControl
        {
            get { return _innerControl; }
        }

        public void Activate()
        {
            Canvas.SetZIndex(this, 1);
            VisualStateManager.GoToState(this, "Active", false);

            Activated?.Invoke(this, this);
        }

        public event EventHandler<MDIChild> Activated;
        public event EventHandler<MDIChild> Activating;
        public event EventHandler<MDIChild> Closed;

        public void Deactivate()
        {
            Canvas.SetZIndex(this, 0);
            VisualStateManager.GoToState(this, "Deactivated", false);
        }

        public void Maximize()
        {
            SetState(FormWindowState.Maximized);
            _previousSize = new Size(ActualWidth, ActualHeight);
            _previousLocation = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            var container = MDIParent;

            if (container == null)
            {
                return;
            }

            var screenWidth = container.ActualWidth;
            var screenHeight = container.ActualHeight;
            Canvas.SetLeft(this, 0);
            Canvas.SetTop(this, 0);
            Width = screenWidth;
            Height = screenHeight;

            Activate();
            VisualStateManager.GoToState(this, "Maximized", false);
        }

        public void Minimize(MDIContainer mdiContainer)
        {
            SetState(FormWindowState.Minimized);
            _previousSize = new Size(ActualWidth, ActualHeight);
            _previousLocation = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            Deactivate();
            VisualStateManager.GoToState(this, "Minimized", false);

            Width = 230; // TODO Move to visual state
            Height = 48;
        }

        public void Restore()
        {
            SetState(FormWindowState.Normal);
            Restored?.Invoke(this, this);

            Activate();
            VisualStateManager.GoToState(this, "Default", false);

            Canvas.SetLeft(this, _previousLocation.X);
            Canvas.SetTop(this, _previousLocation.Y);
            Width = _previousSize.Width;
            Height = _previousSize.Height;
        }

        public event EventHandler<MDIChild> Restored;

        public void SetState(FormWindowState newState)
        {
            PreviousFormWindowState = FormWindowState;
            FormWindowState = newState;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded -= OnLoaded;
            Loaded += OnLoaded;

            _mainContainer = GetTemplateChild("MainContainer") as Panel;
            DragThumb = GetTemplateChild("DragThumb") as Thumb;
            ResizeLeft = GetTemplateChild("ResizeLeft") as Thumb;
            ResizeTop = GetTemplateChild("ResizeTop") as Thumb;
            ResizeRight = GetTemplateChild("ResizeRight") as Thumb;
            ResizeBottom = GetTemplateChild("ResizeBottom") as Thumb;
            ResizeTopLeft = GetTemplateChild("ResizeTopLeft") as Thumb;
            ResizeTopRight = GetTemplateChild("ResizeTopRight") as Thumb;
            ResizeBottomRight = GetTemplateChild("ResizeBottomRight") as Thumb;
            ResizeBottomLeft = GetTemplateChild("ResizeBottomLeft") as Thumb;
            Title = GetTemplateChild("Title") as TextBlock;
            MDIContent = GetTemplateChild("MDIContent") as ContentControl;
            RestoreButton = GetTemplateChild("RestoreButton") as Button;
            MaximizeButton = GetTemplateChild("MaximizeButton") as Button;
            CloseButton = GetTemplateChild("CloseButton") as Button;
            MinimizeButton = GetTemplateChild("MinimizeButton") as Button;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnLoaded;

            MDIContent.Content = _innerControl;

            var apBorder = (FormBorderStyle)_innerControl.GetValue(FormProperties.FormBorderStyleProperty);

            if (apBorder != FormBorderStyle.Fixed)
            {
                EnableResizing();
            }
            else
            {
                DisableResizing();
            }

            var apTitle = _innerControl.GetValue(FormProperties.TextProperty);

            if (apTitle != null)
            {
                Title.Text = (string)apTitle;
            }

            _mainContainer.PointerPressed += MainContainer_OnPointerPressed;
            DragThumb.DragStarted += DragThumb_OnDragStarted;
            DragThumb.DragDelta += DragThumbOnDragDelta;
            RestoreButton.Click += RestoreButtonOnClick;
            MaximizeButton.Click += MaximizeButtonOnClick;
            CloseButton.Click += CloseButtonOnClick;
        }

        private void EnableResizing()
        {
            ResizeRight.DragDelta += OnResizeRightOnDragDelta;
            ResizeLeft.DragDelta += OnResizeLeftOnDragDelta;
            ResizeTop.DragDelta += OnResizeTopOnDragDelta;
            ResizeBottom.DragDelta += OnResizeBottomOnDragDelta;
            ResizeBottomRight.DragDelta += OnResizeBottomRightOnDragDelta;
            ResizeTopRight.DragDelta += OnResizeTopRightOnDragDelta;
            ResizeTopLeft.DragDelta += OnResizeTopLeftOnDragDelta;
            ResizeBottomLeft.DragDelta += OnResizeBottomLeftOnDragDelta;

            ResizeRight.IsEnabled = true;
            ResizeLeft.IsEnabled = true;
            ResizeTop.IsEnabled = true;
            ResizeBottom.IsEnabled = true;
            ResizeBottomRight.IsEnabled = true;
            ResizeTopRight.IsEnabled = true;
            ResizeTopLeft.IsEnabled = true;
            ResizeBottomLeft.IsEnabled = true;
        }

        private void DisableResizing()
        {
            ResizeRight.DragDelta -= OnResizeRightOnDragDelta;
            ResizeLeft.DragDelta -= OnResizeLeftOnDragDelta;
            ResizeTop.DragDelta -= OnResizeTopOnDragDelta;
            ResizeBottom.DragDelta -= OnResizeBottomOnDragDelta;
            ResizeBottomRight.DragDelta -= OnResizeBottomRightOnDragDelta;
            ResizeTopRight.DragDelta -= OnResizeTopRightOnDragDelta;
            ResizeTopLeft.DragDelta -= OnResizeTopLeftOnDragDelta;
            ResizeBottomLeft.DragDelta -= OnResizeBottomLeftOnDragDelta;

            ResizeRight.IsEnabled = false;
            ResizeLeft.IsEnabled = false;
            ResizeTop.IsEnabled = false;
            ResizeBottom.IsEnabled = false;
            ResizeBottomRight.IsEnabled = false;
            ResizeTopRight.IsEnabled = false;
            ResizeTopLeft.IsEnabled = false;
            ResizeBottomLeft.IsEnabled = false;
        }
        private void CloseButtonOnClick(object o, RoutedEventArgs routedEventArgs)
        {
            RemoveEventHandlers();
            Closed?.Invoke(this, this);
        }

        private void DragThumb_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            if (FormWindowState == FormWindowState.Minimized)
            {
                return;
            }

            Activating?.Invoke(this, this);
            Activate();
        }

        private void DragThumbOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            var currentLeft = Canvas.GetLeft(this);
            var currentTop = Canvas.GetTop(this);

            var newLeft = currentLeft + args.HorizontalChange;
            var newTop = currentTop + args.VerticalChange;

            if (newLeft < 0)
            {
                newLeft = 0;
            }

            if (newTop < 0)
            {
                newTop = 0;
            }

            Canvas.SetLeft(this, newLeft);
            Canvas.SetTop(this, newTop);
        }

        private double ChangeWidth(double amount)
        {
            if (this.Width + amount < MinimumWidth)
            {
                var changeAmount = -(this.Width - MinimumWidth);
                this.Width += changeAmount;

                return changeAmount;
            }
            else
            {
                this.Width += amount;
                return amount;
            }
        }

        private double ChangeHeight(double amount)
        {
            if (this.Height + amount < MinimumHeight)
            {
                var changeAmount = -(this.Height - MinimumHeight);
                this.Height += changeAmount;
                return changeAmount;
            }
            else
            {
                this.Height += amount;
                return amount;
            }
        }

        void OnResizeRightOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            ChangeWidth(args.HorizontalChange);
        }


        void OnResizeLeftOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            var newLeft = args.HorizontalChange;
            var positionLeft = Canvas.GetLeft(this);

            if (positionLeft + newLeft < 0)
            {
                newLeft = 0 - positionLeft;
            }

            var changedWidth = ChangeWidth(-newLeft);
            Canvas.SetLeft(this, positionLeft + -changedWidth);
        }

        void OnResizeTopOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            var newTop = args.VerticalChange;
            var positionTop = Canvas.GetTop(this);

            if (positionTop + newTop < 0)
            {
                newTop = 0 - positionTop;
            }

            var changedHeight = ChangeHeight(-newTop);
            Canvas.SetTop(this, positionTop + -changedHeight);
        }

        void OnResizeBottomOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            ChangeHeight(args.VerticalChange);
        }

        void OnResizeBottomRightOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            ChangeHeight(args.VerticalChange);
            ChangeWidth(args.HorizontalChange);
        }

        void OnResizeTopRightOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            ChangeWidth(args.HorizontalChange);

            var newTop = args.VerticalChange;
            var positionTop = Canvas.GetTop(this);

            if (positionTop + newTop < 0)
            {
                newTop = 0 - positionTop;
            }

            var changedHeight = ChangeHeight(-newTop);
            Canvas.SetTop(this, positionTop + -changedHeight);
        }

        void OnResizeTopLeftOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            var newTop = args.VerticalChange;
            var positionTop = Canvas.GetTop(this);

            if (positionTop + newTop < 0)
            {
                newTop = 0 - positionTop;
            }

            var changedHeight = ChangeHeight(-newTop);
            Canvas.SetTop(this, positionTop + -changedHeight);

            var newLeft = args.HorizontalChange;
            var positionLeft = Canvas.GetLeft(this);

            if (positionLeft + newLeft < 0)
            {
                newLeft = 0 - positionLeft;
            }

            var changedWidth = ChangeWidth(-newLeft);
            Canvas.SetLeft(this, positionLeft + -changedWidth);
        }

        private void OnResizeBottomLeftOnDragDelta(object sender, DragDeltaEventArgs args)
        {
            ChangeHeight(args.VerticalChange);

            var newLeft = args.HorizontalChange;
            var positionLeft = Canvas.GetLeft(this);

            if (positionLeft + newLeft < 0)
            {
                newLeft = 0 - positionLeft;
            }

            var changedWidth = ChangeWidth(-newLeft);
            Canvas.SetLeft(this, positionLeft + -changedWidth);
        }

        private void MainContainer_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Activating?.Invoke(this, this);
            Activate();
        }

        private void MaximizeButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Maximize();
        }

        private void RemoveEventHandlers()
        {
            _mainContainer.PointerPressed -= MainContainer_OnPointerPressed;
            DragThumb.DragStarted -= DragThumb_OnDragStarted;
            DragThumb.DragDelta -= DragThumbOnDragDelta;
            RestoreButton.Click -= RestoreButtonOnClick;
            MaximizeButton.Click -= MaximizeButtonOnClick;

            ResizeRight.DragDelta -= OnResizeRightOnDragDelta;
            ResizeLeft.DragDelta -= OnResizeLeftOnDragDelta;
            ResizeTop.DragDelta -= OnResizeTopOnDragDelta;
            ResizeBottom.DragDelta -= OnResizeBottomOnDragDelta;
            ResizeBottomRight.DragDelta -= OnResizeBottomRightOnDragDelta;
            ResizeTopRight.DragDelta -= OnResizeTopRightOnDragDelta;
            ResizeTopLeft.DragDelta -= OnResizeTopLeftOnDragDelta;
            ResizeBottomLeft.DragDelta -= OnResizeBottomLeftOnDragDelta;
        }

        private void RestoreButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Restore();
        }
    }
}