using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace task6
{
    public partial class Form1 : Form
    {
		Bitmap bmp;
		Graphics g;
		Pen pen_shape = new Pen(Color.Blue); // для фигуры
		Pen pen_axis = new Pen(Color.Black); // для осей
		int centerX, centerY; // центр pictureBox
        bool is_axis = false; // выбрана ли ось для поворота
        my_point axis_P1, axis_P2; // точки оси для поворота
		List<face> shape = new List<face>(); // фигура - список граней
		List<my_point> points = new List<my_point>(); // список точек

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox.Size.Width, pictureBox.Size.Height);
            pictureBox.Image = bmp; centerX = pictureBox.Width / 2; centerY = pictureBox.Height / 2;
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			g = Graphics.FromImage(bmp);
		}

		private void build_points()
		{
			points.Clear();
			foreach (face sh in shape)
				for (int i = 0; i < sh.points.Count; i++)
					if (!points.Contains(sh.points[i]))
						points.Add(sh.points[i]);
		}

        private void draw_point(my_point p) // рисуем точку
        {
            g.FillEllipse(new SolidBrush(Color.Red), (int)Math.Round(p.X + centerX - 3), (int)Math.Round(-p.Y + centerY - 3), 6, 6);
        }

        private void draw_face(face f) // рисуем грань
        {
            int n = f.points.Count - 1;

			int x1 = (int)Math.Round(f.points[0].X + centerX);
            int x2 = (int)Math.Round(f.points[n].X + centerX);
			int y1 = (int)Math.Round(-f.points[0].Y + centerY);
            int	y2 = (int)Math.Round(-f.points[n].Y + centerY);
			g.DrawLine(pen_shape, x1, y1, x2, y2);

			for (int i = 0; i < n; i++)
            {
				x1 = (int)Math.Round(f.points[i].X + centerX);
                x2 = (int)Math.Round(f.points[i + 1].X + centerX);
                y1 = (int)Math.Round(-f.points[i].Y + centerY);
                y2 = (int)Math.Round(-f.points[i + 1].Y + centerY);
                g.DrawLine(pen_shape, x1, y1, x2, y2);
            }
        }

		Bitmap bmp2;
		bool flag = true;
        private void redraw_image() // перерисовываем картинку
        {
			g.Clear(Color.White);
			g.DrawLine(pen_axis, new Point(0, centerY), new Point(pictureBox.Width, centerY));
            g.DrawLine(pen_axis, new Point(centerX, 0), new Point(centerX, pictureBox.Height));
            if (axis_P2 != null)
            {
                draw_point(axis_P1);
                draw_point(axis_P2);
                g.DrawLine(pen_axis, (int)Math.Round(axis_P1.X + centerX), (int)Math.Round(-axis_P1.Y + centerY), (int)Math.Round(axis_P2.X + centerX), (int)Math.Round(-axis_P2.Y + centerY));
            }
            foreach (face f in shape)
                draw_face(f);
            pictureBox.Image = bmp;
        }

        private void build_tetrahedron()
        {
            double h = Math.Sqrt(3) / 2 * 100;

            my_point p1 = new my_point(-50, -h/2, 0);
            my_point p2 = new my_point(0, -h/2, -h);
            my_point p3 = new my_point(50, -h/2, 0);
            my_point p4 = new my_point(0, h/2, 0);

			face f1 = new face();	f1.add(p1);		f1.add(p2);		f1.add(p3);		shape.Add(f1);
			face f2 = new face();	f2.add(p1);		f2.add(p4);		f2.add(p2);		shape.Add(f2);
			face f3 = new face();	f3.add(p4);		f3.add(p2);		f3.add(p3);		shape.Add(f3); 
			face f4 = new face();	f4.add(p1);		f4.add(p4);		f4.add(p3);		shape.Add(f4);
        }

        private void build_hexahedron()
        {
            my_point p1 = new my_point(-50, -50, -50);
            my_point p2 = new my_point(-50, 50, -50);
            my_point p3 = new my_point(50, 50, -50);
            my_point p4 = new my_point(50, -50, -50);
            my_point p5 = new my_point(-50, -50, 50);
            my_point p6 = new my_point(-50, 50, 50);
            my_point p7 = new my_point(50, 50, 50);
            my_point p8 = new my_point(50, -50, 50);

            face f1 = new face();	f1.add(p1);		f1.add(p2);		f1.add(p3);		f1.add(p4);		shape.Add(f1); 
			face f2 = new face();	f2.add(p1);		f2.add(p2);		f2.add(p6);		f2.add(p5);		shape.Add(f2); 
			face f3 = new face();	f3.add(p5);		f3.add(p6);		f3.add(p7);		f3.add(p8);		shape.Add(f3); 
			face f4 = new face();	f4.add(p4);		f4.add(p3);		f4.add(p7);		f4.add(p8);		shape.Add(f4); 
			face f5 = new face();	f5.add(p2);		f5.add(p6);		f5.add(p7);		f5.add(p3);		shape.Add(f5); 
			face f6 = new face();	f6.add(p1);		f6.add(p5);		f6.add(p8);		f6.add(p4);		shape.Add(f6);
        }

        private void build_octahedron()
        {
            double a = Math.Sqrt(3) / 2 * 100;
            double p = (a + a + 100) / 2;
            double h = 2 * Math.Sqrt(p * (p - 100) * (p - a) * (p - a)) / 100;
            
			my_point p1 = new my_point(0, -h, 0);
            my_point p2 = new my_point(-50, 0, -50);
            my_point p3 = new my_point(0, h, 0);
            my_point p4 = new my_point(50, 0, -50);
            my_point p5 = new my_point(-50, 0, 50);
            my_point p6 = new my_point(50, 0, 50);

            face f1 = new face();	f1.add(p2);		f1.add(p3);		f1.add(p4);		shape.Add(f1);			
			face f2 = new face();	f2.add(p2);		f2.add(p1);		f2.add(p4);		shape.Add(f2);			
			face f3 = new face();	f3.add(p2);		f3.add(p3);		f3.add(p5);		shape.Add(f3);
			face f4 = new face();	f4.add(p2);		f4.add(p1);		f4.add(p5);		shape.Add(f4);			
			face f5 = new face();	f5.add(p4);		f5.add(p3);		f5.add(p6);		shape.Add(f5);			
			face f6 = new face();	f6.add(p4);		f6.add(p1);		f6.add(p6);		shape.Add(f6);			
			face f7 = new face();	f7.add(p5);		f7.add(p3);		f7.add(p6);		shape.Add(f7);			
			face f8 = new face();	f8.add(p5);		f8.add(p1);		f8.add(p6);		shape.Add(f8);
        }


        private void displacement(int kx, int ky, int kz) // сдвиг
        {
            
        }

        private void rotate(double xAngle, double yAngle, double zAngle) // поворот
        {
           
        }


        private my_point center_point() // центр фигуры
        {
            double sumX = 0, sumY = 0, sumZ = 0;
            int count = 0;
            for (int i = 0; i < shape.Count; i++)
                for (int j = 0; j < shape[i].points.Count; j++)
                {
                    sumX += shape[i].points[j].X;
                    sumY += shape[i].points[j].Y;
                    sumZ += shape[i].points[j].Z;
                    ++count;
                }
            return new my_point(sumX / count, sumY / count, sumZ / count);
        }

        private void scaling(double xScale, double yScale, double zScale) // масштабирование
        {
            my_point center_P = center_point();

        }

        private void reflection(int ind) // отражение: 1 - XOY, 2 - XOZ, 3 - YOZ
        {
            
        }



        private void axis_rotate(my_point pt1, my_point pt2, double angle) // поворот вокруг оси
        {

        }

        private void displacement_button_Click(object sender, EventArgs e) // перенос
        {
            int kx = (int)x_shift.Value, ky = (int)y_shift.Value, kz = (int)z_shift.Value;
            displacement(kx, ky, kz);
            redraw_image();
        }

        private void rotate_button_Click(object sender, EventArgs e) // поворот
        {
            double x_angle = ((double)x_rotate.Value * Math.PI) / 180;
            double y_angle = ((double)y_rotate.Value * Math.PI) / 180;
            double z_angle = ((double)z_rotate.Value * Math.PI) / 180;
            rotate(x_angle, y_angle, z_angle);
            redraw_image();
        }

        private void scale_button_Click(object sender, EventArgs e) // масштабирование
        {
			scaling((double)x_scale.Value, (double)y_scale.Value, (double)z_scale.Value);
            redraw_image();
        }

        private void reflect_button_Click(object sender, EventArgs e) // отражение
        {
            if (xoy_reflect.Checked)
                reflection(1);
            if (xoz_reflect.Checked)
                reflection(2);
            if (yoz_reflect.Checked)
                reflection(3);
            redraw_image();
        }


        private void axis_choice_button_Click(object sender, EventArgs e) // выбираем точки для поворота вокруг оси
        {
            if (axis_P1 != null)
            {
                axis_P1 = axis_P2 = null;
                redraw_image();
            } 
            is_axis = true;
        }

        private void axis_rotate_button_Click(object sender, EventArgs e) // поворот вокруг оси
        {
            if (axis_P1 == null || axis_P2 == null)
            {
                MessageBox.Show("Выберите ось!", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            axis_P1.Z = (double)z1_coord.Value;
            axis_P2.Z = (double)z2_coord.Value;
            double axisAngle = ((double)axis_angle.Value * Math.PI) / 180;
            axis_rotate(axis_P1, axis_P2, axisAngle);
            redraw_image();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!is_axis || e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            if (axis_P1 == null)
            {
                axis_P1 = new my_point((e.X - centerX), (-e.Y + centerY), 0);
                draw_point(axis_P1);
            }
            else
            {
                axis_P2 = new my_point((e.X - centerX), (-e.Y + centerY), 0);
                draw_point(axis_P2);
                g.DrawLine(pen_axis, (int)Math.Round(axis_P1.X + centerX), (int)Math.Round(-axis_P1.Y + centerY), (int)Math.Round(axis_P2.X + centerX), (int)Math.Round(-axis_P2.Y + centerY));
                is_axis = false;
            }
            pictureBox.Image = bmp;
        }

        private void axis_del_button_Click(object sender, EventArgs e) // удаляем ось
        {
            axis_P1 = axis_P2 = null;
            redraw_image();
        }

		private void shape_CheckedChanged(object sender, EventArgs e)
		{
			if ((sender as RadioButton).Checked == false)
				return;
			shape.Clear();
			if (tetrahedron.Checked)
				build_tetrahedron();
			else if (hexahedron.Checked)
				build_hexahedron();
			else if (octahedron.Checked)
				build_octahedron();
			build_points();
			redraw_image();
		}

    }

    public class my_point
    {
        public double X, Y, Z;

        public my_point()
        {
            this.X = this.Y = this.Z = 0;
        }

        public my_point(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

    }

    public class face
    {
        public List<my_point> points;

        public face()
        {
            points = new List<my_point>();
        }

        public void add(my_point p)
        {
            points.Add(p);
        }
    }
}
