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
		private KKMCodes kkmc = null;

		/// <summary>
		/// Конструктор. Запускает главную форму
		/// </summary>
		public TextToKKMForm ()
			{
			// Функция, используемая для добавления новых моделей
			//TablesInterpreter.Interpretate ();

			// Инициализация
			InitializeComponent ();

			// Загрузка списка ККМ
			kkmc = new KKMCodes ();

			// Формирование списка ККМ
			KKMList.Items.AddRange (kkmc.KKMTypeNames);
			KKMList.SelectedIndex = 0;

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

			DescriptionLabel.Text = kkmc.GetKKMTypeDescription ((uint)KKMList.SelectedIndex);
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

				if ((s = kkmc.GetCode ((uint)KKMList.SelectedIndex, b[0])) == KKMCodes.EmptyCode)
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
		private void KKMList_SelectedIndexChanged (object sender, EventArgs e)
			{
			// Обработка делимитера
			if (KKMList.Text == "——————————")
				{
				if (KKMList.SelectedIndex == KKMList.Items.Count - 1)
					KKMList.SelectedIndex = 0;
				else
					KKMList.SelectedIndex++;

				return;
				}

			// Обработка текста
			TextToConvert_TextChanged (sender, e);
			}

		// Отображение справки
		private void MainForm_HelpButtonClicked (object sender, System.ComponentModel.CancelEventArgs e)
			{
			// Отмена обработки события вызова справки
			e.Cancel = true;

			// Отображение
			ProgramDescription.ShowAbout ();
			}
		}
	}
