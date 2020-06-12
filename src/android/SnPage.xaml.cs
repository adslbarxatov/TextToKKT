﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает страницу проверки заводских номеров и ИНН
	/// </summary>
 	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class SnPage:ContentPage
		{
		/// <summary>
		/// Конструктор. Запускает страницу
		/// </summary>
		public SnPage ()
			{
			InitializeComponent ();
			}

		/// <summary>
		/// Переопределение для кнопки возврата
		/// </summary>
		protected override bool OnBackButtonPressed ()
			{
			App app = (App)App.Current;
			app.CallHeadersPage ();

			return true;
			}
		}
	}
