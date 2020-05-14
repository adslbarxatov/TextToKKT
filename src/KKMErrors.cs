using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к сообщениям об ошибках ККМ
	/// </summary>
	public class KKMErrorsList
		{
		// Переменные
		private List<List<KKMError>> errors = new List<List<KKMError>> ();
		private List<string> names = new List<string> ();
		private char[] splitters = new char[] { ';' };

		// Константы
		/// <summary>
		/// Код, возвращаемый при указании некорректных параметров
		/// </summary>
		public const string EmptyCode = "\x7";

		/// <summary>
		/// Конструктор. Инициализирует таблицу ошибок
		/// </summary>
		public KKMErrorsList ()
			{
			// Получение файла ошибок
#if !ANDROID
			byte[] s = Properties.TextToKKMResources.Errors;
#else
			byte[] s = Properties.Resources.Codes;
#endif
			string buf = Encoding.UTF8.GetString (s);
			StringReader SR = new StringReader (buf);

			// Формирование массива 
			uint line = 0;
			string str;

			try
				{
				while ((str = SR.ReadLine ()) != null)
					{
					// Обработка строки
					line++;
					string[] values = str.Split (splitters, StringSplitOptions.RemoveEmptyEntries);
					switch (values.Length)
						{
						case 1:
							errors.Add (new List<KKMError> ());
							names.Add (values[0]);
							break;

						case 2:
							errors[errors.Count - 1].Add (new KKMError (values[0], values[1]));
							break;

						default:
							throw new Exception ();
						}
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
		/// Метод возвращает код или сообщение указанной ошибки
		/// </summary>
		/// <param name="KKMType">Модель ККТ</param>
		/// <returns>Возвращает код (сообщение) ошибки или \x7 в случае, если входные параметры некорректны</returns>
		public List<KKMError> GetErrorCodes (uint KKMType)
			{
			return errors[(int)KKMType];
			}

		/// <summary>
		/// Метод возвращает описание указанной ошибки
		/// </summary>
		/// <param name="KKMType">Модель ККТ</param>
		/// <param name="ErrorNumber">Порядковый номер сообщения</param>
		/// <returns>Возвращает описание ошибки или \x7 в случае, если входные параметры некорректны</returns>
		public string GetErrorText (uint KKMType, uint ErrorNumber)
			{
			if (((int)KKMType < names.Count) && ((int)ErrorNumber < errors[(int)KKMType].Count))
				return errors[(int)KKMType][(int)ErrorNumber].ErrorText;
			else
				return EmptyCode;
			}

		// Методы, возвращающие количество сообщений в группах по ККМ, не требуются

		/// <summary>
		/// Метод возвращает название ККМ по её типу
		/// </summary>
		public List<string> KKMTypeNames
			{
			get
				{
				return names;
				}
			}
		}

	/// <summary>
	/// Класс описывает отдельную ошибку ККМ
	/// </summary>
	public class KKMError
		{
		/// <summary>
		/// Код или сообщение ошибки
		/// </summary>
		public string ErrorCode
			{
			get
				{
				return errorCode;
				}
			}
		string errorCode;

		/// <summary>
		/// Описание ошибки
		/// </summary>
		public string ErrorText
			{
			get
				{
				return errorText;
				}
			}
		string errorText;

		/// <summary>
		/// Конструктор. Создаёт объект-ошибку
		/// </summary>
		/// <param name="Code"></param>
		/// <param name="Text"></param>
		public KKMError (string Code, string Text)
			{
			errorCode = ((Code == null) || (Code == "")) ? "—" : Code;
			errorText = ((Text == null) || (Text == "")) ? "—" : Text;
			}
		}
	}
