using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading.Tasks;

#if DEBUG
[assembly: Application (Debuggable = true)]
#else
[assembly: Application (Debuggable = false)]
#endif

namespace RD_AAOW.Droid
	{
	/// <summary>
	/// Класс описывает загрузчик приложения
	/// </summary>
	[Activity (Label = "Text to KKT", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity:global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
		{
		/// <summary>
		/// Обработчик события создания экземпляра
		/// </summary>
		/// <param name="savedInstanceState"></param>
		protected override void OnCreate (Bundle savedInstanceState)
			{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate (savedInstanceState);
			global::Xamarin.Forms.Forms.Init (this, savedInstanceState);
			LoadApplication (new App ());
			}
		}

	/// <summary>
	/// Класс описывает экран-заставку приложения
	/// </summary>
	[Activity (Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
#if EVOTOR
	[IntentFilter (new[] { Intent.ActionMain },
		Categories = new[] { Intent.CategorySampleCode, "android.intent.category.EVOTOR" })]
#endif
	public class SplashActivity:global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
		{
		/// <summary>
		/// Обработчик события создания экземпляра
		/// </summary>
		/// <param name="savedInstanceState"></param>
		protected override void OnCreate (Bundle savedInstanceState)
			{
			base.OnCreate (savedInstanceState);
			}

		/// <summary>
		/// Prevent the back button from canceling the startup process
		/// </summary>
		public override void OnBackPressed ()
			{
			}

		/// <summary>
		/// Launches the startup task
		/// </summary>
		protected override void OnResume ()
			{
			base.OnResume ();
			Task startup = new Task (() =>
				{
					StartActivity (new Intent (Application.Context, typeof (MainActivity)));
				});
			startup.Start ();
			}
		}
	}
