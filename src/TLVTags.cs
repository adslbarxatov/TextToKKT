using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к описаниям тегов TLV согласно актуальному ФФД
	/// </summary>
	public class TLVTags
		{
		// Переменные
		private List<uint> tags = new List<uint> ();
		private List<string> descriptions = new List<string> ();
		private List<string> types = new List<string> ();
		private List<string> possibleValues = new List<string> ();
		private List<int> links = new List<int> ();

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		public TLVTags ()
			{
			// Получение файлов
#if !ANDROID
			string buf = Encoding.UTF8.GetString (RD_AAOW.Properties.TextToKKMResources.TLVTags);
#else
			string buf = Encoding.UTF8.GetString (RD_AAOW.Properties.Resources.TLVTags);
#endif
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

					// Описания тегов
					if (values.Length >= 3)
						{
						// Список команд
						tags.Add (uint.Parse (values[0]));
						descriptions.Add (BuildDescription (values[1], tags[tags.Count - 1]));
						types.Add (BuildType (values[2]));

						if (values.Length > 3)
							links.Add (int.Parse (values[3]) - 1);
						else
							links.Add (-1);
						}

					// Описания значений тегов
					else if (values.Length == 2)
						{
						if (int.Parse (values[0]) > possibleValues.Count)
							possibleValues.Add ("");

						possibleValues[possibleValues.Count - 1] += (values[1] + "\r\n");
						}

					// Пропуски
					else
						{
						continue;
						}
					}
				}
			catch
				{
				}

			// Завершено
			SR.Close ();
			}

		/// <summary>
		/// Метод возвращает описание тип тега в соответствующие свойства экземпляра TLVTags
		/// </summary>
		/// <param name="Tag">Номер тега или фрагмент описания</param>
		/// <returns>Возвращает true в случае обнаружения</returns>
		public bool FindTag (string Tag)
			{
			// Защита
			if (string.IsNullOrWhiteSpace (Tag))
				return false;
			string tagString = Tag.ToLower ();

			// Поиск по коду
			uint tag = 0;
			int i;
			if (uint.TryParse (tagString, out tag))
				{
				i = tags.IndexOf (tag);
				}

			// Поиск по описанию
			else
				{
				lastIndex++;
				for (i = 0; i < descriptions.Count; i++)
					if (descriptions[(i + lastIndex) % descriptions.Count].ToLower ().Contains (tagString))
						{
						i = lastIndex = (i + lastIndex) % descriptions.Count;
						break;
						}
				}

			// Проверка
			if ((i < 0) || (i >= descriptions.Count))
				return false;

			// Найдено
			lastType = types[i];
			lastDescription = descriptions[i];

			if (links[i] < 0)
				lastValuesSet = "";
			else
				lastValuesSet = possibleValues[links[i]];
			if (lastValuesSet.EndsWith ("\r\n"))
				lastValuesSet = lastValuesSet.Substring (0, lastValuesSet.Length - 2);

			return true;
			}
		private int lastIndex = 0;

		// Метод распаковывает текст описания
		private string BuildDescription (string Source, uint TagNumber)
			{
			string res = Source;

			res = res.Replace ("FLAGPR", "(печатается, только если указан)");
			res = res.Replace ("OBJ", "товара, работы, услуги, платежа, выплаты или иного предмета расчёта");
			res = res.Replace ("APPEND", "(дополнительный реквизит чека, условия применения и значение которого " +
				"определяется ФНС России)");
			res = res.Replace ("KTFMT", "Код товара в формате");
			res = res.Replace ("OLDCOR", "Итоговая сумма корректировок НДС по");
			res = res.Replace ("TOTALS", "Итоговые количества и суммы расчётов");
			res = res.Replace ("EXTREQ", "Дополнительный реквизит отчёта");
			res = res.Replace ("EXTREP", "Дополнительные данные отчёта");
			res = res.Replace ("OFD", "оператора фискальных данных");
			res = res.Replace ("INC1", "Псевдотег, инкапсулирующий");
			res = res.Replace ("INC2", "в выгрузке архива ФН");
			res = res.Replace ("OLD", "[устарел]");

			return TagNumber.ToString () + " (0x" + TagNumber.ToString ("X4") + "): " + res;
			}

		// Метод распаковывает тип тега
		private string BuildType (string Source)
			{
			string res = Source;

			// Отдельная подмена для ссылок на таблицы
			if (res.Contains ("TABLE"))
				{
				int i = res.IndexOf ("TABLE");
				res = res.Replace ("TABLE", "");
				uint v = uint.Parse (res.Substring (i));
				res = res.Substring (0, i) + " (значения приведены в таблице " + v.ToString () + ")";
				}

			// Простые замены
			if (res == "BOOL")
				return "Целое, 1 байт (0 или 1)";
			if (res == "UNIX")
				return "UnixTime (целое, 4 байта)";
			if (res == "INNF")
				return "Строка длиной 12 байт (дополняется пробелами справа)";
			if (res == "STRD")
				return "Строка длиной 10 байт в формате ДД.ММ.ГГГГ";
			if (res == "PRICE")
				return "Целое с фиксированной запятой (до 6 байт, 2 знака)";

			// Замены с длинами
			if (res.StartsWith ("STRLE"))
				{
				res = res.Replace ("STRLE", "");
				uint v = uint.Parse (res);
				return "Строка длиной не более " + v.ToString () + " байт";
				}
			if (res.StartsWith ("STREA"))
				{
				res = res.Replace ("STREA", "");
				uint v = uint.Parse (res);
				return "Строка длиной " + v.ToString () + " байт (дополняется пробелами справа при необходимости)";
				}
			if (res.StartsWith ("STRE"))
				{
				res = res.Replace ("STRE", "");
				uint v = uint.Parse (res);
				return "Строка длиной " + v.ToString () + " байт";
				}
			if (res.StartsWith ("SCTLE"))
				{
				res = res.Replace ("SCTLE", "");
				uint v = uint.Parse (res);
				return "Структура длиной не более " + v.ToString () + " байт";
				}
			if (res.StartsWith ("ARRAY"))
				{
				res = res.Replace ("ARRAY", "");
				uint v = uint.Parse (res);
				return "Байт-массив длиной не более " + v.ToString () + " байт";
				}

			// Подстановки
			if (res.StartsWith ("UINT8"))
				res = res.Replace ("UINT8", "Целое, 1 байт");
			if (res.StartsWith ("UINT32"))
				res = res.Replace ("UINT32", "Целое, 4 байта");
			if (res.StartsWith ("BIT8"))
				res = res.Replace ("BIT8", "Битовое поле, 1 байт");
			if (res.StartsWith ("UINT16"))
				res = res.Replace ("UINT16", "Целое, 2 байта");
			if (res.StartsWith ("BIT32"))
				res = res.Replace ("BIT32", "Битовое поле, 4 байта");

			return res;
			}

		/// <summary>
		/// Возвращает последний найденный тип тега
		/// </summary>
		public string LastType
			{
			get
				{
				return lastType;
				}
			}
		private string lastType = "";

		/// <summary>
		/// Возвращает последнее найденное описание тега
		/// </summary>
		public string LastDescription
			{
			get
				{
				return lastDescription;
				}
			}
		private string lastDescription = "";

		/// <summary>
		/// Возвращает последний найденный набор значений тега
		/// </summary>
		public string LastValuesSet
			{
			get
				{
				return lastValuesSet;
				}
			}
		private string lastValuesSet = "";
		}
	}
