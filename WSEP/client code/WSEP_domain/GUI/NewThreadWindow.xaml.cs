using client;
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
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AddThreadWindow.xaml
    /// </summary>
    public partial class NewThreadWindow : Window
    {
        private string forumName;
        private string subForumName;
        private string userName;
        private Iclient cl;

        public NewThreadWindow(string forumName, string subForumName, string userName, Iclient cl)
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));

            this.userName = userName;
            this.cl = cl;
            this.forumName = forumName;
            this.subForumName = subForumName;
            this.ResizeMode = ResizeMode.CanMinimize;

        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (titleText.Text.Equals("") && contentText.Text.Equals(""))
            {
                GuiUtils.displayError("Must fill the title/content fields");
                return;
            }

            


           string  response = cl.writePost(forumName, subForumName, 0, userName, titleText.Text, contentText.Text);

            if (response.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                GuiUtils.displaySuccess("Thread created successfully");
                this.Close();
            }
            else
                GuiUtils.displayError(response);

        }
    }
}
