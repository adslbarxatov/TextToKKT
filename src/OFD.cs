using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к параметрам ОФД
	/// </summary>
	public class OFD
		{
		// Переменные
		private List<string> names = new List<string> (),
			inn = new List<string> (),
			dnsNames = new List<string> (),
			ip = new List<string> (),
			ports = new List<string> (),
			emails = new List<string> (),
			links = new List<string> ();

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		public OFD ()
			{
			// Получение файла символов
#if !ANDROID
			byte[] s = Properties.TextToKKMResources.OFD;
#else
			byte[] s = Properties.Resources.OFD;
#endif
			string buf = Encoding.UTF8.GetString (s);
			StringReader SR = new StringReader (buf);

			// Формирование массива 
			string str;
			char[] splitters = new char[] { ';' };
			uint line = 0;

			try
				{
				// Чтение параметров
				while ((str = SR.ReadLine ()) != null)
					{
					line++;
					string[] values = str.Split (splitters, StringSplitOptions.RemoveEmptyEntries);
					if (values.Length != 7)
						continue;

					inn.Add (values[0]);
					names.Add (values[1]);
					dnsNames.Add (values[2]);
					ip.Add (values[3]);
					ports.Add (values[4]);
					emails.Add (values[5]);
					links.Add (values[6]);
					}
				}
			catch
				{
				}

			// Завершено
			SR.Close ();
			}

		/// <summary>
		/// Метод возвращает список названий ОФД
		/// </summary>
		public List<string> GetOFDNames ()
			{
			return new List<string> (names);
			}

		/// <summary>
		/// Метод возвращает параметры указанного ОФД
		/// </summary>
		/// <param name="INN">ИНН требуемого ОФД</param>
		/// <returns>Параметры ОФД в порядке: ИНН, название, dns-имя, IP, порт, E-mail, сайт</returns>
		public List<string> GetOFDParameters (string INN)
			{
			// Защита
			if (!inn.Contains (INN))
				return new List<string> { "?", "Неизвестный ОФД", "", "", "", "", "" };

			// Возврат
			int i = inn.IndexOf (INN);
			return new List<string> { inn[i], names[i], dnsNames[i], ip[i], ports[i], emails[i], links[i] };
			}

		/// <summary>
		/// Метод возвращает ИНН ОФД по названию
		/// </summary>
		/// <param name="OFDName">Название ОФД</param>
		/// <returns>ИНН ОФД</returns>
		public string GetOFDINNByName (string OFDName)
			{
			// Защита
			if (!names.Contains (OFDName))
				return "";

			// Возврат
			return inn[names.IndexOf (OFDName)];
			}
		}
	}
