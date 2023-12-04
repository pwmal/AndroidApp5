using Android.Content;
using Android.Views;
using Android.OS;
using Java.IO;
using Xamarin.Android.Net;
using System.Net;
using System.Net.Http;
using static Android.App.ActionBar;
using Android.Webkit;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Reflection.Metadata;

namespace AndroidApp5
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]

    public class MainActivity : Activity
    {
        HttpClient httpClient = new HttpClient();
        async protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            Button btn1 = FindViewById<Button>(Resource.Id.button1);
            Button btn2 = FindViewById<Button>(Resource.Id.button2);
            Button btn3 = FindViewById<Button>(Resource.Id.button3);
            EditText EditText = FindViewById<EditText>(Resource.Id.editText);

            btn1.Click += delegate
            {
                ShortAlert(EditText.Text);
            };

            btn2.Click += delegate
            {
                OpenFile(GetExternalFilesDir(string.Empty).ToString() + "file.pdf");
            };

            btn3.Click += delegate
            {
                System.IO.File.Delete(GetExternalFilesDir(string.Empty).ToString() + "file.pdf");
            };
        }

        public void OpenFile(string filePath)
        {
            Intent intent = new Intent(Intent.ActionView);

            intent.SetDataAndType(Android.Net.Uri.Parse(filePath), "application/pdf");

            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.ClearWhenTaskReset);
            intent.AddFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

        async public void ShortAlert(string index)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://ntv.ifmo.ru/file/journal/{index}.pdf");
            using HttpResponseMessage response = await httpClient.SendAsync(request);

            string type = "application/pdf";
            if (response.Content.Headers.ContentType.ToString() == type)
            {
                WebClient wc = new WebClient();
                string url = $"https://ntv.ifmo.ru/file/journal/{index}.pdf";
                string save_path = GetExternalFilesDir(string.Empty).ToString();
                string name = "file.pdf";
                wc.DownloadFile(url, save_path + name);
            }
            else
            {
                Toast.MakeText(Application.Context, "False", ToastLength.Short).Show();
            }
        }
    }
}
