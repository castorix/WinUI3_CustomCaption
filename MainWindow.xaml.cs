using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_CustomCaption
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        IntPtr hWnd = IntPtr.Zero;
        private Microsoft.UI.Windowing.AppWindow _appW = null;

        private TranslateTransform translateTransform1 = null;
        private TransformGroup trfg1 = null;
        private TextBlock text1 = null;
        private Border border1 = null;
        private double nCurrentTranslateTransformX = 0;
        private double nTextX = 0;

        public MainWindow()
        {
            this.InitializeComponent();

            hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            _appW = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);

            _appW.Resize(new Windows.Graphics.SizeInt32(600, 250));
            _appW.Move(new Windows.Graphics.PointInt32(600, 400));

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private float _ScrollSpeed = 1.0f;

        private float ScrollSpeed
        {
            get => _ScrollSpeed;
            set
            {
                _ScrollSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScrollSpeed)));
            }
        }
        public double GetSpeed() => _ScrollSpeed;
        public void SetSpeed(double x) => ScrollSpeed = (float)x;

        private void CompositionTarget_Rendering(object sender, object e)
        {
            if (bRender)
                Render();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScrollingText(!bRender);           
        }
 
        private bool bRender = false;
        private void Render()
        {
            if (text1 != null)
            {
                if (translateTransform1 == null)
                {
                    translateTransform1 = new TranslateTransform();
                    trfg1 = new TransformGroup();
                    trfg1.Children.Add(translateTransform1);
                    text1.RenderTransform = trfg1;
                }

                translateTransform1.X = nTextX;
                nTextX -= _ScrollSpeed;
                if (nTextX <= -text1.ActualWidth)
                {
                    nTextX = nCurrentTranslateTransformX;
                }
            }
        }

        private void ShowScrollingText(bool bShow)
        {
            if (bShow)
            {
                if (text1 == null && border1 == null)
                {
                    text1 = new TextBlock()
                    {
                        Text = "This is a scrolling text",
                        FontFamily = new FontFamily("Times New Roman"),
                        FontSize = 20,                       
                        TextWrapping = TextWrapping.NoWrap,
                        TextTrimming = TextTrimming.None                        
                    };
                    text1.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Lime);
                    text1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    text1.Margin = new Thickness(0, 0, -text1.ActualWidth, 0);

                    border1 = new Border()
                    {
                        //BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.GreenYellow),
                        BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0)),
                        BorderThickness = new Thickness(2),                       
                        Margin = new Thickness(190, 1, 98, 1),
                        Child = text1,
                        Background = new SolidColorBrush(Microsoft.UI.Colors.Black)
                    };

                    border1.SizeChanged += Border1_SizeChanged;
                    border1.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    nTextX = border1.DesiredSize.Width;

                    border1.SetValue(Grid.RowProperty, 1);
                    border1.SetValue(Grid.ColumnProperty, 1);
                    AppTitleBar.Children.Add(border1);                   
                }
                text1.Visibility = Visibility.Visible;
                border1.Visibility = Visibility.Visible;
                sliderSpeed.Visibility = Visibility.Visible;
                myButton.Content = "Hide Scrolling text";
                bRender = true;
            }
            else
            {                
                text1.Visibility = Visibility.Collapsed;
                border1.Visibility = Visibility.Collapsed;
                sliderSpeed.Visibility = Visibility.Collapsed;
                myButton.Content = "Show Scrolling text";                
                bRender = false;
            }
        }     
  
        private void Border1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            nCurrentTranslateTransformX = e.NewSize.Width;
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Windows.Foundation.Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
            ((UIElement)sender).Clip = rg;
        }
    }
}
