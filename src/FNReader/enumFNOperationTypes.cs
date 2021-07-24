namespace RD_AAOW
	{
	/// <summary>
	/// Возможные кассовые операции
	/// </summary>
	public enum FNOperationTypes
		{
		/// <summary>
		/// Приход
		/// </summary>
		Incoming = 1,

		/// <summary>
		/// Возврат прихода
		/// </summary>
		ReverseIncoming = 2,

		/// <summary>
		/// Расход
		/// </summary>
		Outcoming = 3,

		/// <summary>
		/// Возврат расхода
		/// </summary>
		ReverseOutcoming = 4,

		/// <summary>
		/// Неизвестная операция
		/// </summary>
		UnknownType = 255
		}
	}
