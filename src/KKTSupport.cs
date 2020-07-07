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
		/// Метод возвращает модель ФН по его заводскому номеру
		/// </summary>
		/// <param name="FNSerialNumber">ЗН ФН</param>
		/// <returns>Модель ФН</returns>
		public static string GetFNName (string FNSerialNumber)
			{
			if (FNSerialNumber.Contains ("87100001"))
				return "ФН-1 (13) ООО РИК";
			if (FNSerialNumber.Contains ("87110001"))
				return "ФН-1 исп. 2 (36) ООО РИК";
			if (FNSerialNumber.Contains ("8712000100"))
				return "ФН-1 исп. 3 v2 (13) ООО Прагматик";
			if (FNSerialNumber.Contains ("8712000101"))
				return "ФН-1 исп. 3 v1 (13) ООО НТЦ Измеритель";
			if (FNSerialNumber.Contains ("87150001"))
				return "ФН-1 Пр13-2 (13) ООО Прагматик";
			if (FNSerialNumber.Contains ("87160001"))
				return "ФН-1 Из13-2 (13) ООО НТЦ Измеритель";
			if (FNSerialNumber.Contains ("92810001"))
				return "ФН-1.1 исп. 4 (36) ООО Инвента";
			if (FNSerialNumber.Contains ("92820001") || FNSerialNumber.Contains ("92824403"))
				return "ФН-1.1 исп. 5-15-2 (15) ООО Инвента";
			if (FNSerialNumber.Contains ("92834403"))
				return "ФН-1.1 ЭВ36-2 (36) ООО Эвотор";
			if (FNSerialNumber.Contains ("92840001"))
				return "ФН-1.1 ЭВ15-2 (15) ООО Эвотор";
			if (FNSerialNumber.Contains ("92850001"))
				return "ФН-1.1 Из15-2 (15) ООО НТЦ Измеритель";
			if (FNSerialNumber.Contains ("92860001"))
				return "ФН-1.1 исп. 3 (15) АО Концерн Автоматика";
			if (FNSerialNumber.Contains ("92874403"))
				return "ФН-1.1 исп. 2 (36) ООО Инвента";
			if (FNSerialNumber.Contains ("92880001"))
				return "ФН-1.1 исп. 5-15-1 (15) ООО Инвента";
			if (FNSerialNumber.Contains ("92890001"))
				return "ФН-1.1 Пр15-2 (15) ООО Прагматик";
			if (FNSerialNumber.Contains ("92514403"))
				return "ФН-1.1 исп. 6-15-2 (15) ООО Дримкас";
			if (FNSerialNumber.Contains ("92804403"))
				return "ФН-1.1 Ав15-2 (15) АО Концерн Автоматика";
			if (FNSerialNumber.Contains ("92524403"))
				return "ФН-1.1 Ав36-2 (36) АО Концерн Автоматика";

			if (FNSerialNumber.Contains ("99990789"))
				return "Массо-габаритная модель ФН (МГМ)";

			return "неизвестная модель ФН";
			}

		/// <summary>
		/// Метод возвращает модель ККТ по её заводскому номеру
		/// </summary>
		/// <param name="KKTSerialNumber">ЗН ККТ</param>
		/// <returns>Модель ККТ</returns>
		public static string GetKKTModel (string KKTSerialNumber)
			{
			kktSerialNumber = KKTSerialNumber;

			switch (KKTSerialNumber.Trim ().Length)
				{
				case 7:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0))
						return "ЭКР 2102 / Миника 1102";

					if (TEST_SN_D (0, 1) && TEST_SN_D (1, 6))
						return "ПРИМ 07Ф";

					if (TEST_SN_D (0, 1) && TEST_SN_D (1, 7))
						return "ПРИМ 08Ф, 21Ф";
					break;

				/////////////////////////////////////////////////
				case 8:
					if (TEST_SN_D (0, 0))
						{
						if (TEST_SN_D (1, 0))
							return "Меркурий 115Ф";

						if (TEST_SN_D (1, 2))
							return "Меркурий 119Ф";

						if (TEST_SN_D (1, 4))
							return "Меркурий 185Ф";

						if (TEST_SN_D (1, 6))
							return "Меркурий 130Ф";

						if (TEST_SN_D (1, 8))
							return "Меркурий 180Ф";
						}

					if (TEST_SN_D (0, 1) && TEST_SN_D (1, 8))
						return "Нева 01Ф / Мещера 01Ф";

					break;

				/////////////////////////////////////////////////
				case 10:
					if (TEST_SN_D (0, 0))
						{
						if (TEST_SN_D (1, 1) && TEST_SN_D (2, 2) && TEST_SN_D (3, 8))
							return "Пирит 2Ф";

						if (TEST_SN_D (1, 4) && TEST_SN_D (2, 9))
							{
							if (TEST_SN_D (3, 1))
								return "Вики Мини Ф";

							if (TEST_SN_D (3, 3))
								return "Вики Принт 57Ф";

							if (TEST_SN_D (3, 6))
								return "Дримкас Ф";
							}
						}

					if (TEST_SN_D (0, 5) && TEST_SN_D (1, 0) && TEST_SN_D (2, 1))
						return "Пионер 114Ф";

					break;

				/////////////////////////////////////////////////
				case 12:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 2))
						return "АМС 100Ф";

					if (TEST_SN_D (0, 0) && TEST_SN_D (2, 1) && TEST_SN_D (3, 7))
						return "Орион 100Ф";

					if (TEST_SN_D (0, 1) && TEST_SN_D (3, 0) && TEST_SN_D (6, 0))
						{
						if (TEST_SN_D (1, 7) && TEST_SN_D (2, 7) && TEST_SN_D (4, 4) && TEST_SN_D (5, 4))
							return "MSPOS-Е-Ф";

						if (TEST_SN_D (1, 9) && TEST_SN_D (2, 9) && TEST_SN_D (4, 3))
							{
							if (TEST_SN_D (5, 1))
								return "ПТК MSTAR-TK";

							if (TEST_SN_D (5, 6))
								return "MSPOS-K";
							}
						}

					break;

				/////////////////////////////////////////////////
				case 14:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 1) && TEST_SN_D (3, 0))
						{
						if (TEST_SN_D (4, 5) && TEST_SN_D (5, 7))
							return "Атол 25Ф";

						if (TEST_SN_D (4, 6))
							{
							if (TEST_SN_D (5, 1))
								return "Атол 30Ф";

							if (TEST_SN_D (5, 2))
								return "Атол 55Ф";

							if (TEST_SN_D (5, 3))
								return "Атол 22Ф";

							if (TEST_SN_D (5, 4))
								return "Атол 52Ф";

							if (TEST_SN_D (5, 7))
								return "Атол 11Ф";

							if (TEST_SN_D (5, 9))
								return "Атол 77Ф";
							}

						if (TEST_SN_D (4, 7))
							{
							if (TEST_SN_D (5, 2))
								return "Атол 90Ф";

							if (TEST_SN_D (5, 5))
								return "Атол 60Ф";

							if (TEST_SN_D (5, 6))
								return "Казначей ФА";

							if (TEST_SN_D (5, 8))
								return "Атол 15Ф";
							}

						if (TEST_SN_D (4, 8))
							{
							if (TEST_SN_D (5, 0))
								return "Атол 50Ф";

							if (TEST_SN_D (5, 1))
								return "Атол 20Ф";

							if (TEST_SN_D (5, 2))
								return "Атол 91Ф";

							if (TEST_SN_D (5, 4))
								return "Атол 92Ф";

							if (TEST_SN_D (5, 6))
								return "Атол Sigma 10 (150Ф)";
							}

						if (TEST_SN_D (4, 9))
							{
							if (TEST_SN_D (5, 0))
								return "Атол Sigma 7Ф";

							if (TEST_SN_D (5, 1))
								return "Атол Sigma 8Ф";
							}
						}

					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 3) && TEST_SN_D (3, 0))
						{
						if (TEST_SN_D (4, 7) && TEST_SN_D (5, 4))
							return "Эвотор СТ2Ф (7.2)";

						if (TEST_SN_D (4, 7) && TEST_SN_D (5, 9))
							return "Эвотор СТ3Ф (7.3 / 10)";

						if (TEST_SN_D (4, 8) && TEST_SN_D (5, 3))
							return "Эвотор СТ5Ф (5)";
						}

					break;

				/////////////////////////////////////////////////
				case 16:
					if (TEST_SN_D (0, 0))
						{
						if (TEST_SN_D (8, 0))
							{
							if (TEST_SN_D (9, 1))
								return "Штрих-Мини-01Ф";

							if (TEST_SN_D (9, 2))
								return "Штрих-ФР-01Ф";

							if (TEST_SN_D (9, 4))
								return "Штрих-Online";

							if (TEST_SN_D (9, 5))
								return "PayOnline-01 ФА";

							if (TEST_SN_D (9, 6))
								return "Штрих-М-01Ф";

							if (TEST_SN_D (9, 7))
								return "Штрих-Лайт-01Ф";

							if (TEST_SN_D (9, 8))
								return "Штрих-М-02Ф";

							if (TEST_SN_D (9, 9))
								return "Штрих-Лайт-02Ф";
							}

						if (TEST_SN_D (8, 1))
							{
							if (TEST_SN_D (9, 2))
								return "Ритейл-01Ф";

							if (TEST_SN_D (9, 4))
								return "РР-03Ф";

							if (TEST_SN_D (9, 5))
								return "РР-04Ф";

							if (TEST_SN_D (9, 9))
								return "Штрих-ФР-02Ф";
							}

						if (TEST_SN_D (8, 2))
							{
							if (TEST_SN_D (9, 4))
								return "Штрих-Мини-02Ф";

							if (TEST_SN_D (9, 7))
								return "PayVKP-80 ФА";
							}

						if (TEST_SN_D (8, 3) && TEST_SN_D (9, 0))
							return "Элвес-МФ";
						}

					break;
				}

			return "???";
			}

		private static string kktSerialNumber;
		private static bool TEST_SN_D (uint position, byte digit)
			{
			return kktSerialNumber[(int)position] == digit.ToString ()[0];
			}

		/// <summary>
		/// Метод формирует дату истечения срока эксплуатации ФН с указанными параметрами
		/// </summary>
		/// <param name="StartDate">Дата фискализации</param>
		/// <param name="FN15">Флаг указывает на выбор ФН на 15 месяцев вместо 36</param>
		/// <param name="FNExactly13">Флаг указывает на выбор ФН на 13 месяцев вместо 15</param>
		/// <param name="GenericTax">Флаг указыввает на применение ОСН</param>
		/// <param name="Goods">Флаг указывает на режим товаров вместо услуг</param>
		/// <param name="SeasonOrAgents">Флаг указывает на агентскую схему или сезонный режим работы</param>
		/// <param name="Excise">Флаг указывает на наличие подакцизных товаров</param>
		/// <param name="Autonomous">Флаг указывает на работу без передачи данных</param>
		/// <returns>Возвращает строку с датой или пустую строку, если указанная модель ФН
		/// не может быть использована с указанными режимами и параметрами</returns>
		public static string GetFNLifeEndDate (DateTime StartDate, bool FN15, bool FNExactly13,
			bool GenericTax, bool Goods, bool SeasonOrAgents, bool Excise, bool Autonomous)
			{
			string res = "";

			// Отсечение недопустимых вариантов
			if (GenericTax && !FN15 && Goods ||
				!GenericTax && FN15 && !SeasonOrAgents && !Excise && !Autonomous)
				{
				res = "!";
				}

			// Определение срока жизни
			int length = 1110;

			if (Excise)
				{
				length = 410;
				}
			else if (Autonomous)
				{
				if (FN15)
					length = 410;
				else
					length = 560;
				}
			else if (FN15)
				{
				length = FNExactly13 ? 410 : 470;
				}

			// Результат
			return res + StartDate.AddDays (length).ToString ("dd.MM.yyyy");
			}

		// Контрольная последовательность для определения корректности ИНН
		private static byte[] innCheckSequence = new byte[] { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };

		/// <summary>
		/// Метод проверяет корректность ввода ИНН
		/// </summary>
		/// <param name="INN">ИНН для проверки</param>
		/// <returns>Возвращает true, если ИНН корректен</returns>
		public static bool CheckINN (string INN)
			{
			// Контроль параметра
			UInt64 inn = 0;
			try
				{
				inn = UInt64.Parse (INN);
				}
			catch
				{
				return false;
				}

			if ((INN.Length != 10) && (INN.Length != 12))
				return false;

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
					return true;

				return false;
				}

			// Для 12 цифр
			d = 100;
			for (int i = 0; i < 10; i++)
				{
				n1 += (uint)((byte)((inn / d) % 10) * innCheckSequence[10 - i]);
				d *= 10;
				}

			if ((n1 % 11) != ((inn / 10) % 10))
				return false;

			d = 10;
			for (int i = 0; i < 11; i++)
				{
				n2 += (uint)((byte)((inn / d) % 10) * innCheckSequence[10 - i]);
				d *= 10;
				}

			if ((n2 % 11) != (inn % 10))
				return false;

			return true;
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

			if (!CheckINN (INN) || (Serial.Length > 20) || (RNMFirstPart.Length > 10))
				return "";

			// Расчёт контрольной суммы
			string msg = RNMFirstPart.PadLeft (10, '0') + INN.PadLeft (12, '0') + Serial.PadLeft (20, '0');
			UInt16 crc = GetCRC16 (msg);

			return RNMFirstPart.PadLeft (10, '0') + crc.ToString ().PadLeft (6, '0');
			}
		}
	}
