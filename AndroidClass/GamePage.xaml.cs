using System; // Подключение пространства имен System для использования базовых классов и структур данных
using Xamarin.Forms; // Подключение пространства имен Xamarin.Forms для создания мобильного пользовательского интерфейса

namespace AndroidClass // Определение пространства имен AndroidClass
{
    public partial class GamePage : ContentPage // Определение класса GamePage, наследующего ContentPage
    {
        private const int Rows = 5; // Количество строк в игровой сетке
        private const int Columns = 5; // Количество столбцов в игровой сетке
        private const int MineCount = 4; // Количество мин на игровой сетке

        private bool isGameRunning; // Переменная, указывающая на текущее состояние игры (запущена или нет)
        private int[,] mines; // Массив для хранения информации о расположении мин на игровой сетке
        private Button[,] buttons; // Двумерный массив кнопок, представляющий игровую сетку

        public GamePage() // Конструктор класса GamePage
        {
            InitializeComponent(); // Инициализация компонентов страницы
            InitializeGame(); // Вызов метода инициализации игры при создании страницы
        }

        protected override void OnAppearing() // Переопределение метода OnAppearing
        {
            base.OnAppearing(); // Вызов базовой реализации метода OnAppearing
            StartGame(); // Вызов метода начала игры при отображении страницы
        }

        private void InitializeGame() // Метод инициализации игры
        {
            isGameRunning = false; // Игра не запущена при инициализации
            statusLabel.Text = "Tap to start"; // Установка текста для статусной метки
            CreateGameGrid(); // Создание игровой сетки
        }

        private void CreateGameGrid() // Метод создания игровой сетки
        {
            mines = new int[Rows, Columns]; // Инициализация массива для мин
            buttons = new Button[Rows, Columns]; // Инициализация двумерного массива для кнопок игровой доски

            // Инициализация игровой доски
            for (int row = 0; row < Rows; row++) // Цикл по строкам
            {
                for (int col = 0; col < Columns; col++) // Цикл по столбцам
                {
                    Button button = new Button // Создание новой кнопки
                    {
                        Text = "", // Пустой текст на кнопке
                        BackgroundColor = Color.DarkGray, // Цвет для ненажатых плиток
                        FontSize = 20, // Размер шрифта текста на кнопке
                        HeightRequest = 50, // Высота кнопки
                        WidthRequest = 50, // Ширина кнопки
                        BorderColor = Color.Black, // Цвет границы кнопки
                        BorderWidth = 1, // Толщина границы кнопки
                        CommandParameter = new Tuple<int, int>(row, col) // Параметр команды, который содержит позицию кнопки в двумерном массиве
                    };
                    button.Clicked += OnCellClicked; // Подписка на событие нажатия кнопки

                    buttons[row, col] = button; // Сохранение кнопки в массиве кнопок
                    gameGrid.Children.Add(button, col, row); // Добавление кнопки на игровую сетку
                }
            }

            // Размещение мин на доске
            Random random = new Random(); // Создание объекта для генерации случайных чисел
            for (int i = 0; i < MineCount; i++) // Цикл по количеству мин
            {
                int row, col;
                do
                {
                    row = random.Next(0, Rows); // Получение случайного числа для строки
                    col = random.Next(0, Columns); // Получение случайного числа для столбца
                } while (mines[row, col] == -1); // Проверка, чтобы в одной клетке не было размещено несколько мин

                mines[row, col] = -1; // -1 представляет мину
            }
        }

        private void StartGame() // Метод начала игры
        {
            isGameRunning = true; // Игра запущена
            statusLabel.Text = ""; // Очистка статусной метки
            ResetGameGrid(); // Сброс игровой сетки
        }

        private void ResetGameGrid() // Метод сброса игровой сетки
        {
            // Очистка и повторная инициализация игровой доски
            foreach (Button button in buttons) // Цикл по всем кнопкам в массиве
            {
                button.Text = ""; // Очистка текста кнопки
                button.IsEnabled = true; // Включение кнопки
                button.BackgroundColor = Color.DarkGray; // Цвет для ненажатых плиток
            }
        }

        private void OnCellClicked(object sender, EventArgs e) // Обработчик события нажатия на кнопку
        {
            if (!isGameRunning) return; // Если игра не запущена, выход

            Button button = (Button)sender; // Получение кнопки, на которую нажал игрок
            Tuple<int, int> position = (Tuple<int, int>)button.CommandParameter; // Получение позиции кнопки в массиве кнопок
            int row = position.Item1;
            int col = position.Item2;

            if (mines[row, col] == -1) // Если игрок нашел мину
            {
                // Игрок нашел мину, игра завершается
                GameOver("Game Over! You hit a mine.");
            }
            else
            {
                // Показываем количество мин вокруг
                int mineCount = CountAdjacentMines(row, col); // Подсчет количества мин вокруг текущей клетки
                button.Text = mineCount > 0 ? mineCount.ToString() : ""; // Установка текста кнопки
                button.IsEnabled = false; // Отключение кнопки
                                          // Установка цвета фона кнопки в зависимости от наличия мины
                button.BackgroundColor = mines[row, col] == -1 ? Color.Red : Color.LightSteelBlue;
                if (mineCount == 0)
                {
                    button.BackgroundColor = Color.LightSteelBlue; // Цвет для пустых клеток
                }

                // Если игрок открыл все ячейки без мин, он побеждает
                if (CheckForVictory())
                {
                    GameOver("Congratulations! You won!");
                }
                else if (mineCount == 0)
                {
                    // Если нажата пустая клетка, открываем все смежные пустые клетки
                    OpenEmptyCells(row, col);
                }
            }
        }

        private void OpenEmptyCells(int row, int col)
        {
            // Открываем все смежные пустые клетки и их смежные клетки и т.д.
            for (int i = Math.Max(0, row - 1); i <= Math.Min(Rows - 1, row + 1); i++)
            {
                for (int j = Math.Max(0, col - 1); j <= Math.Min(Columns - 1, col + 1); j++)
                {
                    if (mines[i, j] != -1 && buttons[i, j].IsEnabled)
                    {
                        // Если клетка не содержит мину и она еще не открыта
                        buttons[i, j].Text = CountAdjacentMines(i, j) > 0 ? CountAdjacentMines(i, j).ToString() : "";
                        buttons[i, j].IsEnabled = false;
                        buttons[i, j].BackgroundColor = Color.LightSteelBlue; // Цвет для нажатых плиток

                        if (CountAdjacentMines(i, j) == 0)
                        {
                            // Если открытая клетка пуста, рекурсивно открываем ее соседей
                            OpenEmptyCells(i, j);
                        }
                    }
                }
            }
        }

        private int CountAdjacentMines(int row, int col)
        {
            int mineCount = 0;

            for (int i = Math.Max(0, row - 1); i <= Math.Min(Rows - 1, row + 1); i++)
            {
                for (int j = Math.Max(0, col - 1); j <= Math.Min(Columns - 1, col + 1); j++)
                {
                    if (mines[i, j] == -1)
                    {
                        mineCount++;
                    }
                }
            }

            return mineCount;
        }

        private bool CheckForVictory()
        {
            foreach (Button button in buttons)
            {
                Tuple<int, int> position = (Tuple<int, int>)button.CommandParameter;
                int row = position.Item1;
                int col = position.Item2;

                if (mines[row, col] != -1 && button.IsEnabled)
                {
                    // Есть нераскрытая ячейка без мины
                    return false;
                }
            }

            return true; // Все нераскрытые ячейки - мины
        }

        private void GameOver(string message)
        {
            isGameRunning = false; // Игра завершена
            statusLabel.Text = message; // Отображение сообщения о завершении игры

            // Отображение всех мин при проигрыше
            foreach (Button button in buttons)
            {
                Tuple<int, int> position = (Tuple<int, int>)button.CommandParameter;
                int row = position.Item1;
                int col = position.Item2;

                if (mines[row, col] == -1)
                {
                    button.BackgroundColor = Color.White; // Цвет для мин
                    button.Text = "💣"; // Иконка мин
                }
            }

            // Добавляем кнопку для начала новой игры
            Button restartButton = new Button
            {
                Text = "Restart",
                BackgroundColor = Color.Gray,
                TextColor = Color.White,
                Margin = new Thickness(10),
                VerticalOptions = LayoutOptions.End
            };
            restartButton.Clicked += (sender, args) => StartGame(); // Подписка на событие нажатия кнопки рестарта игры

            gameGrid.Children.Add(restartButton, 0, Rows, Rows, Rows + 1); // Добавление кнопки на игровую сетку
        }
    }
}
















