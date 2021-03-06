﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает главную форму программы
	/// </summary>
	public partial class TextToKKTForm: Form
		{
		// Переменные
		private KKTCodes kkmc = null;
		private KKTErrorsList kkme = null;
		private OFD ofd = null;
		private LowLevel ll = null;
		private UserManuals um = null;
		private ConfigAccessor ca = null;
		private KKTSupport.FNLifeFlags fnlf;

		/// <summary>
		/// Конструктор. Запускает главную форму
		/// </summary>
		public TextToKKTForm ()
			{
			// Инициализация
			InitializeComponent ();
			ca = new ConfigAccessor (this.Width, this.Height);

			// Загрузка списка кодов и ошибок
			kkmc = new KKTCodes ();
			kkme = new KKTErrorsList ();
			ofd = new OFD ();
			ll = new LowLevel ();
			um = new UserManuals (ca.ExtendedFunctions);

			// Настройка контролов
			OnlyNewCodes_CheckedChanged (null, null);
			OnlyNewErrors_CheckedChanged (null, null);
			LowLevelCommandATOL_CheckedChanged (null, null);

			FNLifeStartDate.Value = DateTime.Now;

			OFDNamesList.Items.Add ("Неизвестный ОФД");
			OFDNamesList.Items.AddRange (ofd.GetOFDNames ().ToArray ());
			OFDNamesList.SelectedIndex = 0;

			this.Text = ProgramDescription.AssemblyTitle;

			// Получение настроек
			this.Left = ca.WindowLeft;
			this.Top = ca.WindowTop;

			KeepAppState.Checked = ca.KeepApplicationState;

			MainTabControl.SelectedIndex = (int)ca.CurrentTab;

			OnlyNewErrors.Checked = ca.OnlyNewKKTErrors;
			OnlyNewErrors.Enabled = ca.AllowExtendedFunctionsLevel2;
			KKTListForErrors.SelectedIndex = (int)ca.KKTForErrors;
			ErrorCodesList.SelectedIndex = (int)ca.ErrorCode;

			FNLifeSN.Text = ca.FNSerial;
			if (ca.GenericTaxFlag)
				GenericTaxFlag.Checked = true;
			else
				OtherTaxFlag.Checked = true;
			if (ca.GoodsFlag)
				GoodsFlag.Checked = true;
			else
				ServicesFlag.Checked = true;
			SeasonFlag.Checked = ca.SeasonFlag;
			AgentsFlag.Checked = ca.AgentsFlag;
			ExciseFlag.Checked = ca.ExciseFlag;
			AutonomousFlag.Checked = ca.AutonomousFlag;
			FNLifeDeFacto.Checked = ca.FNLifeDeFacto;
			FNLifeDeFacto.Enabled = ca.AllowExtendedFunctionsLevel2;

			RNMSerial.Text = ca.KKTSerial;
			RNMUserINN.Text = ca.UserINN;
			RNMValue.Text = ca.RNMKKT;
			RNMSerial_TextChanged (null, null); // Для протяжки пустых полей

			OFDINN.Text = ca.OFDINN;

			if (ca.LowLevelCommandsATOL)
				LowLevelCommandATOL.Checked = true;
			else
				LowLevelCommandSHTRIH.Checked = true;
			LowLevelCommand.SelectedIndex = (int)ca.LowLevelCode;

			OnlyNewCodes.Checked = ca.OnlyNewKKTCodes;
			OnlyNewCodes.Enabled = ca.AllowExtendedFunctionsLevel2;
			KKTListForCodes.SelectedIndex = (int)ca.KKTForCodes;
			TextToConvert.Text = ca.CodesText;

			KKTListForManuals.Items.AddRange (um.GetKKTList ().ToArray ());
			KKTListForManuals.SelectedIndex = (int)ca.KKTForManuals;
			OperationsListForManuals.Items.AddRange (um.OperationTypes);
			try
				{
				OperationsListForManuals.SelectedIndex = (int)ca.OperationForManuals;
				}
			catch
				{
				OperationsListForManuals.SelectedIndex = 0;
				}

			// Блокировка расширенных функций при необходимости
			RNMGenerate.Visible = LowLevelTab.Enabled = ca.AllowExtendedFunctionsLevel2;
			CodesTab.Enabled = ca.AllowExtendedFunctionsLevel1;

			RNMTip.Text = "Индикатор ФФД: красный – поддержка не планируется; зелёный – поддерживается; " +
				"жёлтый – планируется; синий – нет сведений\n(на момент релиза этой версии приложения)";
			if (ca.AllowExtendedFunctionsLevel2)
				RNMTip.Text += ("\n\nПервые 10 цифр РН являются порядковым номером ККТ в реестре и могут быть указаны " +
					"вручную при генерации");

			if (!ca.AllowExtendedFunctionsLevel2)
				RNMLabel.Text = "Укажите регистрационный номер для проверки:";

			if (!ca.AllowExtendedFunctionsLevel2)
				{
				UnlockField.Visible = UnlockLabel.Visible = true;
				UnlockLabel.Text = ca.LockMessage;
				}
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

		// Выбор ККТ
		private void KKTListForCodes_SelectedIndexChanged (object sender, EventArgs e)
			{
			TextToConvert_TextChanged (null, null);
			}

		// Отображение справки
		private void BHelp_Clicked (object sender, EventArgs e)
			{
			ProgramDescription.ShowAbout (false);
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

		// Дополнительные функции
		private void GetFNReader_Click (object sender, EventArgs e)
			{
			try
				{
				Process.Start ("mailto://adslbarxatov@gmail.com" + ("?subject=Request for FNReader").Replace (" ", "%20"));
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
			fnlf.FN15 = FNLife13.Checked;
			fnlf.FNExactly13 = FNLifeName.Text.Contains ("(13)");
			fnlf.GenericTax = GenericTaxFlag.Checked;
			fnlf.Goods = GoodsFlag.Checked;
			fnlf.SeasonOrAgents = SeasonFlag.Checked || AgentsFlag.Checked;
			fnlf.Excise = ExciseFlag.Checked;
			fnlf.Autonomous = AutonomousFlag.Checked;
			fnlf.DeFacto = FNLifeDeFacto.Checked;

			string res = KKTSupport.GetFNLifeEndDate (FNLifeStartDate.Value, fnlf);

			FNLifeResult.Text = "ФН прекратит работу ";
			if (res.Contains ("!"))
				{
				FNLifeResult.ForeColor = Color.FromArgb (255, 0, 0);
				fnLifeResult = res.Substring (1);
				FNLifeResult.Text += (fnLifeResult + "\n(выбранный ФН неприменим с указанными параметрами)");
				}
			else
				{
				FNLifeResult.ForeColor = Color.FromArgb (0, 0, 0);
				fnLifeResult = res;
				FNLifeResult.Text += res;
				}

			if (!(FNLife13.Enabled && FNLife36.Enabled) && // Признак корректно заданного ЗН ФН
				!KKTSupport.IsFNCompatibleWithFFD12 (FNLifeSN.Text))
				{
				FNLifeResult.Text += "\n(выбранный ФН должен быть зарегистрирован до 6.08.21)";
				FNLifeResult.ForeColor = Color.FromArgb (255, 0, 0);
				}
			}

		// Копирование срока действия ФН
		private string fnLifeResult = "";
		private void FNLifeResult_Click (object sender, EventArgs e)
			{
			SendToClipboard (fnLifeResult);
			}

		// Ввод номеров в разделе РН
		private void RNMSerial_TextChanged (object sender, EventArgs e)
			{
			// Заводской номер ККТ
			if (RNMSerial.Text != "")
				{
				RNMSerialResult.Text = KKTSupport.GetKKTModel (RNMSerial.Text);

				KKTSupport.FFDSupportStatuses[] statuses = KKTSupport.GetFFDSupportStatus (RNMSerial.Text);
				RNMSupport105.BackColor = StatusToColor (statuses[0]);
				RNMSupport11.BackColor = StatusToColor (statuses[1]);
				RNMSupport12.BackColor = StatusToColor (statuses[2]);
				}
			else
				{
				RNMSerialResult.Text = "(введите ЗН ККТ)";
				RNMSupport105.BackColor = RNMSupport11.BackColor = RNMSupport12.BackColor =
					StatusToColor (KKTSupport.FFDSupportStatuses.Unknown);
				}

			// ИНН пользователя
			RegionLabel.Text = "";
			int checkINN = KKTSupport.CheckINN (RNMUserINN.Text);
			if (checkINN < 0)
				RNMUserINN.BackColor = StatusToColor (KKTSupport.FFDSupportStatuses.Unknown);
			else if (checkINN == 0)
				RNMUserINN.BackColor = StatusToColor (KKTSupport.FFDSupportStatuses.Supported);
			else
				RNMUserINN.BackColor = StatusToColor (KKTSupport.FFDSupportStatuses.Planned);   // Не ошибка
			RegionLabel.Text = KKTSupport.GetRegionName (RNMUserINN.Text);

			// РН
			if (RNMValue.Text.Length < 10)
				RNMValue.BackColor = StatusToColor (KKTSupport.FFDSupportStatuses.Unknown);
			else if (KKTSupport.GetFullRNM (RNMUserINN.Text, RNMSerial.Text, RNMValue.Text.Substring (0, 10)) == RNMValue.Text)
				RNMValue.BackColor = StatusToColor (KKTSupport.FFDSupportStatuses.Supported);
			else
				RNMValue.BackColor = StatusToColor (KKTSupport.FFDSupportStatuses.Unsupported);
			}

		// Запрос цвета, соответствующего статусу поддержки
		private Color StatusToColor (KKTSupport.FFDSupportStatuses Status)
			{
			if (Status == KKTSupport.FFDSupportStatuses.Planned)
				return Color.FromArgb (255, 255, 200);

			if (Status == KKTSupport.FFDSupportStatuses.Supported)
				return Color.FromArgb (200, 255, 200);

			if (Status == KKTSupport.FFDSupportStatuses.Unsupported)
				return Color.FromArgb (255, 200, 200);

			// Остальные
			return Color.FromArgb (200, 200, 255);
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

			OFDNalogSite.Text = "www.nalog.ru";
			}

		// Копирование в буфер обмена
		private void OFDDNSName_Click (object sender, EventArgs e)
			{
			SendToClipboard (((Button)sender).Text);
			}

		private void SendToClipboard (string Text)
			{
			try
				{
				Clipboard.SetData (DataFormats.Text, Text);
				}
			catch { }
			}

		private void OFDNameCopy_Click (object sender, EventArgs e)
			{
			SendToClipboard (OFDNamesList.Text.Replace ('«', '\"').Replace ('»', '\"'));
			}

		private void OFDINNCopy_Click (object sender, EventArgs e)
			{
			SendToClipboard (OFDINN.Text);
			}

		// Выбор списка команд нижнего уровня
		private void LowLevelCommandATOL_CheckedChanged (object sender, EventArgs e)
			{
			LowLevelCommand.Items.Clear ();
			LowLevelCommand.Items.AddRange (ll.GetATOLCommandsList ().ToArray ());
			LowLevelCommand.SelectedIndex = 0;
			}

		private void LowLevelCommandSHTRIH_CheckedChanged (object sender, EventArgs e)
			{
			LowLevelCommand.Items.Clear ();
			LowLevelCommand.Items.AddRange (ll.GetSHTRIHCommandsList ().ToArray ());
			LowLevelCommand.SelectedIndex = 0;
			}

		// Выбор команды
		private void LowLevelCommand_SelectedIndexChanged (object sender, EventArgs e)
			{
			if (LowLevelCommandATOL.Checked)
				{
				LowLevelCommandCode.Text = ll.GetATOLCommand ((uint)LowLevelCommand.SelectedIndex, false);
				LowLevelCommandDescr.Text = ll.GetATOLCommand ((uint)LowLevelCommand.SelectedIndex, true);
				}
			else
				{
				LowLevelCommandCode.Text = ll.GetSHTRIHCommand ((uint)LowLevelCommand.SelectedIndex, false);
				LowLevelCommandDescr.Text = ll.GetSHTRIHCommand ((uint)LowLevelCommand.SelectedIndex, true);
				}
			}

		// Выбор модели аппарата
		private void KKTListForManuals_SelectedIndexChanged (object sender, EventArgs e)
			{
			UMOperationText.Text = um.GetManual ((uint)KKTListForManuals.SelectedIndex,
				(uint)OperationsListForManuals.SelectedIndex);
			}

		// Сохранение настроек приложения
		private void TextToKKMForm_FormClosing (object sender, FormClosingEventArgs e)
			{
			ca.WindowLeft = this.Left;
			ca.WindowTop = this.Top;

			ca.KeepApplicationState = KeepAppState.Checked;
			if (!ca.KeepApplicationState)
				return;

			ca.CurrentTab = (uint)MainTabControl.SelectedIndex;

			ca.OnlyNewKKTErrors = OnlyNewErrors.Checked;
			ca.KKTForErrors = (uint)KKTListForErrors.SelectedIndex;
			ca.ErrorCode = (uint)ErrorCodesList.SelectedIndex;

			ca.FNSerial = FNLifeSN.Text;
			ca.GenericTaxFlag = GenericTaxFlag.Checked;
			ca.GoodsFlag = GoodsFlag.Checked;
			ca.SeasonFlag = SeasonFlag.Checked;
			ca.AgentsFlag = AgentsFlag.Checked;
			ca.ExciseFlag = ExciseFlag.Checked;
			ca.AutonomousFlag = AutonomousFlag.Checked;
			ca.FNLifeDeFacto = FNLifeDeFacto.Checked;

			ca.KKTSerial = RNMSerial.Text;
			ca.UserINN = RNMUserINN.Text;
			ca.RNMKKT = RNMValue.Text;

			ca.OFDINN = OFDINN.Text;

			ca.LowLevelCommandsATOL = LowLevelCommandATOL.Checked;
			ca.LowLevelCode = (uint)LowLevelCommand.SelectedIndex;

			ca.OnlyNewKKTCodes = OnlyNewCodes.Checked;
			ca.KKTForCodes = (uint)KKTListForCodes.SelectedIndex;
			ca.CodesText = TextToConvert.Text;

			ca.KKTForManuals = (uint)KKTListForManuals.SelectedIndex;
			ca.OperationForManuals = (uint)OperationsListForManuals.SelectedIndex;
			}

		// Разблокировка функционала
		private void UnlockField_TextChanged (object sender, EventArgs e)
			{
			if (ca.TestPass (UnlockField.Text))
				{
				UnlockField.Enabled = false;
				UnlockLabel.Text = ConfigAccessor.UnlockMessage;
				UnlockLabel.TextAlign = ContentAlignment.MiddleCenter;
				}
			}

		// Поиск по тексту ошибки
		private int lastErrorSearchOffset = 0;
		private void ErrorFindButton_Click (object sender, EventArgs e)
			{
			List<string> codes = kkme.GetErrorCodesList ((uint)KKTListForErrors.SelectedIndex);

			for (int i = lastErrorSearchOffset; i < codes.Count; i++)
				if (codes[i].ToLower ().Contains (ErrorSearchText.Text.ToLower ()))
					{
					lastErrorSearchOffset = i + 1;
					ErrorCodesList.SelectedIndex = i;
					return;
					}

			for (int i = 0; i < lastErrorSearchOffset; i++)
				if (codes[i].ToLower ().Contains (ErrorSearchText.Text.ToLower ()))
					{
					lastErrorSearchOffset = i + 1;
					ErrorCodesList.SelectedIndex = i;
					return;
					}
			}

		private void ErrorSearchText_KeyDown (object sender, KeyEventArgs e)
			{
			if (e.KeyCode == Keys.Return)
				ErrorFindButton_Click (null, null);
			}

		// Поиск по тексту ошибки
		private int lastLowLevelSearchOffset = 0;
		private void LowLevelFindButton_Click (object sender, EventArgs e)
			{
			List<string> codes = LowLevelCommandATOL.Checked ? ll.GetATOLCommandsList () : ll.GetSHTRIHCommandsList ();

			for (int i = lastLowLevelSearchOffset; i < codes.Count; i++)
				if (codes[i].ToLower ().Contains (LowLevelSearchText.Text.ToLower ()))
					{
					lastLowLevelSearchOffset = i + 1;
					LowLevelCommand.SelectedIndex = i;
					return;
					}

			for (int i = 0; i < lastLowLevelSearchOffset; i++)
				if (codes[i].ToLower ().Contains (LowLevelSearchText.Text.ToLower ()))
					{
					lastLowLevelSearchOffset = i + 1;
					LowLevelCommand.SelectedIndex = i;
					return;
					}
			}

		private void LowLevelSearchText_KeyDown (object sender, KeyEventArgs e)
			{
			if (e.KeyCode == Keys.Return)
				LowLevelFindButton_Click (null, null);
			}

		// Поиск по названию ОФД
		private int lastOFDSearchOffset = 0;
		private void OFDFindButton_Click (object sender, EventArgs e)
			{
			List<string> codes = ofd.GetOFDNames ();

			for (int i = lastOFDSearchOffset; i < codes.Count; i++)
				if (codes[i].ToLower ().Contains (OFDSearchText.Text.ToLower ()))
					{
					lastOFDSearchOffset = i + 1;
					OFDNamesList.SelectedIndex = i + 1;
					return;
					}

			for (int i = 0; i < lastOFDSearchOffset; i++)
				if (codes[i].ToLower ().Contains (OFDSearchText.Text.ToLower ()))
					{
					lastOFDSearchOffset = i + 1;
					OFDNamesList.SelectedIndex = i + 1;
					return;
					}
			}

		private void OFDSearchText_KeyDown (object sender, KeyEventArgs e)
			{
			if (e.KeyCode == Keys.Return)
				OFDFindButton_Click (null, null);
			}

		// Очистка полей
		private void RNMSerialClear_Click (object sender, EventArgs e)
			{
			RNMSerial.Text = "";
			RNMUserINN.Text = "";
			RNMValue.Text = "";
			}

		private void OFDINNClear_Click (object sender, EventArgs e)
			{
			OFDINN.Text = "";
			}

		private void TextToConvertClear_Click (object sender, EventArgs e)
			{
			TextToConvert.Text = "";
			}

		private void FNLifeSNClear_Click (object sender, EventArgs e)
			{
			FNLifeSN.Text = "";
			}
		}
	}
