using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Webkit;
using Android.Widget;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Linq;
using Permission = Plugin.Permissions.Abstractions.Permission;

namespace Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        public string mGeolocationOrigin;
        public GeolocationPermissions.ICallback mGeolocationCallback;
        public ProgressBar progress;
        public WebView webView;
        bool IsRestart = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
           
            base.OnCreate(savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            UserDialogs.Init(this);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.SetGeolocationEnabled(true);
            webView.Settings.CacheMode = CacheModes.NoCache;
            webView.Settings.SetAppCacheEnabled(true);
            webView.Settings.EnableSmoothTransition();
            webView.Settings.LightTouchEnabled = true;
            //webView.Settings.SafeBrowsingEnabled = true;
            webView.SetWebViewClient(new HybridWebView(this));
            webView.SetWebChromeClient(new HybridWebViewChormClient(this));
            webView.LoadUrl("https://ilicar.ir");
          


        }

     

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (permissions.Contains(Manifest.Permission.AccessFineLocation))
            {
                bool allow = false;
                if (grantResults[0] == Android.Content.PM.Permission.Granted)
                {
                    // user has allowed these permissions
                    allow = true;
                }
                if (mGeolocationCallback != null)
                {
                    mGeolocationCallback.Invoke(mGeolocationOrigin, allow, false);
                }
            }
        }
     
        protected override void OnStart()
        {
            if (!IsRestart)
            {
                UserDialogs.Instance.ShowLoading("لطفا صبر کنید", MaskType.Black);
            }

            base.OnStart();
        }
        protected override void OnRestart()
        {
            IsRestart = true;
            base.OnRestart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResumeFragments()
        {
          

            base.OnResumeFragments();
        }

       

    }


    public class HybridWebView : WebViewClient
    {
        MainActivity currentActivity;
        public HybridWebView(MainActivity activity)
        {
            currentActivity = activity;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            UserDialogs.Instance.ShowLoading("در حال دریافت اطلاعات", MaskType.Black);

            if (request.Url.Scheme == "tel")
            {
                var uri = Android.Net.Uri.Parse("tel:" + request.Url.SchemeSpecificPart.Trim());
                var intent = new Intent(Intent.ActionDial, uri);
                currentActivity.StartActivity(intent);
                UserDialogs.Instance.HideLoading();
                return true;
            }
            else if (request.Url.Scheme == "sms")
            {
                var uri = Android.Net.Uri.Parse("sms:" + request.Url.SchemeSpecificPart.Trim());
                var intent = new Intent(Intent.ActionSend, uri);
                currentActivity.StartActivity(intent);
                UserDialogs.Instance.HideLoading();
                return true;
            }
            else
            {


                return base.ShouldOverrideUrlLoading(view, request);

            }



        }

       
        public override void OnPageFinished(WebView view, string url)
        {
            UserDialogs.Instance.HideLoading();
        }

        





    }


    public class HybridWebViewChormClient : WebChromeClient
    {

        MainActivity currentActivity;
        public HybridWebViewChormClient(MainActivity activity)
        {
            currentActivity = activity;
        }




        public async override void OnGeolocationPermissionsShowPrompt(string origin, GeolocationPermissions.ICallback callback)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (Build.VERSION.SdkInt < BuildVersionCodes.M || status == PermissionStatus.Granted)
            {
                // we're on SDK < 23 OR user has already granted permission
                callback.Invoke(origin, true, false);
            }
            else
            {
                // ask the user for permissions
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
                currentActivity.mGeolocationOrigin = origin;
                currentActivity.mGeolocationCallback = callback;
            }
        }



    }
}