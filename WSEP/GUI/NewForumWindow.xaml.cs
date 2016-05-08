using client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;

using System.Windows.Media;


namespace GUI
{

    public partial class NewForumWindow : Window
    {
        private Iclient cl;
        private ObservableCollection<string> availableAdmins;
        private ObservableCollection<string> finalAdmins;

        public NewForumWindow(Iclient cl)
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));
            availableAdminsListView.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            finalAdminsListView.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));

            this.cl = cl;

            List<string> users = cl.getAllUsers("superAdmin");
            availableAdmins = new ObservableCollection<string>(users);

            finalAdmins = new ObservableCollection<string>();

            availableAdminsListView.ItemsSource = availableAdmins;
            finalAdminsListView.ItemsSource = finalAdmins;
            this.ResizeMode = ResizeMode.CanMinimize;

        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUsers = availableAdminsListView.SelectedItems;
            List<string> temp = new List<string>();
            foreach (string item in selectedUsers)
            {
                temp.Add(item);
            }

            foreach (string selectedUser in temp)
            {
                availableAdmins.Remove(selectedUser);
                finalAdmins.Add(selectedUser);

            }
        }



        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            string forumName = nameTxt.Text;
            string response = "";


            response = cl.addForum(forumName, "superAdmin");
            if (response.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (string admin in finalAdmins)
                    cl.assignAdmin(forumName, admin, "superAdmin");
                GuiUtils.displaySuccess("Forum " + forumName + " added successfully");
                GuiUtils.switchWindow(this, new MainWindow(cl));
            }
            else
                GuiUtils.displayError(response);

        }


    }
}
