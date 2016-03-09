using System.Windows;

namespace WpfApplication3
{
    public partial class MainWindow : Window
    {
        double m_turntable_angle;
        public double turntable_angle
        {
            get { return m_turntable_angle; }
            set
            {
                arm.moveTurntable(value);
                m_turntable_angle = value;
            }
        }



        double m_shoulder_angle;

        public double shoulder_angle
        {
            get { return m_shoulder_angle; }
            set
            {
                arm.moveShoulder(value);
                m_shoulder_angle = value;
            }
        }


        double m_elbow_angle;

        public double elbow_angle
        {
            get { return m_elbow_angle; }
            set
            {
                arm.moveElbow(value);
                m_elbow_angle = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}

