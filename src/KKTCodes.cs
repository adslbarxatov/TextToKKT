using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к кодам символов ККТ
	/// </summary>
	public class KKTCodes
		{
		// Переменные
		private List<string> names = new List<string> ();
		private List<List<int>> codes = new List<List<int>> ();
		private List<string> presentations = new List<string> ();
		private List<string> descriptions = new List<string> ();
		private int newKKTCount;

		/// <summary>
		/// Код, возвращаемый при указании некорректных параметров
		/// </summary>
		public const string EmptyCode = "\x7";

		/// <summary>
		/// Конструктор. Инициализирует таблицы кодов
		/// </summary>
		public KKTCodes ()
			{
			// Получение файла символов
#if !ANDROID
			byte[] s = Properties.TextToKKMResources.Codes;
#else
			byte[] s = Properties.Resources.Codes;
#endif
			string buf = Encoding.UTF8.GetString (s);
			StringReader SR = new StringReader (buf);

			// Формирование массива 
			string str;
			uint line = 0;
			try
				{
				// Чтение количества новых ККТ
				newKKTCount = int.Parse (SR.ReadLine ());

				// Чтение кодов
				while ((str = SR.ReadLine ()) != null)
					{
					// Чтение имени ККТ
					line++;

					// Чтение кодов
					names.Add (str);
					codes.Add (new List<int> ());

					for (int i = 0; i < 0x100; i++)
						{
						str = SR.ReadLine ();   // Любое недопустимое значение в этой строке вызовет исключение в следующей
						codes[codes.Count - 1].Add (int.Parse (str));
						line++;
						}

					// Чтение представления
					presentations.Add (SR.ReadLine ());
					line++;

					// Чтение примечания
					descriptions.Add (SR.ReadLine ());
					line++;
					}

				if ((codes.Count != names.Count) || (names.Count != descriptions.Count) ||
					(descriptions.Count != presentations.Count))
					{
					throw new Exception ();
					}

				// Верификация количества
				if (newKKTCount < 1)
					newKKTCount = 1;
				if (newKKTCount > names.Count)
					newKKTCount = names.Count;
				}
			catch
				{
				throw new Exception ("База кодов программы повреждена. Работа программы невозможна.\n" +
					"Ошибка встречена в строке " + line.ToString ());
				}

			// Завершено
			SR.Close ();
			}

		/// <summary>
		/// Метод возвращает код указанного символа
		/// </summary>
		/// <param name="KKTType">Модель ККТ</param>
		/// <param name="CodeNumber">Порядковый номер символа в таблице</param>
		/// <returns>Возвращает код (сообщение) ошибки или \x7 в случае, если входные параметры некорректны</returns>
		public string GetCode (uint KKTType, byte CodeNumber)
			{
			if ((int)KKTType < names.Count)
				{
				if (codes[(int)KKTType][CodeNumber] < 0)
					{
					return EmptyCode;
					}

				return codes[(int)KKTType][CodeNumber].ToString (presentations[(int)KKTType]);
				}
			else
				{
				return EmptyCode;
				}
			}

		/// <summary>
		/// Метод возвращает список названий ККТ
		/// </summary>
		/// <param name="NewOnly">Флаг указывает на необходимость вернуть имена
		/// только новых моделей ККТ</param>
		public List<string> GetKKTTypeNames (bool NewOnly)
			{
			if (NewOnly)
				return names.GetRange (0, newKKTCount);

			return names;
			}

		/// <summary>
		/// Метод возвращает пояснение к способу ввода текста в ККТ по её типу
		/// </summary>
		/// <param name="KKTType">Модель ККТ</param>
		/// <returns>Возвращает пояснение или \x7 в случае, если входные параметры некорректны</returns>
		public string GetKKTTypeDescription (uint KKTType)
			{
			if ((int)KKTType < names.Count)
				return descriptions[(int)KKTType];
			else
				return EmptyCode;
			}
		}
	}
