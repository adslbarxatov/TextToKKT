using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		#region Настройки стилей отображения

		private readonly Color
			headersMasterBackColor = Color.FromHex ("#E8E8E8"),
			headersFieldBackColor = Color.FromHex ("#E0E0E0"),

			userManualsMasterBackColor = Color.FromHex ("#F4E8FF"),
			userManualsFieldBackColor = Color.FromHex ("#ECD8FF"),

			errorsMasterBackColor = Color.FromHex ("#FFF0F0"),
			errorsFieldBackColor = Color.FromHex ("#FFD0D0"),

			fnLifeMasterBackColor = Color.FromHex ("#FFF0E0"),
			fnLifeFieldBackColor = Color.FromHex ("#FFE0C0"),

			rnmMasterBackColor = Color.FromHex ("#E0F0FF"),
			rnmFieldBackColor = Color.FromHex ("#C0E0FF"),

			ofdMasterBackColor = Color.FromHex ("#F0F0FF"),
			ofdFieldBackColor = Color.FromHex ("#C8C8FF"),

			tagsMasterBackColor = Color.FromHex ("#E0FFF0"),
			tagsFieldBackColor = Color.FromHex ("#C8FFE4"),

			lowLevelMasterBackColor = Color.FromHex ("#FFF0FF"),
			lowLevelFieldBackColor = Color.FromHex ("#FFC8FF"),

			kktCodesMasterBackColor = Color.FromHex ("#FFFFF0"),
			kktCodesFieldBackColor = Color.FromHex ("#FFFFD0"),

			aboutMasterBackColor = Color.FromHex ("#F0FFF0"),
			aboutFieldBackColor = Color.FromHex ("#D0FFD0"),

			connectorsMasterBackColor = Color.FromHex ("#F8FFF0"),
			connectorsFieldBackColor = Color.FromHex ("#F0FFE0"),

			barCodesMasterBackColor = Color.FromHex ("#FFEED4"),
			barCodesFieldBackColor = Color.FromHex ("#E8CFB6"),

			untoggledSwitchColor = Color.FromHex ("#505050"),
			errorColor = Color.FromHex ("#FF0000"),
			correctColor = Color.FromHex ("#008000"),

			disabledFieldColor = Color.FromHex ("#808080"),
			disabledPageColor = Color.FromHex ("#C0C0C0");
		private const string firstStartRegKey = "HelpShownAt";

		#endregion

		#region Переменные страниц

		private ContentPage headersPage, kktCodesPage, errorsPage, aboutPage, connectorsPage,
			ofdPage, fnLifePage, rnmPage, lowLevelPage, userManualsPage, tagsPage, barCodesPage;

		private Label kktCodesSourceTextLabel, kktCodesHelpLabel, kktCodesErrorLabel, kktCodesResultText,
			errorsResultText, cableLeftSideText, cableRightSideText, cableLeftPinsText, cableRightPinsText,
			cableDescriptionText, aboutLabel,
			fnLifeLabel, fnLifeModelLabel, fnLifeGenericTaxLabel, fnLifeGoodsLabel,
			rnmKKTTypeLabel, rnmINNCheckLabel, rnmRNMCheckLabel, rnmSupport105, rnmSupport11, rnmSupport12,
			lowLevelCommandDescr, unlockLabel,
			tlvDescriptionLabel, tlvTypeLabel, tlvValuesLabel, tlvObligationLabel,
			barcodeDescriptionLabel, rnmTip;
		private List<Label> operationTextLabels = new List<Label> ();

		private Xamarin.Forms.Button kktCodesKKTButton, fnLifeResult, cableTypeButton,
			errorsKKTButton, errorsCodeButton, userManualsKKTButton,
			ofdNameButton, ofdDNSNameButton, ofdIPButton, ofdPortButton, ofdEmailButton, ofdSiteButton, ofdFNSButton,
			ofdDNSNameMButton, ofdIPMButton, ofdPortMButton, ofdDNSNameKButton, ofdIPKButton, ofdPortKButton,
			lowLevelProtocol, lowLevelCommand, lowLevelCommandCode, rnmGenerate;

		private Editor codesSourceText, errorSearchText, commandSearchText, ofdSearchText,
			ofdINN, unlockField,
			fnLifeSerial, tlvTag,
			rnmKKTSN, rnmINN, rnmRNM,
			barcodeField;

		private Xamarin.Forms.Switch fnLife13, fnLifeGenericTax, fnLifeGoods, fnLifeSeason, fnLifeAgents, fnLifeExcise,
			fnLifeAutonomous, fnLifeFFD12, keepAppState, allowService;

		private Xamarin.Forms.DatePicker fnLifeStartDate;

		private StackLayout userManualLayout;

		#endregion

		#region Основной функционал 

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
					HeaderNumber.ToString ("D02"), PageTitle, PageBackColor, HeaderButton_Clicked, true);
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

			// Переход в статус запуска для отмены вызова из оповещения
			AndroidSupport.AppIsRunning = true;

			// Переопределение цветов для закрытых функций
			if (!ca.AllowExtendedFunctionsLevel1)
				{
				kktCodesFieldBackColor = disabledFieldColor;
				kktCodesMasterBackColor = disabledPageColor;
				}
			if (!ca.AllowExtendedFunctionsLevel2)
				{
				tagsFieldBackColor = lowLevelFieldBackColor = connectorsFieldBackColor = disabledFieldColor;
				tagsMasterBackColor = lowLevelMasterBackColor = connectorsMasterBackColor = disabledPageColor;
				}

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

			kktCodesPage = ApplyPageSettings ("KKTCodesPage", "Перевод текста в коды ККТ",
				kktCodesMasterBackColor, headerNumber++);
			kktCodesPage.IsEnabled = ca.AllowExtendedFunctionsLevel1;

			connectorsPage = ApplyPageSettings ("ConnectorsPage", "Разъёмы",
				connectorsMasterBackColor, headerNumber++);
			connectorsPage.IsEnabled = ca.AllowExtendedFunctionsLevel2;

			barCodesPage = ApplyPageSettings ("BarCodesPage", "Штрих-коды",
				barCodesMasterBackColor, headerNumber++);

			aboutPage = ApplyPageSettings ("AboutPage", "О приложении",
				aboutMasterBackColor, headerNumber);

			#endregion

			#region Страница «оглавления»

			AndroidSupport.ApplyLabelSettingsForKKT (headersPage, "KeepAppStateLabel",
				"Помнить настройки приложения", false, false);
			keepAppState = AndroidSupport.ApplySwitchSettings (headersPage, "KeepAppState", false,
				headersFieldBackColor, null, ca.KeepApplicationState);

			AndroidSupport.ApplyLabelSettingsForKKT (headersPage, "AllowServiceLabel",
				"Оставить службу активной после выхода", false, false);
			allowService = AndroidSupport.ApplySwitchSettings (headersPage, "AllowService", false,
				headersFieldBackColor, AllowService_Toggled, AndroidSupport.AllowServiceToStart);

			try
				{
				((CarouselPage)MainPage).CurrentPage = ((CarouselPage)MainPage).Children[(int)ca.CurrentTab];
				}
			catch { }

			#endregion

			#region Страница инструкций

			Label ut = AndroidSupport.ApplyLabelSettingsForKKT (userManualsPage, "SelectionLabel", "Модель ККТ:",
				true, false);
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
				"   ", userManualsFieldBackColor, UserManualsKKTButton_Clicked, true);

			AndroidSupport.ApplyTipLabelSettings (userManualsPage, "HelpLabel",
				"<...> – индикация на экране, [...] – клавиши ККТ", untoggledSwitchColor);

			UserManualsKKTButton_Clicked (null, null);

			#endregion

			#region Страница кодов символов ККТ

			AndroidSupport.ApplyLabelSettingsForKKT (kktCodesPage, "SelectionLabel", "Модель ККТ:",
				true, false);

			string kktTypeName;
			try
				{
				kktTypeName = kkmc.GetKKTTypeNames ()[(int)ca.KKTForCodes];
				}
			catch
				{
				kktTypeName = kkmc.GetKKTTypeNames ()[0];
				ca.KKTForCodes = 0;
				}
			kktCodesKKTButton = AndroidSupport.ApplyButtonSettings (kktCodesPage, "KKTButton",
				kktTypeName, kktCodesFieldBackColor, CodesKKTButton_Clicked, true);

			kktCodesSourceTextLabel = AndroidSupport.ApplyLabelSettingsForKKT (kktCodesPage, "SourceTextLabel",
				"Исходный текст:", true, false);

			codesSourceText = AndroidSupport.ApplyEditorSettings (kktCodesPage, "SourceText",
				kktCodesFieldBackColor, Keyboard.Default, 72, ca.CodesText, SourceText_TextChanged, true);
			codesSourceText.HorizontalOptions = LayoutOptions.Fill;

			AndroidSupport.ApplyLabelSettingsForKKT (kktCodesPage, "ResultTextLabel", "Коды ККТ:",
				true, false);

			kktCodesErrorLabel = AndroidSupport.ApplyTipLabelSettings (kktCodesPage, "ErrorLabel",
				"Часть введённых символов не поддерживается данной ККТ или требует специальных действий для ввода",
				errorColor);

			kktCodesResultText = AndroidSupport.ApplyResultLabelSettings (kktCodesPage, "ResultText", "",
				kktCodesFieldBackColor, true);
			kktCodesResultText.HorizontalTextAlignment = TextAlignment.Start;

			kktCodesHelpLabel = AndroidSupport.ApplyTipLabelSettings (kktCodesPage, "HelpLabel",
				kkmc.GetKKTTypeDescription (ca.KKTForCodes), untoggledSwitchColor);

			AndroidSupport.ApplyButtonSettings (kktCodesPage, "Clear",
				AndroidSupport.ButtonsDefaultNames.Delete, kktCodesFieldBackColor, CodesClear_Clicked);
			AndroidSupport.ApplyButtonSettings (kktCodesPage, "GetFromClipboard",
				AndroidSupport.ButtonsDefaultNames.Copy, kktCodesFieldBackColor, CodesLineGet_Clicked);

			SourceText_TextChanged (null, null);    // Протягивание кодов

			#endregion

			#region Страница ошибок

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "SelectionLabel", "Модель ККТ:", true, false);

			try
				{
				kktTypeName = kkme.GetKKTTypeNames ()[(int)ca.KKTForErrors];
				}
			catch
				{
				kktTypeName = kkme.GetKKTTypeNames ()[0];
				ca.KKTForErrors = 0;
				}
			errorsKKTButton = AndroidSupport.ApplyButtonSettings (errorsPage, "KKTButton",
				kktTypeName, errorsFieldBackColor, ErrorsKKTButton_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "ErrorCodeLabel", "Код / сообщение:", true, false);

			try
				{
				kktTypeName = kkme.GetErrorCodesList (ca.KKTForErrors)[(int)ca.ErrorCode];
				}
			catch
				{
				kktTypeName = kkme.GetErrorCodesList (ca.KKTForErrors)[0];
				ca.ErrorCode = 0;
				}
			errorsCodeButton = AndroidSupport.ApplyButtonSettings (errorsPage, "ErrorCodeButton",
				kktTypeName, errorsFieldBackColor, ErrorsCodeButton_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (errorsPage, "ResultTextLabel", "Расшифровка:", true, false);

			errorsResultText = AndroidSupport.ApplyResultLabelSettings (errorsPage, "ResultText",
				kkme.GetErrorText (ca.KKTForErrors, ca.ErrorCode), errorsFieldBackColor, true);
			errorsResultText.HorizontalTextAlignment = TextAlignment.Start;

			errorSearchText = AndroidSupport.ApplyEditorSettings (errorsPage, "ErrorSearchText", errorsFieldBackColor,
				Keyboard.Default, 30, "", null, true);

			AndroidSupport.ApplyButtonSettings (errorsPage, "ErrorSearchButton",
				AndroidSupport.ButtonsDefaultNames.Find, errorsFieldBackColor, Errors_Find);
			AndroidSupport.ApplyButtonSettings (errorsPage, "ErrorClearButton",
				AndroidSupport.ButtonsDefaultNames.Delete, errorsFieldBackColor, Errors_Clear);

			#endregion

			#region Страница "О программе"

			aboutLabel = AndroidSupport.ApplyLabelSettings (aboutPage, "AboutLabel",
				ProgramDescription.AssemblyTitle + "\n" +
				ProgramDescription.AssemblyDescription + "\n\n" +
				RDGenerics.AssemblyCopyright + "\nv " +
				ProgramDescription.AssemblyVersion +
				"; " + ProgramDescription.AssemblyLastUpdate);
			aboutLabel.FontAttributes = FontAttributes.Bold;
			aboutLabel.HorizontalTextAlignment = TextAlignment.Center;

			AndroidSupport.ApplyButtonSettings (aboutPage, "AppPage", "Страница проекта",
				aboutFieldBackColor, AppButton_Clicked, false);
			AndroidSupport.ApplyButtonSettings (aboutPage, "ADPPage", "Политика разработки и EULA",
				aboutFieldBackColor, ADPButton_Clicked, false);
			AndroidSupport.ApplyButtonSettings (aboutPage, "DevPage", "Спросить разработчика",
				aboutFieldBackColor, DevButton_Clicked, false);
			AndroidSupport.ApplyButtonSettings (aboutPage, "ManualPage", "Видеоруководство пользователя",
				aboutFieldBackColor, ManualButton_Clicked, false);

			AndroidSupport.ApplyButtonSettings (aboutPage, "UpdatePage",
				"Инструмент чтения данных ФН FNReader", aboutFieldBackColor, UpdateButton_Clicked, false);
			AndroidSupport.ApplyButtonSettings (aboutPage, "CommunityPage",
				RDGenerics.AssemblyCompany, aboutFieldBackColor, CommunityButton_Clicked, false);

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				unlockLabel = AndroidSupport.ApplyLabelSettingsForKKT (aboutPage, "UnlockLabel", ca.LockMessage, false, true);
				unlockLabel.IsVisible = true;

				unlockField = AndroidSupport.ApplyEditorSettings (aboutPage, "UnlockField", aboutFieldBackColor,
					Keyboard.Default, 32, "", UnlockMethod, true);
				unlockField.IsVisible = true;
				}

			#endregion

			#region Страница определения срока жизни ФН

			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetModelLabel", "ЗН, номинал или модель ФН:",
				true, false);
			fnLifeSerial = AndroidSupport.ApplyEditorSettings (fnLifePage, "FNLifeSerial", fnLifeFieldBackColor,
				Keyboard.Default, 16, ca.FNSerial, FNLifeSerial_TextChanged, true);
			fnLifeSerial.Margin = new Thickness (0);

			fnLife13 = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLife13", true,
				fnLifeFieldBackColor, FnLife13_Toggled, false);

			fnLifeLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeLabel",
				"", false, false);

			//
			fnLifeModelLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeModelLabel",
				"", false, true);
			fnLifeModelLabel.HorizontalOptions = LayoutOptions.Fill;
			fnLifeModelLabel.HorizontalTextAlignment = TextAlignment.Center;

			//
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetUserParameters", "Значимые параметры:",
				true, false);

			fnLifeGenericTax = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeGenericTax", true,
				fnLifeFieldBackColor, FnLife13_Toggled, !ca.GenericTaxFlag);
			fnLifeGenericTaxLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeGenericTaxLabel",
				"", false, false);

			fnLifeGoods = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeGoods", true,
				fnLifeFieldBackColor, FnLife13_Toggled, !ca.GoodsFlag);
			fnLifeGoodsLabel = AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeGoodsLabel",
				"", false, false);

			fnLifeSeason = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeSeason", false,
				fnLifeFieldBackColor, FnLife13_Toggled, ca.SeasonFlag);
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeSeasonLabel", "Сезонная торговля",
				false, false);

			fnLifeAgents = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeAgents", false,
				fnLifeFieldBackColor, FnLife13_Toggled, ca.AgentsFlag);
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeAgentsLabel", "Платёжный (суб)агент",
				false, false);

			fnLifeExcise = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeExcise", false,
				fnLifeFieldBackColor, FnLife13_Toggled, ca.ExciseFlag);
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeExciseLabel", "Подакцизный товар",
				false, false);

			fnLifeAutonomous = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeAutonomous", false,
				fnLifeFieldBackColor, FnLife13_Toggled, ca.AutonomousFlag);
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeAutonomousLabel", "Автономный режим",
				false, false);

			fnLifeFFD12 = AndroidSupport.ApplySwitchSettings (fnLifePage, "FNLifeDeFacto", false,
				fnLifeFieldBackColor, FnLife13_Toggled, ca.FFD12Flag);
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "FNLifeDeFactoLabel", "ФФД 1.2",
				false, false);

			//
			AndroidSupport.ApplyLabelSettingsForKKT (fnLifePage, "SetDate", "Дата фискализации:",
				false, false);
			fnLifeStartDate = AndroidSupport.ApplyDatePickerSettings (fnLifePage, "FNLifeStartDate", fnLifeFieldBackColor,
				FnLifeStartDate_DateSelected);
			fnLifeStartDate.FontSize *= fontSizeMultiplier;

			//
			fnLifeResult = AndroidSupport.ApplyButtonSettings (fnLifePage, "FNLifeResult", "", fnLifeFieldBackColor,
				FNLifeResultCopy, true);

			AndroidSupport.ApplyTipLabelSettings (fnLifePage, "FNLifeHelpLabel",
				"Нажатие кнопки копирует дату окончания срока жизни в буфер обмена", untoggledSwitchColor);

			//
			AndroidSupport.ApplyButtonSettings (fnLifePage, "Clear",
				AndroidSupport.ButtonsDefaultNames.Delete, fnLifeFieldBackColor, FNLifeClear_Clicked);
			AndroidSupport.ApplyButtonSettings (fnLifePage, "Find",
				AndroidSupport.ButtonsDefaultNames.Find, fnLifeFieldBackColor, FNLifeFind_Clicked);

			// Применение всех названий
			FNLifeSerial_TextChanged (null, null);

			#endregion

			#region Страница заводских и рег. номеров

			AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "SNLabel", "ЗН или модель ККТ:",
				true, false);
			rnmKKTSN = AndroidSupport.ApplyEditorSettings (rnmPage, "SN", rnmFieldBackColor, Keyboard.Default,
				kkts.MaxSerialNumberLength, ca.KKTSerial, RNM_TextChanged, true);

			rnmKKTTypeLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "TypeLabel",
				"", false, false);

			AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "INNLabel", "ИНН пользователя:", true, false);
			rnmINN = AndroidSupport.ApplyEditorSettings (rnmPage, "INN", rnmFieldBackColor, Keyboard.Numeric, 12,
				ca.UserINN, RNM_TextChanged, true);

			rnmINNCheckLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "INNCheckLabel", "", false, false);

			if (ca.AllowExtendedFunctionsLevel2)
				AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMLabel",
					"Регистрационный номер для проверки или произвольное число для генерации:", true, false);
			else
				AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMLabel",
					"Регистрационный номер для проверки:", true, false);

			rnmRNM = AndroidSupport.ApplyEditorSettings (rnmPage, "RNM", rnmFieldBackColor, Keyboard.Numeric, 16,
				ca.RNMKKT, RNM_TextChanged, true);

			rnmRNMCheckLabel = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMCheckLabel", "", false, false);

			rnmGenerate = AndroidSupport.ApplyButtonSettings (rnmPage, "RNMGenerate", "Сгенерировать",
				rnmFieldBackColor, RNMGenerate_Clicked, true);
			rnmGenerate.IsVisible = ca.AllowExtendedFunctionsLevel2;
			rnmGenerate.Margin = new Thickness (0);

			string rnmAbout = "Индикатор ФФД: " +
				"<b><font color=\"#FF4040\">красный</font></b> – поддержка не планируется; " +
				"<b><font color=\"#00C000\">зелёный</font></b> – поддерживается; " +
				"<b><font color=\"#FFFF00\">жёлтый</font></b> – планируется; " +
				"<b><font color=\"#6060FF\">синий</font></b> – нет сведений (на момент релиза этой версии приложения)";
			if (ca.AllowExtendedFunctionsLevel2)
				rnmAbout += ("<br/><br/>Первые 10 цифр РН являются порядковым номером ККТ в реестре и могут быть указаны " +
					"вручную при генерации");

			rnmTip = AndroidSupport.ApplyTipLabelSettings (rnmPage, "RNMAbout", rnmAbout, untoggledSwitchColor);
			rnmTip.TextType = TextType.Html;

			AndroidSupport.ApplyButtonSettings (rnmPage, "Clear",
				AndroidSupport.ButtonsDefaultNames.Delete, rnmFieldBackColor, RNMClear_Clicked);
			AndroidSupport.ApplyButtonSettings (rnmPage, "Find",
				AndroidSupport.ButtonsDefaultNames.Find, rnmFieldBackColor, RNMFind_Clicked);

			rnmSupport105 = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMSupport105", "ФФД 1.05", false, false);
			rnmSupport11 = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMSupport11", "ФФД 1.1", false, false);
			rnmSupport12 = AndroidSupport.ApplyLabelSettingsForKKT (rnmPage, "RNMSupport12", "ФФД 1.2", false, false);
			rnmSupport105.Padding = rnmSupport11.Padding = rnmSupport12.Padding = new Thickness (6);

			RNM_TextChanged (null, null);   // Применение значений

			#endregion

			#region Страница настроек ОФД

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDINNLabel", "ИНН ОФД:", true, false);
			ofdINN = AndroidSupport.ApplyEditorSettings (ofdPage, "OFDINN", ofdFieldBackColor, Keyboard.Numeric, 10,
				ca.OFDINN, OFDINN_TextChanged, true);

			AndroidSupport.ApplyButtonSettings (ofdPage, "OFDINNCopy",
				AndroidSupport.ButtonsDefaultNames.Copy, ofdFieldBackColor, OFDINNCopy_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDNameLabel", "Название:", true, false);
			ofdNameButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDName", "- Выберите или введите ИНН -",
				ofdFieldBackColor, OFDName_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameLabel", "Адрес ОФД:", true, false);
			ofdDNSNameButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSName", "", ofdFieldBackColor,
				Field_Clicked, true);
			ofdIPButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDIP", "", ofdFieldBackColor,
				Field_Clicked, true);
			ofdPortButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPort", "", ofdFieldBackColor,
				Field_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameMLabel", "Адрес ИСМ:", true, false);
			ofdDNSNameMButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSNameM", "", ofdFieldBackColor,
				Field_Clicked, true);
			ofdIPMButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDIPM", "", ofdFieldBackColor,
				Field_Clicked, true);
			ofdPortMButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPortM", "", ofdFieldBackColor,
				Field_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDDNSNameKLabel", "Адрес ОКП:", true, false);
			ofdDNSNameKButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDDNSNameK", OFD.OKPSite,
				ofdFieldBackColor, Field_Clicked, true);
			ofdIPKButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDIPK", OFD.OKPIP,
				ofdFieldBackColor, Field_Clicked, true);
			ofdPortKButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDPortK", OFD.OKPPort,
				ofdFieldBackColor, Field_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDEmailLabel", "E-mail ОФД:", true, false);
			ofdEmailButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDEmail", "", ofdFieldBackColor,
				Field_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDSiteLabel", "Сайт ОФД:", true, false);
			ofdSiteButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDSite", "", ofdFieldBackColor,
				Field_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (ofdPage, "OFDNalogSiteLabel", "Сайт ФНС:", true, false);
			ofdFNSButton = AndroidSupport.ApplyButtonSettings (ofdPage, "OFDNalogSite", OFD.FNSSite,
				ofdFieldBackColor, Field_Clicked, true);

			AndroidSupport.ApplyTipLabelSettings (ofdPage, "OFDHelpLabel",
				"Нажатие кнопок копирует их подписи в буфер обмена", untoggledSwitchColor);

			AndroidSupport.ApplyButtonSettings (ofdPage, "Clear",
				AndroidSupport.ButtonsDefaultNames.Delete, ofdFieldBackColor, OFDClear_Clicked);

			ofdSearchText = AndroidSupport.ApplyEditorSettings (ofdPage, "OFDSearchText", ofdFieldBackColor,
				Keyboard.Default, 30, "", null, true);

			AndroidSupport.ApplyButtonSettings (ofdPage, "OFDSearchButton",
				AndroidSupport.ButtonsDefaultNames.Find, ofdFieldBackColor, OFD_Find);

			OFDINN_TextChanged (null, null); // Протягивание значений

			#endregion

			#region Страница TLV-тегов

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVSearchLabel", "Номер или часть описания:",
				true, false);
			tlvTag = AndroidSupport.ApplyEditorSettings (tagsPage, "TLVSearchText", tagsFieldBackColor,
				Keyboard.Default, 20, ca.TLVData, null, true);

			AndroidSupport.ApplyButtonSettings (tagsPage, "TLVSearchButton",
				AndroidSupport.ButtonsDefaultNames.Find, tagsFieldBackColor, TLVFind_Clicked);
			AndroidSupport.ApplyButtonSettings (tagsPage, "TLVClearButton",
				AndroidSupport.ButtonsDefaultNames.Delete, tagsFieldBackColor, TLVClear_Clicked);

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVDescriptionLabel", "Описание тега:",
				true, false);
			tlvDescriptionLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVDescription", "",
				tagsFieldBackColor, true);
			tlvDescriptionLabel.HorizontalTextAlignment = TextAlignment.Start;

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVTypeLabel", "Тип тега:",
				true, false);
			tlvTypeLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVType", "",
				tagsFieldBackColor, true);
			tlvTypeLabel.HorizontalTextAlignment = TextAlignment.Start;

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVValuesLabel", "Возможные значения тега:",
				true, false);
			tlvValuesLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVValues", "",
				tagsFieldBackColor, true);
			tlvValuesLabel.HorizontalTextAlignment = TextAlignment.Start;

			AndroidSupport.ApplyLabelSettingsForKKT (tagsPage, "TLVObligationLabel", "Обязательность:",
				true, false);
			tlvObligationLabel = AndroidSupport.ApplyResultLabelSettings (tagsPage, "TLVObligation", "",
				tagsFieldBackColor, true);
			tlvObligationLabel.HorizontalTextAlignment = TextAlignment.Start;
			tlvObligationLabel.TextType = TextType.Html;

			AndroidSupport.ApplyTipLabelSettings (tagsPage, "TLVObligationHelpLabel",
				TLVTags.ObligationBasic, untoggledSwitchColor);

			TLVFind_Clicked (null, null);

			#endregion

			#region Страница команд нижнего уровня

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "ProtocolLabel", "Протокол:",
				true, false);
			lowLevelProtocol = AndroidSupport.ApplyButtonSettings (lowLevelPage, "ProtocolButton",
				ll.GetProtocolsNames ()[(int)ca.LowLevelProtocol], lowLevelFieldBackColor, LowLevelProtocol_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandLabel", "Команда:",
				true, false);
			lowLevelCommand = AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandButton",
				ll.GetCommandsList (ca.LowLevelProtocol)[(int)ca.LowLevelCode],
				lowLevelFieldBackColor, LowLevelCommandCodeButton_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandCodeLabel", "Код команды:",
				true, false);
			lowLevelCommandCode = AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandCodeButton",
				ll.GetCommand (ca.LowLevelProtocol, ca.LowLevelCode, false),
				lowLevelFieldBackColor, Field_Clicked, true);

			AndroidSupport.ApplyLabelSettingsForKKT (lowLevelPage, "CommandDescrLabel", "Описание:",
				true, false);

			lowLevelCommandDescr = AndroidSupport.ApplyResultLabelSettings (lowLevelPage, "CommandDescr",
				ll.GetCommand (ca.LowLevelProtocol, ca.LowLevelCode, true), lowLevelFieldBackColor, true);
			lowLevelCommandDescr.HorizontalTextAlignment = TextAlignment.Start;

			AndroidSupport.ApplyTipLabelSettings (lowLevelPage, "LowLevelHelpLabel",
				"Нажатие кнопки копирует команду в буфер обмена", untoggledSwitchColor);

			commandSearchText = AndroidSupport.ApplyEditorSettings (lowLevelPage, "CommandSearchText",
				lowLevelFieldBackColor, Keyboard.Default, 30, "", null, true);

			AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandSearchButton",
				AndroidSupport.ButtonsDefaultNames.Find, lowLevelFieldBackColor, Command_Find);
			AndroidSupport.ApplyButtonSettings (lowLevelPage, "CommandClearButton",
				AndroidSupport.ButtonsDefaultNames.Delete, lowLevelFieldBackColor, Command_Clear);

			#endregion

			#region Страница штрих-кодов

			AndroidSupport.ApplyLabelSettingsForKKT (barCodesPage, "BarcodeFieldLabel", "Данные штрих-кода:",
				true, false);
			barcodeField = AndroidSupport.ApplyEditorSettings (barCodesPage, "BarcodeField",
				barCodesFieldBackColor, Keyboard.Default, BarCodes.MaxSupportedDataLength,
				ca.BarcodeData, BarcodeText_TextChanged, true);

			AndroidSupport.ApplyLabelSettingsForKKT (barCodesPage, "BarcodeDescriptionLabel",
				"Описание штрих-кода:", true, false);
			barcodeDescriptionLabel = AndroidSupport.ApplyResultLabelSettings (barCodesPage, "BarcodeDescription", "",
				barCodesFieldBackColor, true);
			barcodeDescriptionLabel.HorizontalTextAlignment = TextAlignment.Start;

			AndroidSupport.ApplyButtonSettings (barCodesPage, "Clear",
				AndroidSupport.ButtonsDefaultNames.Delete, barCodesFieldBackColor, BarcodeClear_Clicked);
			AndroidSupport.ApplyButtonSettings (barCodesPage, "GetFromClipboard",
				AndroidSupport.ButtonsDefaultNames.Copy, barCodesFieldBackColor, BarcodeGet_Clicked);

			BarcodeText_TextChanged (null, null);

			#endregion

			#region Страница распиновок

			AndroidSupport.ApplyLabelSettingsForKKT (connectorsPage, "CableLabel", "Тип кабеля:", true, false);
			cableTypeButton = AndroidSupport.ApplyButtonSettings (connectorsPage, "CableTypeButton",
				conn.GetCablesNames ()[(int)ca.CableType], connectorsFieldBackColor, CableTypeButton_Clicked, true);

			cableLeftSideText = AndroidSupport.ApplyLabelSettingsForKKT (connectorsPage, "CableLeftSide",
				" ", false, false);
			cableLeftSideText.HorizontalTextAlignment = TextAlignment.Center;
			cableLeftSideText.HorizontalOptions = LayoutOptions.Center;

			cableLeftPinsText = AndroidSupport.ApplyResultLabelSettings (connectorsPage, "CableLeftPins", " ",
				connectorsFieldBackColor, true);

			cableRightSideText = AndroidSupport.ApplyLabelSettingsForKKT (connectorsPage, "CableRightSide",
				" ", false, false);
			cableRightSideText.HorizontalTextAlignment = TextAlignment.Center;
			cableRightSideText.HorizontalOptions = LayoutOptions.Center;

			cableRightPinsText = AndroidSupport.ApplyResultLabelSettings (connectorsPage, "CableRightPins", " ",
				connectorsFieldBackColor, true);

			cableDescriptionText = AndroidSupport.ApplyTipLabelSettings (connectorsPage, "CableDescription",
				" ", untoggledSwitchColor);

			CableTypeButton_Clicked (null, null);

			#endregion

			// Обязательное принятие Политики и EULA
			AcceptPolicy ();
			}

		// Контроль принятия Политики и EULA
		private async void AcceptPolicy ()
			{
			// Политика
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

			// Вступление
			Preferences.Set (firstStartRegKey, ProgramDescription.AssemblyVersion); // Только после принятия

			await ((CarouselPage)MainPage).CurrentPage.DisplayAlert (ProgramDescription.AssemblyVisibleName,
					"Вас приветствует инструмент сервис-инженера ККТ (54-ФЗ)!\r\n\r\n" +

					"На этой странице находится перечень функций приложения, который позволяет перейти к нужному разделу. " +
					"Вернуться сюда можно с помощью кнопки «Назад». Перемещение " +
					"между разделами также доступно по свайпу влево-вправо",

					"Далее");

			await ((CarouselPage)MainPage).CurrentPage.DisplayAlert (ProgramDescription.AssemblyVisibleName,
					"Часть функций скрыта от рядовых пользователей. Чтобы открыть расширенный функционал для специалистов, " +
					"перейдите на страницу «О приложении» и ответьте на несколько простых вопросов. Вопросы для расширенного " +
					"и полного набора опций отличаются",

					"ОК");
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

		// Отправка значения в буфер обмена и получение значения из него
		private void SendToClipboard (string Text)
			{
			try
				{
				Clipboard.SetTextAsync (Text);
				Toast.MakeText (Android.App.Application.Context, "Скопировано в буфер обмена",
					ToastLength.Short).Show ();
				}
			catch { }
			}

		private async Task<string> GetFromClipboard ()
			{
			// Запрос
			string res = "";
			try
				{
				res = await Clipboard.GetTextAsync ();
				}
			catch { }

			// Обработка
			if (string.IsNullOrWhiteSpace (res))
				return "";

			return res;
			}

		// Отправка значения кнопки в буфер
		private void Field_Clicked (object sender, EventArgs e)
			{
			SendToClipboard (((Xamarin.Forms.Button)sender).Text);
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

		// Включение / выключение фоновой службы
		private void AllowService_Toggled (object sender, ToggledEventArgs e)
			{
			AndroidSupport.AllowServiceToStart = allowService.IsToggled;
			}

		/// <summary>
		/// Обработчик события перехода в ждущий режим
		/// </summary>
		protected override void OnSleep ()
			{
			// Переключение состояния
			if (!allowService.IsToggled)
				AndroidSupport.StopRequested = true;

			AndroidSupport.AppIsRunning = false;

			// Сохранение настроек
			ca.KeepApplicationState = keepAppState.IsToggled;
			if (!keepAppState.IsToggled)
				return;

			ca.CurrentTab = (uint)((CarouselPage)MainPage).Children.IndexOf (((CarouselPage)MainPage).CurrentPage);

			// ca.KKTForErrors	// Обновляется в коде программы
			// ca.ErrorCode		// -||-

			ca.FNSerial = fnLifeSerial.Text;
			ca.GenericTaxFlag = !fnLifeGenericTax.IsToggled;
			ca.GoodsFlag = !fnLifeGoods.IsToggled;
			ca.SeasonFlag = fnLifeSeason.IsToggled;
			ca.AgentsFlag = fnLifeAgents.IsToggled;
			ca.ExciseFlag = fnLifeExcise.IsToggled;
			ca.AutonomousFlag = fnLifeAutonomous.IsToggled;
			ca.FFD12Flag = fnLifeFFD12.IsToggled;

			ca.KKTSerial = rnmKKTSN.Text;
			ca.UserINN = rnmINN.Text;
			ca.RNMKKT = rnmRNM.Text;

			ca.OFDINN = ofdINN.Text;

			//ca.LowLevelProtocol	// -||-
			//ca.LowLevelCode		// -||-

			//ca.KKTForCodes	// -||-
			ca.CodesText = codesSourceText.Text;

			ca.BarcodeData = barcodeField.Text;
			//ca.CableType		// -||-

			ca.TLVData = tlvTag.Text;
			}

		/// <summary>
		/// Возврат в интерфейс
		/// </summary>
		protected override void OnResume ()
			{
			AndroidSupport.AppIsRunning = true;
			}

		#endregion

		#region Коды ошибок ККТ

		// Выбор модели ККТ
		private KKTErrorsList kkme = new KKTErrorsList ();
		private async void ErrorsKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			List<string> list = kkme.GetKKTTypeNames ();
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
			}

		private void Errors_Clear (object sender, EventArgs e)
			{
			errorSearchText.Text = "";
			}

		#endregion

		#region Коды символов ККТ

		// Ввод текста для перевода в коды символов
		private void SourceText_TextChanged (object sender, TextChangedEventArgs e)
			{
			kktCodesResultText.Text = "";
			kktCodesErrorLabel.IsVisible = !Decode ();

			kktCodesSourceTextLabel.Text = "Исходный текст (" + codesSourceText.Text.Length.ToString () + "):";
			}

		// Выбор модели ККТ
		private KKTCodes kkmc = new KKTCodes ();
		private async void CodesKKTButton_Clicked (object sender, EventArgs e)
			{
			// Запрос модели ККТ
			List<string> list = kkmc.GetKKTTypeNames ();
			string res = await kktCodesPage.DisplayActionSheet ("Выберите модель ККТ:", "Отмена", null, list.ToArray ());

			// Установка модели
			int i;
			if ((i = list.IndexOf (res)) < 0)
				return;

			kktCodesKKTButton.Text = res;
			ca.KKTForCodes = (uint)i;
			kktCodesHelpLabel.Text = kkmc.GetKKTTypeDescription (ca.KKTForCodes);

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
					kktCodesResultText.Text += "xxx   ";
					res = false;
					}
				else
					{
					kktCodesResultText.Text += (s + "   ");
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

		// Очистка полей
		private void CodesClear_Clicked (object sender, EventArgs e)
			{
			codesSourceText.Text = "";
			}

		// Получение из буфера
		private async void CodesLineGet_Clicked (object sender, EventArgs e)
			{
			codesSourceText.Text = await GetFromClipboard ();
			}

		#endregion

		#region Команды нижнего уровня

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
				LowLevelCommandCodeButton_Clicked (sender, null);
				}
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
			}

		private void Command_Clear (object sender, EventArgs e)
			{
			commandSearchText.Text = "";
			}

		#endregion

		#region Срок жизни ФН

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
				fnLifeGenericTaxLabel.Text = "УСН / ЕСХН / ПСН";
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
			fnlf.FFD12 = fnLifeFFD12.IsToggled;
			fnlf.MarkFN = fnLife13.IsEnabled || fns.IsFNCompatibleWithFFD12 (fnLifeSerial.Text);    // Признак распознанного ЗН ФН

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
				if (!fnlf.MarkFN)
					{
					fnLifeResult.TextColor = errorColor;

					fnLifeResult.Text += ("\n(выбранный ФН исключён из реестра ФНС)");
					fnLifeModelLabel.BackgroundColor = StatusToColor (KKTSerial.FFDSupportStatuses.Unsupported);
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

		// Копирование срока жизни ФН
		private string fnLifeResultDate = "";
		private void FNLifeResultCopy (object sender, EventArgs e)
			{
			SendToClipboard (fnLifeResultDate);
			}

		// Очистка полей
		private void FNLifeClear_Clicked (object sender, EventArgs e)
			{
			fnLifeSerial.Text = "";
			}

		// Поиск по сигнатуре
		private void FNLifeFind_Clicked (object sender, EventArgs e)
			{
			string sig = fns.FindSignatureByName (fnLifeSerial.Text);
			if (sig != "")
				fnLifeSerial.Text = sig;
			}

		#endregion

		#region РНМ и ЗН

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

		// Очистка полей
		private void RNMClear_Clicked (object sender, EventArgs e)
			{
			rnmKKTSN.Text = "";
			rnmINN.Text = "";
			rnmRNM.Text = "";
			}

		// Поиск по сигнатуре
		private void RNMFind_Clicked (object sender, EventArgs e)
			{
			string sig = kkts.FindSignatureByName (rnmKKTSN.Text);
			if (sig != "")
				rnmKKTSN.Text = sig;
			}

		#endregion

		#region ОФД

		// Ввод названия или ИНН ОФД
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

		// Копирование ИНН в буфер обмена
		private void OFDINNCopy_Clicked (object sender, EventArgs e)
			{
			SendToClipboard (ofdINN.Text);
			}

		// Поиск по названию ОФД
		private int lastOFDSearchOffset2 = 0;
		private void OFD_Find (object sender, EventArgs e)
			{
			List<string> codes = ofd.GetOFDNames ();
			codes.AddRange (ofd.GetOFDINNs ());

			string text = ofdSearchText.Text.ToLower ();

			lastOFDSearchOffset2++;
			for (int i = 0; i < codes.Count; i++)
				if (codes[(i + lastOFDSearchOffset2) % codes.Count].ToLower ().Contains (text))
					{
					lastOFDSearchOffset2 = (i + lastOFDSearchOffset2) % codes.Count;
					ofdNameButton.Text = codes[lastOFDSearchOffset2 % (codes.Count / 2)];

					string s = ofd.GetOFDINNByName (ofdNameButton.Text);
					if (s != "")
						ca.OFDINN = ofdINN.Text = s;

					return;
					}
			}

		// Очистка полей
		private void OFDClear_Clicked (object sender, EventArgs e)
			{
			ofdINN.Text = ofdSearchText.Text = "";
			}

		#endregion

		#region О приложении

		// Страница обновлений
		private async void UpdateButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				await Launcher.OpenAsync (ProgramDescription.AssemblyFNReaderLink);
				}
			catch
				{
				await aboutPage.DisplayAlert (ProgramDescription.AssemblyVisibleName,
					"Веб-браузер отсутствует на этом устройстве", "OK");
				}
			}

		// Страница проекта
		private async void AppButton_Clicked (object sender, EventArgs e)
			{
			try
				{
				await Launcher.OpenAsync (RDGenerics.AssemblyGitLink + ProgramDescription.AssemblyMainName);
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
				await Launcher.OpenAsync (RDGenerics.ADPLink);
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
			List<string> comm = new List<string> { "Приветственная страница", "ВКонтакте", "Telegram" };
			string res = await aboutPage.DisplayActionSheet ("Выберите сообщество", "Отмена", null, comm.ToArray ());

			if (!comm.Contains (res))
				return;

			try
				{
				switch (comm.IndexOf (res))
					{
					case 1:
						await Launcher.OpenAsync (RDGenerics.LabVKLink);
						break;

					case 2:
						await Launcher.OpenAsync (RDGenerics.LabTGLink);
						break;

					case 0:
						await Launcher.OpenAsync (RDGenerics.DPModuleLink);
						break;
					}
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
					To = new List<string> () { RDGenerics.LabMailLink }
					};
				await Email.ComposeAsync (message);
				}
			catch
				{
				Toast.MakeText (Android.App.Application.Context, "Почтовый агент отсутствует на этом устройстве",
					ToastLength.Long).Show ();
				}
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

		#endregion

		#region Руководства пользователей

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

		#endregion

		#region Теги TLV

		private TLVTags tlvt = new TLVTags ();
		private void TLVFind_Clicked (object sender, EventArgs e)
			{
			if (tlvt.FindTag (tlvTag.Text))
				{
				tlvDescriptionLabel.Text = tlvt.LastDescription;
				tlvTypeLabel.Text = tlvt.LastType;
				tlvValuesLabel.Text = tlvt.LastValuesSet;
				tlvObligationLabel.Text = tlvt.LastObligation;
				}
			else
				{
				tlvDescriptionLabel.Text = tlvTypeLabel.Text = tlvValuesLabel.Text =
					tlvObligationLabel.Text = "(не найдено)";
				}
			}

		private void TLVClear_Clicked (object sender, EventArgs e)
			{
			tlvTag.Text = "";
			TLVFind_Clicked (null, null);
			}

		#endregion

		#region Штрих-коды

		// Ввод данных штрих-кода
		private BarCodes barc = new BarCodes ();
		private void BarcodeText_TextChanged (object sender, TextChangedEventArgs e)
			{
			barcodeDescriptionLabel.Text = barc.GetBarcodeDescription (barcodeField.Text);
			}

		// Очистка полей
		private void BarcodeClear_Clicked (object sender, EventArgs e)
			{
			barcodeField.Text = "";
			}

		// Получение из буфера
		private async void BarcodeGet_Clicked (object sender, EventArgs e)
			{
			barcodeField.Text = await GetFromClipboard ();
			}

		#endregion

		#region Распиновки

		private Connectors conn = new Connectors ();
		private async void CableTypeButton_Clicked (object sender, EventArgs e)
			{
			int idx = (int)ca.CableType;
			string res = conn.GetCablesNames ()[idx];

			if (sender != null)
				{
				// Запрос модели ККТ
				res = await connectorsPage.DisplayActionSheet ("Выберите тип кабеля:", "Отмена", null,
					conn.GetCablesNames ().ToArray ());

				// Установка типа кабеля
				idx = conn.GetCablesNames ().IndexOf (res);
				if (idx < 0)
					return;
				}

			// Установка полей
			cableTypeButton.Text = res;
			ca.CableType = (uint)idx;

			cableLeftSideText.Text = "Со стороны " + conn.GetCableConnector ((uint)idx, false);
			cableLeftPinsText.Text = conn.GetCableConnectorPins ((uint)idx, false);

			cableRightSideText.Text = "Со стороны " + conn.GetCableConnector ((uint)idx, true);
			cableRightPinsText.Text = conn.GetCableConnectorPins ((uint)idx, true);

			cableDescriptionText.Text = conn.GetCableConnectorDescription ((uint)idx, false) + "\n\n" +
				conn.GetCableConnectorDescription ((uint)idx, true);
			}

		#endregion
		}
	}
