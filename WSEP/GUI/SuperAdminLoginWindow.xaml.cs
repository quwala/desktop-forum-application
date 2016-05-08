using client;

using System.Windows;

using System.Windows.Media;


namespace GUI
{
   
    public partial class SuperAdminLoginWindow : Window
    {
        private Iclient cl;
        private MainWindow parent;

        public SuperAdminLoginWindow(Iclient cl, MainWindow window)
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));
            this.parent = window;
            this.cl = cl;
            //this.Visibility = System.Windows.Visibility.Hidden;
            this.ResizeMode = ResizeMode.CanMinimize;
            this.usernameTxt.Focus();

        }


        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {


            if (usernameTxt.Text.Equals("superAdmin") && passwordTxt.Password.Equals("superAdmin"))
            {
                GuiUtils.displaySuccess("Successfully logged in as Super Admin");
                parent.setSuperAdmin();
                this.Close();
                //GuiUtils.switchWindow(this, new MainWindow(cl));
            }
            else
                GuiUtils.displayError("Wrong Credentials");

        }

        
    }
}
