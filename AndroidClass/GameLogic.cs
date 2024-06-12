using System;
using Xamarin.Forms;

namespace AndroidClass
{
    public class GameLogic
    {
        private const int Rows = 5;
        private const int Columns = 5;
        private const int MineCount = 4;

        private bool isGameRunning;
        private int[,] mines;
        private Button[,] buttons;
        private Label statusLabel;

        public GameLogic(Label statusLabel)
        {
            this.statusLabel = statusLabel;
            InitializeGame();
        }

        public void InitializeGame()
        {
            isGameRunning = false;
            statusLabel.Text = "Tap to start";
            CreateGameGrid();
        }

        private void CreateGameGrid()
        {
            mines = new int[Rows, Columns];
            buttons = new Button[Rows, Columns];
        }

        public void InitializeButtons(Grid gameGrid)
        {
            gameGrid.Children.Clear();
            gameGrid.RowDefinitions.Clear();
            gameGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < Rows; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                for (int j = 0; j < Columns; j++)
                {
                    if (i == 0)
                    {
                        gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }

                    var button = new Button();
                    button.Clicked += OnButtonClicked;
                    buttons[i, j] = button;
                    gameGrid.Children.Add(button, j, i);
                }
            }
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (!isGameRunning)
            {
                StartGame();
            }

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Обработка нажатия на кнопку
            }
        }

        public void StartGame()
        {
            isGameRunning = true;
            statusLabel.Text = "Game started!";
            PlaceMines();
        }

        private void PlaceMines()
        {
            Random random = new Random();
            int minesPlaced = 0;
            while (minesPlaced < MineCount)
            {
                int row = random.Next(Rows);
                int col = random.Next(Columns);
                if (mines[row, col] == 0)
                {
                    mines[row, col] = 1; // 1 означает мину
                    minesPlaced++;
                }
            }
        }
    }
}

