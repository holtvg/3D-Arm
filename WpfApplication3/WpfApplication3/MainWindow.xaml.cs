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
using UnityEngine;
using System.Net.Sockets;
using System.Net;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // static UdpClient client;
        //  static UdpClient client = new UdpClient(0);
        //static IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //static byte[] receiveBytes = client.Receive(ref RemoteIpEndPoint);
        //string returnData = Encoding.ASCII.GetString(receiveBytes);

        double angle2;
        

        Model3DGroup arm;

        BoxVisual3D mybox;

        Model3D rest;
        Model3D armbase;
        Model3D armturntable;
        Model3D armshoulder;
        Model3D armelbow;
        
        Model3D armhand;

        // public System.Windows.Media.Transform RobotUpperArm;
        public UnityEngine.Transform RobotUpperArm;  //attempt to reference and use unity engine in helix

        //  RobotUpperArm.Rotate(robotUpperArmSliderValue* upperArmTurnRate, 0, 0);
        private float robotUpperArmXRot;


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

        //property for the turntable movement
        double m_turntable_angle;
        public double turntable_angle
        {
            get { return m_turntable_angle; }
            set
            {
                move_turntable(value);
                m_turntable_angle = value;
            }
        }



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


        public Model3D our_Model { get; set; }
        public MainWindow()
        {
            InitializeComponent();
  

            // RobotUpperArm.Rotate(robotUpperArmSliderValue * upperArmTurnRate, 0, 0);


            ModelImporter importer = new ModelImporter();

            
            System.Windows.Media.Media3D.Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Beige));
            importer.DefaultMaterial = material;

            arm = new Model3DGroup();

            //load the model files

            //   rest = importer.Load("Robot.obj");
            //  armbase = importer.Load("base3.obj");
            armbase = importer.Load("base4.obj");
            // armturntable = importer.Load("shoulder2.obj");
           // armturntable = importer.Load("shoulder4.obj");
            armturntable = importer.Load("shoulder5.obj");
            armshoulder = importer.Load("shoulder3.obj");

            armelbow = importer.Load("elbow3.obj");
          //  armelbow = importer.Load("elbow3.obj");
            
            armhand = importer.Load("newarmhand2.obj");


            

            //add the pieces to the 3d model
            //  arm.Children.Add(rest);
            arm.Children.Add(armbase);
            arm.Children.Add(armturntable);
            arm.Children.Add(armshoulder);
            arm.Children.Add(armelbow);
            
            //  arm.Children.Add(armhand);


            // displahy model
            foo.Content = arm;

            //creates a small cube for determining 3d coordinates
            mybox = new BoxVisual3D();
            mybox.Height = 1;
            mybox.Width = 1;
            mybox.Length = 1;

            //   mybox.Center = new Point3D(0, 0, -5);
            //  mybox.Center = new Point3D(0, 1, 0);
            //  mybox.Center = new Point3D(0, 0, 0);
            //  mybox.Center = new Point3D(0, 29, 0);
            //  mybox.Center = new Point3D(0, 19, 5);
            // mybox.Center = new Point3D(0, 9, 1);
            mybox.Center = new Point3D(7, 0, 0);

            m_helix_viewport.Children.Add(mybox);
            boxcontrol.DataContext = this;
            
            overall_grid.DataContext = this;

            connect();  //udp connection
        }


        void move_turntable(double angle)  //rotate turntable
        {

            //rotate the object by "angle", the vector describes the axis
            
            //  RotateTransform3D turntable_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
            RotateTransform3D turntable_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
            // RotateTransform3D turntabble_transform = new RotateTransform3D(new Point3D(0, 1, 0), angle));
            Console.WriteLine("turntable:" + angle);
            // System.Diagnostics.Debug.WriteLine(angle);

            //tell where the point of rotation is

            turntable_transform.CenterX = 0;
            turntable_transform.CenterY = 0;
            // turntable_transform.CenterY = 0;
            // turntable_transform.CenterZ = -5;
            turntable_transform.CenterZ = 0;

            //apply transformation
            
            armturntable.Transform = turntable_transform;

          //  RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
          //  elbow_transform.CenterX = 0;
          //  elbow_transform.CenterY = 0;
            //  elbow_transform.CenterZ = -5;
          //  elbow_transform.CenterZ = 0;

          //  armelbow.Transform = elbow_transform;

              move_shoulder(elbow_angle);
            //  move_elbow(elbow_angle);

           angle2 = angle;
            

        }

        
        void move_shoulder(double angle)  //pivots shoulder
        {
            //new group of transformations, the group will add movements
            var Group_3D = new Transform3DGroup();
            
            Group_3D.Children.Add(armturntable.Transform);

            //find out where our old point is
            //  Point3D origin = Group_3D.Transform(new Point3D(461, 1457, -157));
            Point3D origin = Group_3D.Transform(new Point3D(0, 9, 1));

            //create new transformation
            
            //   RotateTransform3D shoulder_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angle));
            RotateTransform3D shoulder_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(Math.Cos(angle2), 0, Math.Sin(angle2)), angle));  //issue is here i believe
            //  RotateTransform3D shoulder_transform2 = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), angle));
            Console.WriteLine("shoulder:"+angle);
            // System.Diagnostics.Debug.WriteLine(angle);

            shoulder_transform.CenterX = origin.X;
            shoulder_transform.CenterY = origin.Y;
            shoulder_transform.CenterZ = origin.Z;

            //shoulder_transform2.CenterX = origin.X;
            //shoulder_transform2.CenterY = origin.Y;
            //shoulder_transform2.CenterZ = origin.Z;

            //add it to the transformation group, turntable transform will be applied to shoulder as well
            
            Group_3D.Children.Add(shoulder_transform);
          //  Group_3D.Children.Add(shoulder_transform2);

            armshoulder.Transform = Group_3D;

            move_elbow(elbow_angle);  //move elbow with shoulder
        }

        void move_elbow(double angle)  //pivots elbow
        {
            
            var Group_3D = new Transform3DGroup();
            //  Group_3D.Children.Add(armelbow.Transform);
            Group_3D.Children.Add(armshoulder.Transform);
          

            
            //  Point3D origin = Group_3D.Transform(new Point3D(461, 1457, -157));
            Point3D origin = Group_3D.Transform(new Point3D(0, 19, 5));
            //  Point3D.Transform = new TranslateTransform3D(new Vector3D(0, 19, 5));
            Console.WriteLine("elbow:"+angle);


            
            //  RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angle));
             RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(Math.Cos(angle2), 0, Math.Sin(angle2)), angle)); //same issue here with pivoting
            // RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, -1), angle));
            //  RotateTransform3D elbow_transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(robotUpperArmXRot, RobotUpperArm.eulerAngles.y, RobotUpperArm.eulerAngles.z), angle));
            //  RotateTransform3D elbow_transform = new RotateTransform3D(new Vector3D(robotUpperArmXRot, RobotUpperArm.eulerAngles.y, RobotUpperArm.eulerAngles.z));

            Console.WriteLine(angle2);
            Console.WriteLine(Math.Cos(angle2));
            

            elbow_transform.CenterX = origin.X;
            elbow_transform.CenterY = origin.Y;
            elbow_transform.CenterZ = origin.Z;

            Group_3D.Children.Add(elbow_transform);

            armelbow.Transform = Group_3D;
          //  armelbow.Transform = Group_3D;

         //   move_wrist(wrist_angle);
        }

        void move_wrist(double angle)  //moves wrist
        {
          
        }

        void move_fingers(double angle)  //moves fingers
        {
            
        }

        void connect()   //attempt at receiving commands through udp, since it's going to be integrated into terminals
          // it's probably not needed anymore
        {
            UdpClient client = new UdpClient(8080);

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            string returnData = null;
            if (client.Available > 0)
            {
                Byte[] receiveBytes = client.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine(returnData);
                //  Console.WriteLine("shit");

                if (returnData[0] == '1')
                {
                    move_turntable(double.Parse(returnData.Substring(1, 2)));
                }
                if (returnData[0] == '2')
                {
                    move_shoulder(double.Parse(returnData));
                }
                if (returnData[0] == '3')
                {
                    move_elbow(double.Parse(returnData));
                }
                if (returnData[0] == '4')
                {
                    move_wrist(double.Parse(returnData));
                }
                else
                {
                    Console.WriteLine("invalid data received");
                }
            }
        }
        }
    }

