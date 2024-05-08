using System; // Импорт пространства имен System - содержит базовые типы данных и основные классы .NET
using Xamarin.Forms; // Импорт пространства имен Xamarin.Forms - содержит классы для создания пользовательского интерфейса мобильных приложений

namespace AndroidClass
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); // Инициализация компонентов страницы
        }

        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            // Перейти на страницу с игрой (GamePage)
            Navigation.PushAsync(new GamePage()); // Переход на новую страницу (GamePage) при нажатии кнопки "Начать"
        }

        private void OnExitButtonClicked(object sender, EventArgs e)
        {
            // Закрыть приложение
            Environment.Exit(0); // Выход из приложения с кодом успешного завершения
        }

    }
}



