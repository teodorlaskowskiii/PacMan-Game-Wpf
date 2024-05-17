using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PacMan_Game_Wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MenuWindow.xaml
    /// </summary>
    /// 

    public partial class MenuWindow : Window
    {
       
        private static string nazwaPliku = @"C:\Temp\pacmanrecords.txt";
        private List<int> records = new List<int>();

        public MenuWindow(int? min, int? sec)
        {
            InitializeComponent();
            if(min != null && sec != null)
            {
                //record.Text=min.ToString() +":" + sec.ToString();
                int minuta = (int)min;
                int sekunda = (int)sec;
                save(minuta, sekunda);

                

                loadRecords();
                //recordsListBox.ItemsSource = records;

                int lowestRecord = FindLowestRecord();

                if(lowestRecord < 60)
                {
                    record.Text = 00 + ":" + lowestRecord;
                }
                else
                {
                    int lowestMin = lowestRecord / 60;
                    int lowestSec = lowestRecord % 60;
                    record.Text = lowestMin + ":" + lowestSec;
                }


                /*foreach (var record in recordsList)
                {
                    Console.WriteLine($"{record[0]}:{record[1]}");
                }*/


            }
            
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void loadRecords()
        {
            try
            {
                foreach (var line in File.ReadLines(nazwaPliku))
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        int record;
                        bool success = Int32.TryParse(line.Trim(), out record);
                        if (success)
                        {
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error loading records: {ex.Message}");
            }
        }
        private int FindLowestRecord()
        {
            if (records.Count != 0)
            {


                return records.Min();
            }
            else { return 0; }
        }
        private void save(int min, int sec)
        {
            if (min != 100 && sec != 100)
            {




                if (min > 0)
                {
                    sec = sec + min * 60;
                }
                


                string tresc = $"{sec}\n";

                File.AppendAllText(nazwaPliku, tresc);
            }
            else {
                string tresc = "";
                File.AppendAllText(nazwaPliku, tresc); 
            };
           
            
        }
        
    }
}
