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
		/// <param name="FNSerialNumber">Заводской номер ФН</param>
		/// <returns>Модель ФН</returns>
		public static string GetFNName (string FNSerialNumber)
			{
			if (FNSerialNumber.StartsWith ("87100001"))
				return "ФН-1 (13) ООО РИК";
			if (FNSerialNumber.StartsWith ("87110001"))
				return "ФН-1 исп. 2 (36) ООО РИК";
			if (FNSerialNumber.StartsWith ("8712000100"))
				return "ФН-1 исп. 3 v2 (13) ООО Прагматик";
			if (FNSerialNumber.StartsWith ("8712000101"))
				return "ФН-1 исп. 3 v1 (13) ООО НТЦ Измеритель";
			if (FNSerialNumber.StartsWith ("87150001"))
				return "ФН-1 Пр13-2 (13) ООО Прагматик";
			if (FNSerialNumber.StartsWith ("87160001"))
				return "ФН-1 Из13-2 (13) ООО НТЦ Измеритель";

			if (FNSerialNumber.StartsWith ("92810001"))
				return "ФН-1.1 исп. 4 (36) ООО Инвента";
			if (FNSerialNumber.StartsWith ("92820001") || FNSerialNumber.StartsWith ("92824403"))
				return "ФН-1.1 исп. 5-15-2 (15) ООО Инвента";
			if (FNSerialNumber.StartsWith ("92834403"))
				return "ФН-1.1 ЭВ36-2 (36) ООО Эвотор";
			if (FNSerialNumber.StartsWith ("92840001"))
				return "ФН-1.1 ЭВ15-2 (15) ООО Эвотор";
			if (FNSerialNumber.StartsWith ("92850001") || FNSerialNumber.StartsWith ("92854403"))
				return "ФН-1.1 Из15-2 (15) ООО НТЦ Измеритель";
			if (FNSerialNumber.StartsWith ("92860001"))
				return "ФН-1.1 исп. 3 (15) АО Концерн Автоматика";
			if (FNSerialNumber.StartsWith ("92874403"))
				return "ФН-1.1 исп. 2 (36) ООО Инвента";
			if (FNSerialNumber.StartsWith ("92880001"))
				return "ФН-1.1 исп. 5-15-1 (15) ООО Инвента";
			if (FNSerialNumber.StartsWith ("92890001"))
				return "ФН-1.1 Пр15-2 (15) ООО Прагматик";
			if (FNSerialNumber.StartsWith ("92514403"))
				return "ФН-1.1 исп. 6-15-2 (15) ООО Дримкас";
			if (FNSerialNumber.StartsWith ("92804403"))
				return "ФН-1.1 Ав15-2 (15) АО Концерн Автоматика";
			if (FNSerialNumber.StartsWith ("92524403"))
				return "ФН-1.1 Ав36-2 (36) АО Концерн Автоматика";

			if (FNSerialNumber.StartsWith ("99604403"))
				return "ФН-1.1М Ин15-1М (15) ООО Инвента";
			if (FNSerialNumber.StartsWith ("99614403"))
				return "ФН-1.1М Ин36-1М (36) ООО Инвента";

			if (FNSerialNumber.StartsWith ("99990789"))
				return "Массо-габаритная модель ФН (МГМ)";

			return "неизвестная модель ФН";
			}

		/// <summary>
		/// Возвращает флаг, указывающий на поддержку ФФД 1.2 моделью ФН, соответствующей указанному ЗН
		/// </summary>
		/// <param name="FNSerialNumber">Заводской номер ФН</param>
		public static bool IsFNCompatibleWithFFD12 (string FNSerialNumber)
			{
			return GetFNName (FNSerialNumber).Contains ("1М");
			}

		/// <summary>
		/// Метод возвращает модель ККТ по её заводскому номеру
		/// </summary>
		/// <param name="KKTSerialNumber">Заводской номер ККТ</param>
		public static string GetKKTModel (string KKTSerialNumber)
			{
			return GetKKTModelData (KKTSerialNumber).Substring (4);
			}

		/// <summary>
		/// Доступные статусы поддержки ФФД
		/// </summary>
		public enum FFDSupportStatuses
			{
			/// <summary>
			/// Поддерживается
			/// </summary>
			Supported,

			/// <summary>
			/// Не поддерживается
			/// </summary>
			Unsupported,

			/// <summary>
			/// На данный момент не определён
			/// </summary>
			Unknown,

			/// <summary>
			/// Запланирован
			/// </summary>
			Planned
			}

		/// <summary>
		/// Метод возвращает статус поддержки ФФД для ККТ по её заводскому номеру
		/// </summary>
		/// <param name="KKTSerialNumber">Заводской номер ККТ</param>
		/// <returns>Возвращает вектор из трёх состояний для ФФД 1.05, 1.1 и 1.2</returns>
		public static FFDSupportStatuses[] GetFFDSupportStatus (string KKTSerialNumber)
			{
			string status = GetKKTModelData (KKTSerialNumber).Substring (0, 3);
			FFDSupportStatuses[] res = new FFDSupportStatuses[]
				{
				FFDSupportStatuses.Unknown, // 1.05
				FFDSupportStatuses.Unknown, // 1.1
				FFDSupportStatuses.Unknown	// 1.2
				};

			if (status == "???")
				return res;

			for (int i = 0; i < 3; i++)
				{
				if (status[i] == '1')
					res[i] = FFDSupportStatuses.Supported;
				else if (status[i] == '0')
					res[i] = FFDSupportStatuses.Unsupported;
				else if (status[i] == '+')
					res[i] = FFDSupportStatuses.Planned;
				}

			return res;
			}

		private static string GetKKTModelData (string KKTSerialNumber)
			{
			kktSerialNumber = KKTSerialNumber;

			// Перед названием ККТ установлен префикс, указывающий на поддержку ФФД 1.05, 1.1 и 1.2
			switch (KKTSerialNumber.Trim ().Length)
				{
				case 7:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0))
						return "1?+|ЭКР 2102 / Миника 1102";

					if (TEST_SN_D (0, 1) && TEST_SN_D (1, 6))
						return "1?0|ПРИМ 07Ф";

					if (TEST_SN_D (0, 1) && TEST_SN_D (1, 7))
						return "1?+|ПРИМ 08Ф, 21ФА";
					break;

				/////////////////////////////////////////////////
				case 8:
					if (TEST_SN_D (0, 0))
						{
						if (TEST_SN_D (1, 0))
							return "1?+|Меркурий 115Ф";

						if (TEST_SN_D (1, 2))
							return "1?+|Меркурий 119Ф";

						if (TEST_SN_D (1, 4))
							return "1?+|Меркурий 185Ф";

						if (TEST_SN_D (1, 6))
							return "1?+|Меркурий 130Ф";

						if (TEST_SN_D (1, 8))
							return "1?+|Меркурий 180Ф";
						}

					if (TEST_SN_D (0, 1) && TEST_SN_D (1, 8))
						return "1?+|Нева 01Ф / Мещера 01Ф";

					break;

				/////////////////////////////////////////////////
				case 10:
					if (TEST_SN_D (0, 0))
						{
						if (TEST_SN_D (1, 1) && TEST_SN_D (2, 2))
							{
							if (TEST_SN_D (3, 6))
								return "1?0|Пирит 1Ф";

							if (TEST_SN_D (3, 8))
								return "1?+|Пирит 2Ф";

							if (TEST_SN_D (3, 9))
								return "11+|Пирит 2СФ";
							}

						if (TEST_SN_D (1, 4) && TEST_SN_D (2, 9))
							{
							if (TEST_SN_D (3, 1))
								return "1?+|Вики Мини Ф";

							if (TEST_SN_D (3, 2))
								return "1?+|Viki Tower Ф";

							if (TEST_SN_D (3, 3))
								return "1?+|Вики Принт 57Ф";

							if (TEST_SN_D (3, 4))
								return "1?+|Вики Принт 57 плюс Ф";

							if (TEST_SN_D (3, 5))
								return "1?+|Вики Принт 80 плюс Ф";

							if (TEST_SN_D (3, 6))
								return "1?+|Дримкас Ф";

							if (TEST_SN_D (3, 7))
								return "1?+|Касса Ф";

							if (TEST_SN_D (3, 8))
								return "1?+|Пульс ФА";

							if (TEST_SN_D (3, 9))
								return "110|Спутник Ф";
							}
						}

					if (TEST_SN_D (0, 5) && TEST_SN_D (1, 0) && TEST_SN_D (2, 1))
						return "1?+|Пионер 114Ф";

					break;

				/////////////////////////////////////////////////
				case 12:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 2))
						return "1?0|АМС 100Ф";

					if (TEST_SN_D (0, 0) && TEST_SN_D (2, 1) && TEST_SN_D (3, 7))
						return "1?0|Орион 100Ф";

					if (TEST_SN_D (0, 1) && TEST_SN_D (3, 0))
						{
						if (TEST_SN_D (1, 7) && TEST_SN_D (2, 7) && TEST_SN_D (4, 4) && TEST_SN_D (5, 4))
							return "1?+|MSPOS-Е-Ф";

						if (TEST_SN_D (1, 9) && TEST_SN_D (2, 9) && TEST_SN_D (4, 3))
							{
							if (TEST_SN_D (5, 1))
								return "1?+|ПТК MSTAR-TK";

							if (TEST_SN_D (5, 6))
								return "1?+|MSPOS-K";
							}
						}

					break;

				/////////////////////////////////////////////////
				case 13:
					if (TEST_SN_D (4, 1) && TEST_SN_D (5, 0))
						{
						if (TEST_SN_D (3, 4))
							return "1??|POSprint FP410-Ф";

						if (TEST_SN_D (3, 5))
							return "1??|POSprint FP510-Ф";
						}

					break;

				/////////////////////////////////////////////////
				case 14:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 1) && TEST_SN_D (3, 0))
						{
						if (TEST_SN_D (4, 5) && TEST_SN_D (5, 7))
							return "11+|Атол 25Ф";

						if (TEST_SN_D (4, 6))
							{
							if (TEST_SN_D (5, 1))
								return "11+|Атол 30Ф";

							if (TEST_SN_D (5, 2))
								return "11+|Атол 55Ф";

							if (TEST_SN_D (5, 3))
								return "11+|Атол 22Ф";

							if (TEST_SN_D (5, 4))
								return "1?0|Атол 52Ф";

							if (TEST_SN_D (5, 7))
								return "11+|Атол 11Ф";

							if (TEST_SN_D (5, 9))
								return "11+|Атол 77Ф";
							}

						if (TEST_SN_D (4, 7))
							{
							if (TEST_SN_D (5, 2))
								return "1?0|Атол 90Ф";

							if (TEST_SN_D (5, 5))
								return "1?0|Атол 60Ф";

							if (TEST_SN_D (5, 6))
								return "11+|Казначей ФА";

							if (TEST_SN_D (5, 8))
								return "1?+|Атол 15Ф";
							}

						if (TEST_SN_D (4, 8))
							{
							if (TEST_SN_D (5, 0))
								return "11+|Атол 50Ф";

							if (TEST_SN_D (5, 1))
								return "11+|Атол 20Ф";

							if (TEST_SN_D (5, 2))
								return "11+|Атол 91Ф";

							if (TEST_SN_D (5, 4))
								return "1?+|Атол 92Ф";

							if (TEST_SN_D (5, 6))
								return "11+|Атол Sigma 10 (150Ф)";
							}

						if (TEST_SN_D (4, 9))
							{
							if (TEST_SN_D (5, 0))
								return "11+|Атол Sigma 7Ф";

							if (TEST_SN_D (5, 1))
								return "11+|Атол Sigma 8Ф";
							}
						}

					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 3) && TEST_SN_D (3, 0))
						{
						if (TEST_SN_D (4, 7) && TEST_SN_D (5, 4))
							return "11+|Эвотор СТ2Ф (7.2)";

						if (TEST_SN_D (4, 7) && TEST_SN_D (5, 9))
							return "11+|Эвотор СТ3Ф (7.3 / 10)";

						if (TEST_SN_D (4, 8) && TEST_SN_D (5, 3))
							return "11+|Эвотор СТ5Ф (5)";
						}

					break;

				/////////////////////////////////////////////////
				case 16:
					if (TEST_SN_D (0, 0))
						{
						if (TEST_SN_D (8, 0))
							{
							if (TEST_SN_D (9, 1))
								return "1?+|Штрих-Мини-01Ф";

							if (TEST_SN_D (9, 2))
								return "1?+|Штрих-ФР-01Ф";

							if (TEST_SN_D (9, 3))
								return "1?+|РР-01Ф";

							if (TEST_SN_D (9, 4))
								return "1?+|Штрих-Online";

							if (TEST_SN_D (9, 5))
								return "1?+|PayOnline-01 ФА";

							if (TEST_SN_D (9, 6))
								return "11+|Штрих-М-01Ф";

							if (TEST_SN_D (9, 7))
								return "11+|Штрих-Лайт-01Ф";

							if (TEST_SN_D (9, 8))
								return "1?+|Штрих-М-02Ф";

							if (TEST_SN_D (9, 9))
								return "1?+|Штрих-Лайт-02Ф";
							}

						if (TEST_SN_D (8, 1))
							{
							if (TEST_SN_D (9, 2))
								return "1?+|Ритейл-01Ф";

							if (TEST_SN_D (9, 3))
								return "1?+|РР-02Ф";

							if (TEST_SN_D (9, 4))
								return "1?+|РР-03Ф";

							if (TEST_SN_D (9, 5))
								return "1?+|РР-04Ф";

							if (TEST_SN_D (9, 9))
								return "1?0|Штрих-ФР-02Ф";
							}

						if (TEST_SN_D (8, 2))
							{
							if (TEST_SN_D (9, 2))
								return "1?+|Элвес ФР-Ф";

							if (TEST_SN_D (9, 4))
								return "1?0|Штрих-Мини-02Ф";

							if (TEST_SN_D (9, 7))
								return "1?0|PayVKP-80 ФА";
							}

						if (TEST_SN_D (8, 3))
							{
							if (TEST_SN_D (9, 0))
								return "1?0|Элвес-МФ";

							if (TEST_SN_D (9, 3))
								return "1?+|Штрих-СМАРТПОС-Ф v 1";
							}

						if (TEST_SN_D (8, 4))
							{
							if (TEST_SN_D (9, 0))
								return "1?0|Элвес-МФ (ФР)";

							if (TEST_SN_D (9, 3))
								return "1?+|Штрих-СМАРТПОС-Ф v 2";
							}
						}

					break;

				/////////////////////////////////////////////////
				case 20:
					if (TEST_SN_D (0, 0) && TEST_SN_D (1, 0) && TEST_SN_D (2, 0) && TEST_SN_D (3, 1))
						return "1?0|Уникум-ФА";

					break;
				}

			return "???|неизвестная модель ККТ";
			}

		private static string kktSerialNumber;
		private static bool TEST_SN_D (uint position, byte digit)
			{
			return kktSerialNumber[(int)position] == digit.ToString ()[0];
			}

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
		/// <returns>Возвращает 0, если ИНН корректен, 1, если ИНН имеет некорректную КС, -1, если строка не является ИНН</returns>
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
