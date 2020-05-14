using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к кодам символов ККМ
	/// </summary>
	public class KKMCodes
		{
		// Переменные
		private List<string> names = new List<string> ();
		private List<List<int>> codes = new List<List<int>> ();
		private List<string> presentations = new List<string> ();
		private List<string> descriptions = new List<string> ();

		/// <summary>
		/// Код, возвращаемый при указании некорректных параметров
		/// </summary>
		public const string EmptyCode = "\x7";

		/// <summary>
		/// Конструктор. Инициализирует таблицы кодов
		/// </summary>
		public KKMCodes ()
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
				while ((str = SR.ReadLine ()) != null)
					{
					// Чтение имени ККМ
					line++;
#if ANDROID
					if (str == "——————————")
						continue;
#endif

					// Чтение кодов
					names.Add (str);
					codes.Add (new List<int> ());

					if (str != "——————————")
						{
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
#if !ANDROID
					else
						{
						for (int i = 0; i < 0x100; i++)
							{
							codes[codes.Count - 1].Add (-1);
							}

						presentations.Add ("D1");
						descriptions.Add ("—");
						}
#endif
					}

				if ((codes.Count != names.Count) || (names.Count != descriptions.Count) ||
					(descriptions.Count != presentations.Count))
					{
					throw new Exception ();
					}
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
		/// <param name="KKMType">Модель ККМ</param>
		/// <param name="CodeNumber">Порядковый номер символа в таблице</param>
		/// <returns>Возвращает код (сообщение) ошибки или \x7 в случае, если входные параметры некорректны</returns>
		public string GetCode (uint KKMType, byte CodeNumber)
			{
			if ((int)KKMType < names.Count)
				{
				if (codes[(int)KKMType][CodeNumber] < 0)
					{
					return EmptyCode;
					}

				return codes[(int)KKMType][CodeNumber].ToString (presentations[(int)KKMType]);
				}
			else
				{
				return EmptyCode;
				}
			}

		/// <summary>
		/// Возвращает список названий ККТ
		/// </summary>
		public List<string> KKMTypeNames
			{
			get
				{
				return names;
				}
			}

		/// <summary>
		/// Метод возвращает пояснение к способу ввода текста в ККМ по её типу
		/// </summary>
		/// <param name="KKMType">Модель ККМ</param>
		/// <returns>Возвращает пояснение или \x7 в случае, если входные параметры некорректны</returns>
		public string GetKKMTypeDescription (uint KKMType)
			{
			if ((int)KKMType < names.Count)
				return descriptions[(int)KKMType];
			else
				return EmptyCode;
			}
		}
	}
