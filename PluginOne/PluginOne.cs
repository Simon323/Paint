using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PluginOne
{
    public class PluginOne : Interface.IPlugin
    {
        private Guid _guid = Guid.NewGuid();

        private Canvas _canvas;
        private Color _color;
        private int _thikness;

        public string GetString()
        {
            return "Hello world";
        }

        public string GetID()
        {
            return _guid.ToString();
        }

        public MenuItem GetMenuItem()
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "Line";
            return menuItem;
        }

        public event EventHandler Finished;

        public void Initialize(Canvas canvas, Color color, int thikness)
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

        public void Dispose()
        {
            if (_canvas != null)
            {
                _canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
                _canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                _canvas.MouseMove -= Canvas_MouseMove;
            }
        }

        public void UpdateParameters(Color color, int thinkess)
        {
            _color = color;
            _thikness = thinkess;
        }

        private void RaiseFinishedEvent()
        {
            var copy = Finished;
            if (Finished != null)
            {
                copy(this, new EventArgs());
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (canvas.CaptureMouse())
            {
                var startPoint = e.GetPosition(canvas);
                var line = new Line
                {
                    Stroke = new SolidColorBrush(_color),
                    StrokeThickness = _thikness,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = startPoint.X,
                    Y2 = startPoint.Y,
                };

                canvas.Children.Add(line);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (canvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                var line = canvas.Children.OfType<Line>().LastOrDefault();

                if (line != null)
                {
                    var endPoint = e.GetPosition(canvas);
                    line.X2 = endPoint.X;
                    line.Y2 = endPoint.Y;
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
