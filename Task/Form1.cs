using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Task
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> imgInput;
        Rectangle rect;
        Point StartLocation;
        Point EndLcation;
        bool IsMouseDown = false;


        public Form1()
        {
            InitializeComponent();
            listBox1.SetSelected(0,true);
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            //pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        Bitmap originalbitmap;

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            
            // RESIZE THE IMAGE
             double sizeFactor = e.Delta * 0.001;
             if (e.Delta < 0) sizeFactor = sizeFactor * -1 + 1;
             if (e.Delta == 120) sizeFactor = 0.88;
             if (sizeFactor == 0.88) sizeFactor = 1.12;
             else if (sizeFactor == 1.12) sizeFactor = 0.88;
             Bitmap originalBitmap = (Bitmap)pictureBox1.Image;
             Size newSize = new Size((int)(originalBitmap.Width * sizeFactor), (int)(originalBitmap.Height * sizeFactor));
             Bitmap bmp = new Bitmap(originalbitmap, newSize);
             pictureBox1.Image = bmp;

            //RESIZE RECTANGLE PLEASE
            /*int a = Convert.ToInt32(rect.Width * sizeFactor);
            int b = Convert.ToInt32(rect.Height * sizeFactor);
            Size recSize = new Size(a, b);
            rect = new Rectangle(rect.Location,recSize);
            */
            IsMouseDown = true;
            pictureBox1_MouseUp_1(sender, e);
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Environment.CurrentDirectory + "\\..\\..\\image" +"-"+ (listBox1.SelectedIndex + 1)+".jpg";
            pictureBox1.Load();
            originalbitmap = (Bitmap)pictureBox1.Image;

            imgInput = new Image<Bgr, byte>((Bitmap)pictureBox1.Image);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == true)
            {
                EndLcation = e.Location;
                pictureBox1.Invalidate();
            }
            

        }
        private Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            StartLocation = e.Location;
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == true)
            {
                EndLcation = e.Location;
                IsMouseDown = false;
                if (rect != null)
                {
                    Image img = pictureBox1.Image;
                    int Zoomwidth = pictureBox1.Width;
                    int Zoomheight = pictureBox2.Height;


                    if (img.Width <= (e.X + Zoomwidth))
                    {
                        Zoomwidth = img.Width - e.X;
                    }
                    if (img.Height <= (e.Y + Zoomheight))
                    {
                        Zoomheight = img.Height - e.Y;
                    }

                    Rectangle rec = new Rectangle(e.X, e.Y, Zoomwidth, Zoomheight);

                    pictureBox2.Image = cropImage(img, rect);
                }
            }
            /*
            if (IsMouseDown == true)
            {
                EndLcation = e.Location;
                IsMouseDown = false;
                if (rect != null)
                {
                    imgInput.ROI = rect;
                    Image<Bgr, byte> temp = imgInput.CopyBlank();
                    imgInput.CopyTo(temp);
                    imgInput.ROI = Rectangle.Empty;
                    pictureBox2.Image = temp.Bitmap;
                }
            }*/
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (rect != null)
            {
                e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
            }
        }

        private Rectangle GetRectangle()
        {
            rect = new Rectangle();
            rect.X = Math.Min(StartLocation.X, EndLcation.X);
            rect.Y = Math.Min(StartLocation.Y, EndLcation.Y);
            rect.Width = Math.Abs(StartLocation.X - EndLcation.X);
            rect.Height = Math.Abs(StartLocation.Y - EndLcation.Y);

            return rect;
        }
        
    }
}
