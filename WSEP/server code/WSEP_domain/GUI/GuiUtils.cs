using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI
{
    public static class GuiUtils
    {
        public static void switchWindow(Window oldOne, Window newOne)
        {
            newOne.Left = oldOne.Left;
            newOne.Top = oldOne.Top;
            newOne.Show();
            oldOne.Close();
        }

        public static void show(Window oldOne, Window newOne)
        {
            newOne.Left = oldOne.Left;
            newOne.Top = oldOne.Top;
            newOne.Show();
        }

       


        public static void setBackground(ContentControl e)
        {
            e.Background = new SolidColorBrush(Color.FromRgb(69, 70, 73));
            //e.Background = System.Windows.SystemColors.MenuHighlightBrush;
        }

        internal static void displayError(string v)
        {
            MessageBox.Show(v,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        internal static void displaySuccess(string v)
        {
            MessageBox.Show(v, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
