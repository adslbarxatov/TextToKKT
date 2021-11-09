// Общие перечисления

// Возможные типы фискальных документов
enum FNDocumentTypes
	{
	// Отчёт о регистрации
	Registration = 0x01,

	// Отчёт о регистрации под ФФД 1.2
	Registration_12 = 0x41,

	// Отчёт об изменении реквизитов регистрации
	RegistrationChange = 0x0B,

	// Отчёт об изменении реквизитов регистрации под ФФД 1.2
	RegistrationChange_12 = 0x4B,

	// Открытие смены
	OpenSession = 2,

	// Отчёт о текущем состоянии расчётов
	CurrentState = 21,

	// Кассовый чек
	Bill = 3,

	// Чек коррекции
	CorrectionBill = 31,

	// БСО
	Blank = 4,

	// БСО коррекции
	CorrectionBlank = 41,

	// Закрытие смены
	CloseSession = 5,

	// Закрытие фискального режима
	CloseFiscalStorage = 6,

	// Подтверждение оператора
	Confirmation = 7,

	// Запрос о коде маркировки
	MarkCodeRequest = 81,

	// Уведомление о реализации кода маркировки
	MarkCodeNotification = 82,

	// Ответ на запрос о коде маркировки
	MarkCodeRequestAnswer = 83,

	// Ответ на уведомление о реализации кода маркировки
	MarkCodeNotificationAnswer = 84,

	// Неизвестный тип документа
	UnknownType = 255
	};

#define REG_CAUSE(type)		(type == Registration) || (type == RegistrationChange) || \
	(type == Registration_12) || (type == RegistrationChange_12)
#define REREG_CAUSE(type)	(type == RegistrationChange) || (type == RegistrationChange_12)

// Возможные фазы жизни ФН
enum FNLifePhases
	{
	// Технологический режим
	FactoryMode = 0x00,

	// Готовность к фискализации
	ReadyForLaunch = 0x01,

	// Фискальный режим
	FiscalMode = 0x03,

	// Архив закрыт, передача документов
	AfterFiscalMode = 0x07,

	// Архив закрыт, документы переданы
	ArchiveClosed = 0x0F,

	// Неизвестный режим
	UnknownMode = -1
	};

// Возможные результаты общения с COM-портом
enum SendAndReceiveResults
	{
	// Успешно
	Ok = 0,

	// Ошибка записи
	WriteError = -11,

	// Таймаут чтения
	ReadTimeout = -21,

	// Ошибка чтения
	ReadError = -22,

	// Некорректная длина ответного сообщения
	AnswerLengthError = -23,

	// Некорректная контрольная сумма ответного сообщения
	AnswerCRCError = -24,

	// Получен ответ, содержащий сообщение об ошибке
	AnswerLogicError = -31
	};

// Обрабатываемые TLV-теги
enum TLVTags
	{
	// Стандартные теги

	// Флаг работы в составе автоматического устройства расчётов (1001)
	AutomaticFlag = 0x03E9,

	#define	PROC_AUTOMATICFLAG(src,dest,type)\
		case AutomaticFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  ККТ установлена в автомате\r\n", dest);\
			break;

	// Флаг автономного режима (1002)
	AutonomousFlag = 0x03EA,

	#define PROC_AUTONOMOUSFLAG(src,dest,type)\
		case AutonomousFlag:\
			if (REG_CAUSE (type))\
				sprintf (dest, src ? "%s  Автономный режим (без ОФД)\r\n" : "%s  Режим передачи данных\r\n", dest);\
			break;

	// E-mail/телефон покупателя (1008)
	ClientAddress = 0x03F0,

	#define PROC_CLIENTADDRESS(src,dest)\
		case ClientAddress:\
			sprintf (dest, "%s  E-mail или телефон покупателя: %s\r\n", dest, src);\
			break;

	// Адрес расчётов (1009)
	RegistrationAddress = 0x03F1,

	#define PROC_REGISTRATIONADDRESS(src,dest,type,preproc)\
		case RegistrationAddress:\
			if (REG_CAUSE (type))\
				{\
				preproc;\
				sprintf (dest, "%s  Адрес расчёта: %s\r\n", dest, src);\
				}\
			break;

	// Дата и время документа (1012)
	DocumentDateTime = 0x03F4,

	// Заводской номер ККТ (1013)
	RegisterSerialNumber = 0x03F5,

	// ИНН ОФД (1017)
	OFD_INN = 0x03F9,

	// ИНН пользователя (1018)
	UserINN = 0x03FA,

	#define PROC_USERINN	"%s  ИНН пользователя: %s\r\n"

	// Сумма расчёта (1020)
	OperationSumma = 0x03FC,

	// Имя кассира (1021)
	CashierName = 0x03FD,

	#define PROC_CASHIERNAME(src,dest,preproc)\
		case CashierName:\
			preproc;\
			sprintf (dest, "%s  Кассир: %s\r\n", dest, src);\
			break;

	// Количество товара (1023)
	GoodsCount = 0x03FF,

	// Наименование предмета расчёта (1030)
	GoodName = 0x0406,

	// Сумма наличными (1031)
	RealCashValue = 0x0407,

	// Номер автомата (терминала) (1036)
	TerminalNumber = 0x040C,

	#define PROC_TERMINALNUMBER(src,dest,type)\
		case TerminalNumber:\
			if (REG_CAUSE (type))\
				sprintf (dest, "%s  Номер автомата: %s\r\n", dest, src);\
			break;

	// Регистрационный номер ККТ (1037)
	RegistrationNumber = 0x040D,

	#define PROC_REGISTRATIONNUMBER	"%s  Регистрационный номер ККТ: %s\r\n"

	// Номер смены, в которой создан документ (1038)
	SessionNumber = 0x040E,

	#define PROC_SESSIONNUMBER	"%s  Номер смены: %u\r\n"

	// Номер фискального документа (1040)
	FiscalDocumentNumber = 0x0410,

	// Заводской номер ФН (1041)
	FNSerialNumber = 0x0411,

	// Стоимость товарной позиции (1043)
	ItemResult = 0x0413,
		
	// Имя пользователя (1048)
	UserName = 0x0418,

	#define PROC_USERNAME	"%s  Пользователь: %s\r\n"

	// Признак расчёта (1054)
	OperationType = 0x041E,

	// Применённая СНО (1055)
	AppliedTaxSystem = 0x041F,

	// Флаг шифрования (1056)
	EncryptionFlag = 0x0420,

	#define PROC_ENCRYPTIONFLAG(src,dest,type)\
		case EncryptionFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  Шифрование фискальных данных\r\n", dest);\
			break;

	// Название предмета расчёта (1059)
	PaymentObject = 0x0423,

	// Адрес сайта ФНС (1060)
	TaxServiceAddress = 0x0424,
		
	// Флаги систем налогообложения (1062)
	TaxFlags = 0x0426,

	#define PROC_TAXFLAGS(src,dest)	\
		case TaxFlags:\
		case AppliedTaxSystem:\
			sprintf (dest, "%s  Налогообложение: %s\r\n", dest, GetTaxFlags (src));\
			break;

	// Фискальный признак документа (1077)
	FiscalSignature = 0x0435,

	// Фискальный признак сообщения (квитанция, 1078)
	FiscalConfirmation = 0x0436,

	// Стоимость единицы товара (1079)
	ItemCost = 0x0437,

	// Сумма безналичными (1081)
	ElectronicCashValue = 0x0439,

	// Количество непереданных документов (1097)
	UnsentDocumentsCount = 0x0449,

	// Количество непереданных документов (1098)
	FirstUnsentDocumentDate = 0x044A,

	// Причина перерегистрации (1101)
	RegistrationChangeCause = 0x044D,

	#define PROC_REGISTRATIONCHANGECAUSE(src,dest,type)\
		case RegistrationChangeCause:\
			if (REREG_CAUSE (type))\
				sprintf (dest, "%s  Причина перерегистрации: %s\r\n", dest, GetRegistrationChangeCause (src));\
			break;

	// Флаг работы в сети Интернет (1108)
	InternetFlag = 0x0454,

	#define PROC_INTERNETFLAG(src,dest,type)\
		case InternetFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  Работа в сети интернет\r\n", dest);\
			break;

	// Флаг режима услуг (1109)
	ServiceFlag = 0x0455,

	#define PROC_SERVICEFLAG(src,dest,type)\
		case ServiceFlag:\
			if (REG_CAUSE (type))\
				sprintf (dest, src ? "%s  Режим услуг\r\n" : "%s  Режим товаров\r\n", dest);\
			break;

	// Флаг формирования БСО (1110)
	BlankFlag = 0x0456,

	#define PROC_BLANKFLAG(src,dest,type)\
		case BlankFlag:\
			if (REG_CAUSE (type))\
				sprintf (dest, src ? "%s  Режим системы БСО\r\n": "%s  Режим кассовых чеков\r\n", dest);\
			break;

	// Код товарной позиции (код маркировки) (1162)
	GoodCode = 0x048A,

	// Место расчётов (1187)
	RegistrationPlace = 0x04A3,

	#define PROC_REGISTRATIONPLACE(src,dest,type,preproc)\
		case RegistrationPlace:\
			if (REG_CAUSE (type))\
				{\
				preproc;\
				sprintf (dest, "%s  Место расчёта: %s\r\n", dest, src);\
				}\
			break;

	// Флаг азартных игр (1193)
	GamesFlag = 0x04A9,

	#define PROC_GAMESFLAG(src,dest,type)\
		case GamesFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  Используется при проведении азартных игр\r\n", dest);\
			break;

	// Ставка НДС (1199)
	NDS = 0x04AF,

	// ИНН кассира (1203)
	CashierINN = 0x04B3,

	#define PROC_CASHIERINN(src,dest)\
		case CashierINN:\
			sprintf (dest, "%s  ИНН кассира: %s\r\n", dest, src);\
			break;

	// Флаг подакцизных товаров (1207)
	ExciseFlag = 0x04B7,

	#define PROC_EXCISEFLAG(src,dest,type)\
		case ExciseFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  Используется для продажи подакцизных товаров\r\n", dest);\
			break;

	// Версия формата фискальных документов (1209)
	FFDVersion = 0x04B9,

	#define PROC_FFDVERSION(src,dest)\
		case FFDVersion:\
			sprintf (dest, "%s  Версия ФФД: %s\r\n", dest, GetFFDVersion (src));\
			break;

	// Признак предмета расчёта (1212)
	ResultObject = 0x04BC,

	// Признак способа расчёта (1214)
	ResultMethod = 0x04BE,

	// Теги файлов архивов ФН

	// Отчёт о регистрации
	FNA_Registration = Registration + 100,

	// Отчёт об изменении реквизитов регистрации
	FNA_RegistrationChange = RegistrationChange + 100,

	// Открытие смены
	FNA_OpenSession = OpenSession + 100,

	// Отчёт о текущем состоянии расчётов
	FNA_CurrentState = CurrentState + 100,

	// Кассовый чек
	FNA_Bill = Bill + 100,

	// Чек коррекции
	FNA_CorrectionBill = CorrectionBill + 100,

	// БСО
	FNA_Blank = Blank + 100,

	// БСО коррекции
	FNA_CorrectionBlank = CorrectionBlank + 100,

	// Закрытие смены
	FNA_CloseSession = CloseSession + 100,

	// Закрытие фискального режима
	FNA_CloseFiscalStorage = CloseFiscalStorage + 100,

	// Подтверждение оператора
	FNA_Confirmation = Confirmation + 100,

	// Фискальный документ (автономный режим, 65001)
	FNA_AutonomousArray = 0xFDE9,

	// Фискальный документ (режим передачи данных, 65002)
	FNA_OFDArray = 0xFDEA,

	// Фискальный документ (автономный режим, ФФД 1.1, 65011)
	FNA_AutonomousArray_1_1 = 0xFDF3,

	// Фискальный документ (режим передачи данных, ФФД 1.1, 65012)
	FNA_OFDArray_1_1 = 0xFDF4,

	// Фискальный документ (автономный режим, ФФД 1.2, 65021)
	FNA_AutonomousArray_1_2 = 0xFDFD,

	// Фискальный документ (режим передачи данных, ФФД 1.2, 65022)
	FNA_OFDArray_1_2 = 0xFDFE,
		
	// Тег, сообщающий о внутренней ошибке ФН при считывании
	ReadingFailure = 9999,

	// ФП сообщения
	FNASignature = 301,

	// Заголовок выгрузки данных регистрации
	FNARegistrationProtocol = 304,

	// Заголовок выгрузки данных закрытия архива
	FNAClosingProtocol = 305,
	};
