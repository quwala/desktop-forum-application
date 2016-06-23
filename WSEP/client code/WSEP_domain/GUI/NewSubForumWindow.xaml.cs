using client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;

using System.Windows.Media;


namespace GUI
{

    public partial class NewSubForumWindow : Window
    {
        public static int DEFAULT_DAYS = 10;
        private Iclient cl;
        private ObservableCollection<string> availableMods;
        private ObservableCollection<string> finalMods;
        private string userName;
        private string forumName;
        public NewSubForumWindow(string forumName,string userName, Iclient cl)
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));
            availableModsListView.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            finalModsListView.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));

            this.cl = cl;
            this.userName = userName;
            this.forumName = forumName;

            List<string> users = cl.getUsersInForum(forumName, userName);
            availableMods = new ObservableCollection<string>(users);

            finalMods = new ObservableCollection<string>();

            availableModsListView.ItemsSource = availableMods;
            finalModsListView.ItemsSource = finalMods;
            this.ResizeMode = ResizeMode.CanMinimize;

        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUsers = availableModsListView.SelectedItems;
            List<string> temp = new List<string>();
            foreach (string item in selectedUsers)
            {
                temp.Add(item);
            }

            foreach (string selectedUser in temp)
            {
                availableMods.Remove(selectedUser);
                finalMods.Add(selectedUser);

            }
        }



        private void doneBtn_Click(object sender, RoutedEventArgs e)
        {
            string subForumName = nameTxt.Text;
            string response = "";
            List<Tuple<string, string, int>> mods = new List<Tuple<string, string, int>>();

            foreach (string mod in finalMods)
                mods.Add(new Tuple<string, string, int>(mod, userName, DEFAULT_DAYS));

            response = cl.addSubForum(forumName,subForumName,mods, userName);
            if (response.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                GuiUtils.displaySuccess("Sub Forum " + subForumName + " added successfully");
                this.Close();
            }
            else
                GuiUtils.displayError(response);

        }


    }
}
