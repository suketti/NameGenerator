using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nevgenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> lastNames = new ObservableCollection<string>();
        private ObservableCollection<string> firstNames = new ObservableCollection<string>();
        private ObservableCollection<string> names = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            rbSelectionOne.IsChecked = true;
        }

        private void LoadNames(ObservableCollection<string> names, ListBox lbNames)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                foreach (var name in File.ReadAllLines(ofd.FileName).ToList())
                {
                    names.Add(name);
                }
                lbNames.ItemsSource = names;
            }
        }

        private void GenerateNames(ObservableCollection<string> names, int count, bool middleName)
        {
            Random rnd = new Random();
            if (middleName)
            {
                for (int i = 0; i < count; i++)
                {
                    names.Add($"{lastNames[rnd.Next(0, lastNames.Count)]} {firstNames[rnd.Next(0, firstNames.Count)]} {firstNames[rnd.Next(0, firstNames.Count)]}");
                }
            } 
            else
            {
                for (int i = 0; i < count; i++)
                {
                    names.Add($"{lastNames[rnd.Next(0, lastNames.Count)]} {firstNames[rnd.Next(0, firstNames.Count)]}");
                }
            }
            textBlockGeneratedNameCount.Text = $"Generált nevek száma: {names.Count()}";
            
        }
        private void JumpToTheEndOfNameList()
        {
            lbLastNames.Items.MoveCurrentToLast();
            lbLastNames.ScrollIntoView(lbLastNames.Items.CurrentItem);
            lbFirstNames.Items.MoveCurrentToLast();
            lbFirstNames.ScrollIntoView(lbFirstNames.Items.CurrentItem);
        }

 

        private void btnLoadLastNames_Click(object sender, RoutedEventArgs e)
        {
            LoadNames(lastNames, lbLastNames);
            int lastNameCount = lastNames.Count;
            lbl_LastNameCounter.Content = lastNameCount;
            sliderNameCount.Maximum = lastNameCount;
            lblMaxNameCount.Content = lastNameCount;
            JumpToTheEndOfNameList();
        }

        private void btnLoadFirstNames_Click(object sender, RoutedEventArgs e)
        {
            LoadNames(firstNames, lbFirstNames);
            lbl_FirstNameCounter.Content = firstNames.Count;
            JumpToTheEndOfNameList();
        }

        private void btnGenerateNames_Click(object sender, RoutedEventArgs e)
        {

            if (rbSelectionOne.IsChecked == true)
            {
                GenerateNames(names, Convert.ToInt32(sliderNameCount.Value), false);
            } 
            else
            {
                GenerateNames(names, Convert.ToInt32(sliderNameCount.Value), true);
            }

            lbGeneratedNames.ItemsSource = names;
            lbGeneratedNames.Items.MoveCurrentToLast();
            lbGeneratedNames.ScrollIntoView(lbGeneratedNames.Items.CurrentItem);
            
        }

        private void btnDeleteNames_Click(object sender, RoutedEventArgs e)
        {
            names.Clear();
        }

        private void btnSortNames_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> temp;
            temp = new ObservableCollection<string>(names.OrderBy(p => p));
            names.Clear();
            foreach (string j in temp)
            {
                names.Add(j);
            }
            names = temp;
            textBlockSortedStatus.Text = "Rendezett névsor!";
        }

        private void btnSaveNames_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "txt";
            sfd.Filter = "Szöveges fájl (*.txt)|*.txt|CSV fájl (*.csv)|*.csv|Összes fájl (*.*)|*.*";
            sfd.Title = "Adja meg a névsor nevét";
            if (sfd.ShowDialog() == true)
            {
               try
                {
                    if (System.IO.Path.GetExtension(sfd.FileName) == ".csv")
                    {
                        List<string> temp = new List<string>();
                        temp = names.ToList();
                        for (int i = 0; i < temp.Count(); i++)
                        {
                            temp[i] = temp[i].Replace(" ", ";");
                        }
                        File.WriteAllLines(sfd.FileName, temp);
                    } 
                    else
                    {
                        File.WriteAllLines(sfd.FileName, names);
                    }
                    MessageBox.Show("Sikeres mentés!", "Mentés", MessageBoxButton.OK, MessageBoxImage.Information);
                } 
                catch
                {
                    MessageBox.Show("Sikertelen mentés!");
                }
            }
        }

        private void sliderNameCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderNameCount.Value = Math.Round(sliderNameCount.Value, 0);
        }
    }
}
