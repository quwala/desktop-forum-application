using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using client;
using System.Windows.Media;
using System;

namespace GUI
{
    
    public partial class ForumWindow : Window
    {
        private Iclient app;
        private string forumName;
        private string userName;
        private permission permission;

        public ForumWindow(Iclient app,string forumName)
        {
            InitializeComponent();
            subForumsListView.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));

            this.app = app;
            this.forumName = forumName;
            userName = "guest";
            permission = permission.GUEST;

            List<string> subForums = app.getSubForums(forumName, userName);
            subForums.Add("Test Sub Forum 1");
            subForumsListView.ItemsSource = subForums;

            logoutBtn.Visibility = Visibility.Hidden;
            sendPrivateMessageBtn.Visibility = Visibility.Hidden;

            addSubForumBtn.Visibility = Visibility.Hidden;
            this.ResizeMode = ResizeMode.CanMinimize;

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void forumsBtn_Click(object sender, RoutedEventArgs e)
        {
            GuiUtils.switchWindow(this, new MainWindow(app));
        }

        private void subForumsListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var subForum = (sender as ListView).SelectedItem;
            if (subForum != null)
            {
                GuiUtils.switchWindow(this, new SubForumWindow(forumName, (string)subForum,userName,app));
            }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow(forumName,userNameTxt.Text,passwordTxt.Password,app).ShowDialog();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string userNameInput = userNameTxt.Text;
            string passwordInput = passwordTxt.Password;
           
            loginStatus response = app.login(forumName, userNameInput, passwordInput);
            if (response == loginStatus.FALSE)
            {
                GuiUtils.displayError("Incorrect Username or Password");
                return;
            }
           

                this.userName = userNameInput;
                userNameTxt.Text = "";
                passwordTxt.Password = "";



                userNameLbl.Visibility = Visibility.Hidden;
                passwordLbl.Visibility = Visibility.Hidden; 
                passwordTxt.Visibility = Visibility.Hidden;
                userNameTxt.Visibility = Visibility.Hidden;
                loginBtn.Visibility = Visibility.Hidden;
                registerBtn.Visibility = Visibility.Hidden;



                this.permission = app.getUserPermissionsForForum(forumName, userName);

                loggedInTxt.Text = "Logged in as " + userName;

                if(permission == permission.ADMIN)
                {

                    logoutBtn.Visibility = Visibility.Visible;
                    sendPrivateMessageBtn.Visibility = Visibility.Visible;
                    addSubForumBtn.Visibility = Visibility.Visible;
                }
               else if (permission== permission.MEMBER)
                {
                    logoutBtn.Visibility = Visibility.Visible;
                    sendPrivateMessageBtn.Visibility = Visibility.Visible;
                }




            }
        

        

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.userName = "guest";
            loggedInTxt.Text = "Welcome, Guest ";

            userNameLbl.Visibility = Visibility.Visible;
            passwordLbl.Visibility = Visibility.Visible;
            passwordTxt.Visibility = Visibility.Visible;
            userNameTxt.Visibility = Visibility.Visible;
            loginBtn.Visibility = Visibility.Visible;
            registerBtn.Visibility = Visibility.Visible;


            if (permission == permission.ADMIN)
            {

                logoutBtn.Visibility = Visibility.Hidden;
                sendPrivateMessageBtn.Visibility = Visibility.Hidden;
                addSubForumBtn.Visibility = Visibility.Hidden;
            }
            else if (permission == permission.MEMBER)
            {
                logoutBtn.Visibility = Visibility.Hidden;
                sendPrivateMessageBtn.Visibility = Visibility.Hidden;
            }

            this.permission = permission.GUEST;


        }

        private void sendMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            new SendPrivateMessageWindow(forumName, userName, app).ShowDialog();
        }

        private void addSubForumBtn_Click(object sender, RoutedEventArgs e)
        {
            new NewSubForumWindow(forumName,userName,app).ShowDialog();
            List<string> subForums = app.getSubForums(forumName, userName);
            subForumsListView.ItemsSource = subForums;
        }
    }
}
