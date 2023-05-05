using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace PopUP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp_Page : ContentPage
    {
        private int questionIndex; //индекс текущего вопроса в тесте
        private int score; //количество правильных ответов в тесте
        private List<int> numbers; //список случайных чисел, которые будут использоваться в тесте
        private Random random; //экземпляр класса Random, используемый для генерации случайных чисел

        public PopUp_Page()
        {
            InitializeComponent();
            random = new Random();
        }

        private async void OnStartTestClicked(object sender, EventArgs e)
        {
            // метод, который вызывается при нажатии на кнопку "Start Test" и запускает тест, используя введенное имя и список случайных чисел
            var name = NameEntry.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Viga", "Palun sisesta oma nimi.", "OK");
                return;
            }

            
            questionIndex = 0;
            score = 0;
            numbers = new List<int>();

            
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(random.Next(1, 11));
            }

            
            await DisplayQuestion(); //метод, который отображает вопрос, ждет ответа пользователя и проверяет его, увеличивая счетчик правильных ответов и переходя к следующему вопрос
        }

        private async System.Threading.Tasks.Task DisplayQuestion()
        {
            // Получить первое число для вопроса
            var number1 = numbers[questionIndex];

            // Создать случайное второе число для вопроса
            var number2 = random.Next(1, 11);

            // Вычислите правильный ответ
            var answer = number1 * number2;

            // Показать вопрос
            var response = await DisplayPromptAsync($"Küsimus {questionIndex + 1}", $"Kui palju on {number1} x {number2}?", placeholder: "Vastus");

            await SaveDataToFileAsync("test.txt", $"{number1} x {number2} = {answer}\n");

            // Проверить ответ
            if (int.TryParse(response, out int userAnswer))
            {
                if (userAnswer == answer)
                {
                    score++;
                }
            }

            //Перейти к следующему вопросу или закончить тест
            if (questionIndex < 9)
            {
                questionIndex++;
                await DisplayQuestion();
            }
            else
            {
                var percentageScore = (double)score /10 * 100;

                var letterGrade = GetLetterGrade(percentageScore); // метод, который возвращает оценку (в виде строки), основываясь на проценте правильных ответов

                await DisplayAlert("Test lõpetatud", $"{NameEntry.Text}, sinu punktid {score} / 10. ({percentageScore}%)\nHinne: {letterGrade}", "OK");
            }
        }

        private string GetLetterGrade(double percentageScore)
        {
            if (percentageScore >= 90)
            {
                return "5";
            }
            else if(percentageScore >= 75)
            {
                return "4";
            }
            else if (percentageScore >= 50)
            {
                return "3";
            }
            else
            {
                return "2";
            }
        }

        private async void Button_Clicked(object sender, EventArgs e) // метод, который вызывается при нажатии на кнопку "Back" и переводит пользователя на предыдущую страницу
        {
            await Navigation.PopAsync();
        }

        async Task SaveDataToFileAsync(string fileName, string data) //метод, который сохраняет данные в файл на устройстве пользователя
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, fileName);

            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                await sw.WriteAsync(data).ConfigureAwait(false);
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e) //метод, который вызывается при нажатии на кнопку "Delete" и удаляет файл с устройства пользователя
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullPath = Path.Combine(path, "test.txt");

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    await DisplayAlert("Korras", $"Faili kustutamine õnnestus", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Viga", $"Viga faili kustutamisel: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Viga", "Faili ei leitud", "OK");
            }
        }
    }
}