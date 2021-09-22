using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает доступ к функционалу контроля и расшифровки штрих-кодов
	/// </summary>
	public class BarCodes
		{
		// Переменные
		private List<uint> rangeStart = new List<uint> ();
		private List<uint> rangeEnd = new List<uint> ();
		private List<string> descriptions = new List<string> ();
		private List<bool> country = new List<bool> ();

		/// <summary>
		/// Максимальная поддерживаемая длина данных штрих-кода
		/// </summary>
		public const uint MaxSupportedDataLength = 13;

		/// <summary>
		/// Конструктор. Инициализирует таблицу
		/// </summary>
		public BarCodes ()
			{
			// Получение файлов
#if !ANDROID
			string buf = Encoding.UTF8.GetString (RD_AAOW.Properties.TextToKKMResources.BarCodes);
#else
			string buf = Encoding.UTF8.GetString (RD_AAOW.Properties.Resources.BarCodes);
#endif
			StringReader SR = new StringReader (buf);

			// Формирование массива 
			string str;
			char[] splitters = new char[] { ';' };

			try
				{
				// Чтение параметров
				while ((str = SR.ReadLine ()) != null)
					{
					string[] values = str.Split (splitters);

					// Описания тегов
					if (values.Length < 3)
						continue;

					rangeStart.Add (uint.Parse (values[0]));
					if (values[1] == "")
						rangeEnd.Add (rangeStart[rangeStart.Count - 1]);
					else
						rangeEnd.Add (uint.Parse (values[1]));
					descriptions.Add (values[2]);
					country.Add (values.Length == 3);
					}
				}
			catch
				{
				}

			// Завершено
			SR.Close ();
			}

		/// <summary>
		/// Типы поддерживаемых штрих-кодов
		/// </summary>
		private enum SupportedBarcodesTypes
			{
			/// <summary>
			/// EAN-8
			/// </summary>
			EAN8 = 1,

			/// <summary>
			/// EAN-13
			/// </summary>
			EAN13 = 2,

			/// <summary>
			/// Неподдерживаемый тип штрих-кода
			/// </summary>
			Unsupported = -1
			}

		// Метод определяет тип штрих-кода
		private SupportedBarcodesTypes GetBarcodeType (string BarcodeData)
			{
			// Контроль
			if (string.IsNullOrWhiteSpace (BarcodeData))
				return SupportedBarcodesTypes.Unsupported;
			string data = BarcodeData.Trim ();

			// Определение типа
			if ((data.Length == 8) && CheckEAN (data, false))
				return SupportedBarcodesTypes.EAN8;

			if ((data.Length == 13) && CheckEAN (data, true))
				return SupportedBarcodesTypes.EAN13;

			return SupportedBarcodesTypes.Unsupported;
			}

		// Контроль целостности кодов типа EAN
		private bool CheckEAN (string BarcodeData, bool EAN13)
			{
			// Разбор штрихкода
			int checksum = 0;
			for (int i = 0; i < (EAN13 ? 12 : 7); i++)
				{
				byte step = ((i % 2) == (EAN13 ? 1 : 0)) ? (byte)3 : (byte)1;
				try
					{
					checksum += (byte.Parse (BarcodeData[i].ToString ()) * step);
					}
				catch
					{
					return false;
					}
				}

			return ((10 - (checksum % 10)).ToString () == BarcodeData[EAN13 ? 12 : 7].ToString ());
			}

		// Метод возвращает область применения штрих-кода или страну-производителя, которой он соответствует
		private string GetEANUsage (string BarcodeData)
			{
			// Контроль
			const string unknownPrefix = "Применение: неизвестно";
			if (string.IsNullOrWhiteSpace (BarcodeData) || (BarcodeData.Length < 3))
				return unknownPrefix;

			// Поиск
			uint prefix = 0;
			try
				{
				prefix = uint.Parse (BarcodeData.Substring (0, 3));
				}
			catch
				{
				return unknownPrefix;
				}

			for (int i = 0; i < rangeStart.Count; i++)
				if ((prefix >= rangeStart[i]) && (prefix <= rangeEnd[i]))
					return (country[i] ? "Производитель: " : "Применение: ") + descriptions[i];

			return unknownPrefix;
			}

		/// <summary>
		/// Метод возвращает описание штрих-кода по его данным
		/// </summary>
		/// <param name="BarcodeData">Данные штрих-кода</param>
		/// <returns></returns>
		public string GetBarcodeDescription (string BarcodeData)
			{
			// Тип штрих-кода
			SupportedBarcodesTypes type = GetBarcodeType (BarcodeData);
			if (type == BarCodes.SupportedBarcodesTypes.Unsupported)
				return "Штрих-код неполный, некорректный или не поддерживается";
			string res = "Тип штрих-кода: " + type.ToString () + "\r\n";

			// Данные области применения
			res += GetEANUsage (BarcodeData);

			// Завершено
			return res;
			}
		}
	}
