using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace EnelSmartInfo
{
    public partial class VoicePage : PhoneApplicationPage
    {
        public VoicePage()
        {
            InitializeComponent();
        }

        public class TariffChartData
        {
            public double Value { get; set; }
        }

        public class EnergyChartData
        {
            public string TimeUnit { get; set; }

            public double Value { get; set; }

            public double AverageValue { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DateTime actualDateTime = DateTime.Now;
            if (NavigationContext.QueryString.ContainsKey("voiceCommandName") && ValueListBox.Items.Count == 0)
            {
                List<AppParser.EnergyEntry> selectedEntries = new List<AppParser.EnergyEntry>();
                switch (NavigationContext.QueryString["voiceCommandName"])
                {
                    case "MostraConsumiOggi":
                        {
                            RestRequest consReq = new RestRequest("/tables/EnergiaAttiva");
                            //consReq.AddParameter("year", NavigationContext.QueryString["year"]);   //
                            //consReq.AddParameter("month", NavigationContext.QueryString["month"]); // Oppure singola stringa
                            //consReq.AddParameter("day", NavigationContext.QueryString["day"]);     //
                            App.enelClient.ExecuteAsync(consReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        string todayString = DateTime.Now.ToString("yyyy/MM/dd");
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                if (resultEntry.Day.Split(' ')[0] == todayString && resultEntry.Values.Count > 0)
                                                {
                                                    selectedEntries.Add(resultEntry);
                                                    break;
                                                }
                                            }
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadEnergyChart(selectedEntries, "day");    
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    case "MostraConsumiGiorno":
                        {
                            int monthNum = AppParser.GetMonth(NavigationContext.QueryString["month"]);
                            string dayString;
                            if (monthNum < 10)
                            {
                                dayString = "0" + monthNum + "/" + AppParser.GetDay(NavigationContext.QueryString["day"]); 
                            }
                            else
                            {
                                dayString = monthNum + "/" + AppParser.GetDay(NavigationContext.QueryString["day"]);
                            }
                            if (dayString.Contains("-1"))
                            {
                                MessageBox.Show("Dettati valori non validi per il recupero dati, riprova!", "Comando errato", MessageBoxButton.OK);
                                return;
                            }
                            RestRequest consReq = new RestRequest("/tables/EnergiaAttiva");
                            //consReq.AddParameter("year", NavigationContext.QueryString["year"]);   //
                            //consReq.AddParameter("month", NavigationContext.QueryString["month"]); // Oppure singola stringa
                            //consReq.AddParameter("day", NavigationContext.QueryString["day"]);     //
                            App.enelClient.ExecuteAsync(consReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                string resultTime = resultEntry.Day.Split(' ')[0].Split('/')[1] + "/" + resultEntry.Day.Split(' ')[0].Split('/')[2];
                                                if (resultTime == dayString && resultEntry.Values.Count > 0)
                                                {
                                                    selectedEntries.Add(resultEntry);
                                                    break;
                                                }
                                            }
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadEnergyChart(selectedEntries, "day");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    case "MostraConsumiMensile":
                        {
                            RestRequest consReq = new RestRequest("/tables/EnergiaAttiva");
                            //consReq.AddParameter("month", NavigationContext.QueryString["month"]);
                            App.enelClient.ExecuteAsync(consReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                
                                                if (DateTime.Now.Year == Convert.ToInt32(resultEntry.CreatedAt.Split('/')[0]))
                                                {
                                                    int resultMonth = Convert.ToInt32(resultEntry.CreatedAt.Split('/')[1]);
                                                    if (resultMonth == AppParser.GetMonth(NavigationContext.QueryString["month"]) && resultEntry.Values.Count > 0)
                                                    {
                                                        selectedEntries.Add(resultEntry);
                                                    }
                                                }
                                            }
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadEnergyChart(selectedEntries, "month");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    /*case "MostraConsumiMensileAnno":
                        {
                            string monthString = int.Parse(NavigationContext.QueryString["year"]) + "/" + AppParser.GetMonth(NavigationContext.QueryString["month"]);
                            if (monthString.Contains("-1"))
                            {
                                MessageBox.Show("Dettati valori non validi per il recupero dati, riprova!", "Comando errato", MessageBoxButton.OK);
                                return;
                            }
                            RestRequest consReq = new RestRequest("/tables/EnergiaAttiva");
                            //consReq.AddParameter("month", NavigationContext.QueryString["month"]);
                            //consReq.AddParameter("year", NavigationContext.QueryString["year"]);
                            App.enelClient.ExecuteAsync(consReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                if (DateTime.Now.Year == Convert.ToInt32(resultEntry.CreatedAt.Split('/')[0]))
                                                {
                                                    int resultMonth = Convert.ToInt32(resultEntry.CreatedAt.Split('/')[1]);
                                                    if (resultMonth == AppParser.GetMonth(NavigationContext.QueryString["month"]))
                                                    {
                                                        selectedEntries.Add(resultEntry);
                                                    }
                                                }
                                            }
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadEnergyChart(selectedEntries, "month");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }*/
                    case "MostraConsumiSettimana":
                        {
                            RestRequest consReq = new RestRequest("/tables/EnergiaAttiva");
                            //consReq.AddParameter("month", NavigationContext.QueryString["month"]);
                            App.enelClient.ExecuteAsync(consReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                DateTime time = DateTime.Parse(resultEntry.Day);
                                                if (actualDateTime.Subtract(time).Days <= 7 && resultEntry.Values.Count > 0)
                                                {
                                                    selectedEntries.Add(resultEntry);
                                                }
                                            }
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadEnergyChart(selectedEntries, "week");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    case "MostraTariffa":
                        {
                            RestRequest tarReq = new RestRequest("/tables/TariffaCode");
                            App.enelClient.ExecuteAsync(tarReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            if (result.First().Values.Count > 0)
                                            selectedEntries.Add(result.First());
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadTariffChart(selectedEntries);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    case "MostraTariffaMensileAnno":
                        {
                            RestRequest tarReq = new RestRequest("/tables/TariffaCode");
                            //tarReq.AddParameter("month", NavigationContext.QueryString["month"]);
                            App.enelClient.ExecuteAsync(tarReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                if (/*int.Parse(NavigationContext.QueryString["year"])*/ DateTime.Now.Year == Convert.ToInt32(resultEntry.CreatedAt.Split('/')[0]))
                                                {
                                                    int resultMonth = Convert.ToInt32(resultEntry.CreatedAt.Split('/')[1]);
                                                    if (resultMonth == AppParser.GetMonth(NavigationContext.QueryString["month"]) && resultEntry.Values.Count > 0)
                                                    {
                                                        selectedEntries.Add(resultEntry);
                                                    }
                                                }
                                            }
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            LoadTariffChart(selectedEntries);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    case "ControllaSforoTariffa":
                        {
                            RestRequest tarReq = new RestRequest("/tables/TariffaCode");
                            //tarReq.AddParameter("month", NavigationContext.QueryString["month"]);
                            App.enelClient.ExecuteAsync(tarReq, (response) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    if (response.Content != null && response.Content != "")
                                    {
                                        AppParser.EnergyEntry[] result = JsonConvert.DeserializeObject<AppParser.EnergyEntry[]>(response.Content);
                                        List<string> resultEntriesStrings = Regex.Split(response.Content, @"\{(.*?)\}", RegexOptions.IgnoreCase).ToList<string>();
                                        for (int i = 0; i < resultEntriesStrings.Count; i++)
                                        {
                                            if (resultEntriesStrings[i].Length < 4)
                                            {
                                                resultEntriesStrings.Remove(resultEntriesStrings[i]);
                                            }
                                        }
                                        if (result.Length == resultEntriesStrings.Count)
                                        {
                                            for (int i = 0; i < result.Length; i++)
                                            {
                                                result[i].Values = AppParser.GetJsonValues(resultEntriesStrings[i]);
                                            }
                                            foreach (var resultEntry in result)
                                            {
                                                MessageBox.Show(resultEntry.CreatedAt.Split('/')[0] + "/" + resultEntry.CreatedAt.Split('/')[1]);
                                                string resultTimeString = resultEntry.CreatedAt.Split(' ')[0];
                                                if (NavigationContext.QueryString["period"] == "oggi" && resultTimeString == actualDateTime.ToString("yyyy-MM-dd"))
                                                {
                                                    if (resultEntry.Values.Any(t => Math.Round(t) == 3))
                                                    {
                                                        //Manage data
                                                        return;
                                                    }
                                                    break;
                                                }
                                                else if (NavigationContext.QueryString["period"] == "ultima settimana" || NavigationContext.QueryString["period"] == "ultimasettimana")
                                                {
                                                    DateTime time = DateTime.Parse(resultEntry.CreatedAt);
                                                    if (actualDateTime.Subtract(time).Days <= 7)
                                                    {
                                                        selectedEntries.Add(resultEntry);
                                                    }
                                                }
                                                else
                                                {
                                                    if (DateTime.Now.Year == Convert.ToInt32(resultEntry.CreatedAt.Split('/')[0]))
                                                    {
                                                        int resultMonth = Convert.ToInt32(resultEntry.CreatedAt.Split('/')[1]);
                                                        if (resultMonth == DateTime.Now.Month && resultEntry.Values.Count > 0)
                                                        {
                                                            selectedEntries.Add(resultEntry);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(response.Content);
                                        }
                                        if (selectedEntries.Count > 0)
                                        {
                                            if (selectedEntries.Any(i => i.Values.Any(v => Math.Round(v) == 1)))
                                            {
                                                CheckTariffLimit(true, selectedEntries);
                                            }
                                            else
                                            {
                                                CheckTariffLimit(false, selectedEntries);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nessun valore utile o coerente col comando trovato", "Risultati", MessageBoxButton.OK);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Errore nella connessione, controlla le tue impostazioni di rete!", "Connessione fallita", MessageBoxButton.OK);
                                    }
                                });
                            });
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Nessun comando riconosciuto!");
                            break;
                        }
                }
            }
            base.OnNavigatedTo(e);
        }

        public class Data
        {
            public string Value { get; set; }

            public string Date { get; set; }
        }

        private void LoadEnergyChart(List<AppParser.EnergyEntry> list, string timeType)
        {
            try
            {
                foreach (var entry in list)
                {
                    foreach (var value in entry.Values)
                    {
                        Data item = new Data
                        {
                            Value = value + " kWh",
                            Date = entry.Day.Split(' ')[0] + " alle " + GetWarningTime(entry.Values.IndexOf(value))
                        };
                        ValueListBox.Items.Add(item);
                    }
                }
                ValueListBox.Visibility = Visibility.Visible;
                this.ResultsTextBlock.Text = ResultsTextBlock.Text + " consumi";
                this.TariffChart.Visibility = Visibility.Collapsed;

                /*switch (timeType)
                {
                    case "day":
                        {
                            var selectedItem = list.FirstOrDefault();
                            for (int j = 0; j < (selectedItem.Values.Count / 4); j = j + 4)
                            {
                                double sumValue = selectedItem.Values[j] + selectedItem.Values[j + 1] + selectedItem.Values[j + 2] + selectedItem.Values[j + 3];
                                double mediumValue = sumValue / 4;
                                EnergyChartData item = new EnergyChartData { TimeUnit = (j / 4) + ":00", Value = sumValue, AverageValue = mediumValue };
                                data.Add(item);
                            }
                            break;
                        }
                    case "week":
                        {
                            foreach (var selectedItem in list)
                            {
                                double sumValue = selectedItem.Values.Sum();
                                double mediumValue = selectedItem.Values.Average();
                                EnergyChartData item = new EnergyChartData { TimeUnit = DateTime.Parse(selectedItem.Day).ToString("DDD", CultureInfo.CurrentCulture), Value = sumValue, AverageValue = mediumValue };
                                data.Add(item);
                            }
                            break;
                        }
                    case "month":
                        {
                            foreach (var selectedItem in list)
                            {
                                double sumValue = selectedItem.Values.Sum();
                                double mediumValue = selectedItem.Values.Average();
                                EnergyChartData item = new EnergyChartData { TimeUnit = DateTime.Parse(selectedItem.Day).Day.ToString(), Value = sumValue, AverageValue = mediumValue };
                                data.Add(item);
                            }
                            break;
                        }
                }
                if (data.Count > 0)
                {
                    /*this.EnergyChart.DataContext = data;
                    this.ResultsTextBlock.Text = ResultsTextBlock.Text + " consumi";
                    this.EnergyChart.Visibility = Visibility.Visible;
                    this.TariffChart.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Errore nel popolamento dei dati");
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
            }
        }

        private void LoadTariffChart(List<AppParser.EnergyEntry> list)
        {
            List<int> largeTariff = new List<int>();
            List<int> mediumTariff = new List<int>();
            List<int> smallTariff = new List<int>();
            List<TariffChartData> data = new List<TariffChartData>();
            foreach (var selectedItem in list)
            {
                foreach (double value in selectedItem.Values)
                {
                    switch (Convert.ToInt32(value))
                    {
                        case 1:
                            {
                                largeTariff.Add(Convert.ToInt32(value));
                                break;
                            }
                        case 2:
                            {
                                mediumTariff.Add(Convert.ToInt32(value));
                                break;
                            }
                        case 3:
                            {
                                smallTariff.Add(Convert.ToInt32(value));
                                break;
                            }
                    }
                }
            }
            data.Add(new TariffChartData { Value = smallTariff.Count });
            data.Add(new TariffChartData { Value = mediumTariff.Count });
            data.Add(new TariffChartData { Value = largeTariff.Count });
            this.TariffChart.Series[0].ItemsSource = data;
            this.ResultsTextBlock.Text = ResultsTextBlock.Text + " tariffe";
            this.EnergyChart.Visibility = Visibility.Collapsed;
            this.TariffChart.Visibility = Visibility.Visible;
        }

        private void CheckTariffLimit(bool overLimit, List<AppParser.EnergyEntry> list)
        {
            if (!overLimit)
            {
                TariffButton.Background = new SolidColorBrush(Color.FromArgb(255, 58, 193, 0));
                TariffTextBlock.Text = "Tariffa mai sforata";
                TariffImage.Source = new BitmapImage(new Uri("Assets/Icons/check.png", UriKind.Relative));
                if (NavigationContext.QueryString["period"] == "questomese" || NavigationContext.QueryString["period"] == "questasettimana")
                {
                    if (NavigationContext.QueryString["period"] == "questomese")
                    {
                        TariffCongratulationTextBlock.Text = "Complimenti, questo mese non hai mai sforato! Continua così!";
                    }
                    else
                    {
                        TariffCongratulationTextBlock.Text = "Complimenti, questa settimana non hai mai sforato! Continua così!";
                    }
                }
                else
                {
                    TariffCongratulationTextBlock.Text = "Complimenti, " + NavigationContext.QueryString["period"] + " non hai mai sforato! Continua così!";
                }
                TariffCongratulationTextBlock.FontSize = 38;
                TariffCongratulationTextBlock.FontWeight = FontWeights.SemiBold;
                TariffCongratulationTextBlock.Visibility = Visibility.Visible;
                TariffLimitPanel.Visibility = Visibility.Visible;
            }
            else
            {
                TariffButton.Background = new SolidColorBrush(Color.FromArgb(255, 183, 0, 0));
                TariffTextBlock.Text = "Tariffa sforata";
                TariffImage.Source = new BitmapImage(new Uri("Assets/Icons/close.png", UriKind.Relative));
                ObservableCollection<StackPanel> collection = new ObservableCollection<StackPanel>();
                foreach (var warningItem in list)
                {
                    foreach (var value in warningItem.Values)
                    {
                        if (value > 0 && value < 2)
                        {
                            StackPanel itemPanel = new StackPanel
                            {
                                Orientation = System.Windows.Controls.Orientation.Horizontal,
                                Margin = new Thickness(0, 10, 0, 10),
                                Background = new SolidColorBrush(Color.FromArgb(255, 183, 0, 0))
                            };
                            TextBlock warnLogo = new TextBlock
                            {
                                Text = "!",
                                FontSize = 50,
                                Foreground = new SolidColorBrush(Colors.White),
                                Width = 40,
                                FontWeight = FontWeights.Bold,
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                TextAlignment = TextAlignment.Center
                            };
                            TextBlock warnDay = new TextBlock
                            {
                                FontSize = 30,
                                TextWrapping = System.Windows.TextWrapping.Wrap,
                                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                TextAlignment = TextAlignment.Left,
                                Margin = new Thickness(20, 0, 0, 0)
                            };
                            warnDay.Text = warningItem.CreatedAt.Split(' ')[0] + " alle " + GetWarningTime(warningItem.Values.IndexOf(value));

                            itemPanel.Children.Add(warnLogo);
                            itemPanel.Children.Add(warnDay);

                            collection.Add(itemPanel);
                        }
                    }
                }
                ValueListBox.ItemsSource = collection;
                ValueListBox.Visibility = Visibility.Visible;
            }
            TariffLimitPanel.Visibility = Visibility.Visible;
        }

        private string GetWarningTime(int index)
        {
            double resto = index % 4; 
            if (resto == 0)
            {
                return (index / 4) + ":00";
            }
            else
            {
                switch (Convert.ToInt32(resto * 100))
                {
                    case 25:
                        {
                            return (index / 4) + ":15";
                        }
                    case 5:
                    case 50:
                        {
                            return (index / 4) + ":30";
                        }
                    case 75:
                        {
                            return (index / 4) + ":45";
                        }
                    default:
                        {
                            return "TimeError";
                        }
                }
            }
        }
    }
}