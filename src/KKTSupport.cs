using System;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает вспомогательные методы
	/// </summary>
	public static class KKTSupport
		{
		/// <summary>
		/// Структура используется для передачи списка параметров определения срока жизни ФН
		/// </summary>
		public struct FNLifeFlags
			{
			/// <summary>
			/// Флаг указывает на выбор ФН на 13/15 месяцев вместо 36
			/// </summary>
			public bool FN15;

			/// <summary>
			/// Флаг указывает на выбор ФН на 13 месяцев вместо 15
			/// </summary>
			public bool FNExactly13;

			/// <summary>
			/// Флаг указыввает на применение ОСН
			/// </summary>
			public bool GenericTax;

			/// <summary>
			/// Флаг указывает на режим товаров вместо услуг
			/// </summary>
			public bool Goods;

			/// <summary>
			/// Флаг указывает на агентскую схему или сезонный режим работы
			/// </summary>
			public bool SeasonOrAgents;

			/// <summary>
			/// Флаг указывает на наличие подакцизных товаров
			/// </summary>
			public bool Excise;

			/// <summary>
			/// Флаг указывает на работу без передачи данных
			/// </summary>
			public bool Autonomous;

			/// <summary>
			/// Флаг, указывающий на расчёт срока жизни ФН без учёта некоторых недействующих по факту ограничений
			/// </summary>
			public bool DeFacto;
			}

		/// <summary>
		/// Метод формирует дату истечения срока эксплуатации ФН с указанными параметрами
		/// </summary>
		/// <param name="StartDate">Дата фискализации</param>
		/// <param name="Flags">Параметры расчёта срока действия</param>
		/// <returns>Возвращает строку с датой или пустую строку, если указанная модель ФН
		/// не может быть использована с указанными режимами и параметрами</returns>
		public static string GetFNLifeEndDate (DateTime StartDate, FNLifeFlags Flags)
			{
			string res = "";

			// Отсечение недопустимых вариантов
			if (Flags.GenericTax && !Flags.FN15 && Flags.Goods ||               // Нельзя игнорировать

				!Flags.DeFacto && !Flags.GenericTax && Flags.FN15 &&
				!Flags.SeasonOrAgents && !Flags.Excise && !Flags.Autonomous)    // Можно игнорировать (флаг де-факто)
				{
				res = "!";
				}

			// Определение срока жизни
			int length = 1110;

			if (Flags.Excise && !Flags.DeFacto)     // Можно игнорировать (флаг де-факто)
				{
				length = 410;
				}
			else if (Flags.Autonomous)
				{
				if (Flags.FN15)
					length = 410;
				else
					length = 560;
				}
			else if (Flags.FN15)
				{
				length = Flags.FNExactly13 ? 410 : 470;
				}

			// Результат
			return (res + StartDate.AddDays (length).ToString ("dd.MM.yyyy"));
			}

		// Контрольная последовательность для определения корректности ИНН
		private static byte[] innCheckSequence = new byte[] { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };

		/// <summary>
		/// Метод проверяет корректность ввода ИНН
		/// </summary>
		/// <param name="INN">ИНН для проверки</param>
		/// <returns>Возвращает 0, если ИНН корректен, 1, если ИНН имеет некорректную КС, 
		/// -1, если строка не является ИНН</returns>
		public static int CheckINN (string INN)
			{
			// Контроль параметра
			if ((INN.Length != 10) && (INN.Length != 12))
				return -1;

			UInt64 inn = 0;
			try
				{
				inn = UInt64.Parse (INN);
				}
			catch
				{
				return -1;
				}

			// Расчёт контрольной суммы
			uint n1 = 0, n2 = 0;
			UInt64 d;

			// Для 10 цифр
			if (INN.Length == 10)
				{
				d = 10;
				for (int i = 0; i < 9; i++)
					{
					n1 += (uint)((byte)((inn / d) % 10) * innCheckSequence[10 - i]);
					d *= 10;
					}

				if ((n1 % 11) == (inn % 10))
					return 0;

				return 1;
				}

			// Для 12 цифр
			d = 100;
			for (int i = 0; i < 10; i++)
				{
				n1 += (uint)((byte)((inn / d) % 10) * innCheckSequence[10 - i]);
				d *= 10;
				}

			if ((n1 % 11) != ((inn / 10) % 10))
				return 1;

			d = 10;
			for (int i = 0; i < 11; i++)
				{
				n2 += (uint)((byte)((inn / d) % 10) * innCheckSequence[10 - i]);
				d *= 10;
				}

			if ((n2 % 11) != (inn % 10))
				return 1;

			return 0;
			}

		// Таблица полинома CRC16
		private static UInt16[] crc16_table = new UInt16[] {
			0, 4129, 8258, 12387, 16516, 20645, 24774, 28903, 33032, 37161, 41290, 45419, 49548, 53677, 57806, 61935,
			4657, 528, 12915, 8786, 21173, 17044, 29431, 25302, 37689, 33560, 45947, 41818, 54205, 50076, 62463, 58334,
			9314, 13379, 1056, 5121, 25830, 29895, 17572, 21637, 42346, 46411, 34088, 38153, 58862, 62927, 50604, 54669,
			13907, 9842, 5649, 1584, 30423, 26358, 22165, 18100, 46939, 42874, 38681, 34616, 63455, 59390, 55197, 51132,
			18628, 22757, 26758, 30887, 2112, 6241, 10242, 14371, 51660, 55789, 59790, 63919, 35144, 39273, 43274, 47403,
			23285, 19156, 31415, 27286, 6769, 2640, 14899, 10770, 56317, 52188, 64447, 60318, 39801, 35672, 47931, 43802,
			27814, 31879, 19684, 23749, 11298, 15363, 3168, 7233, 60846, 64911, 52716, 56781, 44330, 48395, 36200, 40265,
			32407, 28342, 24277, 20212, 15891, 11826, 7761, 3696, 65439, 61374, 57309, 53244, 48923, 44858, 40793, 36728,
			37256, 33193, 45514, 41451, 53516, 49453, 61774, 57711, 4224, 161, 12482, 8419, 20484, 16421, 28742, 24679,
			33721, 37784, 41979, 46042, 49981, 54044, 58239, 62302, 689, 4752, 8947, 13010, 16949, 21012, 25207, 29270,
			46570, 42443, 38312, 34185, 62830, 58703, 54572, 50445, 13538, 9411, 5280, 1153, 29798, 25671, 21540, 17413,
			42971, 47098, 34713, 38840, 59231, 63358, 50973, 55100, 9939, 14066, 1681, 5808, 26199, 30326, 17941, 22068,
			55628, 51565, 63758, 59695, 39368, 35305, 47498, 43435, 22596, 18533, 30726, 26663, 6336, 2273, 14466, 10403,
			52093, 56156, 60223, 64286, 35833, 39896, 43963, 48026, 19061, 23124, 27191, 31254, 2801, 6864, 10931, 14994,
			64814, 60687, 56684, 52557, 48554, 44427, 40424, 36297, 31782, 27655, 23652, 19525, 15522, 11395, 7392, 3265,
			61215, 65342, 53085, 57212, 44955, 49082, 36825, 40952, 28183, 32310, 20053, 24180, 11923, 16050, 3793, 7920
			};

		/// <summary>
		/// Метод рассчитывает CRC16 для сообщения
		/// </summary>
		/// <param name="Message">Строка сообщения</param>
		/// <returns>Значение CRC16</returns>
		public static UInt16 GetCRC16 (string Message)
			{
			// Контроль
			if ((Message == null) || (Message == ""))
				return 0;
			byte[] msg = Encoding.UTF8.GetBytes (Message);

			// Переменные
			UInt16 crc16 = 0xFFFF;
			UInt16 crc16_i = 0;
			UInt16 crc16_e = (UInt16)(msg.Length - 1);

			// Вычисление
			for (; crc16_i <= crc16_e; ++crc16_i)
				{
				crc16 = (UInt16)((crc16 << 8) ^ crc16_table[(crc16 >> 8) ^ (UInt16)msg[crc16_i]]);
				}

			return crc16;
			}

		/// <summary>
		/// Метод формирует полный регистрационный номер ККТ
		/// </summary>
		/// <param name="INN">ИНН пользователя</param>
		/// <param name="RNMFirstPart">Первая часть (10 цифр) регистрационного номера ККТ (порядковый номер в ФНС)</param>
		/// <param name="Serial">Заводской номер ККТ</param>
		/// <returns>Возвращает полный регистрационный номер (с проверочным кодом)</returns>
		public static string GetFullRNM (string INN, string Serial, string RNMFirstPart)
			{
			// Контроль параметра
			try
				{
				UInt64 serial = UInt64.Parse (Serial);
				UInt64 rnmStart = UInt64.Parse (RNMFirstPart);
				}
			catch
				{
				return "";
				}

			if ((CheckINN (INN) < 0) || (Serial.Length > 20) || (RNMFirstPart.Length > 10))
				return "";

			// Расчёт контрольной суммы
			string msg = RNMFirstPart.PadLeft (10, '0') + INN.PadLeft (12, '0') + Serial.PadLeft (20, '0');
			UInt16 crc = GetCRC16 (msg);

			return RNMFirstPart.PadLeft (10, '0') + crc.ToString ().PadLeft (6, '0');
			}

		/// <summary>
		/// Метод возвращает название региона по коду ИНН
		/// </summary>
		/// <param name="INN">ИНН пользователя</param>
		/// <returns>Возвращает название региона</returns>
		public static string GetRegionName (string INN)
			{
			// Контроль
			if (INN.Length < 2)
				return "неизвестный регион";

			// Обработка
			switch (INN.Substring (0, 2))
				{
				case "01":
					return "Республика Адыгея";
				case "02":
					return "Республика Башкортостан";
				case "03":
					return "Республика Бурятия";
				case "04":
					return "Республика Алтай";
				case "05":
					return "Республика Дагестан";
				case "06":
					return "Республика Ингушетия";
				case "07":
					return "Кабардино-Балкарская республика";
				case "08":
					return "Республика Калмыкия";
				case "09":
					return "Республика Карачаево-Черкесия";
				case "10":
					return "Республика Карелия";
				case "11":
					return "Республика Коми";
				case "12":
					return "Республика Марий Эл";
				case "13":
					return "Республика Мордовия";
				case "14":
					return "Республика Саха (Якутия)";
				case "15":
					return "Республика Северная Осетия – Алания";
				case "16":
					return "Республика Татарстан";
				case "17":
					return "Республика Тыва";
				case "18":
					return "Удмуртская республика";
				case "19":
					return "Республика Хакасия";
				case "20":
					return "Чеченская республика";
				case "21":
					return "Чувашская республика";
				case "22":
					return "Алтайский край";
				case "23":
					return "Краснодарский край";
				case "24":
				case "84":
				case "88":
					return "Красноярский край";
				case "25":
					return "Приморский край";
				case "26":
					return "Ставропольский край";
				case "27":
					return "Хабаровский край";
				case "28":
					return "Амурская область";
				case "29":
					return "Архангельская область";
				case "30":
					return "Астраханская область";
				case "31":
					return "Белгородская область";
				case "32":
					return "Брянская область";
				case "33":
					return "Владимирская область";
				case "34":
					return "Волгоградская область";
				case "35":
					return "Вологодская область";
				case "36":
					return "Воронежская область";
				case "37":
					return "Ивановская область";
				case "38":
				case "85":
					return "Иркутская область";
				case "39":
					return "Калининградская область";
				case "40":
					return "Калужская область";
				case "41":
				case "82":
					return "Камчатский край";
				case "42":
					return "Кемеровская область";
				case "43":
					return "Кировская область";
				case "44":
					return "Костромская область";
				case "45":
					return "Курганская область";
				case "46":
					return "Курская область";
				case "47":
					return "Ленинградская область";
				case "48":
					return "Липецкая область";
				case "49":
					return "Магаданская область";
				case "50":
					return "Московская область";
				case "51":
					return "Мурманская область";
				case "52":
					return "Нижегородская область";
				case "53":
					return "Новгородская область";
				case "54":
					return "Новосибирская область";
				case "55":
					return "Омская область";
				case "56":
					return "Оренбургская область";
				case "57":
					return "Орловская область";
				case "58":
					return "Пензенская область";
				case "59":
				case "81":
					return "Пермский край";
				case "60":
					return "Псковская область";
				case "61":
					return "Ростовская область";
				case "62":
					return "Рязанская область";
				case "63":
					return "Самарская область";
				case "64":
					return "Саратовская область";
				case "65":
					return "Сахалинская область";
				case "66":
					return "Свердловская область";
				case "67":
					return "Смоленская область";
				case "68":
					return "Тамбовская область";
				case "69":
					return "Тверская область";
				case "70":
					return "Томская область";
				case "71":
					return "Тульская область";
				case "72":
					return "Тюменская область";
				case "73":
					return "Ульяновская область";
				case "74":
					return "Челябинская область";
				case "75":
				case "80":
					return "Забайкальский край";
				case "76":
					return "Ярославская область";
				case "77":
				case "97":
					return "Москва";
				case "78":
					return "Санкт-Петербург";
				case "79":
					return "Еврейская автономная область";
				case "91":
					return "Республика Крым";
				case "83":
					return "Ненецкий автономный округ";
				case "86":
					return "Ханты-Мансийский автономный округ – Югра";
				case "87":
					return "Чукотский автономный округ";
				case "89":
					return "Ямало-Ненецкий автономный округ";
				case "92":
					return "Севастополь";
				case "99":
					return "Байконур";

				default:
					return "неизвестный регион";
				}
			}
		}
	}
