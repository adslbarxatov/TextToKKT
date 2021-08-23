using System;
using System.Collections.Generic;
using System.IO;

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
			links = new List<string> (),

			dnsNamesM = new List<string> (),
			ipM = new List<string> (),
			portsM = new List<string> ();
		private const string notFound = "[не найдено]";
		private const string notFoundFlag = "?";
		private const string equivalentFlag = "=";

		/// <summary>
		/// Общий адрес сайта ФНС
		/// </summary>
		public const string FNSSite = "www.nalog.ru";

		/// <summary>
		/// Общий адрес сервера обновления ключей проверки кодов маркировки
		/// </summary>
		public const string OKPSite = "prod01.okp-fn.ru";

		/// <summary>
		/// Общий порт сервера обновления ключей проверки кодов маркировки
		/// </summary>
		public const string OKPPort = "26101";

		/// <summary>
		/// Конструктор. Инициализирует таблицу ОФД
		/// </summary>
		public OFD ()
			{
			// Получение файла символов
			string buf = ProgramDescription.OFDData;
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
					if (values.Length != 10)
						continue;

					inn.Add (values[0]);
					names.Add (values[1]);

					dnsNames.Add (values[2] == notFoundFlag ? notFound : values[2]);
					ip.Add (values[3] == notFoundFlag ? notFound : values[3]);
					ports.Add (values[4] == notFoundFlag ? "[???]" : values[4]);
					emails.Add (values[5] == notFoundFlag ? notFound : values[5]);
					links.Add (values[6] == notFoundFlag ? notFound : values[6]);

					if (values[7] == notFoundFlag)
						dnsNamesM.Add (notFound);
					else if (values[7] == equivalentFlag)
						dnsNamesM.Add (dnsNames[dnsNames.Count - 1]);
					else
						dnsNamesM.Add (values[7]);

					if (values[8] == notFoundFlag)
						ipM.Add (notFound);
					else if (values[8] == equivalentFlag)
						ipM.Add (ip[ip.Count - 1]);
					else
						ipM.Add (values[8]);

					if (values[9] == notFoundFlag)
						portsM.Add ("[???]");
					else if (values[9] == equivalentFlag)
						portsM.Add (ports[ports.Count - 1]);
					else
						portsM.Add (values[9]);
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
			if (INN.Contains ("0000000000"))
				return new List<string> { "?", "Без ОФД", "", "", "", "", "", "", "", "" };

			if (!inn.Contains (INN))
				return new List<string> { "?", "Неизвестный ОФД", "", "", "", "", "", "", "", "" };

			// Возврат
			int i = inn.IndexOf (INN);
			return new List<string> {
				inn[i], names[i],
				dnsNames[i], ip[i], ports[i], emails[i], links[i],
				dnsNamesM[i], ipM[i], portsM[i]
				};
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
