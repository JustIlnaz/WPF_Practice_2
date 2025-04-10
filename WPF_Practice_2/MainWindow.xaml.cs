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
            // Получение данных из интерфейса   
            string name = NameTextBox.Text.Trim();
            DateTime? birthDate = BirthDatePicker.SelectedDate;
            ComboBoxItem languageItem = LanguageComboBox.SelectedItem as ComboBoxItem;
            string language = languageItem?.Content.ToString();

            // Проверка ввода
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
            if (language == null)
            {
                MessageBox.Show("Выберите язык!");
                return;
            }

            // Генерация ЛОГИНА
            string login = GenerateLogin(name, birthDate.Value, language);

            // Генерация ПАРОЛЯ
            string password = GeneratePassword();

            // Вывод результата
            LoginTextBlock.Text = $"ЛОГИН: {login}";
            PasswordTextBlock.Text = $"ПАРОЛЬ: {password}";
        }

        private string GenerateLogin(string name, DateTime birthDate, string language)
        {
            // Определение алфавита в зависимости от языка
            string alphabet = language == "Русский"
                ? "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
                : "abcdefghijklmnopqrstuvwxyz";

            // Преобразование имени в числовую последовательность
            string numericName = string.Concat(name.ToLower().Select(c =>
            {
                int index = alphabet.IndexOf(c);
                return index >= 0 ? (index + 1).ToString() : "";
            }));

            // Сумма цифр даты рождения
            int dateSum = birthDate.Day / 10 + birthDate.Day % 10 +
                          birthDate.Month / 10 + birthDate.Month % 10 +
                          birthDate.Year.ToString().Sum(c => c - '0');

            // Формирование ЛОГИНА
            return numericName + dateSum;
        }

        private string GeneratePassword()
        {
            Random random = new Random();
            string password = "";
            string digits = "0123456789";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string upperLetters = letters.ToUpper();

            // Добавление заглавной буквы
            password += upperLetters[random.Next(upperLetters.Length)];

            // Добавление случайных символов
            for (int i = 1; i < 10; i++)
            {
                if (password.Count(char.IsDigit) < 5 && random.Next(2) == 0)
                {
                    char digit = digits[random.Next(digits.Length)];
                    if (password.LastOrDefault() != digit)
                        password += digit;
                }
                else
                {
                    char letter = letters[random.Next(letters.Length)];
                    password += letter;
                }
            }

            // Добавление специального символа
            password = password.Insert(random.Next(password.Length), specialSymbols[random.Next(specialSymbols.Length)]);

            return password;
        }


    }
}
