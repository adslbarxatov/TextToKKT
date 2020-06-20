using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к командам нижнего уровня
	/// </summary>
	public class LowLevel
		{
		// Переменные
		private List<List<string>> names = new List<List<string>> (),
			commands = new List<List<string>> (),
			descriptions = new List<List<string>> ();

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		public LowLevel ()
			{
			// Получение файлов
#if !ANDROID
			byte[] s1 = Properties.TextToKKMResources.LowLevelAtol,
				s2 = Properties.TextToKKMResources.LowLevelShtrih;
#else
			byte[] s1 = Properties.Resources.LowLevelAtol,
				s2 = Properties.Resources.LowLevelShtrih;
#endif
			string[] buf = new string[] {
				Encoding.UTF8.GetString (s1),
				Encoding.UTF8.GetString (s2)
				};
			StringReader SR;
			string str;
			char[] splitters = new char[] { ';' };

			// Формирование массива 
			for (int i = 0; i < buf.Length; i++)
				{
				SR = new StringReader (buf[i]);
				names.Add (new List<string> ());
				commands.Add (new List<string> ());
				descriptions.Add (new List<string> ());

				try
					{
					// Чтение параметров
					while ((str = SR.ReadLine ()) != null)
						{
						string[] values = str.Split (splitters, StringSplitOptions.RemoveEmptyEntries);
						if (values.Length != 3)
							continue;

						names[i].Add (values[0]);
						commands[i].Add (values[1]);
						descriptions[i].Add (values[2].Replace ("|", "\r\n"));
						}
					}
				catch
					{
					}

				// Первая часть завершена
				SR.Close ();
				}

			// Завершено
			}

		/// <summary>
		/// Метод возвращает список команд АТОЛ
		/// </summary>
		public List<string> GetATOLCommandsList ()
			{
			return GetCommandsList (0);
			}

		/// <summary>
		/// Метод возвращает список команд ШТРИХ
		/// </summary>
		public List<string> GetSHTRIHCommandsList ()
			{
			return GetCommandsList (1);
			}

		private List<string> GetCommandsList (uint ArrayNumber)
			{
			return new List<string> (names[(int)ArrayNumber]);
			}

		/// <summary>
		/// Метод возвращает содержимое команды АТОЛ
		/// </summary>
		/// <param name="CommandNumber">Номер команды из списка</param>
		/// <param name="ReturnDescription">Флаг указывает на возврат описания вместо команды</param>
		public string GetATOLCommand (uint CommandNumber, bool ReturnDescription)
			{
			return GetCommand (0, CommandNumber, ReturnDescription);
			}

		/// <summary>
		/// Метод возвращает содержимое команды ШТРИХ
		/// </summary>
		/// <param name="CommandNumber">Номер команды из списка</param>
		/// <param name="ReturnDescription">Флаг указывает на возврат описания вместо команды</param>
		public string GetSHTRIHCommand (uint CommandNumber, bool ReturnDescription)
			{
			return GetCommand (1, CommandNumber, ReturnDescription);
			}

		private string GetCommand (uint ArrayNumber, uint CommandNumber, bool ReturnDescription)
			{
			if (CommandNumber >= names[(int)ArrayNumber].Count)
				return "";

			return (ReturnDescription ? descriptions[(int)ArrayNumber][(int)CommandNumber] :
				commands[(int)ArrayNumber][(int)CommandNumber]);
			}
		}
	}
