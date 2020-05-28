namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает вспомогательные методы
	/// </summary>
	public static class KKTSupport
		{
		/// <summary>
		/// Метод возвращает название ОФД по его ИНН
		/// </summary>
		/// <param name="OFDINN">ИНН ОФД</param>
		/// <returns>Название ОФД</returns>
		public static string GetOFDName (string OFDINN)
			{
			if (OFDINN.Contains ("2310031475"))
				return "АО «Тандер»";
			if (OFDINN.Contains ("4029017981"))
				return "ЗАО «КАЛУГА АСТРАЛ»";
			if (OFDINN.Contains ("5902034504"))
				return "ООО УЦ «ИнитПро»";
			if (OFDINN.Contains ("6658497833"))
				return "ООО «Контур НТТ»";
			if (OFDINN.Contains ("6663003127"))
				return "АО «ПФ «СКБ Контур»";
			if (OFDINN.Contains ("6686089392"))
				return "ООО «ОФД «Онлайн»";
			if (OFDINN.Contains ("7605016030"))
				return "ООО «Компания «Тензор»";
			if (OFDINN.Contains ("7701553038"))
				return "АО «Информационный центр»";
			if (OFDINN.Contains ("7703282175"))
				return "АО «Энвижн Груп»";
			if (OFDINN.Contains ("7704211201"))
				return "ООО «Такском»";
			if (OFDINN.Contains ("7704358518"))
				return "ООО «Яндекс.ОФД»";
			if (OFDINN.Contains ("7709364346"))
				return "АО «ЭСК» (Первый ОФД)";
			if (OFDINN.Contains ("7710007966"))
				return "ООО «МультиКарта»";
			if (OFDINN.Contains ("7713076301"))
				return "ПАО «Вымпел-Коммуникации»";
			if (OFDINN.Contains ("7728699517"))
				return "ООО «Ярус»";
			if (OFDINN.Contains ("7729633131"))
				return "ООО «Электронный экспресс»";
			if (OFDINN.Contains ("7729642175"))
				return "ООО «ГРУППА ЭЛЕМЕНТ»";
			if (OFDINN.Contains ("7801392271"))
				return "ООО «КОРУС Консалтинг СНГ»";
			if (OFDINN.Contains ("7802870820"))
				return "ООО «Дримкас»";
			if (OFDINN.Contains ("7841465198"))
				return "ООО «ПЕТЕР-СЕРВИС Спецтехнологии»";
			if (OFDINN.Contains ("9715260691"))
				return "ООО «Эвотор ОФД»";

			if (OFDINN.Contains ("0000000000"))
				return "без ОФД";

			return "неизвестный ОФД";
			}

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
			if (FNSerialNumber.Contains ("92820001"))
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

			return "неизвестная модель ККТ";
			}

		private static string kktSerialNumber;
		private static bool TEST_SN_D (uint position, byte digit)
			{
			return kktSerialNumber[(int)position] == digit.ToString ()[0];
			}
		}
	}
