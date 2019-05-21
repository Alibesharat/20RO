using Android.App;
using Android.Content;
using Android.OS;

namespace Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplshActivitycs : Activity
    {
        static readonly string TAG = "X:" + typeof(SplshActivitycs).Name;
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);



        }
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));

        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
    }
}