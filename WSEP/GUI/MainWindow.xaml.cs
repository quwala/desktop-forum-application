
using client;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;


namespace GUI
{
   
    public partial class MainWindow : Window
    {
        private Iclient app;
        private string superAdmin;

        public MainWindow(Iclient cl)
        {
            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));

            InitializeComponent();
            superAdmin = "";

            forumsListView.Background= new SolidColorBrush(Color.FromRgb(83, 83, 83));
            app = cl;

            

            List<string> forums = app.getForums();
            forumsListView.ItemsSource = forums;
            this.ResizeMode = ResizeMode.NoResize;

        }

        private void forumsListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var forum = (sender as ListView).SelectedItem;
            if (forum != null)
            {
                Window newOne = new ForumWindow(app,(string)forum);
                GuiUtils.switchWindow(this, newOne);
            }
        }

        internal void setSuperAdmin()
        {
            superAdmin="true";
        }

        private void superAdminBtn_Click(object sender, RoutedEventArgs e)
        {

            new SuperAdminLoginWindow(app, this).ShowDialog();
        }

        private void newForumBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!superAdmin.Equals(""))
                GuiUtils.switchWindow(this, new NewForumWindow(app));
            else
                GuiUtils.displayError("This action is only available to Super Admins");
        }
    }
}
