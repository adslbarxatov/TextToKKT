#if !ANDROID
using Microsoft.Win32;
#else
using Xamarin.Essentials;
#endif

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к настройкам приложения
	/// </summary>
	public class ConfigAccessor
		{
		// Константы
		private const string nullValue = "0";

		// Метод получает значение настройки из реестра
		private string GetSetting (string ValueName)
			{
			string res = "";
			try
				{
#if !ANDROID
				res = Registry.GetValue (ProgramDescription.AssemblySettingsKey, ValueName, "").ToString ();
#else
				res = Preferences.Get (ValueName, "").ToString ();
#endif
				}
			catch
				{
				}

			return res;
			}

		// Метод задаёт значение настройки в реестре
		private void SetSetting (string ValueName, string Value)
			{
			try
				{
#if !ANDROID
				Registry.SetValue (ProgramDescription.AssemblySettingsKey, ValueName, Value);
#else
				Preferences.Set (ValueName, Value);
#endif
				}
			catch
				{
				}
			}

		/// <summary>
		/// Конструктор. Загружает настройки приложения
		/// </summary>
		public ConfigAccessor ()
			{
			// Получение настроек
			try
				{
				windowLeft = int.Parse (GetSetting (windowLeftPar));
				windowTop = int.Parse (GetSetting (windowTopPar));
				}
			catch { }

			keepApplicationState = GetSetting (keepApplicationStatePar) != nullValue;
			if (!keepApplicationState)
				return;

			try
				{
				currentTab = uint.Parse (GetSetting (currentTabPar));

				kktForErrors = uint.Parse (GetSetting (kktForErrorsPar));
				onlyNewKKTErrors = GetSetting (onlyNewKKTErrorsPar) != nullValue;
				errorCode = uint.Parse (GetSetting (errorCodePar));

				fnSerial = GetSetting (fnSerialPar);
				fnLifeFlags = uint.Parse (GetSetting (fnLifeFlagsPar));

				kktSerial = GetSetting (kktSerialPar);
				userINN = GetSetting (userINNPar);
				rnmKKT = GetSetting (rnmKKTPar);

				ofdINN = GetSetting (ofdINNPar);

				lowLevelCommandsATOL = GetSetting (lowLevelCommandsATOLPar) != nullValue;
				lowLevelCode = uint.Parse (GetSetting (lowLevelCodePar));

				kktForCodes = uint.Parse (GetSetting (kktForCodesPar));
				onlyNewKKTCodes = GetSetting (onlyNewKKTCodesPar) != nullValue;
				codesText = GetSetting (codesTextPar);
				}
			catch
				{
				}
			}

		/// <summary>
		/// Возвращает или задаёт флаг, указывающий на необходимость хранения
		/// и восстановления последних настроек приложения перед выходом из него
		/// </summary>
		public bool KeepApplicationState
			{
			get
				{
				return keepApplicationState;
				}
			set
				{
				keepApplicationState = value;
				SetSetting (keepApplicationStatePar, keepApplicationState ? keepApplicationStatePar : nullValue);
				}
			}
		private bool keepApplicationState = false;
		private const string keepApplicationStatePar = "KAS";

		/// <summary>
		/// Возвращает или задаёт номер текущей выбранной вкладки
		/// </summary>
		public uint CurrentTab
			{
			get
				{
				return currentTab;
				}
			set
				{
				currentTab = value;
				SetSetting (currentTabPar, currentTab.ToString ());
				}
			}
		private uint currentTab = 0;
		private const string currentTabPar = "CT";

		/// <summary>
		/// Возвращает или задаёт номер выбранной ККТ на вкладке ошибок
		/// </summary>
		public uint KKTForErrors
			{
			get
				{
				return kktForErrors;
				}
			set
				{
				kktForErrors = value;
				SetSetting (kktForErrorsPar, kktForErrors.ToString ());
				}
			}
		private uint kktForErrors = 0;
		private const string kktForErrorsPar = "KKTFE";

		/// <summary>
		/// Возвращает или задаёт номер ошибки на вкладке ошибок
		/// </summary>
		public uint ErrorCode
			{
			get
				{
				return errorCode;
				}
			set
				{
				errorCode = value;
				SetSetting (errorCodePar, errorCode.ToString ());
				}
			}
		private uint errorCode = 0;
		private const string errorCodePar = "EC";

		/// <summary>
		/// Возвращает или задаёт заводской номер ФН
		/// </summary>
		public string FNSerial
			{
			get
				{
				return fnSerial;
				}
			set
				{
				fnSerial = value;
				SetSetting (fnSerialPar, fnSerial);
				}
			}
		private string fnSerial = "";
		private const string fnSerialPar = "FNS";

		/// <summary>
		/// Возвращает или задаёт флаг основной системы налогообложения
		/// </summary>
		public bool GenericTaxFlag
			{
			get
				{
				return ((fnLifeFlags & 0x01) == 0);
				}
			set
				{
				SetFNLifeFlag (0x01, !value);
				}
			}

		/// <summary>
		/// Возвращает флаг режима товаров
		/// </summary>
		public bool GoodsFlag
			{
			get
				{
				return ((fnLifeFlags & 0x02) == 0);
				}
			set
				{
				SetFNLifeFlag (0x02, !value);
				}
			}

		/// <summary>
		/// Возвращает флаг сезонного режима торговли
		/// </summary>
		public bool SeasonFlag
			{
			get
				{
				return ((fnLifeFlags & 0x04) != 0);
				}
			set
				{
				SetFNLifeFlag (0x04, value);
				}
			}

		/// <summary>
		/// Возвращает флаг агентской схемы работы
		/// </summary>
		public bool AgentsFlag
			{
			get
				{
				return ((fnLifeFlags & 0x08) != 0);
				}
			set
				{
				SetFNLifeFlag (0x08, value);
				}
			}

		/// <summary>
		/// Возвращает флаг подакцизных товаров
		/// </summary>
		public bool ExciseFlag
			{
			get
				{
				return ((fnLifeFlags & 0x10) != 0);
				}
			set
				{
				SetFNLifeFlag (0x10, value);
				}
			}

		/// <summary>
		/// Возвращает флаг автономного режима работы
		/// </summary>
		public bool AutonomousFlag
			{
			get
				{
				return ((fnLifeFlags & 0x20) != 0);
				}
			set
				{
				SetFNLifeFlag (0x20, value);
				}
			}

		private uint fnLifeFlags = 0;
		private const string fnLifeFlagsPar = "FNLF";

		// Метод устанавливает флаг в поле параметров определения срока жизни ФН
		private void SetFNLifeFlag (uint FlagBit, bool Set)
			{
			fnLifeFlags &= (0x3F ^ FlagBit);
			if (Set)
				fnLifeFlags |= FlagBit;
			SetSetting (fnLifeFlagsPar, fnLifeFlags.ToString ());
			}

		/// <summary>
		/// Возвращает или задаёт заводской номер ККТ
		/// </summary>
		public string KKTSerial
			{
			get
				{
				return kktSerial;
				}
			set
				{
				kktSerial = value;
				SetSetting (kktSerialPar, kktSerial);
				}
			}
		private string kktSerial = "";
		private const string kktSerialPar = "KKTS";

		/// <summary>
		/// Возвращает или задаёт ИНН пользователя
		/// </summary>
		public string UserINN
			{
			get
				{
				return userINN;
				}
			set
				{
				userINN = value;
				SetSetting (userINNPar, userINN);
				}
			}
		private string userINN = "";
		private const string userINNPar = "UINN";

		/// <summary>
		/// Возвращает или задаёт регистрационный номер ККТ
		/// </summary>
		public string RNMKKT
			{
			get
				{
				return rnmKKT;
				}
			set
				{
				rnmKKT = value;
				SetSetting (rnmKKTPar, rnmKKT);
				}
			}
		private string rnmKKT = "";
		private const string rnmKKTPar = "RNMKKT";

		/// <summary>
		/// Возвращает или задаёт ИНН оператора фискальных данных
		/// </summary>
		public string OFDINN
			{
			get
				{
				return ofdINN;
				}
			set
				{
				ofdINN = value;
				SetSetting (ofdINNPar, ofdINN);
				}
			}
		private string ofdINN = "";
		private const string ofdINNPar = "OFDINN";

		/// <summary>
		/// Возвращает или задаёт флаг, указывающий на выбор команды нижнего уровня
		/// из списка АТОЛ
		/// </summary>
		public bool LowLevelCommandsATOL
			{
			get
				{
				return lowLevelCommandsATOL;
				}
			set
				{
				lowLevelCommandsATOL = value;
				SetSetting (lowLevelCommandsATOLPar, lowLevelCommandsATOL ? lowLevelCommandsATOLPar : nullValue);
				}
			}
		private bool lowLevelCommandsATOL = true;
		private const string lowLevelCommandsATOLPar = "LLCA";

		/// <summary>
		/// Возвращает или задаёт номер команды нижнего уровня
		/// </summary>
		public uint LowLevelCode
			{
			get
				{
				return lowLevelCode;
				}
			set
				{
				lowLevelCode = value;
				SetSetting (lowLevelCodePar, lowLevelCode.ToString ());
				}
			}
		private uint lowLevelCode = 0;
		private const string lowLevelCodePar = "LLC";

		/// <summary>
		/// Возвращает или задаёт номер выбранной ККТ на вкладке кодов символов
		/// </summary>
		public uint KKTForCodes
			{
			get
				{
				return kktForCodes;
				}
			set
				{
				kktForCodes = value;
				SetSetting (kktForCodesPar, kktForCodes.ToString ());
				}
			}
		private uint kktForCodes = 0;
		private const string kktForCodesPar = "KKTFC";

		/// <summary>
		/// Возвращает или задаёт текст для преобразования в коды символов
		/// </summary>
		public string CodesText
			{
			get
				{
				return codesText;
				}
			set
				{
				codesText = value;
				SetSetting (codesTextPar, codesText);
				}
			}
		private string codesText = "";
		private const string codesTextPar = "KKTCT";

		/// <summary>
		/// Возвращает или задаёт флаг, указывающий на использование только новой ККТ
		/// на вкладке ошибок
		/// </summary>
		public bool OnlyNewKKTErrors
			{
			get
				{
				return onlyNewKKTErrors;
				}
			set
				{
				onlyNewKKTErrors = value;
				SetSetting (onlyNewKKTErrorsPar, onlyNewKKTErrors ? onlyNewKKTErrorsPar : nullValue);
				}
			}
		private bool onlyNewKKTErrors = true;
		private const string onlyNewKKTErrorsPar = "ONKKTE";

		/// <summary>
		/// Возвращает или задаёт флаг, указывающий на использование только новой ККТ
		/// на вкладке кодов символов
		/// </summary>
		public bool OnlyNewKKTCodes
			{
			get
				{
				return onlyNewKKTCodes;
				}
			set
				{
				onlyNewKKTCodes = value;
				SetSetting (onlyNewKKTCodesPar, onlyNewKKTCodes ? onlyNewKKTCodesPar : nullValue);
				}
			}
		private bool onlyNewKKTCodes = true;
		private const string onlyNewKKTCodesPar = "ONKKTC";

		/// <summary>
		/// Возвращает или задаёт левое смещение окна приложения
		/// </summary>
		public int WindowLeft
			{
			get
				{
				return windowLeft;
				}
			set
				{
				windowLeft = value;
				SetSetting (windowLeftPar, windowLeft.ToString ());
				}
			}
		private int windowLeft = 0;
		private const string windowLeftPar = "WL";

		/// <summary>
		/// Возвращает или задаёт верхнее смещение окна приложения
		/// </summary>
		public int WindowTop
			{
			get
				{
				return windowTop;
				}
			set
				{
				windowTop = value;
				SetSetting (windowTopPar, windowTop.ToString ());
				}
			}
		private int windowTop = 0;
		private const string windowTopPar = "WT";
		}
	}
