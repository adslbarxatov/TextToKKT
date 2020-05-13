using Android.App;
using Android.Content.PM;
using Android.OS;

namespace RD_AAOW.Droid
	{
	/// <summary>
	/// Класс описывает функционал приложения
	/// </summary>
	[Activity (Label = "Text to KKT", Icon = "@mipmap/icon", Theme = "@style/MainTheme", 
		MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
	}