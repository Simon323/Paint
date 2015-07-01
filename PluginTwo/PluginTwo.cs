using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PluginTwo
{
    public class PluginTwo : Interface.IPlugin
    {
        private Guid _guid = Guid.NewGuid();

        private Canvas _canvas;
        private Color _color;
        private int _thikness;

        public string GetString()
        {
            return "Hello world 2";
        }

        public string GetID()
        {
            return _guid.ToString();
        }

        public MenuItem GetMenuItem()
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "Rectangle";
            return menuItem;
        }

        public event EventHandler Finished;

        public void Initialize(System.Windows.Controls.Canvas canvas, Color color, int thikness)
        {
            _canvas = canvas;
            _color = color;
            _thikness = thikness;
            if (_canvas != null)
            {
                _canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                _canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                _canvas.MouseMove += Canvas_MouseMove;
            }
        }

        public void UpdateParameters(Color color, int thinkess)
        {
            _color = color;
            _thikness = thinkess;
        }

        public void Dispose()
        {
            if (_canvas != null)
            {
                _canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
                _canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                _canvas.MouseMove -= Canvas_MouseMove;
            }
        }

        private void RaiseFinishedEvent()
        {
            var copy = Finished;
            if (Finished != null)
            {
                copy(this, new EventArgs());
            }
        }

        private Point startPoint;
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (canvas.CaptureMouse())
            {
                startPoint = e.GetPosition(canvas);
                var rect = new Rectangle
                {
                    Stroke = new SolidColorBrush(_color),
                    StrokeThickness = _thikness,
                };
                Canvas.SetLeft(rect, startPoint.X);
                Canvas.SetTop(rect, startPoint.Y);
                canvas.Children.Add(rect);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (canvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                var rect = canvas.Children.OfType<Rectangle>().LastOrDefault();

                if (rect != null)
                {
                    var endPoint = e.GetPosition(canvas);
                    var x = Math.Min(endPoint.X, startPoint.X);
                    var y = Math.Min(endPoint.Y, startPoint.Y);

                    var w = Math.Max(endPoint.X, startPoint.X) - x;
                    var h = Math.Max(endPoint.Y, startPoint.Y) - y;

                    rect.Width = w;
                    rect.Height = h;

                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);
                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Canvas)sender).ReleaseMouseCapture();

            RaiseFinishedEvent();
        }
    }
}
