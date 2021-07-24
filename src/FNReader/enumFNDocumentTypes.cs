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
		CurrentState = 21,

		/// <summary>
		/// Кассовый чек
		/// </summary>
		Bill = 3,

		/// <summary>
		/// Чек коррекции
		/// </summary>
		CorrectionBill = 31,

		/// <summary>
		/// БСО
		/// </summary>
		Blank = 4,

		/// <summary>
		/// БСО коррекции
		/// </summary>
		CorrectionBlank = 41,

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
