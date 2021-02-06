using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к инструкциям по работе с ККТ
	/// </summary>
	public class UserManuals
		{
		// Переменные
		private List<string> names = new List<string> ();
		private List<List<string>> operations = new List<List<string>> {
			new List<string> (),
			new List<string> (),
			new List<string> (),
			new List<string> (),
			new List<string> (),
			new List<string> (),
			new List<string> (),
			new List<string> () };

		/// <summary>
		/// Возвращает список операций, для которых доступны инструкции
		/// </summary>
		public static string[] OperationTypes
			{
			get
				{
				return operationTypes;
				}
			}
		private static string[] operationTypes = new string[] {
			"Открытие смены",
			"Продажа по коду товара",
			"Продажа по карте",
			"Продажа по свободной цене",
			"Продажа с количеством",
			"Продажа с электронным чеком",
			"Возврат",
			"Закрытие смены" };

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		public UserManuals ()
			{
			// Получение файлов
#if !ANDROID
			byte[] s1 = Properties.TextToKKMResources.UserManuals;
#else
			byte[] s1 = Properties.Resources.UserManuals;
#endif
			string buf = Encoding.UTF8.GetString (s1);
			StringReader SR = new StringReader (buf);
			string str;

			// Формирование массива 
			try
				{
				// Чтение параметров
				while ((str = SR.ReadLine ()) != null)
					{
					if (str == "")
						continue;

					names.Add (str);
					for (int i = 0; i < operations.Count; i++)
						{
						operations[i].Add ("• " + SR.ReadLine ().Replace ("|", "\r\n• "));

						if ((i >= 1) || (i <= 4))
							operations[i][operations[i].Count - 1] = operations[i][operations[i].Count - 1].Replace ("&",
								"Повторить предыдущие действия для всех позиций чека");
						if (i == 3)
							{
							if (operations[i][operations[i].Count - 1] == "• ")
								operations[i][operations[i].Count - 1] = "(не предусмотрена)";
							else
								operations[i][operations[i].Count - 1] +=
									" (отдельно для каждой позиции в чеке);\r\n• Закрыть чек в зависимости от способа оплаты";
							}
						if (i == 6)
							operations[i][operations[i].Count - 1] +=
								";\r\n• Дальнейшие действия совпадают с действиями при продаже";
						if (i == 7)
							operations[i][operations[i].Count - 1] += ";\r\n• Дождаться снятия отчёта";
						}
					}
				}
			catch
				{
				}

			// Первая часть завершена
			SR.Close ();
			}

		/// <summary>
		/// Метод возвращает список ККТ, для которых доступны инструкции
		/// </summary>
		public List<string> GetKKTList ()
			{
			return new List<string> (names);
			}

		/// <summary>
		/// Возвращает инструкцию по открытию смены
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetOpening (uint KKTType)
			{
			return GetManual (KKTType, 0);
			}

		/// <summary>
		/// Возвращает инструкцию по продаже по коду товара
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetOperationWithCode (uint KKTType)
			{
			return GetManual (KKTType, 1);
			}

		/// <summary>
		/// Возвращает инструкцию по продаже с безналичным расчётом
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetOperationWithCard (uint KKTType)
			{
			return GetManual (KKTType, 2);
			}

		/// <summary>
		/// Возвращает инструкцию по продаже по свободной цене
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetOperationWithFreePrice (uint KKTType)
			{
			return GetManual (KKTType, 3);
			}

		/// <summary>
		/// Возвращает инструкцию по продаже с количеством
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetOperationWithQuantity (uint KKTType)
			{
			return GetManual (KKTType, 4);
			}

		/// <summary>
		/// Возвращает инструкцию по продаже с электронным чеком
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetOperationWithContact (uint KKTType)
			{
			return GetManual (KKTType, 5);
			}

		/// <summary>
		/// Возвращает инструкцию по оформлению возврата
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetReturn (uint KKTType)
			{
			return GetManual (KKTType, 6);
			}

		/// <summary>
		/// Возвращает инструкцию по закрытию смены
		/// </summary>
		/// <param name="KKTType">Тип ККТ из списка</param>
		public string GetClosing (uint KKTType)
			{
			return GetManual (KKTType, 7);
			}

		/// <summary>
		/// Возвращает инструкцию по указанному типу операции
		/// </summary>
		/// <param name="KKTType">Тип ККТ</param>
		/// <param name="ManualType">Операция</param>
		public string GetManual (uint KKTType, uint ManualType)
			{
			if (KKTType >= names.Count)
				return "";

			if (ManualType < operations.Count)
				return operations[(int)ManualType][(int)KKTType];

			return names[(int)KKTType];
			}
		}
	}
