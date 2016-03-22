using HelixToolkit.Wpf;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using UnityEngine;

namespace WpfApplication3
{
    public partial class _3DArm : UserControl
    {
        double angle2;

        Model3DGroup arm;

        BoxVisual3D mybox;

        Model3D armBase;
        Model3D turntable;
        Model3D shoulder;
        Model3D elbow;
        Model3D hand;

        private Transform3DGroup transformGroup = new Transform3DGroup();

        double _turntableAngle;
        public double TurntableAngle
        {
            get { return _turntableAngle; }
            set
            {
                moveTurntable(value);
                _turntableAngle = value;
            }
        }

        double _shoulderAngle;
        public double ShoulderAngle
        {
            get { return _shoulderAngle; }
            set
            {
                moveShoulder(value);
                _shoulderAngle = value;
            }
        }


        double _elbowAngle;

        public double ElbowAngle
        {
            get { return _elbowAngle; }
            set
            {
                moveElbow(value);
                _elbowAngle = value;
            }
        }

        private ModelImporter importer = new ModelImporter() { DefaultMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Beige)) };

        public Model3DGroup BaseObj { get { return importer.Load("Resources/base4.obj"); } }
        public Model3DGroup TurntableObj { get { return importer.Load("Resources/shoulder5.obj"); } }
        public Model3DGroup ShoulderObj { get { return importer.Load("Resources/shoulder3.obj"); } }
        public Model3DGroup ElbowObj { get { return importer.Load("Resources/elbow3.obj"); } }
        public Model3DGroup HandObj { get { return importer.Load("Resources/newarmhand2.obj"); } }

        public _3DArm()
        {
            InitializeComponent();

            arm = new Model3DGroup();

            //load the model files
            armBase = importer.Load("Resources/base4.obj");
            turntable = importer.Load("Resources/shoulder5.obj");
            shoulder = importer.Load("Resources/shoulder3.obj");
            elbow = importer.Load("Resources/elbow3.obj");
            hand = importer.Load("Resources/newarmhand2.obj");

            //add the pieces to the 3d model
            arm.Children.Add(armBase);
            arm.Children.Add(turntable);
            arm.Children.Add(shoulder);
            arm.Children.Add(elbow);

            // display model
            foo.Content = arm;

            //creates a small cube for determining 3d coordinates
            mybox = new BoxVisual3D();
            mybox.Height = 1;
            mybox.Width = 1;
            mybox.Length = 1;

            // mybox.Center = new Point3D(0, 0, 0);
           // mybox.Center = new Point3D(0, 19, 5);
            mybox.Center = new Point3D(0, 0, 20);

           // m_helix_viewport.Children.Add(mybox);
            arm.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90));

            DataContext = this;
        }


        public void moveTurntable(double angle)  //rotate turntable
        {
            //apply transformation
            turntable.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));

            moveShoulder(ElbowAngle);

            Console.WriteLine("turntable:" + angle);

            angle2 = angle;
        }


        public void moveShoulder(double angle)  //pivots shoulder
        {
            //new group of transformations, the group will add movements
            var Group_3D = new Transform3DGroup();

            Group_3D.Children.Add(turntable.Transform);

            Point3D origin = Group_3D.Transform(new Point3D(0, 9, 1));


            double angle2_rad = Math.PI / 180 * angle2;


            double x;
            double y;
            double z;

            if (angle2 >= 0)
            {
                x = Math.Cos(-angle2_rad);
                y = 0;
                z= Math.Sin(-angle2_rad);
            }
            else
            {
                x = Math.Cos(-angle2_rad);
                y = 0;
                z = Math.Sin(-angle2_rad);
            }


            RotateTransform3D shoulder_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(x, y, z), angle));


            Console.WriteLine("shoulder:" + angle);


            shoulder_transform.CenterX = origin.X;
            shoulder_transform.CenterY = origin.Y;
            shoulder_transform.CenterZ = origin.Z;

            //add it to the transformation group, turntable transform will be applied to shoulder as well
            Group_3D.Children.Add(shoulder_transform);


            shoulder.Transform = Group_3D;

            moveElbow(ElbowAngle);  //move elbow with shoulder
        }

        public void moveElbow(double angle)  //pivots elbow
        {
            var Group_3D = new Transform3DGroup();
            Group_3D.Children.Add(shoulder.Transform);

            Point3D origin = Group_3D.Transform(new Point3D(0, 19, 5));
           // Point3D origin = Group_3D.Transform(new Point3D(0, 0, 0));

            Console.WriteLine("elbow:" + angle);

            double angle2_rad = Math.PI / 180 * angle2;

            double x;
            double y;
            double z;

            if (angle2 >= 0)
            {
                x = Math.Cos(-angle2_rad);
                y = 0;
                z = Math.Sin(-angle2_rad);
            }
            else
            {
                x = Math.Cos(-angle2_rad);
                y = 0;
                z = Math.Sin(-angle2_rad);
            }

            RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(x, y, z), angle));

            Console.WriteLine(angle2);
            Console.WriteLine(Math.Cos(angle2));


            elbow_transform.CenterX = origin.X;
            elbow_transform.CenterY = origin.Y;
            elbow_transform.CenterZ = origin.Z;

            Group_3D.Children.Add(elbow_transform);

            elbow.Transform = Group_3D;
        }

        public void move_wrist(double angle)  //moves wrist
        {

        }

        public void move_fingers(double angle)  //moves fingers
        {

        }
    }
}


