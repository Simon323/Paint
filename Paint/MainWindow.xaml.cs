using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddPluginOne();
            AddPluginTwo();
            AppendButton();
        }

        #region Loading Plugins

        private void AddPluginOne()
        {
            var assembly = Assembly.LoadFrom(@"C:\Users\Szymon\Documents\Visual Studio 2013\Projects\Paint\PluginOne\bin\Debug\PluginOne.dll");
            var types = assembly.GetTypes();
            Type theType = null;

            foreach (var t in types)
            {
                if (t.IsClass && t.IsPublic && t.GetInterface("IPluginOne") != null)
                {
                    theType = t;
                }
            }

            if (theType != null)
            {
                var plugin = Activator.CreateInstance(theType) as Interface.IPluginOne;
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
                if (t.IsClass && t.IsPublic && t.GetInterface("IPluginTwo") != null)
                {
                    theType = t;
                }
            }

            if (theType != null)
            {
                var plugin = Activator.CreateInstance(theType) as Interface.IPluginTwo;
                //this.menuStrip1.Items.Add(plugin.GetItem());
            }
        }

        public Type LoadPluginOne()
        {
            var assembly = Assembly.LoadFrom(@"C:\Users\Szymon\Documents\Visual Studio 2013\Projects\Paint\PluginOne\bin\Debug\PluginOne.dll");

            Type[] types = assembly.GetTypes();
            Type theType = null;

            foreach (var t in types)
            {
                if (t.IsClass && t.IsPublic && t.GetInterface("IPluginOne") != null)
                {
                    theType = t;
                }
            }

            return theType;
        }

        public Type LoadPluginTwo()
        {
            var assembly = Assembly.LoadFrom(@"C:\Users\Szymon\Documents\Visual Studio 2013\Projects\Paint\PluginTwo\bin\Debug\PluginTwo.dll");

            Type[] types = assembly.GetTypes();
            Type theType = null;

            foreach (var t in types)
            {
                if (t.IsClass && t.IsPublic && t.GetInterface("IPluginTwo") != null)
                {
                    theType = t;
                }
            }

            return theType;
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

            Type theType = LoadPluginTwo();

            if (theType != null)
            {
                var plugin = Activator.CreateInstance(theType) as Interface.IPluginTwo;

                MessageBox.Show("txt: " + plugin.GetString());
            }
            else
                MessageBox.Show("błąd ładowania wtyczki");

        }

        #endregion

        #region Helper

        public void EnableButtons()
        {
            SavePicture.IsEnabled = true;
        }

        public void PictureNew()
        {

        }

        public void PictureOpen()
        {
            //EnableButtons();
            //string currentDirectory = Directory.GetCurrentDirectory();
            //try
            //{
            //    OpenFileDialog ofd = new OpenFileDialog();
            //    ofd.RestoreDirectory = true;
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        NewImage((Bitmap)Bitmap.FromFile(ofd.FileName));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error has ocurred when loading image.Please try with 24bpp images if you can't see it.", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }

        public void PictureSave()
        {

        }

        public void Exit()
        {
            this.Close();
        }

        //private void NewImage(Bitmap bmp)
        //{
        //    iniImg = bmp;
        //    changedImg = (Bitmap)iniImg.Clone();
        //    picImg.Image = changedImg;
        //    graphics = Graphics.FromImage(changedImg);
        //}

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

        

    }
}
