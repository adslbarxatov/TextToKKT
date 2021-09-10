using System;
using System.Collections.Generic;
using System.IO;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к наименованиям моделей ФН
	/// </summary>
	public class FNSerial
		{
		// Переменные
		private List<string> names = new List<string> (),
			serials = new List<string> ();

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		public FNSerial ()
			{
			// Получение файлов
			string buf = ProgramDescription.FNSerialData;
			StringReader SR = new StringReader (buf);

			// Формирование массива 
			string str;
			char[] splitters = new char[] { ';' };

			try
				{
				// Чтение параметров
				while ((str = SR.ReadLine ()) != null)
					{
					string[] values = str.Split (splitters, StringSplitOptions.RemoveEmptyEntries);

					// Имя протокола
					if (values.Length != 2)
						continue;

					names.Add (values[1]);
					serials.Add (values[0]);
					}
				}
			catch
				{
				}

			// Завершено
			SR.Close ();
			}

		/// <summary>
		/// Метод возвращает модель ФН по его заводскому номеру
		/// </summary>
		/// <param name="FNSerialNumber">Заводской номер ФН</param>
		/// <returns>Модель ФН</returns>
		public string GetFNName (string FNSerialNumber)
			{
			for (int i = 0; i < names.Count; i++)
				if (FNSerialNumber.StartsWith (serials[i]))
					return names[i];

			return "неизвестная модель ФН";
			}

		/// <summary>
		/// Возвращает флаг, указывающий на поддержку ФФД 1.2 моделью ФН, соответствующей указанному ЗН
		/// </summary>
		/// <param name="FNSerialNumber">Заводской номер ФН</param>
		public bool IsFNCompatibleWithFFD12 (string FNSerialNumber)
			{
			return GetFNName (FNSerialNumber).Contains ("1М");
			}

		/// <summary>
		/// Метод выполняет поиск по известным моделям ФН и возвращает сигнатуру ЗН в случае успеха
		/// </summary>
		/// <param name="FNModel">Часть или полное название модели ФН</param>
		/// <returns>Сигнатура ЗН или пустая строка в случае отсутствия результатов</returns>
		public string FindSignatureByName (string FNModel)
			{
			// Защита
			if (string.IsNullOrWhiteSpace (FNModel))
				return "";

			// Поиск в названиях
			string model = FNModel.ToLower ();
			int i;
			for (i = 0; i < names.Count; i++)
				if (names[i].ToLower ().Contains (model))
					break;

			if (i >= names.Count)
				return "";

			// Возврат
			return serials[i];
			}
		}
	}