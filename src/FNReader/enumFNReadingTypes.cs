namespace RD_AAOW
	{
	/// <summary>
	/// Возможные режимы считывания данных ФН
	/// </summary>
	public enum FNReadingTypes
		{
		/// <summary>
		/// COM-порт
		/// </summary>
		COM = 0,

		/// <summary>
		/// Дамп ФН новой версии
		/// </summary>
		FSD_4_0 = 1,

		/// <summary>
		/// Дамп ФН из программы FNArc
		/// </summary>
		FNC_1_2_0_1 = 2
		}
	}
