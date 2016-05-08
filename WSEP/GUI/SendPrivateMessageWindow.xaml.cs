using client;
using System;

using System.Windows;

using System.Windows.Media;


namespace GUI
{
    public partial class SendPrivateMessageWindow : Window
    {
        private Iclient cl;
        private string forumName;
        private string userName;

        public SendPrivateMessageWindow(string forumName, string userName, Iclient cl)
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));
            contentTxt.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            recepientTxt.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            titleTxt.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));

            this.cl = cl;
            this.forumName = forumName;
            this.userName = userName;
            this.ResizeMode = ResizeMode.NoResize;

        }


        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            string recepient = recepientTxt.Text;
            string title = titleTxt.Text;
            string content = contentTxt.Text;

          

            string response = cl.sendPM(forumName, userName, recepient,
                "By: "+userName+"\n"+ "To: "+recepient+ "\n" + "Title: "+title+"\n"+content);
            if (response.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                GuiUtils.displaySuccess("Message Sent.");
                this.Close();
            }

            else
                GuiUtils.displayError(response);
            
        }
    }
}
