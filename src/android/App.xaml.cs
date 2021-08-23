using Android.Widget;
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

			headersMasterBackColor = Color.FromHex ("#E8E8E8"),
			headersFieldBackColor = Color.FromHex ("#E0E0E0"),

			rnmMasterBackColor = Color.FromHex ("#E0F0FF"),
			rnmFieldBackColor = Color.FromHex ("#C0E0FF"),

			ofdMasterBackColor = Color.FromHex ("#F0F0FF"),
			ofdFieldBackColor = Color.FromHex ("#C8C8FF"),

			lowLevelMasterBackColor = Color.FromHex ("#FFF0FF"),
			lowLevelFieldBackColor = Color.FromHex ("#FFC8FF"),

			tagsMasterBackColor = Color.FromHex ("#E0FFF0"),
			tagsFieldBackColor = Color.FromHex ("#C8FFE4"),

			userManualsMasterBackColor = Color.FromHex ("#F4E8FF"),
			userManualsFieldBackColor = Color.FromHex ("#ECD8FF"),

			untoggledSwitchColor = Color.FromHex ("#505050"),
			errorColor = Color.FromHex ("#FF0000"),
			correctColor = Color.FromHex ("#008000");
		private const string firstStartRegKey = "HelpShownAt";

		#endregion

		#region Переменные страниц
		private ContentPage headersPage, codesPage, errorsPage, aboutPage,
			ofdPage, fnLifePage, rnmPage, lowLevelPage, userManualsPage, tagsPage;

		private Label codesSourceTextLabel, codesHelpLabel, codesErrorLabel, codesResultText,
			errorsResultText,
			aboutLabel,
			fnLifeLabel, fnLifeModelLabel, fnLifeGenericTaxLabel, fnLifeGoodsLabel,
			rnmKKTTypeLabel, rnmINNCheckLabel, rnmRNMCheckLabel, rnmSupport105, rnmSupport11, rnmSupport12,
			lowLevelCommandDescr, unlockLabel,
			tlvDescriptionLabel, tlvTypeLabel, tlvValuesLabel;
		private List<Label> operationTextLabels = new List<Label> ();

		private Xamarin.Forms.Button codesKKTButton, fnLifeResult,
			errorsKKTButton, errorsCodeButton, userManualsKKTButton,
			ofdNameButton, ofdDNSNameButton, ofdIPButton, ofdPortButton, ofdEmailButton, ofdSiteButton, ofdFNSButton,
			ofdDNSNameMButton, ofdIPMButton, ofdPortMButton, ofdDNSNameKButton, ofdPortKButton,
			lowLevelProtocol, lowLevelCommand, lowLevelCommandCode, rnmGenerate;

		private Editor codesSourceText, errorSearchText, commandSearchText, ofdSearchText,
			ofdINN, unlockField,
			fnLifeSerial, tlvTag,
			rnmKKTSN, rnmINN, rnmRNM;

		private Xamarin.Forms.Switch onlyNewCodes, onlyNewErrors,
			fnLife13, fnLifeGenericTax, fnLifeGoods, fnLifeSeason, fnLifeAgents, fnLifeExcise, fnLifeAutonomous, fnLifeDeFacto,
			keepAppState;

		private Xamarin.Forms.DatePicker fnLifeStartDate;

		private StackLayout userManualLayout;

		#endregion

		private ConfigAccessor ca;
		private KKTSupport.FNLifeFlags fnlf;
		private double fontSizeMultiplier = 1.2;

		// Локальный оформитель страниц приложения
		private ContentPage ApplyPageSettings (string PageName, string PageTitle, Color PageBackColor, uint HeaderNumber)
			{
			// Инициализация страницы
			ContentPage page = AndroidSupport.ApplyPageSettings (MainPage, PageName, PageTitle, PageBackColor);

			// Добавление в содержание
			if (HeaderNumber > 0)
				{
				Xamarin.Forms.Button b = AndroidSupport.ApplyButtonSettings (headersPage, "Button" +
					HeaderNumber.ToString ("D02"), PageTitle, PageBackColor, HeaderButton_Clicked);
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
			errorsPage = ApplyPageSettings ("ErrorsPage", "Коды ошибок ККТ",
				errorsMasterBackColor, headerNumber++);
			fnLifePage = ApplyPageSettings ("FNLifePage", "Срок жизни ФН",
				fnLifeMasterBackColor, headerNumber++);
			rnmPage = ApplyPageSettings ("RNMPage", "Заводской и регистрационный номер ККТ",
				rnmMasterBackColor, headerNumber++);
			ofdPage = ApplyPageSettings ("OFDPage", "Параметры ОФД",
				ofdMasterBackColor, headerNumber++);

			tagsPage = ApplyPageSettings ("TagsPage", "TLV-теги", tagsMasterBackColor, headerNumber++);
			tagsPage.IsEnabled = ca.AllowExtendedFunctionsLevel2;

			lowLevelPage = ApplyPageSettings ("LowLevelPage", "Команды нижнего уровня",
				lowLevelMasterBackColor, headerNumber++);
			lowLevelPage.IsEnabled = ca.AllowExtendedFunctionsLevel2;

			codesPage = ApplyPageSettings ("CodesPage", "Перевод текста в коды ККТ",
				codesMasterBackColor, headerNumber++);
			codesPage.IsEnabled = ca.AllowExtendedFunctionsLevel1;

			aboutPage = ApplyPageSettings ("AboutPage", "О приложении",
				aboutMasterBackColor, headerNumber);

			#endregion

			#region Страница «оглавления»

			keepAppState = (Xamarin.Forms.Switch)headersPage.FindByName ("KeepAppState");
			keepAppState.IsToggled = ca.KeepApplicationState;

			AndroidSupport.ApplyLabelSettingsForKKT (headersPage, "KeepAppStateLabel", "Помнить настройки приложения", true);

			try
				{
				((CarouselPage)MainPage).CurrentPage = ((CarouselPage)MainPage).Children[(int)ca.CurrentTab];
				}
			catch { }

			#endregion

			#region Страница инструкций

			Label ut = AndroidSupport.ApplyLabelSettingsForKKT (userManualsPage, "SelectionLabel", "Модель ККТ:", true);
			userManualLayout = (StackLayout)userManualsPage.FindByName ("UserManualLayout");

			for (int i = 0; i < um.OperationTypes.Length; i++)
				{
				Label l = new Label ();
				l.FontAttributes = FontAttributes.Bold;
				l.FontSize = ut.FontSize * fontSizeMultiplier;
				l.HorizontalOptions = LayoutOptions.Start;
				l.IsVisible = true;
				l.Margin = ut.Margin;
				l.Text = um.OperationTypes[i];
				l.TextColor = ut.TextColor;

				userManualLayout.Children.Add (l);

				Label l2 = new Label ();
				l2.BackgroundColor = userManualsFieldBackColor;
				l2.Text = "   ";
				l2.FontAttributes = FontAttributes.None;
				l2.FontSize = ut.FontSize * fontSizeMultiplier;
				l2.HorizontalOptions = LayoutOptions.Fill;
				l2.HorizontalTextAlignment = TextAlignment.Start;
				l2.Margin = ut.Margin;
				l2.TextColor = ut.TextColor;

				operationTextLabels.Add (l2);
				userManualLayout.Children.Add (operationTextLabels[operationTextLabels.Count - 1]);
				}

			userManualsKKTButton = AndroidSupport.ApplyButtonSettings (userManualsPage, "KKTButton",
				"   ", userManualsFieldBackColor, UserManualsKKTButton_Clicked);
			userManualsKKTButton.FontSize *= fontSizeMultiplier;

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

			onlyNewCodes = (Xamarin.Forms.Switch)codesPage.FindByName ("OnlyNewCodes");
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
			codesKKTButton.FontSize *= fontSizeMultiplier;

			codesSourceTextLabel = AndroidSupport.ApplyLabelSettingsForKKT (codesPage, "SourceTextLabel",
				"Исходный текст:", true);

			codesSourceText = AndroidSupport.ApplyEditorSettings (codesPage, "SourceText",
				codesFieldBackColor, Keyboard.Default, 72, ca.CodesText, SourceText_TextChanged);
			codesSourceText.HorizontalOptions = LayoutOptions.Fill;
			codesSourceText.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (codesPage, "ResultTextLabel", "Коды ККТ:", true);

			codesErrorLabel = AndroidSupport.ApplyTipLabelSettings (codesPage, "ErrorLabel",
				"Часть введённых символов не поддерживается данной ККТ или требует специальных действий для ввода",
				errorColor);

			codesResultText = AndroidSupport.ApplyResultLabelSettings (codesPage, "ResultText", "", codesFieldBackColor);
			codesResultText.HorizontalTextAlignment = TextAlignment.Start;
			codesResultText.FontSize *= fontSizeMultiplier;

			codesHelpLabel = AndroidSupport.ApplyTipLabelSettings (codesPage, "HelpLabel",
				kkmc.GetKKTTypeDescription (ca.KKTForCodes), untoggledSwitchColor);

			AndroidSupport.ApplyButtonSettings (codesPage, "Clear",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Delete),
				codesFieldBackColor, CodesClear_Clicked);

			SourceText_TextChanged (null, null);    // Протягивание кодов

			#endregion

			#region Страница ошибок

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "SelectionLabel", "Модель ККТ:", true);

			onlyNewErrors = (Xamarin.Forms.Switch)errorsPage.FindByName ("OnlyNewErrors");
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
			errorsKKTButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "ErrorCodeLabel", "Код / сообщение:", true);

			errorsCodeButton = AndroidSupport.ApplyButtonSettings (errorsPage, "ErrorCodeButton",
				kkme.GetErrorCodesList (ca.KKTForErrors)[(int)ca.ErrorCode],
				errorsFieldBackColor, ErrorsCodeButton_Clicked);
			errorsCodeButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "ResultTextLabel", "Расшифровка:", true);

			errorsResultText = AndroidSupport.ApplyResultLabelSettings (errorsPage, "ResultText",
				kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode), errorsFieldBackColor);
			errorsResultText.HorizontalTextAlignment = TextAlignment.Start;
			errorsResultText.FontSize *= fontSizeMultiplier;

			errorSearchText = AndroidSupport.ApplyEditorSettings (errorsPage, "ErrorSearchText", errorsFieldBackColor,
				Keyboard.Default, 30, "", null);
			errorSearchText.FontSize *= fontSizeMultiplier;

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
			AndroidSupport.ApplyButtonSettings (aboutPage, "ManualPage", "Видеоруководство пользователя",
				aboutFieldBackColor, ManualButton_Clicked);

			AndroidSupport.ApplyButtonSettings (aboutPage, "UpdatePage",
				"Инструмент чтения данных ФН FNReader", aboutFieldBackColor, UpdateButton_Clicked);
			AndroidSupport.ApplyButtonSettings (aboutPage, "CommunityPage",
				AndroidSupport.MasterLabName, aboutFieldBackColor, CommunityButton_Clicked);

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				unlockLabel = AndroidSupport.ApplyLabelSettingsForKKT (aboutPage, "UnlockLabel", ca.LockMessage, false);
				unlockLabel.IsVisible = true;
				unlockLabel.FontSize *= fontSizeMultiplier;

				unlockField = AndroidSupport.ApplyEditorSettings (aboutPage, "UnlockField", aboutFieldBackColor,
					Keyboard.Default, 32, "", UnlockMethod);
				unlockField.IsVisible = true;
				unlockField.FontSize *= fontSizeMultiplier;
				}

			#endregion

			#region Страница определения срока жизни ФН

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetModelLabel", "ЗН, номинал или модель ФН:",
				true);
			fnLifeSerial = AndroidSupport.ApplyEditorSettings (fnLifePage, "FNLifeSerial", fnLifeFieldBackColor,
				Keyboard.Default, 16, ca.FNSerial, FNLifeSerial_TextChanged);
			fnLifeSerial.Margin = new Thickness (0);
			fnLifeSerial.FontSize *= fontSizeMultiplier;

			fnLife13 = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLife13");
			fnLife13.Toggled += FnLife13_Toggled;
			fnLife13.ThumbColor = untoggledSwitchColor;
			fnLife13.OnColor = fnLifeFieldBackColor;

			fnLifeLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeLabel", "", false);

			//
			fnLifeModelLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeModelLabel", "", false);
			fnLifeModelLabel.FontSize *= fontSizeMultiplier;
			fnLifeModelLabel.HorizontalOptions = LayoutOptions.Fill;
			fnLifeModelLabel.HorizontalTextAlignment = TextAlignment.Center;

			//
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetUserParameters", "Значимые параметры:", true);

			//
			fnLifeGenericTax = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeGenericTax");
			fnLifeGenericTax.IsToggled = !ca.GenericTaxFlag;
			fnLifeGenericTax.Toggled += FnLife13_Toggled;
			fnLifeGenericTax.ThumbColor = untoggledSwitchColor;
			fnLifeGenericTax.OnColor = fnLifeFieldBackColor;

			fnLifeGenericTaxLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeGenericTaxLabel", "", false);

			//
			fnLifeGoods = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeGoods");
			fnLifeGoods.IsToggled = !ca.GoodsFlag;
			fnLifeGoods.Toggled += FnLife13_Toggled;
			fnLifeGoods.ThumbColor = untoggledSwitchColor;
			fnLifeGoods.OnColor = fnLifeFieldBackColor;

			fnLifeGoodsLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeGoodsLabel", "", false);

			//
			fnLifeSeason = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeSeason");
			fnLifeSeason.IsToggled = ca.SeasonFlag;
			fnLifeSeason.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeSeasonLabel", "Сезонная торговля", false);

			//
			fnLifeAgents = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeAgents");
			fnLifeAgents.IsToggled = ca.AgentsFlag;
			fnLifeAgents.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeAgentsLabel", "Платёжный (суб)агент", false);

			//
			fnLifeExcise = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeExcise");
			fnLifeExcise.IsToggled = ca.ExciseFlag;
			fnLifeExcise.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeExciseLabel", "Подакцизный товар", false);

			//
			fnLifeAutonomous = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeAutonomous");
			fnLifeAutonomous.IsToggled = ca.AutonomousFlag;
			fnLifeAutonomous.Toggled += FnLife13_Toggled;

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeAutonomousLabel", "Автономный режим", false);

			//
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetDate", "Дата фискализации:", false);
			fnLifeStartDate = AndroidSupport.ApplyDatePickerSettings (fnLifePage, "FNLifeStartDate", fnLifeFieldBackColor,
				FnLifeStartDate_DateSelected);
			fnLifeStartDate.FontSize *= fontSizeMultiplier;

			//
			fnLifeResult = AndroidSupport.ApplyButtonSettings (fnLifePage, "FNLifeResult", "", fnLifeFieldBackColor,
				FNLifeResultCopy, false);
			fnLifeResult.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyTipLabelSettings (fnLifePage, "FNLifeHelpLabel",
				"Нажатие кнопки копирует дату окончания срока жизни в буфер обмена", untoggledSwitchColor);

			//
			fnLifeDeFacto = (Xamarin.Forms.Switch)fnLifePage.FindByName ("FNLifeDeFacto");
			if (ca.AllowExtendedFunctionsLevel2)
				{
				fnLifeDeFacto.IsToggled = ca.FNLifeDeFacto;
				fnLifeDeFacto.Toggled += FnLife13_Toggled;
				}
			else
				{
				fnLifeDeFacto.IsToggled = fnLifeDeFacto.IsEnabled = false;
				}

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeDeFactoLabel", "Фактический", false);

			//
			AndroidSupport.ApplyButtonSettings (fnLifePage, "Clear",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Delete),
				fnLifeFieldBackColor, FNLifeClear_Clicked);
			AndroidSupport.ApplyButtonSettings (fnLifePage, "Find",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Find),
				fnLifeFieldBackColor, FNLifeFind_Clicked);

			// Применение всех названий
			FNLifeSerial_TextChanged (null, null);

			#endregion

			#region Страница заводских и рег. номеров

			AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "SNLabel", "ЗН или модель ККТ:", true);
			rnmKKTSN = AndroidSupport.ApplyEditorSettings (rnmPage, "SN", rnmFieldBackColor, Keyboard.Default,
				kkts.MaxSerialNumberLength, ca.KKTSerial, RNM_TextChanged);
			rnmKKTSN.FontSize *= fontSizeMultiplier;

			rnmKKTTypeLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "TypeLabel", "", false);

			AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "INNLabel", "ИНН пользователя:", true);
			rnmINN = AndroidSupport.ApplyEditorSettings (rnmPage, "INN", rnmFieldBackColor, Keyboard.Numeric, 12,
				ca.UserINN, RNM_TextChanged);
			rnmINN.FontSize *= fontSizeMultiplier;

			rnmINNCheckLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "INNCheckLabel", "", false);

			if (ca.AllowExtendedFunctionsLevel2)
				AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMLabel",
					"Регистрационный номер для проверки или произвольное число для генерации:", true);
			else
				AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMLabel",
					"Регистрационный номер для проверки:", true);

			rnmRNM = AndroidSupport.ApplyEditorSettings (rnmPage, "RNM", rnmFieldBackColor, Keyboard.Numeric, 16,
				ca.RNMKKT, RNM_TextChanged);
			rnmRNM.FontSize *= fontSizeMultiplier;

			rnmRNMCheckLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMCheckLabel", "", false);

			rnmGenerate = AndroidSupport.ApplyButtonSettings (rnmPage, "RNMGenerate", "Сгенерировать",
				rnmFieldBackColor, RNMGenerate_Clicked);
			rnmGenerate.IsVisible = ca.AllowExtendedFunctionsLevel2;
			rnmGenerate.Margin = new Thickness (0);

			string rnmAbout = "Индикатор ФФД: красный – поддержка не планируется; зелёный – поддерживается; " +
				"жёлтый – планируется; синий – нет сведений (на момент релиза этой версии приложения)";
			if (ca.AllowExtendedFunctionsLevel2)
				rnmAbout += ("\n\nПервые 10 цифр РН являются порядковым номером ККТ в реестре и могут быть указаны " +
					"вручную при генерации");
			AndroidSupport.ApplyTipLabelSettings (rnmPage, "RNMAbout", rnmAbout, untoggledSwitchColor);

			AndroidSupport.ApplyButtonSettings (rnmPage, "Clear",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Delete),
				rnmFieldBackColor, RNMClear_Clicked);
			AndroidSupport.ApplyButtonSettings (rnmPage, "Find",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Find),
				rnmFieldBackColor, RNMFind_Clicked);

			rnmSupport105 = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMSupport105", "ФФД 1.05", false);
			rnmSupport11 = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMSupport11", "ФФД 1.1", false);
			rnmSupport12 = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMSupport12", "ФФД 1.2", false);
			rnmSupport105.Padding = rnmSupport11.Padding = rnmSupport12.Padding = new Thickness (6);

			RNM_TextChanged (null, null);   // Применение значений

			#endregion

			#region Страница настроек ОФД

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDINNLabel", "ИНН ОФД:", true);
			ofdINN = AndroidSupport.ApplyEditorSettings (ofdPage, "OFDINN", ofdFieldBackColor, Keyboard.Numeric, 10,
				ca.OFDINN, OFDINN_TextChanged);
			ofdINN.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyButtonSettings (ofdPage, "OFDINNCopy",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Copy),
				ofdFieldBackColor, OFDINNCopy_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDNameLabel", "Название:", true);
			ofdNameButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDName", "- Выберите или введите ИНН -",
				ofdFieldBackColor, OFDName_Clicked);
			ofdNameButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameLabel", "Адрес ОФД:", true);
			ofdDNSNameButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSName", "", ofdFieldBackColor, Field_Clicked);
			ofdDNSNameButton.FontSize *= fontSizeMultiplier;
			ofdIPButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDIP", "", ofdFieldBackColor, Field_Clicked);
			ofdIPButton.FontSize *= fontSizeMultiplier;
			ofdPortButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPort", "", ofdFieldBackColor, Field_Clicked);
			ofdPortButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameMLabel", "Адрес ИСМ:", true);
			ofdDNSNameMButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSNameM", "", ofdFieldBackColor, Field_Clicked);
			ofdDNSNameMButton.FontSize *= fontSizeMultiplier;
			ofdIPMButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDIPM", "", ofdFieldBackColor, Field_Clicked);
			ofdIPMButton.FontSize *= fontSizeMultiplier;
			ofdPortMButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPortM", "", ofdFieldBackColor, Field_Clicked);
			ofdPortMButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameKLabel", "Адрес ОКП:", true);
			ofdDNSNameKButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSNameK", OFD.OKPSite,
				ofdFieldBackColor, Field_Clicked);
			ofdDNSNameKButton.FontSize *= fontSizeMultiplier;
			ofdPortKButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPortK", OFD.OKPPort,
				ofdFieldBackColor, Field_Clicked);
			ofdPortKButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDEmailLabel", "E-mail ОФД:", true);
			ofdEmailButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDEmail", "", ofdFieldBackColor, Field_Clicked);
			ofdEmailButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDSiteLabel", "Сайт ОФД:", true);
			ofdSiteButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDSite", "", ofdFieldBackColor, Field_Clicked);
			ofdSiteButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDNalogSiteLabel", "Сайт ФНС:", true);
			ofdFNSButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDNalogSite", OFD.FNSSite,
				ofdFieldBackColor, Field_Clicked);
			ofdFNSButton.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyTipLabelSettings (ofdPage, "OFDHelpLabel",
				"Нажатие кнопок копирует их подписи в буфер обмена", untoggledSwitchColor);

			AndroidSupport.ApplyButtonSettings (ofdPage, "Clear",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Delete),
				ofdFieldBackColor, OFDClear_Clicked);

			ofdSearchText = AndroidSupport.ApplyEditorSettings (ofdPage, "OFDSearchText", ofdFieldBackColor,
				Keyboard.Default, 30, "", null);
			ofdSearchText.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyButtonSettings (ofdPage, "OFDSearchButton",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Find),
				ofdFieldBackColor, OFD_Find);

			OFDINN_TextChanged (null, null); // Протягивание значений

			#endregion

			#region Страница TLV-тегов

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				tagsFieldBackColor = tagsMasterBackColor = Color.FromRgb (128, 128, 128);
				tagsPage.BackgroundColor = Color.FromRgb (192, 192, 192);
				}

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVSearchLabel", "Номер или часть описания:", true);
			tlvTag = AndroidSupport.ApplyEditorSettings (tagsPage, "TLVSearchText", tagsFieldBackColor,
				Keyboard.Default, 20, "", null);
			tlvTag.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyButtonSettings (tagsPage, "TLVSearchButton",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Find),
				tagsFieldBackColor, TLVFind_Clicked);
			AndroidSupport.ApplyButtonSettings (tagsPage, "TLVClearButton",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Delete),
				tagsFieldBackColor, TLVClear_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVDescriptionLabel", "Описание тега:", true);
			tlvDescriptionLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVDescription", "",
				tagsFieldBackColor);
			tlvDescriptionLabel.HorizontalTextAlignment = TextAlignment.Start;
			tlvDescriptionLabel.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVTypeLabel", "Тип тега:", true);
			tlvTypeLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVType", "",
				tagsFieldBackColor);
			tlvTypeLabel.HorizontalTextAlignment = TextAlignment.Start;
			tlvTypeLabel.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVValuesLabel", "Возможные значения тега:", true);
			tlvValuesLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVValues", "",
				tagsFieldBackColor);
			tlvValuesLabel.HorizontalTextAlignment = TextAlignment.Start;
			tlvValuesLabel.FontSize *= fontSizeMultiplier;

			#endregion

			#region Страница команд нижнего уровня

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				lowLevelFieldBackColor = lowLevelMasterBackColor = Color.FromRgb (128, 128, 128);
				lowLevelPage.BackgroundColor = Color.FromRgb (192, 192, 192);
				}

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "ProtocolLabel", "Протокол:", true);
			lowLevelProtocol = AndroidSupport.ApplyButtonSettings (lowLevelPage, "ProtocolButton",
				ll.GetProtocolsNames ()[(int)ca.LowLevelProtocol], lowLevelFieldBackColor, LowLevelProtocol_Clicked);
			lowLevelProtocol.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandLabel", "Команда:", true);
			lowLevelCommand = AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandButton",
				ll.GetCommandsList (ca.LowLevelProtocol)[(int)ca.LowLevelCode],
				lowLevelFieldBackColor, LowLevelCommandCodeButton_Clicked);
			lowLevelCommand.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandCodeLabel", "Код команды:", true);
			lowLevelCommandCode = AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandCodeButton",
				ll.GetCommand (ca.LowLevelProtocol, ca.LowLevelCode, false),
				lowLevelFieldBackColor, Field_Clicked);
			lowLevelCommandCode.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandDescrLabel", "Описание:", true);

			lowLevelCommandDescr = AndroidSupport.ApplyResultLabelSettings (lowLevelPage, "CommandDescr",
				ll.GetCommand (ca.LowLevelProtocol, ca.LowLevelCode, true), lowLevelFieldBackColor);
			lowLevelCommandDescr.HorizontalTextAlignment = TextAlignment.Start;
			lowLevelCommandDescr.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyTipLabelSettings (lowLevelPage, "LowLevelHelpLabel",
				"Нажатие кнопки копирует команду в буфер обмена", untoggledSwitchColor);

			commandSearchText = AndroidSupport.ApplyEditorSettings (lowLevelPage, "CommandSearchText", lowLevelFieldBackColor,
				Keyboard.Default, 30, "", null);
			commandSearchText.FontSize *= fontSizeMultiplier;

			AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandSearchButton",
				AndroidSupport.GetDefaultButtonName (AndroidSupport.ButtonsDefaultNames.Find),
				lowLevelFieldBackColor, Command_Find);

			#endregion

			// Обязательное принятие Политики и EULA
			AcceptPolicy ();
			}

		// Контроль принятия Политики и EULA
		private async void AcceptPolicy ()
			{
			if (Preferences.Get (firstStartRegKey, "") != "")
				return;

			while (!await ((CarouselPage)MainPage).CurrentPage.DisplayAlert (ProgramDescription.AssemblyTitle,
					"Перед началом работы с этим инструментом Вы должны принять Политику разработки приложений и " +
					"Пользовательское соглашение. Хотите открыть их тексты в браузере?\r\n\r\n" +
					"• Нажмите «Принять», если Вы уже ознакомились и полностью приняли их;\r\n" +
					"• Нажмите «Читать», если хотите открыть их в браузере;\r\n" +
					"• Чтобы отклонить их, закройте приложение",

					"Принять", "Читать"))
				{
				ADPButton_Clicked (null, null);
				}

			Preferences.Set (firstStartRegKey, ProgramDescription.AssemblyVersion); // Только после принятия
			}

		// Сброс списков ККТ и ошибок
		private void OnlyNewErrors_Toggled (object sender, ToggledEventArgs e)
			{
			/*lastErrorSearchOffset = 0;  // Позволяет избежать сбоя при вторичном вызове поиска по коду ошибки*/
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
			List<string> list = ll.GetCommandsList (ca.LowLevelProtocol);
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

				lowLevelCommandCode.Text = ll.GetCommand (ca.LowLevelProtocol, (uint)i, false);
				lowLevelCommandDescr.Text = ll.GetCommand (ca.LowLevelProtocol, (uint)i, true);
				}

			list.Clear ();
			}

		// Выбор списка команд
		private async void LowLevelProtocol_Clicked (object sender, EventArgs e)
			{
			// Запрос кода ошибки
			List<string> list = ll.GetProtocolsNames ();
			string res = list[0];
			res = await lowLevelPage.DisplayActionSheet ("Выберите протокол:", "Отмена", null, list.ToArray ());

			// Установка результата
			int i = 0;
			if ((i = list.IndexOf (res)) >= 0)
				{
				ca.LowLevelProtocol = (uint)i;
				lowLevelProtocol.Text = res;

				// Вызов вложенного обработчика
				/*lastCommandSearchOffset = 0;   // Позволяет избежать сбоя при вторичном вызове поиска по коду команды*/
				LowLevelCommandCodeButton_Clicked (sender, null);
				}

			//list.Clear ();
			}

		// Ввод ЗН ФН в разделе определения срока жизни
		private FNSerial fns = new FNSerial ();
		private void FNLifeSerial_TextChanged (object sender, TextChangedEventArgs e)
			{
			// Получение описания
			if (fnLifeSerial.Text != "")
				fnLifeModelLabel.Text = fns.GetFNName (fnLifeSerial.Text);
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
			fnlf.FN15 = !fnLife13.IsToggled;
			fnlf.FNExactly13 = fnLifeModelLabel.Text.Contains ("(13)");
			fnlf.GenericTax = !fnLifeGenericTax.IsToggled;
			fnlf.Goods = !fnLifeGoods.IsToggled;
			fnlf.SeasonOrAgents = fnLifeSeason.IsToggled || fnLifeAgents.IsToggled;
			fnlf.Excise = fnLifeExcise.IsToggled;
			fnlf.Autonomous = fnLifeAutonomous.IsToggled;
			fnlf.DeFacto = fnLifeDeFacto.IsToggled;

			string res = KKTSupport.GetFNLifeEndDate (fnLifeStartDate.Date, fnlf);

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

			if (!fnLife13.IsEnabled) // Признак корректно заданного ЗН ФН
				{
				if (!fns.IsFNCompatibleWithFFD12 (fnLifeSerial.Text))
					{
					fnLifeResult.TextColor = errorColor;

					/*string deadLine = KKTSupport.OldFNDeadline.ToString ("d.MM.yy");
					if (DateTime.Now >= KKTSupport.OldFNDeadline)
						{*/
					fnLifeResult.Text += ("\n(выбранный ФН с " + KKTSupport.OldFNDeadline.ToString ("d.MM.yy") +
						" не может быть зарегистрирован в ФНС)");
					fnLifeModelLabel.BackgroundColor = StatusToColor (KKTSerial.FFDSupportStatuses.Unsupported);
					/*}
				else
					{
					fnLifeResult.Text += ("\n(выбранный ФН должен быть зарегистрирован до " + deadLine + ")");
					fnLifeModelLabel.BackgroundColor = StatusToColor (KKTSerial.FFDSupportStatuses.Planned);
					}*/
					}
				else
					{
					fnLifeModelLabel.BackgroundColor = StatusToColor (KKTSerial.FFDSupportStatuses.Supported);
					}
				}
			else
				{
				fnLifeModelLabel.BackgroundColor = StatusToColor (KKTSerial.FFDSupportStatuses.Unknown);
				}
			}

		// Изменение ИНН ОФД и РН ККТ
		private KKTSerial kkts = new KKTSerial ();
		private void RNM_TextChanged (object sender, TextChangedEventArgs e)
			{
			// ЗН ККТ
			if (rnmKKTSN.Text != "")
				{
				rnmKKTTypeLabel.Text = kkts.GetKKTModel (rnmKKTSN.Text);

				KKTSerial.FFDSupportStatuses[] statuses = kkts.GetFFDSupportStatus (rnmKKTSN.Text);
				rnmSupport105.BackgroundColor = StatusToColor (statuses[0]);
				rnmSupport11.BackgroundColor = StatusToColor (statuses[1]);
				rnmSupport12.BackgroundColor = StatusToColor (statuses[2]);
				}
			else
				{
				rnmKKTTypeLabel.Text = "";
				rnmSupport105.BackgroundColor = rnmSupport11.BackgroundColor = rnmSupport12.BackgroundColor =
					StatusToColor (KKTSerial.FFDSupportStatuses.Unknown);
				}

			// ИНН пользователя
			if (KKTSupport.CheckINN (rnmINN.Text) < 0)
				{
				rnmINNCheckLabel.TextColor = rnmINN.TextColor;
				rnmINNCheckLabel.Text = "неполный";
				}
			else if (KKTSupport.CheckINN (rnmINN.Text) == 0)
				{
				rnmINNCheckLabel.TextColor = correctColor;
				rnmINNCheckLabel.Text = "ОК";
				}
			else
				{
				rnmINNCheckLabel.TextColor = errorColor;
				rnmINNCheckLabel.Text = "возможно, некорректный";
				}
			rnmINNCheckLabel.Text += (" (" + KKTSupport.GetRegionName (rnmINN.Text) + ")");

			// РН
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

		// Запрос цвета, соответствующего статусу поддержки
		private Color StatusToColor (KKTSerial.FFDSupportStatuses Status)
			{
			if (Status == KKTSerial.FFDSupportStatuses.Planned)
				return Color.FromHex ("#FFFFC8");

			if (Status == KKTSerial.FFDSupportStatuses.Supported)
				return Color.FromHex ("#C8FFC8");

			if (Status == KKTSerial.FFDSupportStatuses.Unsupported)
				return Color.FromHex ("#FFC8C8");

			// Остальные
			return Color.FromHex ("#C8C8FF");
			}

		private OFD ofd = new OFD ();
		private void OFDINN_TextChanged (object sender, TextChangedEventArgs e)
			{
			List<string> parameters = ofd.GetOFDParameters (ofdINN.Text);

			ofdNameButton.Text = parameters[1];

			ofdDNSNameButton.Text = parameters[2];
			ofdIPButton.Text = parameters[3];
			ofdPortButton.Text = parameters[4];

			ofdEmailButton.Text = parameters[5];
			ofdSiteButton.Text = parameters[6];

			ofdDNSNameMButton.Text = parameters[7];
			ofdIPMButton.Text = parameters[8];
			ofdPortMButton.Text = parameters[9];

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
			SendToClipboard (((Xamarin.Forms.Button)sender).Text);
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
				Toast.MakeText (Android.App.Application.Context, "Скопировано в буфер обмена",
					ToastLength.Short).Show ();
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
		private KKTCodes kkmc = new KKTCodes ();
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
		private KKTErrorsList kkme = new KKTErrorsList ();
		private async void ErrorsKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			List<string> list = kkme.GetKKTTypeNames (onlyNewErrors.IsToggled);
			string res = await errorsPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null,
				list.ToArray ());

			// Установка модели
			if (list.IndexOf (res) < 0)
				return;

			errorsKKTButton.Text = res;
			ca.KKTForErrors = (uint)list.IndexOf (res);

			List<string> list2 = kkme.GetErrorCodesList (ca.KKTForErrors);
			errorsCodeButton.Text = list2[0];

			ca.ErrorCode = 0;
			errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
			list2.Clear ();
			}

		// Выбор кода ошибки
		private async void ErrorsCodeButton_Clicked (object sender, EventArgs e)
			{
			// Запрос кода ошибки
			List<string> list = kkme.GetErrorCodesList (ca.KKTForErrors);
			string res = await errorsPage.DisplayActionSheet ("Выберите код/текст ошибки:", "Отмена", null,
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
				await Launcher.OpenAsync (AndroidSupport.MasterGitLink + "FNReader");
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
				await Launcher.OpenAsync (AndroidSupport.MasterGitLink + ProgramDescription.AssemblyMainName);
				}
			catch
				{
				Toast.MakeText (Android.App.Application.Context, "Веб-браузер отсутствует на этом устройстве",
					ToastLength.Long).Show ();
				}
			}

		// Страница политики и EULA
		private async void ADPButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				await Launcher.OpenAsync (AndroidSupport.ADPLink);
				}
			catch
				{
				Toast.MakeText (Android.App.Application.Context, "Веб-браузер отсутствует на этом устройстве",
					ToastLength.Long).Show ();
				}
			}

		// Видеоруководство пользователя
		private async void ManualButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				await Launcher.OpenAsync (ProgramDescription.AssemblyVideoManualLink);
				}
			catch
				{
				Toast.MakeText (Android.App.Application.Context, "Веб-браузер отсутствует на этом устройстве",
					ToastLength.Long).Show ();
				}
			}

		// Страница лаборатории
		private async void CommunityButton_Clicked (object sender, EventArgs e)
			{
			List<string> comm = new List<string> { "ВКонтакте", "Телеграм" };
			string res = await aboutPage.DisplayActionSheet ("Выберите сообщество", "Отмена", null, comm.ToArray ());

			if (!comm.Contains (res))
				return;

			try
				{
				if (comm.IndexOf (res) == 0)
					await Launcher.OpenAsync (AndroidSupport.MasterCommunityLink);
				else
					await Launcher.OpenAsync (AndroidSupport.CommunityInTelegram);
				}
			catch
				{
				Toast.MakeText (Android.App.Application.Context, "Веб-браузер отсутствует на этом устройстве",
					ToastLength.Long).Show ();
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
				Toast.MakeText (Android.App.Application.Context, "Почтовый агент отсутствует на этом устройстве",
					ToastLength.Long).Show ();
				}
			}

		// Выбор элемента содержания
		private void HeaderButton_Clicked (object sender, EventArgs e)
			{
			Xamarin.Forms.Button b = (Xamarin.Forms.Button)sender;
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
		private UserManuals um;
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
		private int lastErrorSearchOffset2 = 0;
		private void Errors_Find (object sender, EventArgs e)
			{
			List<string> codes = kkme.GetErrorCodesList (ca.KKTForErrors);
			string text = errorSearchText.Text.ToLower ();

			lastErrorSearchOffset2++;
			for (int i = 0; i < codes.Count; i++)
				if (codes[(i + lastErrorSearchOffset2) % codes.Count].ToLower ().Contains (text))
					{
					lastErrorSearchOffset2 = (i + lastErrorSearchOffset2) % codes.Count;

					errorsCodeButton.Text = codes[lastErrorSearchOffset2];
					ca.ErrorCode = (uint)lastErrorSearchOffset2;
					errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
					return;
					}

			/*for (int i = 0; i < lastErrorSearchOffset; i++)
				if (codes[i].ToLower ().Contains (errorSearchText.Text.ToLower ()))
					{
					lastErrorSearchOffset = i + 1;

					errorsCodeButton.Text = codes[i];
					ca.ErrorCode = (uint)i;
					errorsResultText.Text = kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode);
					return;
					}*/
			}

		// Поиск по названию команды нижнего уровня
		private int lastCommandSearchOffset2 = 0;
		private void Command_Find (object sender, EventArgs e)
			{
			List<string> codes = ll.GetCommandsList (ca.LowLevelProtocol);
			string text = commandSearchText.Text.ToLower ();

			lastCommandSearchOffset2++;
			for (int i = 0; i < codes.Count; i++)
				if (codes[(i + lastCommandSearchOffset2) % codes.Count].ToLower ().Contains (text))
					{
					lastCommandSearchOffset2 = (i + lastCommandSearchOffset2) % codes.Count;

					lowLevelCommand.Text = codes[lastCommandSearchOffset2];
					ca.LowLevelCode = (uint)lastCommandSearchOffset2;

					lowLevelCommandCode.Text = ll.GetCommand (ca.LowLevelProtocol,
						(uint)lastCommandSearchOffset2, false);
					lowLevelCommandDescr.Text = ll.GetCommand (ca.LowLevelProtocol,
						(uint)lastCommandSearchOffset2, true);
					return;
					}

			/*for (int i = 0; i < lastCommandSearchOffset; i++)
				if (codes[i].ToLower ().Contains (commandSearchText.Text.ToLower ()))
					{
					lastCommandSearchOffset = i + 1;

					lowLevelCommand.Text = codes[i];
					ca.LowLevelCode = (uint)i;

					lowLevelCommandCode.Text = ll.GetCommand (ca.LowLevelProtocol, (uint)i, false);
					lowLevelCommandDescr.Text = ll.GetCommand (ca.LowLevelProtocol, (uint)i, true);
					return;
					}*/
			}

		// Поиск по названию ОФД
		private int lastOFDSearchOffset2 = 0;
		private void OFD_Find (object sender, EventArgs e)
			{
			List<string> codes = ofd.GetOFDNames ();
			string text = ofdSearchText.Text.ToLower ();

			lastOFDSearchOffset2++;
			for (int i = 0; i < codes.Count; i++)
				if (codes[(i + lastOFDSearchOffset2) % codes.Count].ToLower ().Contains (text))
					{
					lastOFDSearchOffset2 = (i + lastOFDSearchOffset2) % codes.Count;
					ofdNameButton.Text = codes[lastOFDSearchOffset2];

					string s = ofd.GetOFDINNByName (ofdNameButton.Text);
					if (s != "")
						ca.OFDINN = ofdINN.Text = s;

					return;
					}

			/*for (int i = 0; i < lastCommandSearchOffset; i++)
				if (codes[i].ToLower ().Contains (ofdSearchText.Text.ToLower ()))
					{
					lastOFDSearchOffset = i + 1;

					ofdNameButton.Text = codes[i];
					string s = ofd.GetOFDINNByName (ofdNameButton.Text);
					if (s != "")
						ca.OFDINN = ofdINN.Text = s;
					return;
					}*/
			}

		// Очистка полей
		private void RNMClear_Clicked (object sender, EventArgs e)
			{
			rnmKKTSN.Text = "";
			rnmINN.Text = "";
			rnmRNM.Text = "";
			}

		private void OFDClear_Clicked (object sender, EventArgs e)
			{
			ofdINN.Text = "";
			}

		private void CodesClear_Clicked (object sender, EventArgs e)
			{
			codesSourceText.Text = "";
			}

		private void FNLifeClear_Clicked (object sender, EventArgs e)
			{
			fnLifeSerial.Text = "";
			}

		private void FNLifeFind_Clicked (object sender, EventArgs e)
			{
			string sig = fns.FindSignatureByName (fnLifeSerial.Text);
			if (sig != "")
				fnLifeSerial.Text = sig;
			}

		private void RNMFind_Clicked (object sender, EventArgs e)
			{
			string sig = kkts.FindSignatureByName (rnmKKTSN.Text);
			if (sig != "")
				rnmKKTSN.Text = sig;
			}

		private TLVTags tlvt = new TLVTags ();
		private void TLVFind_Clicked (object sender, EventArgs e)
			{
			if (tlvt.FindTag (tlvTag.Text))
				{
				tlvDescriptionLabel.Text = tlvt.LastDescription;
				tlvTypeLabel.Text = tlvt.LastType;
				tlvValuesLabel.Text = tlvt.LastValuesSet;
				}
			}

		private void TLVClear_Clicked (object sender, EventArgs e)
			{
			tlvTag.Text = "";
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
			ca.FNLifeDeFacto = fnLifeDeFacto.IsToggled;

			ca.KKTSerial = rnmKKTSN.Text;
			ca.UserINN = rnmINN.Text;
			ca.RNMKKT = rnmRNM.Text;

			ca.OFDINN = ofdINN.Text;

			//ca.LowLevelProtocol	// -||-
			//ca.LowLevelCode		// -||-

			ca.OnlyNewKKTCodes = onlyNewCodes.IsToggled;
			//ca.KKTForCodes	// -||-
			ca.CodesText = codesSourceText.Text;
			}
		}
	}
