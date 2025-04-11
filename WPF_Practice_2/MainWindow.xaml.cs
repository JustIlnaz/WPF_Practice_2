using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Practice_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string[] specialSymbols = { "-", "+", "_", "=", "\"", "[", "]", "@", "#", "$", "%", "^", "&", "?", "*", "(", ")" };
        public MainWindow()
        {
            InitializeComponent();
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {

            // Получаем данные из интерфейса
            string name = NameTextBox.Text.Trim();
            DateTime? birthDate = BirthDatePicker.SelectedDate;
            string language = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Проверяем, что все поля заполнены
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите имя!");
                return;
            }
            if (!birthDate.HasValue)
            {
                MessageBox.Show("Выберите дату рождения!");
                return;
            }
            if (string.IsNullOrEmpty(language))
            {
                MessageBox.Show("Выберите язык!");
                return;
            }

            //Проверка ввода
            bool hasRussianLetters = name.Any(c => c >= 'А' && c <= 'я'); // Кириллические символы
            bool isRussianLanguage = language == "Русский";

            // сравниваем язык ввода имени с выбранным языком
            if (hasRussianLetters && !isRussianLanguage)
            {
                MessageBox.Show("Имя написано на русском, но выбран английский язык!");
                return;
            }
            if (!hasRussianLetters && isRussianLanguage)
            {
                MessageBox.Show("Имя написано на английском, но выбран русский язык!");
                return;
            }



            // Генерируем логин и пароль
            string login = GenerateLogin(name, birthDate.Value, language);
            string password = GeneratePassword();

            // Выводим результат
            LoginTextBlock.Text = $"ЛОГИН: {login}";
            PasswordTextBlock.Text = $"ПАРОЛЬ: {password}";
        }

        private string GenerateLogin(string name, DateTime birthDate, string language)
        {
            // Определяем алфавит в зависимости от языка
            string alphabet = "";
            if (language == "Русский")
            {
                alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }

            // Преобразуем имя в числовую последовательность
            string numericName = "";
            foreach (char c in name.ToLower())
            {
                int index = alphabet.IndexOf(c);
                if (index >= 0)
                {
                    numericName += (index + 1).ToString();
                }
            }

            // Считаем сумму цифр из даты рождения
            int dateSum = 0;
            dateSum += birthDate.Day / 10 + birthDate.Day % 10;
            dateSum += birthDate.Month / 10 + birthDate.Month % 10;
            foreach (char c in birthDate.Year.ToString())
            {
                dateSum += c - '0';
            }

            // Возвращаем логин
            return numericName + dateSum;
        }

        private string GeneratePassword()
        {
            Random random = new Random();
            string digits = "0123456789";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string upperLetters = letters.ToUpper();

            // Начинаем с заглавной буквы
            string password = upperLetters[random.Next(upperLetters.Length)].ToString();

            // Добавляем случайные символы
            while (password.Length < 9)
            {
                if (password.Count(char.IsDigit) < 5 && random.Next(2) == 0)
                {
                    char digit = digits[random.Next(digits.Length)];
                    if (password.LastOrDefault() != digit)
                    {
                        password += digit;
                    }
                }
                else
                {
                    password += letters[random.Next(letters.Length)];
                }
            }

            // Добавляем специальный символ
            int position = random.Next(password.Length);
            password = password.Insert(position, specialSymbols[random.Next(specialSymbols.Length)]);

            return password;
        }


    }
}
