using System;
using System.Collections.Generic;
using System.Linq;
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
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Model3DGroup arm;

        BoxVisual3D mybox;

        Model3D rest;
        Model3D armbase;
        Model3D armshoulder;
        Model3D armelbow;

        public double boxheigth
        {
            get { return mybox.Height; }
            set { mybox.Height = value; }
        }

        public double boxwidth
        {
            get { return mybox.Width; }
            set { mybox.Width = value; }
        }

        public double boxlength
        {
            get { return mybox.Length; }
            set { mybox.Length = value; }
        }


        public double boxX
        {
            get { return mybox.Center.X; }
            set { mybox.Center = new Point3D(value, mybox.Center.Y, mybox.Center.Z); }
        }


        public double boxY
        {
            get { return mybox.Center.Y; }
            set { mybox.Center = new Point3D(mybox.Center.X, value, mybox.Center.Z); }
        }

        public double boxZ
        {
            get { return mybox.Center.Z; }
            set { mybox.Center = new Point3D(mybox.Center.X, mybox.Center.Y, value); }
        }

        //property for the shoulder movement
        double m_shoulder_angle;
        public double shoulder_angle
        {
            get { return m_shoulder_angle; }
            set
            {
                move_shoulder(value);
                m_shoulder_angle = value;
            }
        }




        double m_elbow_angle;

        public double elbow_angle
        {
            get { return m_elbow_angle; }
            set
            {
                move_elbow(value);
                m_elbow_angle = value;
            }
        }


        double m_elbow_angle2;

        public double elbow_angle2
        {
            get { return m_elbow_angle2; }
            set
            {
                move_elbow2(value);
                m_elbow_angle2 = value;
            }
        }



        public Model3D model { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            
            ModelImporter importer = new ModelImporter();

            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Beige));
            importer.DefaultMaterial = material;

            arm = new Model3DGroup();

            
           //   rest = importer.Load("Robot.obj");
          //  armbase = importer.Load("base3.obj");
             armbase = importer.Load("base4.obj");
           // armshoulder = importer.Load("shoulder2.obj");
            armshoulder = importer.Load("shoulder4.obj");
           //  armelbow = importer.Load("elbow2.obj");
            armelbow = importer.Load("elbow3.obj");

            
            //  arm.Children.Add(rest);
            arm.Children.Add(armbase);
            arm.Children.Add(armshoulder);
            arm.Children.Add(armelbow);


            foo.Content = arm;

            mybox = new BoxVisual3D();
            mybox.Height = 1;
            mybox.Width = 1;
            mybox.Length = 1;

            //   mybox.Center = new Point3D(0, 0, -5);
            //  mybox.Center = new Point3D(0, 1, 0);
            //  mybox.Center = new Point3D(0, 0, 0);
            //  mybox.Center = new Point3D(0, 29, 0);
              mybox.Center = new Point3D(0, 19, 5);

            m_helix_viewport.Children.Add(mybox);
            boxcontrol.DataContext = this;
            
             overall_grid.DataContext = this;
        }

        
        void move_shoulder(double angle)
        {

            
            //  RotateTransform3D shoulder_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
            RotateTransform3D shoulder_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
            // RotateTransform3D shoulder_transform = new RotateTransform3D(new Point3D(0, 1, 0), angle));


            shoulder_transform.CenterX = 0;
            shoulder_transform.CenterY = 0;
            // shoulder_transform.CenterY = 0;
            // shoulder_transform.CenterZ = -5;
            shoulder_transform.CenterZ = 0;

            armshoulder.Transform = shoulder_transform;

            RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
            elbow_transform.CenterX = 0;
            elbow_transform.CenterY = 0;
            //  elbow_transform.CenterZ = -5;
            elbow_transform.CenterZ = 0;

            armelbow.Transform = elbow_transform;
            
            //  move_elbow(elbow_angle);
          //  move_elbow2(elbow_angle);

        }

        
        void move_elbow(double angle)  //rotates elbow
        {
            
            var Group_3D = new Transform3DGroup();
            
            Group_3D.Children.Add(armelbow.Transform);

            
          //  Point3D origin = Group_3D.Transform(new Point3D(461, 1457, -157));
            Point3D origin = Group_3D.Transform(new Point3D(0, 0, 0));

            RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));

            elbow_transform.CenterX = origin.X;
            elbow_transform.CenterY = origin.Y;
            elbow_transform.CenterZ = origin.Z;

            Group_3D.Children.Add(elbow_transform);

            armelbow.Transform = Group_3D;
        }

        void move_elbow2(double angle)  //pivots elbow
        {
            
            var Group_3D = new Transform3DGroup();
            
            Group_3D.Children.Add(armelbow.Transform);

            
            //  Point3D origin = Group_3D.Transform(new Point3D(461, 1457, -157));
            Point3D origin = Group_3D.Transform(new Point3D(0, 19, 5));

            RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angle));

            elbow_transform.CenterX = origin.X;
            elbow_transform.CenterY = origin.Y;
            elbow_transform.CenterZ = origin.Z;

            Group_3D.Children.Add(elbow_transform);

            armelbow.Transform = Group_3D;
        }
    }
}
