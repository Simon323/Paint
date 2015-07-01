using Interface;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Paint
{

    #region Enums
    public enum WidthRectanglePointer
    {
        Big = 21,
        Medium = 13,
        Small = 7
    }

    public enum WidthCirclePointer
    {
        Big = 31,
        Medium = 20,
        Small = 10
    }

    public enum TypePointer
    {
        Rectangle,
        Circle
    }
    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private Color _color = Colors.Black;
        private int _thikness = 3;
        private Dictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private IPlugin _currentActivePlugin = null;

        private List<UIElement> Redos = new List<UIElement>();

        private string _filePath = string.Empty;
        public MenuItem pluginsMenuItem;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            pluginsMenuItem = new MenuItem(){ Header = "Plugins",};
            AddPluginOne();
            AddPluginTwo();
            AppendPluginsToForm();
        }

        #region Loading Plugins

        private void AddPluginOne()
        {
            var assembly = Assembly.LoadFrom(@"C:\Users\Szymon\Documents\Visual Studio 2013\Projects\Paint\PluginOne\bin\Debug\PluginOne.dll");
            var types = assembly.GetTypes();
            Type theType = null;

            foreach (var t in types)
            {
                if (t.IsClass && t.IsPublic && t.GetInterface("IPlugin") != null)
                {
                    theType = t;
                }
            }

            if (theType != null)
            {
                var pluginOne = Activator.CreateInstance(theType) as Interface.IPlugin;

                var menuItem = pluginOne.GetMenuItem();
                menuItem.Tag = pluginOne.GetID();
                menuItem.Click += pluginMenuItem_Click;
                pluginsMenuItem.Items.Add(menuItem);
                Plugins.Add(pluginOne.GetID(), pluginOne);

                //MainMenu.Items.Add(pluginsMenuItem);

                //this.menuStrip1.Items.Add(plugin.GetItem());
            }
        }

        private void AddPluginTwo()
        {
            var assembly = Assembly.LoadFrom(@"C:\Users\Szymon\Documents\Visual Studio 2013\Projects\Paint\PluginTwo\bin\Debug\PluginTwo.dll");
            var types = assembly.GetTypes();
            Type theType = null;

            foreach (var t in types)
            {
                if (t.IsClass && t.IsPublic && t.GetInterface("IPlugin") != null)
                {
                    theType = t;
                }
            }

            if (theType != null)
            {
                var pluginTwo = Activator.CreateInstance(theType) as Interface.IPlugin;

                var menuItem = pluginTwo.GetMenuItem();
                menuItem.Tag = pluginTwo.GetID();
                menuItem.Click += pluginMenuItem_Click;
                pluginsMenuItem.Items.Add(menuItem);
                Plugins.Add(pluginTwo.GetID(), pluginTwo);
                //this.menuStrip1.Items.Add(plugin.GetItem());
            }
        }

        private void pluginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (sender != null)
            {
                string pluginID = menuItem.Tag as string;
                if (!string.IsNullOrEmpty(pluginID))
                {
                    if (_currentActivePlugin != null)
                    {
                        _currentActivePlugin.Dispose();
                    }

                    _currentActivePlugin = Plugins[pluginID];
                    _currentActivePlugin.Initialize(MainCanvas, _color, _thikness);
                    _currentActivePlugin.Finished += _currentActivePlugin_Finished;
                }
            }
        }

        private void _currentActivePlugin_Finished(object sender, EventArgs e)
        {

        }
        

        #endregion

        #region Start settings

        public void AppendPluginsToForm()
        {
            MainMenu.Items.Add(pluginsMenuItem);
        }

        #endregion

        #region Examples

        public void AppendButton()
        {
            Button myButton = new Button();
            // Set properties.
            myButton.Content = "Click Me!";
            myButton.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            //myButton.Margin = new Thickness(-125, 153, 0, 0);
            myButton.Width = 20;

            // Add created button to a previously created container.
            BrushPanel.Children.Add(myButton);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Type theType = LoadPluginTwo();

            //if (theType != null)
            //{
            //    var plugin = Activator.CreateInstance(theType) as Interface.IPluginTwo;

            //    MessageBox.Show("txt: " + plugin.GetString());
            //}
            //else
            //    MessageBox.Show("błąd ładowania wtyczki");

        }

        #endregion

        #region Helper

        public void EnableButtons()
        {
            SavePicture.IsEnabled = true;
        }

        public void ExportToPng(string path, Canvas surface)
        {
            if (path == null) return;

            Size size = new Size(surface.ActualWidth, surface.ActualHeight);

            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            using (FileStream outStream = new FileStream(path, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
                outStream.Close();
            }
        }

        public void PictureNew()
        {
            MainCanvas.Children.Clear();
        }

        public void PictureOpen()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(ofd.FileName);
                image.EndInit();
                //v_Imege.Source = image.Clone();
            }
        }

        public void PictureSave()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                var result = sfd.ShowDialog();
                if (result == true)
                {
                    _filePath = sfd.FileName;
                }
            }

            ExportToPng(_filePath, MainCanvas);
            MainCanvas.Children.Clear();
        }

        public void Exit()
        {
            this.Close();
        }

        private void NewImage()
        {
           
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.S)
                {
                    PictureSave();
                }
                else if (e.Key == Key.Z)
                {
                    if (MainCanvas.Children.Count > 0)
                    {
                        Redos.Add(MainCanvas.Children[MainCanvas.Children.Count - 1]);
                        MainCanvas.Children.RemoveAt(MainCanvas.Children.Count - 1);
                    }
                }
                else if (e.Key == Key.Y)
                {
                    if (Redos.Any())
                    {
                        MainCanvas.Children.Add(Redos.Last());
                        Redos.Remove(Redos.Last());
                    }
                }
            }
        }

        #endregion

        #region Main Menu

        private void NewPicture_Click(object sender, RoutedEventArgs e)
        {
            PictureNew();
        }

        private void OpenPicture_Click(object sender, RoutedEventArgs e)
        {
            PictureOpen();
        }

        private void SavePicture_Click(object sender, RoutedEventArgs e)
        {
            PictureSave();
        }

        private void ClosePaint_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        #endregion

        #region Image Form

        
        #endregion

        #region XCTK Controls

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (ColorPicker.SelectedColor != null)
            {
                _color = ColorPicker.SelectedColor;
                UpdateColors();
            }
        }

        private void UpdateColors()
        {
            if (_currentActivePlugin != null)
            {
                _currentActivePlugin.UpdateParameters(_color, _thikness);
            }
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (IntegerUpDown.Value.HasValue
                && _thikness != IntegerUpDown.Value.Value)
            {
                _thikness = IntegerUpDown.Value.Value;
                UpdateThikness();
            }
        }

        private void UpdateThikness()
        {
            if (_currentActivePlugin != null)
            {
                _currentActivePlugin.UpdateParameters(_color, _thikness);
            }
        }

        #endregion

    }
}
