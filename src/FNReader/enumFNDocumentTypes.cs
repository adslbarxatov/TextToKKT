namespace RD_AAOW
	{
	/// <summary>
	/// Возможные типы фискальных документов
	/// </summary>
	public enum FNDocumentTypes
		{
		/// <summary>
		/// Отчёт о регистрации
		/// </summary>
		Registration = 1,

		/// <summary>
		/// Отчёт об изменении реквизитов регистрации
		/// </summary>
		RegistrationChange = 11,

		/// <summary>
		/// Открытие смены
		/// </summary>
		OpenSession = 2,

		/// <summary>
		/// Отчёт о текущем состоянии расчётов
		/// </summary>
		CurrentState = 0x15,

		/// <summary>
		/// Кассовый чек
		/// </summary>
		Bill = 0x03,

		/// <summary>
		/// Кассовый чек для ФФД 1.1
		/// </summary>
		Bill_11 = 0x83,

		/// <summary>
		/// Кассовый чек для ФФД 1.2
		/// </summary>
		Bill_12 = 0xC3,

		/// <summary>
		/// Чек коррекции
		/// </summary>
		CorrectionBill = 0x1F,

		/// <summary>
		/// Чек коррекции для ФФД 1.1
		/// </summary>
		CorrectionBill_11 = 0x9F,

		/// <summary>
		/// Чек коррекции для ФФД 1.2
		/// </summary>
		CorrectionBill_12 = 0xDF,

		/// <summary>
		/// БСО
		/// </summary>
		Blank = 0x04,

		/// <summary>
		/// БСО для ФФД 1.1
		/// </summary>
		Blank_11 = 0x84,

		/// <summary>
		/// БСО для ФФД 1.2
		/// </summary>
		Blank_12 = 0xC4,

		/// <summary>
		/// БСО коррекции
		/// </summary>
		CorrectionBlank = 0x29,

		/// <summary>
		/// БСО коррекции для ФФД 1.1
		/// </summary>
		CorrectionBlank_11 = 0xA9,

		/// <summary>
		/// БСО коррекции для ФФД 1.2
		/// </summary>
		CorrectionBlank_12 = 0xE9,

		/// <summary>
		/// Закрытие смены
		/// </summary>
		CloseSession = 5,

		/// <summary>
		/// Закрытие фискального режима
		/// </summary>
		CloseFiscalStorage = 6,

		/// <summary>
		/// Подтверждение оператора
		/// </summary>
		Confirmation = 7,

		/// <summary>
		/// Запрос о коде маркировки
		/// </summary>
		MarkCodeRequest = 81,

		/// <summary>
		/// Уведомление о реализации кода маркировки
		/// </summary>
		MarkCodeNotification = 82,

		/// <summary>
		/// Ответ на запрос о коде маркировки
		/// </summary>
		MarkCodeRequestAnswer = 83,

		/// <summary>
		/// Ответ на уведомление о реализации кода маркировки
		/// </summary>
		MarkCodeNotificationAnswer = 84,

		/// <summary>
		/// Неизвестный тип документа
		/// </summary>
		UnknownType = 255
		}
	}
