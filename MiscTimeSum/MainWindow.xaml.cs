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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiscTimeSum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan totalTimeSpan = TimeSpan.Zero;
            for (int i = 0, count = MiscTimeDescription.LineCount; i < count; i++)
            {
                string line = MiscTimeDescription.GetLineText(i);
                string[] times = line.Split('-');
                if(times.Any())
                {
                    int hours = 0, mins = 0;
                    string timePortion = times[0];
                    int idx = timePortion.IndexOf(':');
                    if(idx >= 0) // maybe format like 01:30
                    {
                        // we know the number before it is in hours, shouldnt be anything before it but numbers
                        string strTime = timePortion.Substring(0, idx).Trim();
                        if(strTime.Length > 0)
                            hours = int.Parse(strTime);

                        if(idx+1 < timePortion.Length)
                            mins = int.Parse(timePortion.Substring(idx+1, timePortion.Length - idx -1).Trim());
                    }
                    else // maybe format like 1h 30m
                    {
                        idx = timePortion.IndexOf('h');
                        if (idx >= 0)
                        {
                            // we know the number before it is in hours, shouldnt be anything before it but numbers
                            string strTime = timePortion.Substring(0, idx).Trim();
                            hours = int.Parse(strTime);
                        }

                        idx = timePortion.IndexOf('m');
                        if (idx >= 0)
                        {
                            // we know the number before it is in minutes
                            string strTime = timePortion.Substring(0, idx).Trim();
                            int idxDelim = strTime.IndexOf(' ');
                            if (idxDelim < 0)
                                idxDelim = strTime.IndexOf(',');

                            if (idxDelim >= 0 && idxDelim < idx)
                            {
                                mins = int.Parse(strTime.Substring(idxDelim, idx - idxDelim).Trim());
                            }
                            else
                            {
                                mins = int.Parse(strTime);
                            }
                        }
                    }

                    totalTimeSpan = totalTimeSpan.Add(new TimeSpan(hours, mins, 0));
                }
            }

            int totalHours = totalTimeSpan.Days * 24 + totalTimeSpan.Hours, totalMins = totalTimeSpan.Minutes;
            double workingDays = (totalHours + (totalMins/60.0)) / 8.0;
            TotalTime.Content = string.Format("{0}h {1}m  (about {2:0.##} working days)", totalHours, totalMins, workingDays);
        }
    }
}
