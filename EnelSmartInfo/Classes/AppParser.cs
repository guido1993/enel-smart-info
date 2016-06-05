using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnelSmartInfo
{
    public class AppParser
    {
        static List<string> months = new List<string> { 
                                     "Gennaio", "Febbraio", "Marzo", 
                                     "Aprile", "Maggio", "Giugno", 
                                     "Luglio", "Agosto", "Settembre", 
                                     "Ottobre", "Novembre", "Dicembre" 
                                 };

        public class EnergyEntry
        {
            private string day;
            [JsonProperty("data")]
            public string Day
            {
                get
                {
                    return day;
                }

                set
                {
                    try
                    {
                        DateTime dateTime;
                        string inputString = Regex.Replace(value, "t", " ", RegexOptions.IgnoreCase);
                        inputString = Regex.Replace(inputString, "z", "", RegexOptions.IgnoreCase);
                        dateTime = DateTime.Parse(inputString.Split('.')[0]);
                        day = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    catch (Exception)
                    {
                        day = "";
                    }
                }
            }

            private string type;
            [JsonProperty("tipoEnergia")]
            public string EnergyType
            {
                get
                {
                    return type;
                }

                set
                {
                    try
                    {
                        type = value;
                    }
                    catch (Exception)
                    {
                        type = "";
                    }
                }
            }

            [JsonProperty("id")]
            public string IdDayReport { get; set; }

            [JsonProperty("__version")]
            public string Version { get; set; }

            private string createTime;
            [JsonProperty("__createdAt")]
            public string CreatedAt
            {
                get
                {
                    return createTime;
                }

                set
                {
                    try
                    {
                        DateTime dateTime;
                        string inputString = Regex.Replace(value, "t", " ", RegexOptions.IgnoreCase);
                        inputString = Regex.Replace(inputString, "z", "", RegexOptions.IgnoreCase);
                        dateTime = DateTime.Parse(inputString.Split('.')[0]);
                        createTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    catch (Exception)
                    {
                        createTime = "";
                    }
                }
            }

            private string updateTime;
            [JsonProperty("__updateAt")]
            public string UpdatedAt
            {
                get
                {
                    return updateTime;
                }

                set
                {
                    try
                    {
                        DateTime dateTime;
                        string inputString = Regex.Replace(value, "t", " ", RegexOptions.IgnoreCase);
                        inputString = Regex.Replace(inputString, "z", "", RegexOptions.IgnoreCase);
                        dateTime = DateTime.Parse(inputString.Split('.')[0]);
                        updateTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    catch (Exception)
                    {
                        updateTime = "";
                    }
                }
            }

            public List<double> Values { get; set; }
        }

        public static List<double> GetJsonValues(string jsonEntry)
        {
            List<double> values = new List<double>();
            JToken mainToken = JObject.Parse("{" + jsonEntry + "}");
            try
            {
                foreach (JToken entryToken in mainToken)
                {
                    if (((JProperty)entryToken).Name.StartsWith("ts"))
                    {
                        var dayTokenKey = entryToken as JProperty;
                        Double dayTokenValue = dayTokenKey.Values<Double>().FirstOrDefault();
                        values.Add(Convert.ToDouble(dayTokenValue));
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
                return null;
            }
        }

        public static int GetMonth(string month)
        {
            int monthInt;
            try
            {
                monthInt = months.IndexOf(months.FirstOrDefault(m => m == month)) + 1;
            }
            catch (Exception)
            {
                monthInt = -1;
            }
            return monthInt;
        }

        public static int GetWeek(string week)
        {
            switch (week)
            {
                case "prima":
                    {
                        return 1;
                    }
                case "seconda":
                    {
                        return 2;
                    }
                case "terza":
                    {
                        return 3;
                    }
                case "quarta":
                    {
                        return 4;
                    }
                default:
                    {
                        return -1;
                    }
            }
        }

        public static string GetDay(string day)
        {
            if (day == "primo")
            {
                return "01";
            }
            else
            {
                int num = int.Parse(day);
                if (num > 0 && num < 10)
                {
                    return "0" + num;
                }
                else if (num <= 31)
                {
                    try
                    {
                        return num.ToString();
                    }
                    catch (Exception)
                    {
                        return "-1";
                    }
                }
                else
                {
                    return "-1";
                }
            }
        }
    }
}
