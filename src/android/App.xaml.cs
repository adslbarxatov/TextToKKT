using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает функционал приложения
	/// </summary>
	public partial class App:Application
		{
		// Переменные
		private Label headerLabel, selectionLabel, sourceTextLabel,
			resultTextLabel, helpLabel, errorLabel, resultText;
		private Button kktButton;
		private Editor sourceText;
		private Color masterBackColor = Color.FromRgb (255, 255, 240),
			fieldBackColor = Color.FromRgb (255, 255, 210),
			masterTextColor = Color.FromRgb (0, 0, 128),
			masterHeaderColor = Color.FromRgb (0, 0, 0);
		private int masterFontSize = 18,
			headerFontSize = 24,
			tipsFontSize = 14;
		private Thickness margin = new Thickness (6);

		// Операторы
		private KKMCodes kkmc = new KKMCodes ();
		private int currentKKT = 0;

		/// <summary>
		/// Конструктор. Точка входа приложения
		/// </summary>
		public App ()
			{
			// Инициализация
			InitializeComponent ();

			MainPage = new MainPage
				{
				BackgroundColor = masterBackColor
				};

			// Получение и настройка контролов
			headerLabel = (Label)MainPage.FindByName ("HeaderLabel");
			headerLabel.Text = ProgramDescription.AssemblyTitle;
			headerLabel.HorizontalOptions = LayoutOptions.Center;
			headerLabel.FontAttributes = FontAttributes.Bold;
			headerLabel.FontSize = headerFontSize;
			headerLabel.TextColor = masterHeaderColor;
			headerLabel.Margin = margin;

			selectionLabel = (Label)MainPage.FindByName ("SelectionLabel");
			selectionLabel.Text = "Модель ККТ:";
			selectionLabel.HorizontalOptions = LayoutOptions.Start;
			selectionLabel.FontAttributes = FontAttributes.None;
			selectionLabel.FontSize = masterFontSize;
			selectionLabel.TextColor = masterHeaderColor;
			selectionLabel.Margin = margin;

			kktButton = (Button)MainPage.FindByName ("KKTButton");
			kktButton.BackgroundColor = fieldBackColor;
			kktButton.FontAttributes = FontAttributes.None;
			kktButton.FontSize = masterFontSize;
			kktButton.TextColor = masterTextColor;
			kktButton.Margin = margin;

			kktButton.Text = kkmc.KKMTypeNames[currentKKT];
			kktButton.Clicked += KKTButton_Clicked;

			sourceTextLabel = (Label)MainPage.FindByName ("SourceTextLabel");
			sourceTextLabel.Text = "Исходный текст:";
			sourceTextLabel.HorizontalOptions = LayoutOptions.Start;
			sourceTextLabel.FontAttributes = FontAttributes.None;
			sourceTextLabel.FontSize = masterFontSize;
			sourceTextLabel.TextColor = masterHeaderColor;
			sourceTextLabel.Margin = margin;

			sourceText = (Editor)MainPage.FindByName ("SourceText");
			sourceText.AutoSize = EditorAutoSizeOption.TextChanges;
			sourceText.BackgroundColor = fieldBackColor;
			sourceText.FontAttributes = FontAttributes.None;
			sourceText.FontFamily = "Serif";
			sourceText.FontSize = masterFontSize;
			sourceText.HorizontalOptions = LayoutOptions.Fill;
			sourceText.Keyboard = Keyboard.Default;
			sourceText.MaxLength = 72;
			//sourceText.Placeholder = "...";
			//sourceText.PlaceholderColor = Color.FromRgb (255, 255, 0);
			sourceText.TextColor = masterTextColor;
			sourceText.Margin = margin;

			sourceText.Text = "";
			sourceText.TextChanged += SourceText_TextChanged;

			resultTextLabel = (Label)MainPage.FindByName ("ResultTextLabel");
			resultTextLabel.Text = "Коды ККТ:";
			resultTextLabel.HorizontalOptions = LayoutOptions.Start;
			resultTextLabel.FontAttributes = FontAttributes.None;
			resultTextLabel.FontSize = masterFontSize;
			resultTextLabel.TextColor = masterHeaderColor;
			resultTextLabel.Margin = margin;

			errorLabel = (Label)MainPage.FindByName ("ErrorLabel");
			errorLabel.Text = "Один или несколько введённых символов не поддерживаются данной ККТ";
			errorLabel.HorizontalOptions = LayoutOptions.Center;
			errorLabel.FontAttributes = FontAttributes.Italic;
			errorLabel.FontSize = tipsFontSize;
			errorLabel.TextColor = Color.FromRgb (255, 0, 0);
			errorLabel.HorizontalTextAlignment = TextAlignment.Center;
			errorLabel.IsVisible = false;
			errorLabel.Margin = margin;

			resultText = (Label)MainPage.FindByName ("ResultText");
			resultText.BackgroundColor = fieldBackColor;
			resultText.FontAttributes = FontAttributes.None;
			resultText.FontFamily = "Serif";
			resultText.FontSize = masterFontSize;
			resultText.HorizontalOptions = LayoutOptions.Fill;
			resultText.TextColor = masterTextColor;
			resultText.Text = "";
			resultText.Margin = margin;

			helpLabel = (Label)MainPage.FindByName ("HelpLabel");
			helpLabel.Text = kkmc.GetKKMTypeDescription ((uint)currentKKT);
			helpLabel.FontAttributes = FontAttributes.Italic;
			helpLabel.FontSize = tipsFontSize;
			helpLabel.HorizontalOptions = LayoutOptions.Center;
			helpLabel.HorizontalTextAlignment = TextAlignment.Center;
			helpLabel.TextColor = Color.FromRgb (64, 64, 64);
			helpLabel.Margin = margin;
			}

		// Ввод текста
		private void SourceText_TextChanged (object sender, TextChangedEventArgs e)
			{
			resultText.Text = "";
			if ((sourceText.Text != null) && (sourceText.Text != ""))
				errorLabel.IsVisible = !Decode ();

			sourceTextLabel.Text = "Исходный текст (" + sourceText.Text.Length.ToString () + "):";
			}

		// Выбор модели ККТ
		private async void KKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			string res = await MainPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
				kkmc.KKMTypeNames.ToArray ());

			// Установка модели
			if (kkmc.KKMTypeNames.IndexOf (res) < 0)
				return;

			kktButton.Text = res;
			currentKKT = kkmc.KKMTypeNames.IndexOf (res);
			helpLabel.Text = kkmc.GetKKMTypeDescription ((uint)currentKKT);

			SourceText_TextChanged (null, null);
			}

		// Функция трансляции строки в набор кодов
		private bool Decode ()
			{
			// Выполнение
			bool res = true;
			char[] text = sourceText.Text.ToCharArray ();

			for (int i = 0; i < sourceText.Text.Length; i++)
				{
				string s;

				if ((s = kkmc.GetCode ((uint)currentKKT, CharToCP1251 (sourceText.Text[i]))) ==
					KKMCodes.EmptyCode)
					{
					resultText.Text += "xxx   ";
					res = false;
					}
				else
					{
					resultText.Text += (s + "   ");
					}
				}

			// Означает успех/ошибку преобразования
			return res;
			}

		// Метод-преобразователь символов в коды CP1251
		private byte CharToCP1251 (char Character)
			{
			switch (Character)
				{
				// Поддерживаемые символы
				case ' ':
					return 32;
				case '!':
					return 33;
				case '"':
					return 34;
				case '#':
					return 35;
				case '$':
					return 36;
				case '%':
					return 37;
				case '&':
					return 38;
				case '\'':
					return 39;
				case '(':
					return 40;
				case ')':
					return 41;
				case '*':
					return 42;
				case '+':
					return 43;
				case ',':
					return 44;
				case '-':
					return 45;
				case '.':
					return 46;
				case '/':
					return 47;
				case '0':
					return 48;
				case '1':
					return 49;
				case '2':
					return 50;
				case '3':
					return 51;
				case '4':
					return 52;
				case '5':
					return 53;
				case '6':
					return 54;
				case '7':
					return 55;
				case '8':
					return 56;
				case '9':
					return 57;
				case ':':
					return 58;
				case ';':
					return 59;
				case '<':
					return 60;
				case '=':
					return 61;
				case '>':
					return 62;
				case '?':
					return 63;
				case '@':
					return 64;
				case 'A':
					return 65;
				case 'B':
					return 66;
				case 'C':
					return 67;
				case 'D':
					return 68;
				case 'E':
					return 69;
				case 'F':
					return 70;
				case 'G':
					return 71;
				case 'H':
					return 72;
				case 'I':
					return 73;
				case 'J':
					return 74;
				case 'K':
					return 75;
				case 'L':
					return 76;
				case 'M':
					return 77;
				case 'N':
					return 78;
				case 'O':
					return 79;
				case 'P':
					return 80;
				case 'Q':
					return 81;
				case 'R':
					return 82;
				case 'S':
					return 83;
				case 'T':
					return 84;
				case 'U':
					return 85;
				case 'V':
					return 86;
				case 'W':
					return 87;
				case 'X':
					return 88;
				case 'Y':
					return 89;
				case 'Z':
					return 90;
				case '[':
					return 91;
				case '\\':
					return 92;
				case ']':
					return 93;
				case '^':
					return 94;
				case '_':
					return 95;
				case '`':
					return 96;
				case 'a':
					return 97;
				case 'b':
					return 98;
				case 'c':
					return 99;
				case 'd':
					return 100;
				case 'e':
					return 101;
				case 'f':
					return 102;
				case 'g':
					return 103;
				case 'h':
					return 104;
				case 'i':
					return 105;
				case 'j':
					return 106;
				case 'k':
					return 107;
				case 'l':
					return 108;
				case 'm':
					return 109;
				case 'n':
					return 110;
				case 'o':
					return 111;
				case 'p':
					return 112;
				case 'q':
					return 113;
				case 'r':
					return 114;
				case 's':
					return 115;
				case 't':
					return 116;
				case 'u':
					return 117;
				case 'v':
					return 118;
				case 'w':
					return 119;
				case 'x':
					return 120;
				case 'y':
					return 121;
				case 'z':
					return 122;
				case '{':
					return 123;
				case '|':
					return 124;
				case '}':
					return 125;
				case '~':
					return 126;
				case '':
					return 127;
				case 'Ђ':
					return 128;
				case 'Ѓ':
					return 129;
				case '‚':
					return 130;
				case 'ѓ':
					return 131;
				case '„':
					return 132;
				case '…':
					return 133;
				case '†':
					return 134;
				case '‡':
					return 135;
				case '€':
					return 136;
				case '‰':
					return 137;
				case 'Љ':
					return 138;
				case '‹':
					return 139;
				case 'Њ':
					return 140;
				case 'Ќ':
					return 141;
				case 'Ћ':
					return 142;
				case 'Џ':
					return 143;
				case 'ђ':
					return 144;
				case '‘':
					return 145;
				case '’':
					return 146;
				case '“':
					return 147;
				case '”':
					return 148;
				case '•':
					return 149;
				case '–':
					return 150;
				case '—':
					return 151;
				case '':
					return 152;
				case '™':
					return 153;
				case 'љ':
					return 154;
				case '›':
					return 155;
				case 'њ':
					return 156;
				case 'ќ':
					return 157;
				case 'ћ':
					return 158;
				case 'џ':
					return 159;
				case ' ':
					return 160;
				case 'Ў':
					return 161;
				case 'ў':
					return 162;
				case 'Ј':
					return 163;
				case '¤':
					return 164;
				case 'Ґ':
					return 165;
				case '¦':
					return 166;
				case '§':
					return 167;
				case 'Ё':
					return 168;
				case '©':
					return 169;
				case 'Є':
					return 170;
				case '«':
					return 171;
				case '¬':
					return 172;
				case '­':
					return 173;
				case '®':
					return 174;
				case 'Ї':
					return 175;
				case '°':
					return 176;
				case '±':
					return 177;
				case 'І':
					return 178;
				case 'і':
					return 179;
				case 'ґ':
					return 180;
				case 'µ':
					return 181;
				case '¶':
					return 182;
				case '·':
					return 183;
				case 'ё':
					return 184;
				case '№':
					return 185;
				case 'є':
					return 186;
				case '»':
					return 187;
				case 'ј':
					return 188;
				case 'Ѕ':
					return 189;
				case 'ѕ':
					return 190;
				case 'ї':
					return 191;
				case 'А':
					return 192;
				case 'Б':
					return 193;
				case 'В':
					return 194;
				case 'Г':
					return 195;
				case 'Д':
					return 196;
				case 'Е':
					return 197;
				case 'Ж':
					return 198;
				case 'З':
					return 199;
				case 'И':
					return 200;
				case 'Й':
					return 201;
				case 'К':
					return 202;
				case 'Л':
					return 203;
				case 'М':
					return 204;
				case 'Н':
					return 205;
				case 'О':
					return 206;
				case 'П':
					return 207;
				case 'Р':
					return 208;
				case 'С':
					return 209;
				case 'Т':
					return 210;
				case 'У':
					return 211;
				case 'Ф':
					return 212;
				case 'Х':
					return 213;
				case 'Ц':
					return 214;
				case 'Ч':
					return 215;
				case 'Ш':
					return 216;
				case 'Щ':
					return 217;
				case 'Ъ':
					return 218;
				case 'Ы':
					return 219;
				case 'Ь':
					return 220;
				case 'Э':
					return 221;
				case 'Ю':
					return 222;
				case 'Я':
					return 223;
				case 'а':
					return 224;
				case 'б':
					return 225;
				case 'в':
					return 226;
				case 'г':
					return 227;
				case 'д':
					return 228;
				case 'е':
					return 229;
				case 'ж':
					return 230;
				case 'з':
					return 231;
				case 'и':
					return 232;
				case 'й':
					return 233;
				case 'к':
					return 234;
				case 'л':
					return 235;
				case 'м':
					return 236;
				case 'н':
					return 237;
				case 'о':
					return 238;
				case 'п':
					return 239;
				case 'р':
					return 240;
				case 'с':
					return 241;
				case 'т':
					return 242;
				case 'у':
					return 243;
				case 'ф':
					return 244;
				case 'х':
					return 245;
				case 'ц':
					return 246;
				case 'ч':
					return 247;
				case 'ш':
					return 248;
				case 'щ':
					return 249;
				case 'ъ':
					return 250;
				case 'ы':
					return 251;
				case 'ь':
					return 252;
				case 'э':
					return 253;
				case 'ю':
					return 254;
				case 'я':
					return 255;

				// Все необрабатываемые символы
				default:
					return 0;
				}
			}

		/// <summary>
		/// Обработчик события запуска приложения
		/// </summary>
		protected override void OnStart ()
			{
			}

		/// <summary>
		/// Обработчик события перехода в ждущий режим
		/// </summary>
		protected override void OnSleep ()
			{
			}

		/// <summary>
		/// Обработчик события выхода из ждущего режима
		/// </summary>
		protected override void OnResume ()
			{
			}
		}
	}
