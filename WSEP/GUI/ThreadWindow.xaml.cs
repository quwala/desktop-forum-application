using client;
using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;


namespace GUI
{
   
    public partial class ThreadWindow : Window
    {
        private Iclient cl;
        private string forumName;
        private string subForumName;
        private string userName;
        private int threadId;

        private List<Tuple<string, string, DateTime, int, int, string, DateTime>> posts;


        public ThreadWindow(string forumName, string subForumName,string userName, int threadId ,Iclient cl)
        {
            InitializeComponent();
            this.Title = "Clingr - " + forumName + " - " + subForumName + " - ";
            this.cl = cl;
            this.forumName = forumName;
            this.subForumName = subForumName;
            this.userName = userName;
            this.threadId = threadId;

            postsTV.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));

            subForumBtn.Content = subForumName;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void postsTV_Loaded(object sender, RoutedEventArgs e)
        {
            posts = cl.getThread(forumName,subForumName,threadId,userName);

            var tree = sender as TreeView;
          
            //implement
        }

 

        private void addReplyBtn_Click(object sender, RoutedEventArgs e)
        {
            
            //implement
          
        }
        private void deletePostBtn_Click(object sender, RoutedEventArgs e)
        {

            //implement

        }



        private void subForumBtn_Click(object sender, RoutedEventArgs e)
        {
            GuiUtils.switchWindow(this, new SubForumWindow(forumName, subForumName,userName,cl));
        }
    }
}
