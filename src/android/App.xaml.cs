using System;
using System.Collections.Generic;
using Xamarin.Essentials;
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
		// Общие переменные
		private int masterFontSize = 18, tipsFontSize = 14;
		private Thickness margin = new Thickness (6);

		private ContentPage codesPage, errorsPage, aboutPage, 
			kktSnPage, fnSnPage, ofdINNPage;

		// Операторы
		private readonly KKTCodes kkmc = new KKTCodes ();
		private readonly KKTErrorsList kkme = new KKTErrorsList ();
		private int currentCodesKKT = 0, currentErrorsKKT = 0;

		// Переменные страниц
		private Label codesSelectionLabel, codesSourceTextLabel,
			codesResultTextLabel, codesHelpLabel, codesErrorLabel, codesResultText;
		private Button codesKKTButton;
		private Editor codesSourceText;

		private Label errorsSelectionLabel, errorsCodeLabel,
			errorsResultTextLabel, errorsResultText;
		private Button errorsKKTButton, errorsCodeButton;

		private Label aboutLabel;
		private Button appButton, updateButton, communityButton;

		private Switch onlyNewCodes, onlyNewErrors;
		private Label onlyNewCodesLabel, onlyNewErrorsLabel;

		private Label kktSNLabel, kktTypeLabel, fnSNLabel, fnTypeLabel,
			ofdINNLabel, ofdNameLabel;
		private Editor kktSN, fnSN, ofdINN;

		private readonly Color
			codesMasterBackColor = Color.FromHex ("#FFFFF0"),
			codesFieldBackColor = Color.FromHex ("#FFFFD0"),

			errorsMasterBackColor = Color.FromHex ("#FFF0F0"),
			errorsFieldBackColor = Color.FromHex ("#FFD0D0"),

			aboutMasterBackColor = Color.FromHex ("#F0FFF0"),
			aboutFieldBackColor = Color.FromHex ("#D0FFD0"),

			snMasterBackColor = Color.FromHex ("#F4E8FF"),
			snFieldBackColor = Color.FromHex ("#ECD8FF"),

			masterTextColor = Color.FromHex ("#000080"),
			masterHeaderColor = Color.FromHex ("#202020");

		// Вспомогательные методы
		private void ApplyPageSettings (ref ContentPage Page, string PageName, string PageTitle, Color PageBackColor)
			{
			Page = (ContentPage)MainPage.FindByName (PageName);
			Page.Title = PageTitle;
			Page.BackgroundColor = PageBackColor;

			ApplyHeaderLabelSettings (Page, PageTitle, PageBackColor);
			}

		private void ApplyLabelSettings (ContentPage ParentPage, ref Label ChildLabel, string LabelName,
			string LabelTitle, Color LabelTextColor)
			{
			ChildLabel = (Label)ParentPage.FindByName (LabelName);

			ChildLabel.Text = LabelTitle;
			ChildLabel.HorizontalOptions = LayoutOptions.Start;
			ChildLabel.FontAttributes = FontAttributes.None;
			ChildLabel.FontSize = masterFontSize;
			ChildLabel.TextColor = LabelTextColor;
			ChildLabel.Margin = margin;
			}

		private void ApplyButtonSettings (ContentPage ParentPage, ref Button ChildButton, string ButtonName,
			string ButtonTitle, Color ButtonColor, EventHandler ButtonMethod)
			{
			ChildButton = (Button)ParentPage.FindByName (ButtonName);

			ChildButton.BackgroundColor = ButtonColor;
			ChildButton.FontAttributes = FontAttributes.None;
			ChildButton.FontSize = masterFontSize;
			ChildButton.TextColor = masterTextColor;
			ChildButton.Margin = margin;
			ChildButton.Text = ButtonTitle;
			ChildButton.Clicked += ButtonMethod;
			}

		private void ApplyTipLabelSettings (ContentPage ParentPage, ref Label ChildLabel, string LabelName,
			string LabelTitle, Color LabelTextColor)
			{
			ChildLabel = (Label)ParentPage.FindByName (LabelName);

			ChildLabel.Text = LabelTitle;
			ChildLabel.HorizontalOptions = LayoutOptions.Center;
			ChildLabel.FontAttributes = FontAttributes.Italic;
			ChildLabel.FontSize = tipsFontSize;
			ChildLabel.TextColor = LabelTextColor;
			ChildLabel.HorizontalTextAlignment = TextAlignment.Center;
			ChildLabel.Margin = margin;
			}

		private void ApplyEditorSettings (ContentPage ParentPage, ref Editor ChildEditor, string EditorName,
			Color EditorColor, Keyboard EditorKeyboard, uint MaxLength, EventHandler<TextChangedEventArgs> EditMethod)
			{
			ChildEditor = (Editor)ParentPage.FindByName (EditorName);

			ChildEditor.AutoSize = EditorAutoSizeOption.TextChanges;
			ChildEditor.BackgroundColor = EditorColor;
			ChildEditor.FontAttributes = FontAttributes.None;
			ChildEditor.FontFamily = "Serif";
			ChildEditor.FontSize = masterFontSize;
			ChildEditor.HorizontalOptions = LayoutOptions.Fill;
			ChildEditor.Keyboard = EditorKeyboard;
			ChildEditor.MaxLength = (int)MaxLength;
			//ChildEditor.Placeholder = "...";
			//ChildEditor.PlaceholderColor = Color.FromRgb (255, 255, 0);
			ChildEditor.TextColor = masterTextColor;
			ChildEditor.Margin = margin;

			ChildEditor.Text = "";
			ChildEditor.TextChanged += EditMethod;
			}

		private void ApplyHeaderLabelSettings (ContentPage ParentPage, string LabelTitle, Color BackColor)
			{
			Label childLabel = (Label)ParentPage.FindByName ("HeaderLabel");

			childLabel.BackgroundColor = masterHeaderColor;
			childLabel.FontAttributes = FontAttributes.Bold;
			childLabel.FontSize = masterFontSize;
			childLabel.HorizontalTextAlignment = TextAlignment.Center;
			childLabel.HorizontalOptions = LayoutOptions.Fill;
			childLabel.Padding = margin;
			childLabel.Text = LabelTitle;
			childLabel.TextColor = BackColor;
			}

		/// <summary>
		/// Конструктор. Точка входа приложения
		/// </summary>
		public App ()
			{
			// Инициализация
			InitializeComponent ();

			// Общая конструкция страниц приложения
			MainPage = new MasterPage ();

			ApplyPageSettings (ref codesPage, "CodesPage", "Текст в коды символов ККТ", codesMasterBackColor);
			ApplyPageSettings (ref kktSnPage, "KKTSnPage", "Модель ККТ по зав. номеру", snMasterBackColor);
			ApplyPageSettings (ref fnSnPage, "FNSnPage", "Модель ФН по зав. номеру", snMasterBackColor);
			ApplyPageSettings (ref ofdINNPage, "OFDINNPage", "Название ОФД по ИНН", snMasterBackColor);
			ApplyPageSettings (ref errorsPage, "ErrorsPage", "Расшифровка ошибок ККТ", errorsMasterBackColor);
			ApplyPageSettings (ref aboutPage, "AboutPage", "О приложении", aboutMasterBackColor);

			#region Страница кодов
			ApplyLabelSettings (codesPage, ref codesSelectionLabel, "SelectionLabel",
				"Модель ККТ:", masterHeaderColor);

			onlyNewCodes = (Switch)codesPage.FindByName ("OnlyNewCodes");
			onlyNewCodes.IsToggled = true;
			onlyNewCodes.Toggled += OnlyNewCodes_Toggled;

			ApplyLabelSettings (codesPage, ref onlyNewCodesLabel, "OnlyNewCodesLabel",
				"Только новые", masterTextColor);

			ApplyButtonSettings (codesPage, ref codesKKTButton, "KKTButton",
				kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled)[currentCodesKKT],
				codesFieldBackColor, CodesKKTButton_Clicked);

			ApplyLabelSettings (codesPage, ref codesSourceTextLabel, "SourceTextLabel",
				"Исходный текст:", masterHeaderColor);

			ApplyEditorSettings (codesPage, ref codesSourceText, "SourceText",
				codesFieldBackColor, Keyboard.Default, 72, SourceText_TextChanged);

			ApplyLabelSettings (codesPage, ref codesResultTextLabel, "ResultTextLabel",
				"Коды ККТ:", masterHeaderColor);

			ApplyTipLabelSettings (codesPage, ref codesErrorLabel, "ErrorLabel",
				"Часть введённых символов не поддерживается данной ККТ или требует специальных действий для ввода",
				Color.FromHex ("#FF0000"));
			codesErrorLabel.IsVisible = false;

			ApplyLabelSettings (codesPage, ref codesResultText, "ResultText", "", masterTextColor);
			codesResultText.BackgroundColor = codesFieldBackColor;
			codesResultText.FontFamily = "Serif";
			codesResultText.HorizontalOptions = LayoutOptions.Fill;

			ApplyTipLabelSettings (codesPage, ref codesHelpLabel, "HelpLabel",
				kkmc.GetKKMTypeDescription ((uint)currentCodesKKT), Color.FromHex ("#404040"));
			#endregion

			#region Страница ошибок
			ApplyLabelSettings (errorsPage, ref errorsSelectionLabel, "SelectionLabel",
				"Модель ККТ:", masterHeaderColor);

			onlyNewErrors = (Switch)errorsPage.FindByName ("OnlyNewErrors");
			onlyNewErrors.IsToggled = true;
			onlyNewErrors.Toggled += OnlyNewErrors_Toggled;

			ApplyLabelSettings (errorsPage, ref onlyNewErrorsLabel, "OnlyNewErrorsLabel",
				"Только новые", masterTextColor);

			ApplyButtonSettings (errorsPage, ref errorsKKTButton, "KKTButton",
				kkme.GetKKTTypeNames (onlyNewErrors.IsToggled)[currentErrorsKKT], 
				errorsFieldBackColor, ErrorsKKTButton_Clicked);

			ApplyLabelSettings (errorsPage, ref errorsCodeLabel, "ErrorCodeLabel",
				"Код / сообщение:", masterHeaderColor);

			ApplyButtonSettings (errorsPage, ref errorsCodeButton, "ErrorCodeButton",
				"- Выберите код ошибки -", errorsFieldBackColor, ErrorsCodeButton_Clicked);

			ApplyLabelSettings (errorsPage, ref errorsResultTextLabel, "ResultTextLabel",
				"Расшифровка:", masterHeaderColor);

			ApplyLabelSettings (errorsPage, ref errorsResultText, "ResultText", "", masterTextColor);
			errorsResultText.BackgroundColor = errorsFieldBackColor;
			errorsResultText.FontFamily = "Serif";
			errorsResultText.HorizontalOptions = LayoutOptions.Fill;
			#endregion

			#region Страница "О программе"
			ApplyLabelSettings (aboutPage, ref aboutLabel, "AboutLabel",
				ProgramDescription.AssemblyTitle + "\n" +
				ProgramDescription.AssemblyDescription + "\n\n" +
				ProgramDescription.AssemblyCopyright + "\nv " +
				ProgramDescription.AssemblyVersion +
				"; " + ProgramDescription.AssemblyLastUpdate,
				Color.FromHex ("#000080"));
			aboutLabel.FontAttributes = FontAttributes.Bold;
			aboutLabel.HorizontalOptions = LayoutOptions.Fill;
			aboutLabel.HorizontalTextAlignment = TextAlignment.Center;

			ApplyButtonSettings (aboutPage, ref appButton, "AppPage",
				"Перейти на страницу проекта", aboutFieldBackColor, AppButton_Clicked);

			ApplyButtonSettings (aboutPage, ref updateButton, "UpdatePage",
				"Перейти на страницу анализатора данных ФН", aboutFieldBackColor, UpdateButton_Clicked);

			ApplyButtonSettings (aboutPage, ref communityButton, "CommunityPage",
				"RD AAOW Free utilities production lab", aboutFieldBackColor, CommunityButton_Clicked);
			#endregion

			#region Страница серийных номеров и ИНН
			ApplyLabelSettings (kktSnPage, ref kktSNLabel, "SNLabel", "Заводской номер ККТ:", masterHeaderColor);
			ApplyEditorSettings (kktSnPage, ref kktSN, "SN", snFieldBackColor, Keyboard.Numeric, 16, KKTSN_TextChanged);
			ApplyLabelSettings (kktSnPage, ref kktTypeLabel, "TypeLabel", "", masterTextColor);

			kktTypeLabel.BackgroundColor = snFieldBackColor;
			kktTypeLabel.FontFamily = "Serif";
			kktTypeLabel.HorizontalOptions = LayoutOptions.Fill;
			kktTypeLabel.HorizontalTextAlignment = TextAlignment.Center;

			//
			ApplyLabelSettings (fnSnPage, ref fnSNLabel, "SNLabel", "Заводской номер ФН:", masterHeaderColor);
			ApplyEditorSettings (fnSnPage, ref fnSN, "SN", snFieldBackColor, Keyboard.Numeric, 16, FNSN_TextChanged);
			ApplyLabelSettings (fnSnPage, ref fnTypeLabel, "TypeLabel", "", masterTextColor);

			fnTypeLabel.BackgroundColor = snFieldBackColor;
			fnTypeLabel.FontFamily = "Serif";
			fnTypeLabel.HorizontalOptions = LayoutOptions.Fill;
			fnTypeLabel.HorizontalTextAlignment = TextAlignment.Center;

			//
			ApplyLabelSettings (ofdINNPage, ref ofdINNLabel, "SNLabel", "ИНН ОФД:", masterHeaderColor);
			ApplyEditorSettings (ofdINNPage, ref ofdINN, "SN", snFieldBackColor, Keyboard.Numeric, 10, OFDINN_TextChanged);
			ApplyLabelSettings (ofdINNPage, ref ofdNameLabel, "TypeLabel", "", masterTextColor);

			ofdNameLabel.BackgroundColor = snFieldBackColor;
			ofdNameLabel.FontFamily = "Serif";
			ofdNameLabel.HorizontalOptions = LayoutOptions.Fill;
			ofdNameLabel.HorizontalTextAlignment = TextAlignment.Center;
			#endregion
			}

		// Изменение ИНН ОФД, ЗН ФН и ККТ
		private void KKTSN_TextChanged (object sender, TextChangedEventArgs e)
			{
			if (kktSN.Text != "")
				kktTypeLabel.Text = KKTSupport.GetKKTModel (kktSN.Text);
			else
				kktTypeLabel.Text = "";
			}

		private void FNSN_TextChanged (object sender, TextChangedEventArgs e)
			{
			if (fnSN.Text != "")
				fnTypeLabel.Text = KKTSupport.GetFNName (fnSN.Text);
			else
				fnTypeLabel.Text = "";
			}

		private void OFDINN_TextChanged (object sender, TextChangedEventArgs e)
			{
			if (ofdINN.Text != "")
				ofdNameLabel.Text = KKTSupport.GetOFDName (ofdINN.Text);
			else
				ofdNameLabel.Text = "";
			}

		// Переключение свичей
		private void OnlyNewCodes_Toggled (object sender, ToggledEventArgs e)
			{
			onlyNewCodesLabel.TextColor = e.Value ? masterTextColor : Color.FromHex ("#404040");
			}

		private void OnlyNewErrors_Toggled (object sender, ToggledEventArgs e)
			{
			onlyNewErrorsLabel.TextColor = e.Value ? masterTextColor : Color.FromHex ("#404040");
			}

		// Ввод текста
		private void SourceText_TextChanged (object sender, TextChangedEventArgs e)
			{
			codesResultText.Text = "";
			codesErrorLabel.IsVisible = !Decode ();

			codesSourceTextLabel.Text = "Исходный текст (" + codesSourceText.Text.Length.ToString () + "):";
			}

		// Выбор модели ККТ
		private async void CodesKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			string res = await codesPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
				kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled).ToArray ());

			// Установка модели
			if (kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled).IndexOf (res) < 0)
				return;

			codesKKTButton.Text = res;
			currentCodesKKT = kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled).IndexOf (res);
			codesHelpLabel.Text = kkmc.GetKKMTypeDescription ((uint)currentCodesKKT);

			SourceText_TextChanged (null, null);
			}

		// Функция трансляции строки в набор кодов
		private bool Decode ()
			{
			// Выполнение
			bool res = true;
			char[] text = codesSourceText.Text.ToCharArray ();

			for (int i = 0; i < codesSourceText.Text.Length; i++)
				{
				string s;

				if ((s = kkmc.GetCode ((uint)currentCodesKKT, CharToCP1251 (codesSourceText.Text[i]))) ==
					KKTCodes.EmptyCode)
					{
					codesResultText.Text += "xxx   ";
					res = false;
					}
				else
					{
					codesResultText.Text += (s + "   ");
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

		// Выбор модели ККТ
		private async void ErrorsKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			string res = await errorsPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
				kkme.GetKKTTypeNames (onlyNewErrors.IsToggled).ToArray ());

			// Установка модели
			if (kkme.GetKKTTypeNames (onlyNewErrors.IsToggled).IndexOf (res) < 0)
				return;

			errorsKKTButton.Text = res;
			currentErrorsKKT = kkme.GetKKTTypeNames (onlyNewErrors.IsToggled).IndexOf (res);

			List<string> list = kkme.GetErrorCodesList ((uint)currentErrorsKKT);
			errorsCodeButton.Text = list[0];

			errorsResultText.Text = kkme.GetErrorText ((uint)currentErrorsKKT, 0);
			list.Clear ();
			}

		// Выбор кода ошибки
		private async void ErrorsCodeButton_Clicked (object sender, EventArgs e)
			{
			// Запрос кода ошибки
			List<string> list = kkme.GetErrorCodesList ((uint)currentErrorsKKT);
			string res = await errorsPage.DisplayActionSheet ("Выберите код/сообщение ошибки:", "Отмена", null,
				list.ToArray ());

			// Установка результата
			if (list.IndexOf (res) >= 0)
				{
				errorsCodeButton.Text = res;
				errorsResultText.Text = kkme.GetErrorText ((uint)currentErrorsKKT, (uint)list.IndexOf (res));
				}

			list.Clear ();
			}

		// Страница обновлений
		private void UpdateButton_Clicked (object sender, EventArgs e)
			{
			Launcher.OpenAsync ("https://github.com/adslbarxatov/FNReader");
			}

		// Страница проекта
		private void AppButton_Clicked (object sender, EventArgs e)
			{
			Launcher.OpenAsync ("https://github.com/adslbarxatov/TextToKKT");
			}

		// Страница лаборатории
		private void CommunityButton_Clicked (object sender, EventArgs e)
			{
			Launcher.OpenAsync ("https://vk.com/rdaaow_fupl");
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
