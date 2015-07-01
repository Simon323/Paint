using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        Bitmap iniImg, changedImg;
        //PictureBox picBoxClicked;
        WidthRectanglePointer widthRectanglePointer;
        WidthCirclePointer widthCirclePointer;
        TypePointer typePointer;
        Pen pen;
        Graphics graphics;
        Rectangle rect;
        Color colorFront, colorBack;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            AddPluginOne();
            AddPluginTwo();
            AppendButton();
            LoadSettings();
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

        #region Start settings

        public void LoadSettings()
        {
            widthRectanglePointer = WidthRectanglePointer.Small;
            typePointer = TypePointer.Rectangle;
            colorFront = Color.Black;
            colorBack = Color.White;
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
            EnableButtons();
            Bitmap bmp = new Bitmap(Convert.ToInt32(ImageContainer.Width), Convert.ToInt32(ImageContainer.Height));
            graphics = Graphics.FromImage(bmp);
            //graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, Convert.ToInt32(ImageContainer.Width), Convert.ToInt32(ImageContainer.Height));
            NewImage(bmp, null);
        }

        public void PictureOpen()
        {
            EnableButtons();
            string currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Windows Bitmap (*.BMP)|*.bmp|Joint Photographic Experts Group (*.JPG;*.JIF;*.JPEG)|*.jpg;*.jif;*.jpeg|Portable Network Graphics(*.PNG)|*.pgn";
                ofd.RestoreDirectory = true;
                ofd.ShowDialog();
                //if (ofd.ShowDialog().ToString().Equals("OK"))
                //{
                    NewImage((Bitmap)Bitmap.FromFile(ofd.FileName), ofd.FileName);
                //}
            }
            catch (Exception ex)
            {
               // MessageBox.Show("An error has ocurred when loading image.Please try with 24bpp images if you can't see it.", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void PictureSave()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Windows Bitmap (*.BMP)|*.bmp|Joint Photographic Experts Group (*.JPG;*.JIF;*.JPEG)|*.jpg;*.jif;*.jpeg|Portable Network Graphics(*.PNG)|*.pgn";
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog().ToString().Equals("OK"))
            {
                switch (sfd.FilterIndex)
                {
                    case 1:
                        changedImg.Save(sfd.FileName, ImageFormat.Bmp);
                        break;

                    case 2:
                        changedImg.Save(sfd.FileName, ImageFormat.Jpeg);
                        break;

                    case 3:
                        changedImg.Save(sfd.FileName, ImageFormat.Png);
                        break;
                }
            }
            
        }

        public void Exit()
        {
            this.Close();
        }

        private void NewImage(Bitmap bmp, string path)
        {
            iniImg = bmp;
            changedImg = (Bitmap)iniImg.Clone();
            if (path == null)
            {
                
            }
            else
            {

                ImageContainer.Source = new BitmapImage(new Uri(path));
            }
            graphics = Graphics.FromImage(changedImg);
           // picImg.Image = changedImg;
            
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
        private void ImageContainer_MouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("cos");
            PaintOrErase(e);
        }

        private void PaintOrErase(MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawFigure(colorFront, e);
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                DrawFigure(colorFront, e);
            }
            //if (e.Button == MouseButtons.Left)
            //{
            //    ShowStatus("Painting...", imageList1.Images[0]);
            //    DrawFigure(colorFront, e);
            //    picImg.Refresh();
            //}
            //else
            //    if (e.Button == MouseButtons.Right)
            //    {
            //        ShowStatus("Erasing...", imageList1.Images[1]);
            //        DrawFigure(colorBack, e);
            //        picImg.Refresh();
            //    }
        }

        private void DrawFigure(Color color, MouseEventArgs e)
        {
            //var p = e.GetPosition(null);
            //var q = e.GetPosition(ImageContainer);
            //MousePosText = string.Format("GetPosition(null): X = {0}, Y = {1}", p.X, p.Y);
            
            pen = new Pen(color, 1);
            var positions = e.GetPosition(ImageContainer);
            switch (typePointer)
            {
                case TypePointer.Rectangle:
                    
                    rect = new Rectangle(Convert.ToInt32(positions.X), Convert.ToInt32(positions.Y), (int)widthRectanglePointer, (int)widthRectanglePointer);
                    graphics.DrawRectangle(pen, rect);
                    graphics.FillRectangle(new SolidBrush(color), rect);
                    break;
                case TypePointer.Circle:

                    rect = new Rectangle(Convert.ToInt32(positions.X), Convert.ToInt32(positions.Y), (int)widthCirclePointer, (int)widthCirclePointer);
                    graphics.DrawEllipse(pen, rect);
                    graphics.FillEllipse(new SolidBrush(color), rect);
                    break;
            }
        }
        #endregion

    }
}
