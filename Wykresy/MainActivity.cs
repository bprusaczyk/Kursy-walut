using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace Wykresy
{
    [Activity(Label = "Wykresy", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button pokaz;
        RadioButton bar, point, line, radialGauge, donut, radar, cenyZlota, kursGbp, kursUsd, kursEur, kursChf, kursJpy;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            pokaz = FindViewById<Button>(Resource.Id.Pokaz);
            pokaz.Click += Pokaz_Click;
        }

        private void Pokaz_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(WykresActivity));
            bar = FindViewById<RadioButton>(Resource.Id.Bar);
            point = FindViewById<RadioButton>(Resource.Id.Point);
            line = FindViewById<RadioButton>(Resource.Id.Line);
            radialGauge = FindViewById<RadioButton>(Resource.Id.RadialGauge);
            donut = FindViewById<RadioButton>(Resource.Id.Donut);
            radar = FindViewById<RadioButton>(Resource.Id.Radar);
            cenyZlota = FindViewById<RadioButton>(Resource.Id.CenyZlota);
            kursGbp = FindViewById<RadioButton>(Resource.Id.KursGbp);
            kursUsd = FindViewById<RadioButton>(Resource.Id.KursUsd);
            kursEur = FindViewById<RadioButton>(Resource.Id.KursEur);
            kursChf = FindViewById<RadioButton>(Resource.Id.KursChf);
            kursJpy = FindViewById<RadioButton>(Resource.Id.KursJpy);
            if(bar.Checked)
            {
                i.PutExtra("typ wykresu", "bar");
            }
            if (point.Checked)
            {
                i.PutExtra("typ wykresu", "point");
            }
            if (line.Checked)
            {
                i.PutExtra("typ wykresu", "line");
            }
            if (radialGauge.Checked)
            {
                i.PutExtra("typ wykresu", "radial gauge");
            }
            if (donut.Checked)
            {
                i.PutExtra("typ wykresu", "donut");
            }
            if (radar.Checked)
            {
                i.PutExtra("typ wykresu", "radar");
            }
            if (cenyZlota.Checked)
            {
                i.PutExtra("dane", "ceny złota");
            }
            if (kursGbp.Checked)
            {
                i.PutExtra("dane", "kurs GBP");
            }
            if (kursUsd.Checked)
            {
                i.PutExtra("dane", "kurs USD");
            }
            if (kursEur.Checked)
            {
                i.PutExtra("dane", "kurs EUR");
            }
            if (kursChf.Checked)
            {
                i.PutExtra("dane", "kurs CHF");
            }
            if (kursJpy.Checked)
            {
                i.PutExtra("dane", "kurs JPY");
            }
            StartActivity(i);
        }
    }
}

