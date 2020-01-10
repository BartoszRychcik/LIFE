using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Projekt1
{
    public partial class MainWindow : Window
    {
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public class data
        {
            public int age;
            public bool active;

            public data()
            {
                age = 0;
                active = false;
            }

            public void clear()
            {
                age = 0;
                active = false;
            }

            override public string ToString()
            {
                return (active?"1":"0")+" "+age.ToString()+"\n";
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public class shape
        {
            public string name;
            public int w;
            public int h;
            public bool[,] t;

            public shape()
            {
                name = "";
                w = 0;
                h = 0;
                t = null;
            }

            public shape(string n,int wi,int he,bool[,] s)
            {
                name = n;
                w = wi;
                h = he;
                t = new bool[he,wi];
                for (int i = 0; i < he; i++)
                    for (int j = 0; j < wi; j++)
                        t[i, j] = s[i,j];
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public data[,] tab,pom;
        int x, y,death,alive,maxage,p,d;
        List<shape> shapes = new List<shape>();
        List<string> shapesName = new List<string>();

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public MainWindow(int x, int y)
        {
            InitializeComponent();
            shapes.Add(new shape("1.klocek", 2, 2, new bool[2, 2] {{ true, true },{ true, true }}));
            shapes.Add(new shape("2.bochenek", 4, 4, new bool[4, 4] { {false,true,true,false }, {true,false,false,true }, { true,false,true,false }, {false, true,false ,false } }));
            shapes.Add(new shape("3.koniczyna", 3, 3, new bool[3, 3] { { false, true, false }, { true, false, true }, { false, true, false } }));
            shapes.Add(new shape("4.staw", 4, 4, new bool[4, 4] { { false, true, true, false }, { true, false, false, true }, { true, false, false, true }, { false, true, true, false } }));
            shapes.Add(new shape("5.kryształ", 3, 4, new bool[4, 3] { {false,true,false }, {true,false,true }, { true,false, true }, { false,true,false } }));
            shapes.Add(new shape("6.łódź", 3, 3, new bool[3, 3] { { true,true,false}, { true, false,true }, { false,true, false } }));
            shapes.Add(new shape("7.żabka", 4, 4, new bool[4, 4] { { false, true, true, false }, { true, false, false, false }, { false, false, false, true }, { false, true, true, false } }));
            shapes.Add(new shape("8.szybowiec", 3, 3, new bool[3, 3] { { true, true, true }, { true, false, false }, { false, true, false } }));

            foreach (shape s in shapes)
                shapesName.Add(s.name);

            this.DataContext = shapesName;
            this.x = x;
            this.y = y;
            this.p = 0;
            this.d = 0;
            this.death = 4;
            this.alive = 3;
            this.maxage = 0;
            create_table();    
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void clear_table(data[,] k)
        {
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                    k[i, j].clear();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void copy_table(data[,] a,data[,] b)
        {
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                    {
                    a[i, j].active = b[i, j].active;
                    a[i, j].age = b[i, j].age;
                    }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void create_table()
        {
            plansza.Children.Clear();
            plansza.ColumnDefinitions.Clear();
            plansza.RowDefinitions.Clear();

            tab = new data[y, x];
            pom = new data[y, x];

            for (int i = 0; i < x; i++)
                plansza.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < y; i++)
                plansza.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                {
                    Rectangle button = new Rectangle();
                    button.Fill = Menu_button.Background;
                    button.Width = plansza.Width/x-1.0;
                    button.Height = plansza.Height/y-1.0;
                    tab[i, j] = new data();
                    pom[i, j] = new data();
                    button.MouseDown += Button_MouseDown;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    plansza.Children.Add(button);
                }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle b = sender as Rectangle;
            tab[Grid.GetRow(b), Grid.GetColumn(b)].active = tab[Grid.GetRow(b), Grid.GetColumn(b)].active ? false : true;
            tab[Grid.GetRow(b), Grid.GetColumn(b)].age = tab[Grid.GetRow(b), Grid.GetColumn(b)].active? 1:0;
            update_view();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Button_Click_Menu(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void update_view()
        {
            foreach (Rectangle b in plansza.Children)
            {
                if(combobox.SelectedIndex==0)
                    b.Fill = tab[Grid.GetRow(b), Grid.GetColumn(b)].active ? Brushes.Green : Menu_button.Background;
                else if(combobox.SelectedIndex==1)
                    b.Fill = tab[Grid.GetRow(b), Grid.GetColumn(b)].active ? tab[Grid.GetRow(b), Grid.GetColumn(b)].age==1?Brushes.LightGreen :Brushes.Green : Menu_button.Background;
                else if (combobox.SelectedIndex == 2)
                {
                    if (tab[Grid.GetRow(b), Grid.GetColumn(b)].active && (friends(Grid.GetRow(b), Grid.GetColumn(b)) >= death || friends(Grid.GetRow(b), Grid.GetColumn(b)) < alive - 1 || (pom[Grid.GetRow(b), Grid.GetColumn(b)].age + 1 >= maxage && maxage != 0)))
                        b.Fill = Brushes.Red;
                    else
                        b.Fill = tab[Grid.GetRow(b), Grid.GetColumn(b)].active ? Brushes.Green : Menu_button.Background;
                }
            }

            if (listbox1!=null && listbox1.SelectedItem != null)
            {
                foreach(shape s in shapes)
                {
                    if (listbox1.SelectedItem.ToString() == s.name)
                    {
                        for(int i=0;i<s.h;i++)
                            for(int j = 0; j < s.w; j++)
                            {
                                Rectangle b = (Rectangle)plansza.Children[((i+d)*x)+j+p];
                                if (s.t[i, j])
                                    b.Fill = Brushes.Black;
                            }
                    }
                }
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                    tab[i, j].clear();
            update_view();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        public bool Is_Number(string a)
        {
            if (a==null || a.Length == 0)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] < '0' || a[i] > '9')
                    return false;
            }
            return true;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Combobox_Loaded(object sender, RoutedEventArgs e)
        {
            combobox.SelectedIndex = 0;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void TextChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;
                string x="";

                switch (tb.Name)
                {
                    case "Text_death":
                        {
                            x = Text_death.Text;
                            if (Is_Number(x))
                                death = Int32.Parse(x);
                            break;
                        }
                    case "Text_alive":
                        {
                            x = Text_alive.Text;
                            if (Is_Number(x))
                                alive = Int32.Parse(x);
                            break;
                        }
                    case "Text_maxage":
                        {
                            x = Text_maxage.Text;
                            if (Is_Number(x))
                                maxage = Int32.Parse(x);
                            break;
                        }
                    default:
                        break;
                }

                 
                if(!Is_Number(Text_death.Text)||!Is_Number(Text_alive.Text)||!Is_Number(Text_maxage.Text))
                {
                    Next_Button.IsEnabled = false;
                    Next10_Button.IsEnabled = false;
                }
                else
                {
                    Next_Button.IsEnabled = true;
                    Next10_Button.IsEnabled = true;
                }
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int friends(int i, int j)
        {
            int count = 0;

            if (i != 0)
            {
                if (j != 0 && tab[i - 1, j - 1].active) count++;
                if (tab[i - 1, j].active) count++;
                if (j != x - 1 && tab[i - 1, j + 1].active) count++;
            }

            if (i != y - 1)
            {
                if (j != 0 && tab[i + 1, j - 1].active) count++;
                if (tab[i + 1, j].active) count++;
                if (j != x - 1 && tab[i + 1, j + 1].active) count++;
            }

            if (j != 0 && tab[i, j - 1].active) count++;

            if (j != x - 1 && tab[i, j + 1].active) count++;

            return count;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Listbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update_view();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Listbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (listbox1 != null && listbox1.SelectedItem != null)
            {
                foreach (shape sh in shapes)
                {
                    if (listbox1.SelectedItem.ToString() == sh.name)
                    {
                        if (e.Key == Key.Enter)
                        {
                            for (int i = 0; i < sh.h; i++)
                                for (int j = 0; j < sh.w; j++)
                                {
                                    if (sh.t[i, j])
                                    {
                                        tab[i + d, j + p].active = true;
                                        tab[i + d, j + p].age = 1;
                                    }
                                    else
                                    {
                                        tab[i + d, j + p].active = false;
                                        tab[i + d, j + p].age = 0;
                                    }
                                }
                        }
                        else if (e.Key == Key.Escape)
                        {
                            listbox1.SelectedItem = null;
                            break;
                        }
                        else if (e.Key == Key.A)
                        {
                            if (p != 0) p--;
                        }
                        else if (e.Key == Key.D)
                        {
                            if (p != x - sh.w) p++;
                        }
                        else if (e.Key == Key.W)
                        {
                            if (d != 0) d--;
                        }
                        else if (e.Key == Key.S)
                        {
                            if (d != y - sh.h) d++;
                        }
                        update_view();
                    }
                }
            }
        }

 //--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            int count;
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    count = friends(i, j);

                    if (!tab[i, j].active)
                    {
                        if (count == alive)//narodziny
                        {
                            pom[i, j].active = true;
                            pom[i, j].age = 1;
                        }
                        else
                        {
                            pom[i, j].active = false;
                            pom[i, j].age = 0;
                        }

                    }
                    else
                    {
                        if (count >= death || count < alive - 1 || (pom[i,j].age+1>=maxage && maxage!=0))//smierc
                        {
                            pom[i, j].active = false;
                            pom[i, j].age = 0;
                        }
                        else
                        {
                            pom[i, j].active = true;
                            pom[i, j].age = tab[i, j].age + 1 ;
                        }
                    }
                }
            }
            copy_table(tab, pom);
            update_view();
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p = d = 0;
            update_view();
        }

 //--------------------------------------------------------------------------------------------------------------------------------------------------------------        
        private void Button_Click_Next10(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Button_Click_Next(sender, e); 
            }    
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Stream myStream;
            StreamWriter mystreamWriter;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text Document (.txt)|*.txt";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                if ((myStream = dialog.OpenFile()) != null)
                {
                    mystreamWriter = new StreamWriter(myStream);

                    mystreamWriter.Write(x.ToString()+"\n"+y.ToString()+"\n");

                    for (int i = 0; i < y; i++)
                        for (int j = 0; j < x; j++)
                            mystreamWriter.Write(tab[i,j].ToString());

                    mystreamWriter.Close();
                    myStream.Close();
                }
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            StreamReader mystreamReader;
            OpenFileDialog dialog = new OpenFileDialog();
            string line;
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text Document (.txt)|*.txt";

            if (dialog.ShowDialog() == true)
            {
                mystreamReader = new StreamReader(dialog.FileName);

                try
                {
                    x = Int32.Parse(mystreamReader.ReadLine());
                    y = Int32.Parse(mystreamReader.ReadLine());

                    create_table();

                    for (int i = 0; i < y; i++)
                        for (int j = 0; j < x; j++)
                        {
                            line = mystreamReader.ReadLine();
                            tab[i, j].active = line[0] == '0' ? false : true;
                            line = line.Substring(2);
                            tab[i, j].age = Int32.Parse(line);
                        }

                    update_view();
                }
                catch
                {
                    info.Text = "Struktura pliku jest nieprawidłowa.";
                }  
            }
        }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
