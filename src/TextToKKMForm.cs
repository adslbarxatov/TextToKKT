using System;
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

			// Настройка контролов
			KKTListForCodes.Items.AddRange (kkmc.KKTTypeNames.ToArray ());
			KKTListForCodes.SelectedIndex = 0;

			KKTListForErrors.Items.AddRange (kkme.KKTTypeNames.ToArray ());
			KKTListForErrors.SelectedIndex = 0;

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

			DescriptionLabel.Text = kkmc.GetKKMTypeDescription ((uint)KKTListForCodes.SelectedIndex);
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
		private void MainForm_HelpButtonClicked (object sender, System.ComponentModel.CancelEventArgs e)
			{
			// Отмена обработки события вызова справки
			e.Cancel = true;

			// Отображение
			ProgramDescription.ShowAbout ();
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
		}
	}
