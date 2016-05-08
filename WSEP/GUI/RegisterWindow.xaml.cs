using client;
using System;

using System.Windows;

using System.Windows.Media;

namespace GUI
{
  
    public partial class RegisterWindow : Window
    {
        private Iclient cl;
        private string forumName;
        private string userName;

        public RegisterWindow(string forumName, string userName, string password, Iclient cl)
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));

            this.cl = cl;
            this.forumName = forumName;
            this.userName = userName;
            userNameTxt.Text = userName;
            passwordTxt.Password = password;
            this.ResizeMode = ResizeMode.CanMinimize;


        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            userName = userNameTxt.Text;
            string password = passwordTxt.Password;
            string confirmPassword = confirmPasswordTxt.Password;
            string email = emailTxt.Text;

            if (password != confirmPassword)
            {
                GuiUtils.displayError("Password & Confirmation do not match.");
                return;
            }
           

            string response = cl.registerMemberToForum(forumName,userName, password, email);
            if (response.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
              
                GuiUtils.displaySuccess("Successfully registered user "+userName+"to forum "+forumName);
                this.Close();
            }
            else
            {
                GuiUtils.displayError(response);
            }

        }

        
    }
}
