using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RD_AAOW
	{
	/// <summary>
	/// Типы поддерживаемых штрих-кодов
	/// </summary>
	public enum SupportedBarcodesTypes
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
		/// DataMatrix с маркировкой для сигарет
		/// </summary>
		DataMatrixCigarettes = 3,

		/// <summary>
		/// Неподдерживаемый тип штрих-кода
		/// </summary>
		Unsupported = -1
		}

	/// <summary>
	/// Класс обеспечивает доступ к функционалу контроля и расшифровки штрих-кодов
	/// </summary>
	public class BarCodes
		{
		// Переменные для EAN-8 и EAN-13
		private List<uint> rangeStart = new List<uint> ();
		private List<uint> rangeEnd = new List<uint> ();
		private List<string> descriptions = new List<string> ();
		private List<bool> country = new List<bool> ();

		// Константы для DataMatrix
		private const string dmEncodingLine = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!\"%&'*+-./_,:;=<>?";

		/// <summary>
		/// Максимальная поддерживаемая длина данных штрих-кода
		/// </summary>
		public const uint MaxSupportedDataLength = 29;

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
				throw new Exception ("Barcodes data reading failure, point 1");
				}

			// Завершено
			SR.Close ();
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

			if (data.Length == 29)
				return SupportedBarcodesTypes.DataMatrixCigarettes;

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
		public string GetBarcodeDescription (string BarcodeData)
			{
			// Тип штрих-кода
			SupportedBarcodesTypes type = GetBarcodeType (BarcodeData);
			if (type == SupportedBarcodesTypes.Unsupported)
				return "Штрих-код неполный, некорректный или не поддерживается";
			string res = "Тип штрих-кода: " + type.ToString () + "\r\n";

			// Разбор
			switch (type)
				{
				case SupportedBarcodesTypes.EAN8:
				case SupportedBarcodesTypes.EAN13:
					res += GetEANUsage (BarcodeData);
					break;

				case SupportedBarcodesTypes.DataMatrixCigarettes:
					res += ParseCigarettesDataMatrix (BarcodeData);
					break;
				}

			// Завершено
			return res;
			}

		/// <summary>
		/// Метод возвращает описание штрих-кода DataMatrix для маркированных сигарет
		/// </summary>
		/// <param name="BarcodeData">Данные штрих-кода</param>
		public string ParseCigarettesDataMatrix (string BarcodeData)
			{
			// Контроль
			/*if (string.IsNullOrWhiteSpace (BarcodeData) || (BarcodeData.Length != 29))
				return "Некорректный DataMatrix";*/

			string res = "GTIN: " + BarcodeData.Substring (0, 14) + ", ";
			if (!CheckEAN (BarcodeData.Substring (1, 13), true))
				res += "EAN-13 некорректен;\r\n";
			else if (BarcodeData.StartsWith ("000000"))
				res += (GetEANUsage (BarcodeData.Substring (6, 8)) + ";\r\n");
			else
				res += (GetEANUsage (BarcodeData.Substring (1, 13)) + ";\r\n");

			res += ("Серийный номер упаковки: " + BarcodeData.Substring (14, 7) + " (" +
				DecodeDMLine (BarcodeData.Substring (14, 7), false) + ");\r\n");
			res += ("Максимальная розничная цена: " +
				DecodeDMLine (BarcodeData.Substring (21, 4), true) + ";\r\n");
			res += ("Код проверки: " + BarcodeData.Substring (25, 4) + " (" +
				DecodeDMLine (BarcodeData.Substring (25, 4), false) + ")");

			return res;
			}

		// Расшифровка числовых данных в DataMatrix
		private string DecodeDMLine (string Data, bool AsPrice)
			{
			// Сборка числа
			long v = 0, mul1, mul2;

			for (int i = 1; i <= Data.Length; i++)
				{
				mul1 = (long)Math.Pow (dmEncodingLine.Length, Data.Length - i);
				if ((mul2 = dmEncodingLine.IndexOf (Data[i - 1])) < 0)
					return "значение не читается";
				v += mul1 * mul2;
				}

			// Вывод
			if (AsPrice)
				return (v / 100.0).ToString ("F02") + " р.";

			return v.ToString ();
			}
		}
	}
