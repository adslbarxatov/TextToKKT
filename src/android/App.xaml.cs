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
	public partial class App: Application
		{
		#region Настройки стилей отображения

		private readonly Color
			codesMasterBackColor = Color.FromHex ("#FFFFF0"),
			codesFieldBackColor = Color.FromHex ("#FFFFD0"),

			errorsMasterBackColor = Color.FromHex ("#FFF0F0"),
			errorsFieldBackColor = Color.FromHex ("#FFD0D0"),

			aboutMasterBackColor = Color.FromHex ("#F0FFF0"),
			aboutFieldBackColor = Color.FromHex ("#D0FFD0"),

			fnLifeMasterBackColor = Color.FromHex ("#FFF0E0"),
			fnLifeFieldBackColor = Color.FromHex ("#FFE0C0"),

			//snMasterBackColor = Color.FromHex ("#F4E8FF"),
			//snFieldBackColor = Color.FromHex ("#ECD8FF"),

			headersMasterBackColor = Color.FromHex ("#E8E8E8"),
			headersFieldBackColor = Color.FromHex ("#E0E0E0"),

			rnmMasterBackColor = Color.FromHex ("#F0FFFF"),
			rnmFieldBackColor = Color.FromHex ("#C8FFFF"),

			ofdMasterBackColor = Color.FromHex ("#F0F0FF"),
			ofdFieldBackColor = Color.FromHex ("#C8C8FF"),

			lowLevelMasterBackColor = Color.FromHex ("#FFF0FF"),
			lowLevelFieldBackColor = Color.FromHex ("#FFC8FF"),

			userManualsMasterBackColor = Color.FromHex ("#F4E8FF"),
			userManualsFieldBackColor = Color.FromHex ("#ECD8FF"),

			untoggledSwitchColor = Color.FromHex ("#505050"),
			errorColor = Color.FromHex ("#FF0000"),
			correctColor = Color.FromHex ("#008000");
		private const string firstStartRegKey = "HelpShownAt";

		#endregion

		#region Переменные страниц
		private ContentPage headersPage, codesPage, errorsPage, aboutPage,
			ofdPage, fnLifePage, rnmPage, lowLevelPage, userManualsPage;

		private Label codesSourceTextLabel, codesHelpLabel, codesErrorLabel, codesResultText,
			errorsResultText,
			aboutLabel,
			fnLifeLabel, fnLifeModelLabel, fnLifeGenericTaxLabel, fnLifeGoodsLabel,
			rnmKKTTypeLabel, rnmINNCheckLabel, rnmRNMCheckLabel,
			lowLevelCommandDescr, unlockLabel;
		private List<Label> operationTextLabels = new List<Label> ();

		private Button codesKKTButton, fnLifeResult,
			errorsKKTButton, errorsCodeButton, userManualsKKTButton,
			ofdNameButton, ofdDNSNameButton, ofdIPButton, ofdPortButton, ofdEmailButton, ofdSiteButton,
			lowLevelCommand, lowLevelCommandCode, rnmGenerate;

		private Editor codesSourceText, errorSearchText,
			ofdINN, unlockField,
			fnLifeSerial,
			rnmKKTSN, rnmINN, rnmRNM;

		private Switch onlyNewCodes, onlyNewErrors,
			fnLife13, fnLifeGenericTax, fnLifeGoods, fnLifeSeason, fnLifeAgents, fnLifeExcise, fnLifeAutonomous,
			lowLevelSHTRIH,
			keepAppState;

		private DatePicker fnLifeStartDate;

		#endregion

		private ConfigAccessor ca;

		// Локальный оформитель страниц приложения
		private ContentPage ApplyPageSettings (string PageName, string PageTitle, Color PageBackColor, uint HeaderNumber)
			{
			// Инициализация страницы
			ContentPage page = AndroidSupport.ApplyPageSettings (MainPage, PageName, PageTitle, PageBackColor);

			// Добавление в содержание
			if (HeaderNumber > 0)
				{
				Button b = AndroidSupport.ApplyButtonSettings (headersPage, "Button" + HeaderNumber.ToString ("D02"), PageTitle,
					headersFieldBackColor, HeaderButton_Clicked);
				b.Margin = b.Padding = new Thickness (1);
				b.CommandParameter = page;
				b.IsVisible = true;
				}

			return page;
			}

		/// <summary>
		/// Конструктор. Точка входа приложения
		/// </summary>
		public App ()
			{
			// Инициализация
			InitializeComponent ();
			ca = new ConfigAccessor (0, 0);
			um = new UserManuals (ca.ExtendedFunctions);

			#region Общая конструкция страниц приложения

			MainPage = new MasterPage ();

			uint headerNumber = 0;
			headersPage = ApplyPageSettings ("HeadersPage", "Функции приложения",
				headersMasterBackColor, headerNumber++);

			userManualsPage = ApplyPageSettings ("UserManualsPage", "Инструкции по работе с ККТ",
				userManualsMasterBackColor, headerNumber++);
			errorsPage = ApplyPageSettings ("ErrorsPage", "Расшифровать код ошибки ККТ",
				errorsMasterBackColor, headerNumber++);
			fnLifePage = ApplyPageSettings ("FNLifePage", "Определить срок жизни ФН",
				fnLifeMasterBackColor, headerNumber++);
			rnmPage = ApplyPageSettings ("RNMPage", "Проверить / сгенерировать РНМ",
				rnmMasterBackColor, headerNumber++);
			ofdPage = ApplyPageSettings ("OFDPage", "Запросить параметры ОФД",
				ofdMasterBackColor, headerNumber++);

			lowLevelPage = ApplyPageSettings ("LowLevelPage", "Команды нижнего уровня",
				lowLevelMasterBackColor, headerNumber++);
			lowLevelPage.IsEnabled = ca.AllowExtendedFunctionsLevel2;

			codesPage = ApplyPageSettings ("CodesPage", "Перевести текст в коды ККТ",
				codesMasterBackColor, headerNumber++);
			codesPage.IsEnabled = ca.AllowExtendedFunctionsLevel1;

			aboutPage = ApplyPageSettings ("AboutPage", "О приложении",
				aboutMasterBackColor, headerNumber);

			#endregion

			#region Страница «оглавления»

			keepAppState = (Switch)headersPage.FindByName ("KeepAppState");
			keepAppState.IsToggled = ca.KeepApplicationState;

			AndroidSupport.ApplyLabelSettingsForKKT (headersPage, "KeepAppStateLabel", "Помнить настройки приложения", true);

			try
				{
				((CarouselPage)MainPage).CurrentPage = ((CarouselPage)MainPage).Children[(int)ca.CurrentTab];
				}
			catch { }

			#endregion

			#region Страница инструкций

			AndroidSupport.ApplyLabelSettingsForKKT (userManualsPage, "SelectionLabel", "Модель ККТ:", true);

			for (int i = 0; i < um.OperationTypes.Length; i++)
				{
				Label l = AndroidSupport.ApplyLabelSettingsForKKT (userManualsPage, "OperationLabel" + (i + 1).ToString ("D2"),
					um.OperationTypes[i], true);
				l.IsVisible = true;

				operationTextLabels.Add (AndroidSupport.ApplyResultLabelSettings (userManualsPage,
					"OperationText" + (i + 1).ToString ("D2"), "   ", userManualsFieldBackColor));
				operationTextLabels[operationTextLabels.Count - 1].HorizontalTextAlignment = TextAlignment.Start;
				operationTextLabels[operationTextLabels.Count - 1].IsVisible = true;
				}

			userManualsKKTButton = AndroidSupport.ApplyButtonSettings (userManualsPage, "KKTButton",
				"   ", userManualsFieldBackColor, UserManualsKKTButton_Clicked);
			AndroidSupport.ApplyTipLabelSettings (userManualsPage, "HelpLabel",
				"<...> – индикация на экране, [...] – клавиши ККТ", untoggledSwitchColor);

			UserManualsKKTButton_Clicked (null, null);

			#endregion

			#region Страница кодов

			if (!ca.AllowExtendedFunctionsLevel1)
				{
				codesFieldBackColor = codesMasterBackColor = Color.FromRgb (128, 128, 128);
				codesPage.BackgroundColor = Color.FromRgb (192, 192, 192);
				}

			AndroidSupport.ApplyLabelSettingsForKKT (codesPage, "SelectionLabel", "Модель ККТ:", true);

			onlyNewCodes = (Switch)codesPage.FindByName ("OnlyNewCodes");
			if (ca.AllowExtendedFunctionsLevel2)
				{
				onlyNewCodes.IsToggled = ca.OnlyNewKKTCodes;
				onlyNewCodes.Toggled += OnlyNewCodes_Toggled;
				}
			else
				{
				onlyNewCodes.IsToggled = true;
				onlyNewCodes.IsEnabled = false;
				}

			AndroidSupport.ApplyLabelSettingsForKKT (codesPage, "OnlyNewCodesLabel", "Только новые", false);

			codesKKTButton = AndroidSupport.ApplyButtonSettings (codesPage, "KKTButton",
				kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled)[(int)ca.KKTForCodes],
				codesFieldBackColor, CodesKKTButton_Clicked);

			codesSourceTextLabel = AndroidSupport.ApplyLabelSettingsForKKT (codesPage, "SourceTextLabel",
				"Исходный текст:", true);

			codesSourceText = AndroidSupport.ApplyEditorSettings (codesPage, "SourceText",
				codesFieldBackColor, Keyboard.Default, 72, ca.CodesText, SourceText_TextChanged);
			codesSourceText.HorizontalOptions = LayoutOptions.Fill;

			AndroidSupport.ApplyLabelSettingsForKKT (codesPage, "ResultTextLabel", "Коды ККТ:", true);

			codesErrorLabel = AndroidSupport.ApplyTipLabelSettings (codesPage, "ErrorLabel",
				"Часть введённых символов не поддерживается данной ККТ или требует специальных действий для ввода",
				errorColor);

			codesResultText = AndroidSupport.ApplyResultLabelSettings (codesPage, "ResultText", "", codesFieldBackColor);
			codesResultText.HorizontalTextAlignment = TextAlignment.Start;

			codesHelpLabel = AndroidSupport.ApplyTipLabelSettings (codesPage, "HelpLabel",
				kkmc.GetKKTTypeDescription (ca.KKTForCodes), untoggledSwitchColor);

			SourceText_TextChanged (null, null);    // Протягивание кодов

			#endregion

			#region Страница ошибок

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "SelectionLabel", "Модель ККТ:", true);

			onlyNewErrors = (Switch)errorsPage.FindByName ("OnlyNewErrors");
			if (ca.AllowExtendedFunctionsLevel2)
				{
				onlyNewErrors.IsToggled = ca.OnlyNewKKTErrors;
				onlyNewErrors.Toggled += OnlyNewErrors_Toggled;
				}
			else
				{
				onlyNewErrors.IsToggled = true;
				onlyNewErrors.IsEnabled = false;
				}

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "OnlyNewErrorsLabel", "Только новые", false);

			errorsKKTButton = AndroidSupport.ApplyButtonSettings (errorsPage, "KKTButton",
				kkme.GetKKTTypeNames (onlyNewErrors.IsToggled)[(int)ca.KKTForErrors],
				errorsFieldBackColor, ErrorsKKTButton_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "ErrorCodeLabel", "Код / сообщение:", true);

			errorsCodeButton = AndroidSupport.ApplyButtonSettings (errorsPage, "ErrorCodeButton",
				kkme.GetErrorCodesList (ca.KKTForErrors)[(int)ca.ErrorCode],
				errorsFieldBackColor, ErrorsCodeButton_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "ResultTextLabel", "Расшифровка:", true);

			errorsResultText = AndroidSupport.ApplyResultLabelSettings (errorsPage, "ResultText",
				kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode), errorsFieldBackColor);
			errorsResultText.HorizontalTextAlignment = TextAlignment.Start;

			errorSearchText = AndroidSupport.ApplyEditorSettings (errorsPage, "ErrorSearchText", errorsFieldBackColor,
				Keyboard.Default, 30, "", null);
			AndroidSupport.ApplyButtonSettings (errorsPage, "ErrorSearchButton",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Find),
				errorsFieldBackColor, Errors_Find);

			#endregion

			#region Страница "О программе"

			aboutLabel = AndroidSupport.ApplyLabelSettings (aboutPage, "AboutLabel",
				ProgramDescription.AssemblyTitle + "\n" +
				ProgramDescription.AssemblyDescription + "\n\n" +
				ProgramDescription.AssemblyCopyright + "\nv " +
				ProgramDescription.AssemblyVersion +
				"; " + ProgramDescription.AssemblyLastUpdate);
			aboutLabel.FontAttributes = FontAttributes.Bold;
			aboutLabel.HorizontalTextAlignment = TextAlignment.Center;

			AndroidSupport.ApplyButtonSettings (aboutPage, "AppPage", "Страница проекта",
				aboutFieldBackColor, AppButton_Clicked);
			AndroidSupport.ApplyButtonSettings (aboutPage, "ADPPage", "Политика разработки и EULA",
				aboutFieldBackColor, ADPButton_Clicked);
			AndroidSupport.ApplyButtonSettings (aboutPage, "DevPage", "Спросить разработчика",
				aboutFieldBackColor, DevButton_Clicked);

			AndroidSupport.ApplyButtonSettings (aboutPage, "UpdatePage",
				"Инструмент чтения данных ФН FNReader", aboutFieldBackColor, UpdateButton_Clicked);
			AndroidSupport.ApplyButtonSettings (aboutPage, "CommunityPage",
				AndroidSupport.MasterLabName, aboutFieldBackColor, CommunityButton_Clicked);

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				unlockLabel = AndroidSupport.ApplyLabelSettingsForKKT (aboutPage, "UnlockLabel", ca.LockMessage, false);
				unlockLabel.IsVisible = true;
				unlockField = AndroidSupport.ApplyEditorSettings (aboutPage, "UnlockField", aboutFieldBackColor,
					Keyboard.Default, 32, "", UnlockMethod);
				unlockField.IsVisible = true;
				}

			#endregion

			#region Страница определения срока жизни ФН

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetModelLabel", "Укажите ЗН ФН или его номинал:",
				true);
			fnLifeSerial = AndroidSupport.ApplyEditorSettings (fnLifePage, "FNLifeSerial", fnLifeFieldBackColor,
				Keyboard.Numeric, 16, ca.FNSerial, FNLifeSerial_TextChanged);

			fnLife13 = (Switch)fnLifePage.FindByName ("FNLife13");
			fnLife13.Toggled += FnLife13_Toggled;
			fnLife13.ThumbColor = untoggledSwitchColor;
			fnLife13.OnColor = fnLifeFieldBackColor;

			fnLifeLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeLabel", "", false);

			//
			fnLifeModelLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeModelLabel", "", false);

			fnLifeModelLabel.BackgroundColor = fnLifeFieldBackColor;
			fnLifeModelLabel.FontFamily = "Serif";
			fnLifeModelLabel.HorizontalOptions = LayoutOptions.Fill;
			fnLifeModelLabel.HorizontalTextAlignment = TextAlignment.Center;

			//
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetUserParameters", "Укажите значимые параметры:", true);

			//
			fnLifeGenericTax = (Switch)fnLifePage.FindByName ("FNLifeGenericTax");
			fnLifeGenericTax.IsToggled = !ca.GenericTaxFlag;
			fnLifeGenericTax.Toggled += FnLife13_Toggled;
			fnLifeGenericTax.ThumbColor = untoggledSwitchColor;
			fnLifeGenericTax.OnColor = fnLifeFieldBackColor;

			fnLifeGenericTaxLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeGenericTaxLabel", "", false);

			//
			fnLifeGoods = (Switch)fnLifePage.FindByName ("FNLifeGoods");
			fnLifeGoods.IsToggled = !ca.GoodsFlag;
			fnLifeGoods.Toggled += FnLife13_Toggled;
			fnLifeGoods.ThumbColor = untoggledSwitchColor;
			fnLifeGoods.OnColor = fnLifeFieldBackColor;

			fnLifeGoodsLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeGoodsLabel", "", false);

			//
			fnLifeSeason = (Switch)fnLifePage.FindByName ("FNLifeSeason");
			fnLifeSeason.IsToggled = ca.SeasonFlag;
			fnLifeSeason.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeSeasonLabel", "Сезонная торговля", false);

			//
			fnLifeAgents = (Switch)fnLifePage.FindByName ("FNLifeAgents");
			fnLifeAgents.IsToggled = ca.AgentsFlag;
			fnLifeAgents.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeAgentsLabel", "Платёжный (суб)агент", false);

			//
			fnLifeExcise = (Switch)fnLifePage.FindByName ("FNLifeExcise");
			fnLifeExcise.IsToggled = ca.ExciseFlag;
			fnLifeExcise.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeExciseLabel", "Подакцизный товар", false);

			//
			fnLifeAutonomous = (Switch)fnLifePage.FindByName ("FNLifeAutonomous");
			fnLifeAutonomous.IsToggled = ca.AutonomousFlag;
			fnLifeAutonomous.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeAutonomousLabel", "Автономный режим", false);

			//
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetDate", "Дата фискализации:", false);
			fnLifeStartDate = AndroidSupport.ApplyDatePickerSettings (fnLifePage, "FNLifeStartDate", fnLifeFieldBackColor,
				FnLifeStartDate_DateSelected);

			//
			fnLifeResult = AndroidSupport.ApplyButtonSettings (fnLifePage, "FNLifeResult", "", fnLifeFieldBackColor,
				FNLifeResultCopy, false);

			// Применение всех названий
			FNLifeSerial_TextChanged (null, null);

			#endregion

			#region Страница определения корректности РНМ

			AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "SNLabel", "Заводской номер ККТ:", true);
			rnmKKTSN = AndroidSupport.ApplyEditorSettings (rnmPage, "SN", rnmFieldBackColor, Keyboard.Numeric, 20,
				ca.KKTSerial, RNM_TextChanged);
			rnmKKTTypeLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "TypeLabel", "", false);

			AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "INNLabel", "ИНН пользователя:", true);
			rnmINN = AndroidSupport.ApplyEditorSettings (rnmPage, "INN", rnmFieldBackColor, Keyboard.Numeric, 12,
				ca.UserINN, RNM_TextChanged);
			rnmINNCheckLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "INNCheckLabel", "", false);

			if (ca.AllowExtendedFunctionsLevel2)
				AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMLabel",
					"Регистрационный номер для проверки или произвольное число для генерации¹:", true);
			else
				AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMLabel",
					"Регистрационный номер для проверки:", true);

			rnmRNM = AndroidSupport.ApplyEditorSettings (rnmPage, "RNM", rnmFieldBackColor, Keyboard.Numeric, 16,
				ca.RNMKKT, RNM_TextChanged);
			rnmRNMCheckLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMCheckLabel", "", false);

			rnmGenerate = AndroidSupport.ApplyButtonSettings (rnmPage, "RNMGenerate", "Сгенерировать",
				rnmFieldBackColor, RNMGenerate_Clicked);
			rnmGenerate.IsVisible = ca.AllowExtendedFunctionsLevel2;

			if (ca.AllowExtendedFunctionsLevel2)
				AndroidSupport.ApplyTipLabelSettings (rnmPage, "RNMAbout",
					"¹ Первые 10 цифр РНМ являются порядковым номером ККТ в реестре и могут быть указаны вручную при генерации",
					untoggledSwitchColor);

			RNM_TextChanged (null, null);   // Применение значений

			#endregion

			#region Страница настроек ОФД

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDINNLabel", "ИНН ОФД:", true);
			ofdINN = AndroidSupport.ApplyEditorSettings (ofdPage, "OFDINN", ofdFieldBackColor, Keyboard.Numeric, 10,
				ca.OFDINN, OFDINN_TextChanged);
			AndroidSupport.ApplyButtonSettings (ofdPage, "OFDINNCopy",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Copy),
				ofdFieldBackColor, OFDINNCopy_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDNameLabel", "Название:", true);
			ofdNameButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDName", "- Выберите или введите ИНН -",
				ofdFieldBackColor, OFDName_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameLabel", "Адрес:", true);
			ofdDNSNameButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSName", "", ofdFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDIPLabel", "IP:", true);
			ofdIPButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDIP", "", ofdFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDPortLabel", "Порт:", true);
			ofdPortButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPort", "", ofdFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDEmailLabel", "E-mail:", true);
			ofdEmailButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDEmail", "", ofdFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDSiteLabel", "Сайт:", true);
			ofdSiteButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDSite", "", ofdFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDNalogSiteLabel", "Сайт ФНС:", true);
			AndroidSupport.ApplyButtonSettings (ofdPage, "OFDNalogSite", "www.nalog.ru", ofdFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyTipLabelSettings (ofdPage, "OFDHelpLabel",
				"Нажатие кнопок копирует их подписи в буфер обмена", untoggledSwitchColor);

			OFDINN_TextChanged (null, null); // Протягивание значений

			#endregion

			#region Страница команд нижнего уровня

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				lowLevelFieldBackColor = lowLevelMasterBackColor = Color.FromRgb (128, 128, 128);
				lowLevelPage.BackgroundColor = Color.FromRgb (192, 192, 192);
				}

			lowLevelSHTRIH = (Switch)lowLevelPage.FindByName ("SHTRIHSwitch");
			lowLevelSHTRIH.IsToggled = !ca.LowLevelCommandsATOL;
			lowLevelSHTRIH.Toggled += LowLevelSHTRIH_Toggled;
			lowLevelSHTRIH.ThumbColor = untoggledSwitchColor;
			lowLevelSHTRIH.OnColor = fnLifeFieldBackColor;

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "AtolLabel", "АТОЛ", false);
			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "ShtrihLabel", "ШТРИХ", false);

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandLabel", "Команда:", true);
			lowLevelCommand = AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandButton",
				ca.LowLevelCommandsATOL ? ll.GetATOLCommandsList ()[(int)ca.LowLevelCode] :
				ll.GetSHTRIHCommandsList ()[(int)ca.LowLevelCode],
				lowLevelFieldBackColor, LowLevelCommandCodeButton_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandCodeLabel", "Код команды:", true);
			lowLevelCommandCode = AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandCodeButton",
				ca.LowLevelCommandsATOL ? ll.GetATOLCommand (ca.LowLevelCode, false) :
				ll.GetSHTRIHCommand (ca.LowLevelCode, false),
				lowLevelFieldBackColor, Field_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandDescrLabel", "Описание:", true);

			lowLevelCommandDescr = AndroidSupport.ApplyResultLabelSettings (lowLevelPage, "CommandDescr",
				ca.LowLevelCommandsATOL ? ll.GetATOLCommand (ca.LowLevelCode, true) :
				ll.GetSHTRIHCommand (ca.LowLevelCode, true), lowLevelFieldBackColor);
			lowLevelCommandDescr.HorizontalTextAlignment = TextAlignment.Start;

			AndroidSupport.ApplyTipLabelSettings (lowLevelPage, "LowLevelHelpLabel",
				"Нажатие кнопки копирует команду в буфер обмена", untoggledSwitchColor);

			#endregion

			// Обязательное принятие Политики и EULA
			AcceptPolicy ();
			}

		// Контроль принятия Политики и EULA
		private async void AcceptPolicy ()
			{
			if (Preferences.Get (firstStartRegKey, "") != "")
				return;

			while (await ((CarouselPage)MainPage).CurrentPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Перед началом работы с этим инструментом Вы должны принять Политику разработки приложений и " +
					"Пользовательское соглашение. Хотите открыть их тексты в браузере?\r\n\r\n" +
					"• Нажмите «Принять», если Вы уже ознакомились и полностью приняли их;\r\n" +
					"• Нажмите «Читать», если хотите открыть их в браузере;\r\n" +
					"• Чтобы отклонить их, закройте приложение",

					"Читать", "Принять"))
				{
				ADPButton_Clicked (null, null);
				}

			Preferences.Set (firstStartRegKey, ProgramDescription.AssemblyVersion); // Только после принятия
			}

		// Сброс списков ККТ и ошибок
		private void OnlyNewErrors_Toggled (object sender, ToggledEventArgs e)
			{
			ca.KKTForErrors = ca.ErrorCode = 0;
			errorsKKTButton.Text = kkme.GetKKTTypeNames (onlyNewErrors.IsToggled)[(int)ca.KKTForErrors];

			List<string> list = kkme.GetErrorCodesList (ca.KKTForErrors);
			errorsCodeButton.Text = list[(int)ca.ErrorCode];
			list.Clear ();
			}

		private void OnlyNewCodes_Toggled (object sender, ToggledEventArgs e)
			{
			ca.KKTForCodes = 0;
			codesKKTButton.Text = kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled)[(int)ca.KKTForCodes];
			}

		// Выбор команды нижнего уровня
		private LowLevel ll = new LowLevel ();
		private async void LowLevelCommandCodeButton_Clicked (object sender, EventArgs e)
			{
			// Запрос кода ошибки
			List<string> list = (lowLevelSHTRIH.IsToggled ? ll.GetSHTRIHCommandsList () : ll.GetATOLCommandsList ());
			string res = list[0];
			if (e != null)
				res = await lowLevelPage.DisplayActionSheet ("Выберите команду:", "Отмена", null,
				   list.ToArray ());

			// Установка результата
			int i = 0;
			if ((e == null) || ((i = list.IndexOf (res)) >= 0))
				{
				ca.LowLevelCode = (uint)i;
				lowLevelCommand.Text = res;

				lowLevelCommandCode.Text = (lowLevelSHTRIH.IsToggled ? ll.GetSHTRIHCommand ((uint)i, false) :
					ll.GetATOLCommand ((uint)i, false));
				lowLevelCommandDescr.Text = (lowLevelSHTRIH.IsToggled ? ll.GetSHTRIHCommand ((uint)i, true) :
					ll.GetATOLCommand ((uint)i, true));
				}

			list.Clear ();
			}

		// Выбор списка команд
		private void LowLevelSHTRIH_Toggled (object sender, ToggledEventArgs e)
			{
			ca.LowLevelCommandsATOL = !lowLevelSHTRIH.IsToggled;
			LowLevelCommandCodeButton_Clicked (sender, null);
			}

		// Ввод ЗН ФН в разделе определения срока жизни
		private void FNLifeSerial_TextChanged (object sender, TextChangedEventArgs e)
			{
			// Получение описания
			if (fnLifeSerial.Text != "")
				fnLifeModelLabel.Text = KKTSupport.GetFNName (fnLifeSerial.Text);
			else
				fnLifeModelLabel.Text = "(введите ЗН ФН)";

			// Определение длины ключа
			if (fnLifeModelLabel.Text.Contains ("(13)") || fnLifeModelLabel.Text.Contains ("(15)"))
				{
				fnLife13.IsToggled = false;
				fnLife13.IsVisible = fnLife13.IsEnabled = false;
				}
			else if (fnLifeModelLabel.Text.Contains ("(36)"))
				{
				fnLife13.IsToggled = true;
				fnLife13.IsVisible = fnLife13.IsEnabled = false;
				}
			else
				{
				fnLife13.IsVisible = fnLife13.IsEnabled = true;
				}

			// Принудительное изменение
			FnLife13_Toggled (null, null);
			}

		// Изменение параметров пользователя и даты
		private void FnLifeStartDate_DateSelected (object sender, DateChangedEventArgs e)
			{
			FnLife13_Toggled (null, null);
			}

		private void FnLife13_Toggled (object sender, ToggledEventArgs e)
			{
			// Обновление состояний
			if (fnLife13.IsToggled)
				fnLifeLabel.Text = "36 месяцев";
			else
				fnLifeLabel.Text = "13/15 месяцев";

			if (fnLifeGenericTax.IsToggled)
				fnLifeGenericTaxLabel.Text = "УСН / ЕНВД / ЕСХН / ПСН";
			else
				fnLifeGenericTaxLabel.Text = "ОСН / совмещение с ОСН";

			if (fnLifeGoods.IsToggled)
				fnLifeGoodsLabel.Text = "услуги";
			else
				fnLifeGoodsLabel.Text = "товары";

			// Расчёт срока
			string res = KKTSupport.GetFNLifeEndDate (fnLifeStartDate.Date, !fnLife13.IsToggled,
				fnLifeModelLabel.Text.Contains ("(13)"), !fnLifeGenericTax.IsToggled, !fnLifeGoods.IsToggled,
				fnLifeSeason.IsToggled || fnLifeAgents.IsToggled, fnLifeExcise.IsToggled, fnLifeAutonomous.IsToggled);

			fnLifeResult.Text = "ФН прекратит работу ";
			if (res.Contains ("!"))
				{
				fnLifeResult.TextColor = errorColor;
				fnLifeResultDate = res.Substring (1);
				fnLifeResult.Text += (fnLifeResultDate + "\n(выбранный ФН неприменим с указанными параметрами)");
				}
			else
				{
				fnLifeResult.TextColor = fnLifeStartDate.TextColor;
				fnLifeResultDate = res;
				fnLifeResult.Text += res;
				}
			}

		// Изменение ИНН ОФД и РНМ ККТ
		private void RNM_TextChanged (object sender, TextChangedEventArgs e)
			{
			// ЗН ККТ
			if (rnmKKTSN.Text != "")
				rnmKKTTypeLabel.Text = KKTSupport.GetKKTModel (rnmKKTSN.Text);
			else
				rnmKKTTypeLabel.Text = "";

			// ИНН пользователя
			if (rnmINN.Text.Length < 10)
				{
				rnmINNCheckLabel.TextColor = rnmINN.TextColor;
				rnmINNCheckLabel.Text = "неполный";
				}
			else if (KKTSupport.CheckINN (rnmINN.Text))
				{
				rnmINNCheckLabel.TextColor = correctColor;
				rnmINNCheckLabel.Text = "ОК";
				}
			else
				{
				rnmINNCheckLabel.TextColor = errorColor;
				rnmINNCheckLabel.Text = "некорректный";
				}
			rnmINNCheckLabel.Text += (" (" + KKTSupport.GetRegionName (rnmINN.Text) + ")");

			// РНМ
			if (rnmRNM.Text.Length < 10)
				{
				rnmRNMCheckLabel.TextColor = rnmRNM.TextColor;
				rnmRNMCheckLabel.Text = "неполный";
				}
			else if (KKTSupport.GetFullRNM (rnmINN.Text, rnmKKTSN.Text, rnmRNM.Text.Substring (0, 10)) == rnmRNM.Text)
				{
				rnmRNMCheckLabel.TextColor = correctColor;
				rnmRNMCheckLabel.Text = "OK";
				}
			else
				{
				rnmRNMCheckLabel.TextColor = errorColor;
				rnmRNMCheckLabel.Text = "некорректный";
				}
			}

		private readonly OFD ofd = new OFD ();
		private void OFDINN_TextChanged (object sender, TextChangedEventArgs e)
			{
			List<string> parameters = ofd.GetOFDParameters (ofdINN.Text);

			ofdNameButton.Text = parameters[1];
			ofdDNSNameButton.Text = parameters[2];
			ofdIPButton.Text = parameters[3];
			ofdPortButton.Text = parameters[4];
			ofdEmailButton.Text = parameters[5];
			ofdSiteButton.Text = parameters[6];
			}

		private async void OFDName_Clicked (object sender, EventArgs e)
			{
			// Запрос ОФД по имени
			List<string> list = ofd.GetOFDNames ();
			string res = await ofdPage.DisplayActionSheet ("Выберите название ОФД:", "Отмена", null,
				list.ToArray ());

			// Установка результата
			if (list.IndexOf (res) >= 0)
				{
				ofdNameButton.Text = res;
				SendToClipboard (res.Replace ('«', '\"').Replace ('»', '\"'));
				string s = ofd.GetOFDINNByName (ofdNameButton.Text);
				if (s != "")
					ofdINN.Text = s;
				}
			}

		// Отправка значения в буфер обмена
		private void Field_Clicked (object sender, EventArgs e)
			{
			SendToClipboard (((Button)sender).Text);
			}

		private void OFDINNCopy_Clicked (object sender, EventArgs e)
			{
			SendToClipboard (ofdINN.Text);
			}

		private string fnLifeResultDate = "";
		private void FNLifeResultCopy (object sender, EventArgs e)
			{
			SendToClipboard (fnLifeResultDate);
			}

		private void SendToClipboard (string Text)
			{
			try
				{
				Clipboard.SetTextAsync (Text);
				}
			catch
				{
				}
			}

		// Ввод текста
		private void SourceText_TextChanged (object sender, TextChangedEventArgs e)
			{
			codesResultText.Text = "";
			codesErrorLabel.IsVisible = !Decode ();

			codesSourceTextLabel.Text = "Исходный текст (" + codesSourceText.Text.Length.ToString () + "):";
			}

		// Выбор модели ККТ
		private readonly KKTCodes kkmc = new KKTCodes ();
		private async void CodesKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			string res = await codesPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
				kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled).ToArray ());

			// Установка модели
			int i;
			if ((i = kkmc.GetKKTTypeNames (onlyNewCodes.IsToggled).IndexOf (res)) < 0)
				return;

			codesKKTButton.Text = res;
			ca.KKTForCodes = (uint)i;
			codesHelpLabel.Text = kkmc.GetKKTTypeDescription (ca.KKTForCodes);

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

				if ((s = kkmc.GetCode (ca.KKTForCodes, CharToCP1251 (codesSourceText.Text[i]))) ==
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
		private readonly KKTErrorsList kkme = new KKTErrorsList ();
		private async void ErrorsKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			string res = await errorsPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
				kkme.GetKKTTypeNames (onlyNewErrors.IsToggled).ToArray ());

			// Установка модели
			if (kkme.GetKKTTypeNames (onlyNewErrors.IsToggled).IndexOf (res) < 0)
				return;

			errorsKKTButton.Text = res;
			ca.KKTForErrors = (uint)kkme.GetKKTTypeNames (onlyNewErrors.IsToggled).IndexOf (res);

			List<string> list = kkme.GetErrorCodesList (ca.KKTForErrors);
			errorsCodeButton.Text = list[0];

			ca.ErrorCode = 0;
			errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
			list.Clear ();
			}

		// Выбор кода ошибки
		private async void ErrorsCodeButton_Clicked (object sender, EventArgs e)
			{
			// Запрос кода ошибки
			List<string> list = kkme.GetErrorCodesList (ca.KKTForErrors);
			string res = await errorsPage.DisplayActionSheet ("Выберите код/сообщение ошибки:", "Отмена", null,
				list.ToArray ());

			// Установка результата
			if (list.IndexOf (res) >= 0)
				{
				errorsCodeButton.Text = res;
				ca.ErrorCode = (uint)list.IndexOf (res);
				errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
				}

			list.Clear ();
			}

		// Страница обновлений
		private async void UpdateButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				Launcher.OpenAsync (AndroidSupport.MasterGitLink + "FNReader");
				}
			catch
				{
				await aboutPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Веб-браузер отсутствует на этом устройстве", "OK");
				}
			}

		// Страница проекта
		private async void AppButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				Launcher.OpenAsync (AndroidSupport.MasterGitLink + "TextToKKT");
				}
			catch
				{
				await aboutPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Веб-браузер отсутствует на этом устройстве", "OK");
				}
			}

		// Страница политики и EULA
		private async void ADPButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				Launcher.OpenAsync (AndroidSupport.ADPLink);
				}
			catch
				{
				await aboutPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Веб-браузер отсутствует на этом устройстве", "OK");
				}
			}

		// Страница лаборатории
		private async void CommunityButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				if (await aboutPage.DisplayAlert (ProgramDescription.AssemblyTitle,
						"Выберите сообщество:", "ВКонтакте", "Телеграм"))
					Launcher.OpenAsync (AndroidSupport.CommunityFrontPage);
				else
					Launcher.OpenAsync (AndroidSupport.CommunityInTelegram);
				}
			catch
				{
				await aboutPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Веб-браузер отсутствует на этом устройстве", "OK");
				}
			}

		// Страница политики и EULA
		private async void DevButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				EmailMessage message = new EmailMessage
					{
					Subject = "Wish, advice or bug in " + ProgramDescription.AssemblyTitle,
					Body = "",
					To = new List<string> () { AndroidSupport.MasterDeveloperLink }
					};
				await Email.ComposeAsync (message);
				}
			catch
				{
				await aboutPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Почтовый агент отсутствует на этом устройстве", "ОК");
				}
			}

		// Выбор элемента содержания
		private void HeaderButton_Clicked (object sender, EventArgs e)
			{
			Button b = (Button)sender;
			ContentPage p = (ContentPage)b.CommandParameter;
			((CarouselPage)MainPage).CurrentPage = p;
			}

		/// <summary>
		/// Метод выполняет возврат на страницу содержания
		/// </summary>
		public void CallHeadersPage ()
			{
			((CarouselPage)MainPage).CurrentPage = headersPage;
			}

		// Метод генерирует регистрационный номер ККТ
		private void RNMGenerate_Clicked (object sender, EventArgs e)
			{
			if (rnmRNM.Text.Length < 1)
				rnmRNM.Text = KKTSupport.GetFullRNM (rnmINN.Text, rnmKKTSN.Text, "0");
			else if (rnmRNM.Text.Length < 10)
				rnmRNM.Text = KKTSupport.GetFullRNM (rnmINN.Text, rnmKKTSN.Text, rnmRNM.Text);
			else
				rnmRNM.Text = KKTSupport.GetFullRNM (rnmINN.Text, rnmKKTSN.Text, rnmRNM.Text.Substring (0, 10));
			}

		// Выбор модели ККТ
		private readonly UserManuals um;
		private async void UserManualsKKTButton_Clicked (object sender, EventArgs e)
			{
			int idx = (int)ca.KKTForManuals;
			string res = um.GetKKTList ()[idx];

			if (sender != null)
				{
				// Запрос модели ККТ
				res = await userManualsPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
					um.GetKKTList ().ToArray ());

				// Установка модели
				idx = um.GetKKTList ().IndexOf (res);
				if (idx < 0)
					return;
				}

			userManualsKKTButton.Text = res;
			ca.KKTForManuals = (uint)idx;

			for (int i = 0; i < operationTextLabels.Count; i++)
				operationTextLabels[i].Text = um.GetManual ((uint)idx, (uint)i);
			}

		// Разблокировка расширенного функционала
		private void UnlockMethod (object sender, TextChangedEventArgs e)
			{
			if (ca.TestPass (unlockField.Text))
				{
				unlockField.IsEnabled = false;
				unlockLabel.Text = ConfigAccessor.UnlockMessage;
				unlockLabel.HorizontalTextAlignment = TextAlignment.Center;
				}
			}

		// Поиск по тексту ошибки
		private int lastErrorSearchOffset = 0;
		private void Errors_Find (object sender, EventArgs e)
			{
			List<string> codes = kkme.GetErrorCodesList (ca.KKTForErrors);

			for (int i = lastErrorSearchOffset; i < codes.Count; i++)
				if (codes[i].ToLower ().Contains (errorSearchText.Text.ToLower ()))
					{
					lastErrorSearchOffset = i + 1;

					errorsCodeButton.Text = codes[i];
					ca.ErrorCode = (uint)i;
					errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
					return;
					}

			for (int i = 0; i < lastErrorSearchOffset; i++)
				if (codes[i].ToLower ().Contains (errorSearchText.Text.ToLower ()))
					{
					lastErrorSearchOffset = i + 1;

					errorsCodeButton.Text = codes[i];
					ca.ErrorCode = (uint)i;
					errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
					return;
					}
			}

		/// <summary>
		/// Обработчик события перехода в ждущий режим
		/// </summary>
		protected override void OnSleep ()
			{
			ca.KeepApplicationState = keepAppState.IsToggled;
			if (!ca.KeepApplicationState)
				return;

			ca.CurrentTab = (uint)((CarouselPage)MainPage).Children.IndexOf (((CarouselPage)MainPage).CurrentPage);

			// ca.KKTForErrors	// Обновляется в коде программы
			// ca.ErrorCode		// -||-
			ca.OnlyNewKKTErrors = onlyNewErrors.IsToggled;

			ca.FNSerial = fnLifeSerial.Text;
			ca.GenericTaxFlag = !fnLifeGenericTax.IsToggled;
			ca.GoodsFlag = !fnLifeGoods.IsToggled;
			ca.SeasonFlag = fnLifeSeason.IsToggled;
			ca.AgentsFlag = fnLifeAgents.IsToggled;
			ca.ExciseFlag = fnLifeExcise.IsToggled;
			ca.AutonomousFlag = fnLifeAutonomous.IsToggled;

			ca.KKTSerial = rnmKKTSN.Text;
			ca.UserINN = rnmINN.Text;
			ca.RNMKKT = rnmRNM.Text;

			ca.OFDINN = ofdINN.Text;

			ca.LowLevelCommandsATOL = !lowLevelSHTRIH.IsToggled;
			//ca.LowLevelCode	// -||-

			ca.OnlyNewKKTCodes = onlyNewCodes.IsToggled;
			//ca.KKTForCodes	// -||-
			ca.CodesText = codesSourceText.Text;
			}
		}
	}
