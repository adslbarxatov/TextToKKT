﻿using System.Collections.Generic;
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
			new List<string> (),
			new List<string> (),
			new List<string> () };

		/// <summary>
		/// Возвращает список операций, для которых доступны инструкции
		/// </summary>
		public string[] OperationTypes
			{
			get
				{
				return availableOperations.ToArray ();
				}
			}
		private List<string> availableOperations = new List<string> ();

		private static string[] operationTypes = new string[] {
			"Открытие смены",
			"Продажа по коду товара",
			"Продажа по карте",
			"Продажа по свободной цене",
			"Продажа с количеством",
			"Продажа с электронным чеком",
			"Возврат",
			"Закрытие смены" };
		private static string[] operationTypesExt = new string[] {
			"Коррекция даты",
			"Коррекция времени" };

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		/// <param name="AllowExtended">Флаг разрешает отображение расширенных инструкций</param>
		public UserManuals (bool AllowExtended)
			{
			// Формирование списка
			availableOperations.AddRange (operationTypes);
			if (AllowExtended)
				availableOperations.AddRange (operationTypesExt);

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

					// Загрузка файла целиком (требует структура)
					for (int i = 0; i < operations.Count; i++)
						{
						operations[i].Add ("• " + SR.ReadLine ().Replace ("|", "\r\n• "));

						if ((i >= 1) || (i <= 4))
							operations[i][operations[i].Count - 1] = operations[i][operations[i].Count - 1].Replace ("&",
								"Повторить предыдущие действия для всех позиций чека");
						if (i == 3)
							{
							if (operations[i][operations[i].Count - 1] == "• -")
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
						if ((i == 8) || (i == 9))
							operations[i][operations[i].Count - 1] =
								"• Настоятельно рекомендуется предварительно закрыть смену;\r\n" +
								operations[i][operations[i].Count - 1];
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
		/// Возвращает инструкцию по указанному типу операции
		/// </summary>
		/// <param name="KKTType">Тип ККТ</param>
		/// <param name="ManualType">Операция</param>
		public string GetManual (uint KKTType, uint ManualType)
			{
			if (KKTType >= names.Count)
				return "";

			if (ManualType < availableOperations.Count)
				return operations[(int)ManualType][(int)KKTType];

			return names[(int)KKTType];
			}
		}
	}
