using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microcharts;
using SkiaSharp;
using Microcharts.Droid;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Wykresy
{
    [Activity(Label = "WykresActivity")]
    public class WykresActivity : Activity
    {
        Chart chart;
        ChartView chartView;
        int licznik=0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Wykres);
            chartView = FindViewById<ChartView>(Resource.Id.wykres);
            string typWykresu = Intent.GetStringExtra("typ wykresu");
            switch (typWykresu)
            {
                case "bar":
                    chart = new BarChart();
                    break;
                case "point":
                    chart = new PointChart();
                    break;
                case "line":
                    chart = new LineChart();
                    break;
                case "radial gauge":
                    chart = new RadialGaugeChart();
                    break;
                case "donut":
                    chart = new DonutChart();
                    break;
                case "radar":
                    chart = new RadarChart();
                    break;
            }
            List<Entry> entries = new List<Entry>();
            string dane = Intent.GetStringExtra("dane");
            try
            {
                switch (dane)
                {
                    case "ceny złota":
                        foreach (var item in ZwrocWartosci<CenaZlota>("http://api.nbp.pl/api/cenyzlota/last/5"))
                        {
                            entries.Add(new Entry((float)item.cena) { Label = item.data, ValueLabel = item.cena.ToString(), Color = SKColor.Parse(ZwrocKolor()) });
                        }
                        break;
                    case "kurs GBP":
                        foreach (var item in ZwrocExchangeRatesTable("http://api.nbp.pl/api/exchangerates/rates/a/gbp/last/5"))
                        {
                            entries.Add(new Entry((float)item.mid) { Label = item.effectiveDate, ValueLabel = item.mid.ToString(), Color = SKColor.Parse(ZwrocKolor()) });
                        }
                        break;
                    case "kurs USD":
                        foreach (var item in ZwrocWalute("http://api.nbp.pl/api/exchangerates/rates/c/usd/last/5"))
                        {
                            entries.Add(new Entry((float)item.bid) { Label = item.effectiveDate, ValueLabel = item.bid.ToString(), Color = SKColor.Parse(ZwrocKolor()) });
                        }
                        break;
                    case "kurs EUR":
                        foreach (var item in ZwrocWalute("http://api.nbp.pl/api/exchangerates/rates/c/eur/last/5"))
                        {
                            entries.Add(new Entry((float)item.bid) { Label = item.effectiveDate, ValueLabel = item.bid.ToString(), Color = SKColor.Parse(ZwrocKolor()) });
                        }
                        break;
                    case "kurs CHF":
                        foreach (var item in ZwrocWalute("http://api.nbp.pl/api/exchangerates/rates/c/chf/last/5"))
                        {
                            entries.Add(new Entry((float)item.bid) { Label = item.effectiveDate, ValueLabel = item.bid.ToString(), Color = SKColor.Parse(ZwrocKolor()) });
                        }
                        break;
                    case "kurs JPY":
                        foreach (var item in ZwrocWalute("http://api.nbp.pl/api/exchangerates/rates/c/jpy/last/5"))
                        {
                            entries.Add(new Entry((float)item.bid) { Label = item.effectiveDate, ValueLabel = item.bid.ToString(), Color = SKColor.Parse(ZwrocKolor()) });
                        }
                        break;
                }
            }
            catch (WebException we)
            {
                Toast.MakeText(this, we.Message, ToastLength.Long).Show();
            }
            chart.Entries = entries;
            chartView.Chart = chart;
        }

        private static List<T> ZwrocWartosci<T>(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(uri));
            request.ContentType = "application/json";
            request.Method = "GET";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                Stream responseStream = response.GetResponseStream();
                using (var reader = new StreamReader(responseStream))
                {
                    string responseText = reader.ReadToEnd();
                    IEnumerable<T> results = JsonConvert.DeserializeObject<IEnumerable<T>>(responseText);
                    return results.ToList<T>();
                }
            }
        }

        private List<Rate> ZwrocExchangeRatesTable(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(uri));
            request.ContentType = "application/json";
            request.Method = "GET";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                Stream responseStream = response.GetResponseStream();
                using (var reader = new StreamReader(responseStream))
                {
                    string responseText = reader.ReadToEnd();
                    IEnumerable<Rate> results = JsonConvert.DeserializeObject<ExchangeRatesTable>(responseText).rates;
                    return results.ToList<Rate>();
                }
            }
        }

        private List<Rate> ZwrocWalute(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(uri));
            request.ContentType = "application/json";
            request.Method = "GET";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                Stream responseStream = response.GetResponseStream();
                using (var reader = new StreamReader(responseStream))
                {
                    string responseText = reader.ReadToEnd();
                    IEnumerable<Rate> results = JsonConvert.DeserializeObject<ExchangeRatesSeries>(responseText).rates;
                    return results.ToList<Rate>();
                }
            }
        }

        private string ZwrocKolor()
        {
            licznik = (licznik+1) % 6;
            switch (licznik)
            {
                case 0:
                    return "#ff0000";
                case 1:
                    return "#ff6600";
                case 2:
                    return "#ffff00";
                case 3:
                    return "#33cc33";
                case 4:
                    return "#0066ff";
                case 5:
                    return "#cc33ff";
                default:
                    return "#000000";
            }
        }
    }
}