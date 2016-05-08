using client;
using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;


namespace GUI
{
   
    public partial class SubForumWindow : Window
    {
        private Iclient cl;
        private string forumName;
        private string subForumName;
        private string userName;
        private List<Tuple<string, DateTime, int>> threads;
        public SubForumWindow(string forumName, string subForumName, string userName,Iclient cl)
        {
            InitializeComponent();
            this.Title = "Clingr - " +forumName+" - "+subForumName;
            this.cl = cl;
            this.forumName = forumName;
            this.subForumName = subForumName;
            this.userName = userName;

            threadsListView.Background = new SolidColorBrush(Color.FromRgb(83, 83, 83));
            this.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));

            threads=cl.getThreads(forumName, subForumName, userName);

            List<string> threadsStrings = new List<string>();

            foreach(Tuple<string, DateTime, int> thread in threads)
            {
                threadsStrings.Add(thread.Item2.ToShortDateString() + ": " + thread.Item1);
            }

            threadsListView.ItemsSource = threadsStrings;
            this.ResizeMode = ResizeMode.NoResize;


        }

        private void subForumsBtn_Click(object sender, RoutedEventArgs e)
        {
            GuiUtils.switchWindow(this, new ForumWindow(cl,forumName));
        }

        private void createThreadBtn_Click(object sender, RoutedEventArgs e)
        {
           //implement
        }

        

        private void threadsListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var thread = (sender as ListView).SelectedItem;
            int threadId=0;
            if (thread != null)
            {
                foreach (Tuple<string, DateTime, int> t in threads)
                {
                    if (t.Item1.Equals((string)thread))
                        threadId = t.Item3;    
                }
                GuiUtils.switchWindow(this,new ThreadWindow(forumName, subForumName,userName, threadId, cl));
            }
        }
    }
}
