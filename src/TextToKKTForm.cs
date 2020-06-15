﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает главную форму программы
	/// </summary>
	public partial class TextToKKMForm:Form
		{
		// Переменные
		private KKTCodes kkmc = null;
		private KKTErrorsList kkme = null;
		private OFD ofd = null;

		/// <summary>
		/// Конструктор. Запускает главную форму
		/// </summary>
		public TextToKKMForm ()
			{
			// Функция, используемая для добавления новых моделей
			//TablesInterpreter.Interpretate ();

			// Инициализация
			InitializeComponent ();

			// Загрузка списка кодов и ошибок
			kkmc = new KKTCodes ();
			kkme = new KKTErrorsList ();
			ofd = new OFD ();

			// Настройка контролов
			OnlyNewCodes_CheckedChanged (null, null);
			OnlyNewErrors_CheckedChanged (null, null);

			FNLifeStartDate.Value = DateTime.Now;

			OFDNamesList.Items.Add ("Неизвестный ОФД");
			OFDNamesList.Items.AddRange (ofd.GetOFDNames ().ToArray ());
			OFDNamesList.SelectedIndex = 0;

			// Настройка контролов
			this.Text = ProgramDescription.AssemblyTitle;
			}

		// Завершение работы
		private void BExit_Click (object sender, EventArgs e)
			{
			this.Close ();
			}

		// Изменение текста и его кодировка
		private void TextToConvert_TextChanged (object sender, EventArgs e)
			{
			ResultText.Text = "";

			ErrorLabel.Visible = !Decode ();

			TextLabel.Text = "Текст (" + TextToConvert.Text.Length + "):";

			DescriptionLabel.Text = kkmc.GetKKTTypeDescription ((uint)KKTListForCodes.SelectedIndex);
			}

		// Функция трансляции строки в набор кодов
		private bool Decode ()
			{
			bool res = true;
			char[] text = TextToConvert.Text.ToCharArray ();

			for (int i = 0; i < TextToConvert.Text.Length; i++)
				{
				string s;
				byte[] b = Encoding.GetEncoding (1251).GetBytes (text, i, 1);

				if ((s = kkmc.GetCode ((uint)KKTListForCodes.SelectedIndex, b[0])) == KKTCodes.EmptyCode)
					{
					ResultText.Text += "xxx\t";
					res = false;
					}
				else
					{
					ResultText.Text += (s + "\t");
					}
				}

			// Означает успех/ошибку преобразования
			return res;
			}

		// Выбор ККМ
		private void KKTListForCodes_SelectedIndexChanged (object sender, EventArgs e)
			{
			TextToConvert_TextChanged (null, null);
			}

		// Отображение справки
		private void MainForm_HelpButtonClicked (object sender, CancelEventArgs e)
			{
			// Отмена обработки события вызова справки
			e.Cancel = true;

			// Отображение
			AboutForm af = new AboutForm (SupportedLanguages.ru_ru,
				"https://github.com/adslbarxatov/TextToKKT",
				"https://github.com/adslbarxatov/TextToKKT/releases",
				"", ProgramDescription.AssemblyAbilities);
			}

		// Выбор ошибки
		private void ErrorCodesList_SelectedIndexChanged (object sender, EventArgs e)
			{
			ErrorText.Text = kkme.GetErrorText ((uint)KKTListForErrors.SelectedIndex, (uint)ErrorCodesList.SelectedIndex);
			}

		// Выбор модели аппарата
		private void KKTListForErrors_SelectedIndexChanged (object sender, EventArgs e)
			{
			// Перезаполнение списка
			ErrorCodesList.DataSource = kkme.GetErrors ((uint)KKTListForErrors.SelectedIndex);
			ErrorCodesList.DisplayMember = ErrorCodesList.ValueMember = "ErrorCode";
			ErrorCodesList.SelectedIndex = 0;
			}

		// Запрос модели ФН
		private void FNSerial_TextChanged (object sender, EventArgs e)
			{
			if (FNSerial.Text != "")
				FNResult.Text = KKTSupport.GetFNName (FNSerial.Text);
			else
				FNResult.Text = "(введите ЗН ФН)";
			}

		// Запрос модели ККТ
		private void KKTSerial_TextChanged (object sender, EventArgs e)
			{
			if (KKTSerial.Text != "")
				{
				KKTResult.Text = KKTSupport.GetKKTModel (KKTSerial.Text);
				}
			else
				{
				KKTResult.Text = "(введите ЗН ККТ)";
				}
			}

		// Дополнительные функции
		private void GetFNReader_Click (object sender, EventArgs e)
			{
			try
				{
				Process.Start ("mailto://adslbarxatov@scat-m.ru");
				}
			catch
				{
				}
			}

		private void OtherProjects_Click (object sender, EventArgs e)
			{
			try
				{
				Process.Start ("https://github.com/adslbarxatov/");
				}
			catch
				{
				}
			}

		private void AboutFNReader_Click (object sender, EventArgs e)
			{
			try
				{
				Process.Start ("https://github.com/adslbarxatov/FNReader");
				}
			catch
				{
				}
			}

		private void FNReaderUserManual_Click (object sender, EventArgs e)
			{
			try
				{
				Process.Start ("https://github.com/adslbarxatov/FNReader/blob/master/FNReader.pdf");
				}
			catch
				{
				}
			}

		// Изменение списка ККТ
		private void OnlyNewCodes_CheckedChanged (object sender, EventArgs e)
			{
			KKTListForCodes.Items.Clear ();
			KKTListForCodes.Items.AddRange (kkmc.GetKKTTypeNames (OnlyNewCodes.Checked).ToArray ());
			KKTListForCodes.SelectedIndex = 0;
			}

		private void OnlyNewErrors_CheckedChanged (object sender, EventArgs e)
			{
			KKTListForErrors.Items.Clear ();
			KKTListForErrors.Items.AddRange (kkme.GetKKTTypeNames (OnlyNewErrors.Checked).ToArray ());
			KKTListForErrors.SelectedIndex = 0;
			}

		// Ввод номера ФН в разделе срока жизни
		private void FNLifeSN_TextChanged (object sender, EventArgs e)
			{
			// Получение описания
			if (FNLifeSN.Text != "")
				FNLifeName.Text = KKTSupport.GetFNName (FNLifeSN.Text);
			else
				FNLifeName.Text = "(введите ЗН ФН)";

			// Определение длины ключа
			if (FNLifeName.Text.Contains ("(13)") || FNLifeName.Text.Contains ("(15)"))
				{
				FNLife36.Enabled = false;
				FNLife13.Checked = true;
				}
			else
				{
				FNLife36.Enabled = true;
				}

			if (FNLifeName.Text.Contains ("(36)"))
				{
				FNLife13.Enabled = false;
				FNLife36.Checked = true;
				}
			else
				{
				FNLife13.Enabled = true;
				}

			// Принудительное изменение
			FNLifeStartDate_ValueChanged (null, null);
			}

		// Изменение параметров, влияющих на срок жизни ФН
		private void FNLifeStartDate_ValueChanged (object sender, EventArgs e)
			{
			string res = KKTSupport.GetFNLifeEndDate (FNLifeStartDate.Value, FNLife13.Checked,
				FNLifeName.Text.Contains ("(13)"), GenericTaxFlag.Checked, GoodsFlag.Checked,
				SeasonFlag.Checked || AgentsFlag.Checked, ExciseFlag.Checked, AutonomousFlag.Checked);

			if (res == "")
				{
				FNLifeResult.ForeColor = Color.FromArgb (255, 0, 0);
				FNLifeResult.Text = "Выбранная модель ФН неприменима к указанным параметрам пользователя";
				}
			else
				{
				FNLifeResult.ForeColor = Color.FromArgb (0, 0, 0);
				FNLifeResult.Text = "ФН прекратит работу " + res;
				}
			}

		// Ввод номеров в разделе РНМ
		private void RNMSerial_TextChanged (object sender, EventArgs e)
			{
			// Заводской номер ККТ
			if (RNMSerial.Text != "")
				RNMSerialResult.Text = KKTSupport.GetKKTModel (RNMSerial.Text);
			else
				RNMSerialResult.Text = "(введите ЗН ККТ)";

			// ИНН пользователя
			if (RNMUserINN.Text.Length < 10)
				{
				RNMUserINNResult.ForeColor = Color.FromArgb (0, 0, 0);
				RNMUserINNResult.Text = "???";
				}
			else if (KKTSupport.CheckINN (RNMUserINN.Text))
				{
				RNMUserINNResult.ForeColor = Color.FromArgb (0, 128, 0);
				RNMUserINNResult.Text = "OK";
				}
			else
				{
				RNMUserINNResult.ForeColor = Color.FromArgb (255, 0, 0);
				RNMUserINNResult.Text = "XXX";
				}

			// РНМ
			if (RNMValue.Text.Length < 10)
				{
				RNMValueResult.ForeColor = Color.FromArgb (0, 0, 0);
				RNMValueResult.Text = "???";
				}
			else if (KKTSupport.GetFullRNM (RNMUserINN.Text, RNMSerial.Text, RNMValue.Text.Substring (0, 10)) == RNMValue.Text)
				{
				RNMValueResult.ForeColor = Color.FromArgb (0, 128, 0);
				RNMValueResult.Text = "OK";
				}
			else
				{
				RNMValueResult.ForeColor = Color.FromArgb (255, 0, 0);
				RNMValueResult.Text = "XXX";
				}
			}

		// Генерация регистрационного номера
		private void RNMGenerate_Click (object sender, EventArgs e)
			{
			if (RNMValue.Text.Length < 1)
				RNMValue.Text = KKTSupport.GetFullRNM (RNMUserINN.Text, RNMSerial.Text, "0");
			else if (RNMValue.Text.Length < 10)
				RNMValue.Text = KKTSupport.GetFullRNM (RNMUserINN.Text, RNMSerial.Text, RNMValue.Text);
			else
				RNMValue.Text = KKTSupport.GetFullRNM (RNMUserINN.Text, RNMSerial.Text, RNMValue.Text.Substring (0, 10));
			}

		// Выбор имени ОФД
		private void OFDNamesList_SelectedIndexChanged (object sender, EventArgs e)
			{
			string s = ofd.GetOFDINNByName (OFDNamesList.Text);
			if (s != "")
				OFDINN.Text = s;
			}

		// Выбор ОФД
		private void OFDINN_TextChanged (object sender, EventArgs e)
			{
			List<string> parameters = ofd.GetOFDParameters (OFDINN.Text);

			OFDNamesList.Text = parameters[1];
			OFDDNSName.Text = parameters[2];
			OFDIP.Text = parameters[3];
			OFDPort.Text = parameters[4];
			OFDEmail.Text = parameters[5];
			OFDSite.Text = parameters[6];
			}

		// Копирование в буфер обмена
		private void OFDDNSName_Click (object sender, EventArgs e)
			{
			Clipboard.SetData (DataFormats.Text, ((Button)sender).Text);
			}
		}
	}