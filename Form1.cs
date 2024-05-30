using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ritbooook
{
    public class PenTool
    {
        public Pen Pen { get; private set; }
        public Pen Eraser { get; private set; }

        public PenTool(Color penColor, float penWidth, Color eraserColor, float eraserWidth)
        {
            Pen = new Pen(penColor, penWidth);
            Eraser = new Pen(eraserColor, eraserWidth);
        }

        public void ChangePenColor(Color newColor)
        {
            Pen.Color = newColor;
        }

        public void ChangePenWidth(float newWidth)
        {
            Pen.Width = newWidth;
        }

        public void ChangeEraserWidth(float newWidth)
        {
            Eraser.Width = newWidth;
        }
    }

    public partial class Form1 : Form
    {
        private NumericUpDown sizeSelector;
        private NumericUpDown sizeSelector2;
        private Bitmap bm;
        private Graphics g;
        private bool paint = false;
        private Point px, py;
        private int index;
        private int x, y, sX, sY, cX, Cy;
        private ColorDialog cd = new ColorDialog();
        private Color new_color;
        private PenTool penTool;

        public Form1()
        {
            InitializeComponent();
            this.Width = 950;
            this.Height = 700;
            bm = new Bitmap(bild.Width, bild.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            bild.Image = bm;

            penTool = new PenTool(Color.Black, 1, Color.White, 10);

            sizeSelector = new NumericUpDown();
            sizeSelector.Minimum = 1;
            sizeSelector.Maximum = 10;
            sizeSelector.Value = 5;
            sizeSelector.Location = new Point(10, 10);
            sizeSelector.ValueChanged += SizeSelector_ValueChanged;
            Controls.Add(sizeSelector);

            sizeSelector2 = new NumericUpDown();
            sizeSelector2.Minimum = 1;
            sizeSelector2.Maximum = 10;
            sizeSelector2.Value = 1;
            sizeSelector2.Location = new Point(10, 40);
            sizeSelector2.ValueChanged += SizeSelector2_ValueChanged;
            Controls.Add(sizeSelector2);
        }

        private void SizeSelector_ValueChanged(object sender, EventArgs e)
        {
            penTool.ChangePenWidth((float)sizeSelector.Value);
        }

        private void SizeSelector2_ValueChanged(object sender, EventArgs e)
        {
            penTool.ChangeEraserWidth((float)sizeSelector2.Value);
        }

        private void btn_cirkel_Click(object sender, EventArgs e)
        {
            index = 3;
        }

        private void btn_rekt_Click(object sender, EventArgs e)
        {
            index = 4;
        }

        private void btn_linje_Click(object sender, EventArgs e)
        {
            index = 5;
        }

        private void bild_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (paint)
            {
                if (index == 3)
                {
                    g.DrawEllipse(penTool.Pen, cX, Cy, sX, sY);
                }
                if (index == 4)
                {
                    g.DrawRectangle(penTool.Pen, cX, Cy, sX, sY);
                }
                if (index == 5)
                {
                    g.DrawLine(penTool.Pen, cX, Cy, x, y);
                }
            }
        }

        private void btn_rensa_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            bild.Image = bm;
            index = 0;
        }

        private void btn_färg_Click(object sender, EventArgs e)
        {
            cd.ShowDialog();
            new_color = cd.Color;
            val_färg.BackColor = cd.Color;
            penTool.ChangePenColor(cd.Color);
        }

        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Image.Width / pb.Width;
            float pY = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void färg_väjare_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(färg_väjare, e.Location);
            val_färg.BackColor = ((Bitmap)färg_väjare.Image).GetPixel(point.X, point.Y);
            new_color = val_färg.BackColor;
            penTool.ChangePenColor(val_färg.BackColor);
        }

        private void bild_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;
            cX = e.X;
            Cy = e.Y;
        }

        private void bild_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(penTool.Pen, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(penTool.Eraser, px, py);
                    py = px;
                }
            }
            bild.Refresh();

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - Cy;
        }

        private void bild_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = set_point(bild, e.Location);
                Fill(bm, point.X, point.Y, new_color);
            }
        }

        private void btn_fylla_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        private void bild_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            sX = x - cX;
            sY = y - Cy;

            if (index == 3)
            {
                g.DrawEllipse(penTool.Pen, cX, Cy, sX, sY);
            }
            if (index == 4)
            {
                g.DrawRectangle(penTool.Pen, cX, Cy, sX, sY);
            }
            if (index == 5)
            {
                g.DrawLine(penTool.Pen, cX, Cy, x, y);
            }
        }

        private void btn_penna_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void btn_Sudd_Click(object sender, EventArgs e)
        {
            index = 2;
        }

        private void bild_Click(object sender, EventArgs e)
        {

        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }
        }
    }
}