using System;
using System.Windows;

namespace Projekt1
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Button_Click_Start_Game(object sender, RoutedEventArgs e)
        {
            int x=0,y=0;
            try
            {
                x = Int32.Parse(szerokosc.Text);
                y = Int32.Parse(wysokosc.Text);
                MainWindow window = new MainWindow(x, y);
                window.Show();
                this.Close();
            }
            catch
            {
                info.Text = "Podaj poprawne wartości liczbowe.";
            } 
        }

        private void Button_Click_Quit_Game(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
