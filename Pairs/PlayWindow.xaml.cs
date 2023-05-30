using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Pairs
{
    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
        private Player player;
        private int counter, rows, columns, level, timer;
        private List<BitmapImage> doubledImages;
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private ObservableCollection<BitmapImage> images = new ObservableCollection<BitmapImage>
        {
            new BitmapImage(new Uri(@"/img/butterfly.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/camel.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/cat.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/chicken.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/crab.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/crocodile.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/dinosaur.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/easter-egg.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/kangaroo.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/koala.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/lion.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/monkey.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/mouse.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/owl.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/paintball.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/party-hat.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/pokeball.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/squid.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/traffic-light.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/unicorn.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/package.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/shark.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/rabbit.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/black-cat.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/dove.png", UriKind.Relative)),
            new BitmapImage(new Uri(@"/img/pet-food.png", UriKind.Relative))
        };

        public void Shuffle(ObservableCollection<BitmapImage> images)
        {
            Random random = new Random();
            int n = images.Count;
            while(n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                BitmapImage value = images[k];
                images[k] = images[n];
                images[n] = value;
            }
        }

        public void Shuffle2<T>(IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            while(n>1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public PlayWindow(Player player)
        {
            InitializeComponent();
            this.player = player;
            usernameTextBlock.Text = player.Name;
            usernameImage.Source = images[player.ImageIndex];
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timer--;
            seconds.Text = $"{timer}";
            if(timer == 0)
            {
                gameTimer.Stop();
                foreach(var child in myGrid.Children)
                {
                    if(child is Button button)
                    {
                        button.IsEnabled = false;
                    }
                }
                MessageBox.Show("You lost!");
            }
        }

        private void CreateButtonMatrix(int n, int m)
        {
            counter = 0;
            Shuffle(images);
            ObservableCollection<BitmapImage> randomImages = new ObservableCollection<BitmapImage>(images.Take((n * m) / 2));
            doubledImages = randomImages.Concat(randomImages).ToList();
            Shuffle2(doubledImages);
            myGrid.Children.Clear();
            myGrid.RowDefinitions.Clear();
            myGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < n + m; i++)
            {
                myGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            int j = 0;

            for(int row = 0; row < n; row++)
            {
                for(int col = 0; col < m; col++)
                {
                    BitmapImage image = doubledImages[j++];
                    Button button = new Button();
                    Image buttonImage = new Image() { Source = image };
                    buttonImage.Visibility = Visibility.Collapsed;
                    button.Content = buttonImage;
                    button.Click += ButtonShowClick;
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    myGrid.Children.Add(button);
                }
            }
        }

        private void ButtonShowClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Image image = button.Content as Image;
            button.IsEnabled = false;
            image.Visibility = Visibility.Visible;
            clickedButtons.Add(button);

            if (clickedButtons.Count == 2)
            {
                Button button1 = clickedButtons[0];
                Button button2 = clickedButtons[1];
                Image firstImage = clickedButtons[0].Content as Image;
                BitmapImage firstBitmapImage = firstImage.Source as BitmapImage;
                Image secondImage = clickedButtons[1].Content as Image;
                BitmapImage secondBitmapImage = secondImage.Source as BitmapImage;
                clickedButtons[0].IsEnabled = false;
                clickedButtons[1].IsEnabled = false;
                if (firstBitmapImage.UriSource == secondBitmapImage.UriSource)
                {
                    pairedButtons.Add(clickedButtons[0]);
                    pairedButtons.Add(clickedButtons[1]);
                    Task.Delay(750).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            button1.Visibility = Visibility.Collapsed;
                            button2.Visibility = Visibility.Collapsed;
                        });
                    });
                    counter += 2;
                }
                else
                {
                    Task.Delay(750).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            firstImage.Visibility = Visibility.Collapsed;
                            secondImage.Visibility = Visibility.Collapsed;
                        });
                    });
                }
                clickedButtons[0].IsEnabled = true;
                clickedButtons[1].IsEnabled = true;
                clickedButtons.Clear();
            }

            if (counter == doubledImages.Count)
            {
                if (level == 3)
                {
                    Task.Delay(500).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            XDocument xmlDoc = XDocument.Load("UserData.xml");
                            XElement currentPlayerElement = xmlDoc.Descendants("Player").Where(p => (string)p.Element("username") == player.Name).FirstOrDefault();
                            if (currentPlayerElement != null)
                            {
                                int wonGames = int.Parse(currentPlayerElement.Element("wonGames").Value);
                                wonGames++;
                                currentPlayerElement.Element("wonGames").Value = wonGames.ToString();
                                xmlDoc.Save("UserData.xml");
                            }
                            MessageBox.Show("Congratulations!", "", MessageBoxButton.OK);
                            gameTimer.Stop();
                        });
                    });
                }
                if (level == 2)
                {
                    Task.Delay(1250).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            CreateButtonMatrix(rows, columns);
                            levelTextBlock.Text = "Level 3";
                            level = 3;
                        });
                    });
                }
                if (level == 1)
                {
                    Task.Delay(1250).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            CreateButtonMatrix(rows, columns);
                            levelTextBlock.Text = "Level 2";
                            level = 2;
                        });
                    });
                }
            }
        }

        private List<Button> clickedButtons = new List<Button>();
        private List<Button> pairedButtons = new List<Button>();

        private void Exit_Click(object sender, RoutedEventArgs e) // Exit Click
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void About_Click(object sender, RoutedEventArgs e) // About Click
        {
            MessageBox.Show("Matei Raluca 312 IA", "About", MessageBoxButton.OK);
        }

        private void OnCustomCheck(object sender, RoutedEventArgs e) // Custom Click
        {
            StandardMenuItem.IsChecked = false;
            CustomMenuItem.IsChecked = true;
            CustomWindow customWindow = new CustomWindow();
            customWindow.DataEntered += CustomWindow_DataEntered;
            customWindow.Show();
        }

        private void CustomWindow_DataEntered(object sender, DataEnteredEvent e)
        {
            rows = e.rows;
            columns = e.columns;
        }

        private void OnStandardCheck(object sender, RoutedEventArgs e) // Standard Click
        {
            CustomMenuItem.IsChecked = false;
            StandardMenuItem.IsChecked = true;
            rows = 4;
            columns = 5;
        }

        private void StatisticsButton(object sender, RoutedEventArgs e) // Statistics Button
        {
            Statistics statisticsWindow = new Statistics(player);
            statisticsWindow.Show();
        }

        private void NewGameButton(object sender, RoutedEventArgs e) // New Game Button
        {
            timer = rows * columns * 3 * 3;
            gameTimer.Start();
            XDocument xmlDoc = XDocument.Load("UserData.xml");
            XElement currentPlayerElement = xmlDoc.Descendants("Player").Where(p => (string)p.Element("username") == player.Name).FirstOrDefault();
            if (currentPlayerElement != null)
            {
                int playedGames = int.Parse(currentPlayerElement.Element("playedGames").Value);
                playedGames++;
                currentPlayerElement.Element("playedGames").Value = playedGames.ToString();
                xmlDoc.Save("UserData.xml");
            }
            myGrid.Visibility = Visibility.Visible;
            levelTextBlock.Text = "Level 1";
            CreateButtonMatrix(rows, columns);
            level = 1;
            counter = 0;
            seconds.Text = timer.ToString();
        }

        private void SaveGameButton(object sender, RoutedEventArgs e) 
        {
            XDocument doc = new XDocument();

            
            XElement gameState = new XElement("GameState");

            
            gameState.Add(new XAttribute("PlayerName", player.Name));
            gameState.Add(new XAttribute("PlayerImageIndex", player.ImageIndex));
            gameState.Add(new XAttribute("Counter", counter));
            gameState.Add(new XAttribute("Rows", rows));
            gameState.Add(new XAttribute("Columns", columns));
            gameState.Add(new XAttribute("Level", level));
            gameState.Add(new XAttribute("Timer", timer));

            List<string> imagesXmlList = new List<string>();
            foreach (Button button in pairedButtons)
            {
                Image image = button.Content as Image;
                BitmapImage bitmapImage = image.Source as BitmapImage;
                string imageXml = XamlWriter.Save(image);
                imagesXmlList.Add(imageXml);
            }

            List<string> pairedButtonsXmlList = new List<string>();
            foreach (Button button in pairedButtons)
            {
                string pairedButtonXml = XamlWriter.Save(button);
                pairedButtonsXmlList.Add(pairedButtonXml);
            }

            string imagesXml = string.Join(",", imagesXmlList);
            string pairedButtonsXml = string.Join(",", pairedButtonsXmlList);

            
            gameState.Add(new XElement("Images", imagesXml));
            gameState.Add(new XElement("ClickedButtons", pairedButtonsXml));

            string path = Path.Combine(Environment.CurrentDirectory, "gameData", "GameState.xml");
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                gameState.Save(writer);
            }
            MessageBox.Show("Game saved successfully.");
        }

        private void OpenGameButton(object sender, RoutedEventArgs e) 
        {
            string path = Path.Combine(Environment.CurrentDirectory, "gameData", "GameState.xml");
            if (!File.Exists(path))
            {
                MessageBox.Show("No saved game found.");
                return;
            }

            XDocument doc = XDocument.Load(path);
            XElement gameState = doc.Element("GameState");
            if (gameState == null)
            {
                MessageBox.Show("Invalid game state format.");
                return;
            }

            string playerName = gameState.Attribute("PlayerName")?.Value;
            int playerImageIndex = int.Parse(gameState.Attribute("PlayerImageIndex")?.Value ?? "0");
            player = new Player(playerName, playerImageIndex);
            counter = int.Parse(gameState.Attribute("Counter")?.Value ?? "0");
            rows = int.Parse(gameState.Attribute("Rows")?.Value ?? "0");
            columns = int.Parse(gameState.Attribute("Columns")?.Value ?? "0");
            level = int.Parse(gameState.Attribute("Level")?.Value ?? "0");
            timer = int.Parse(gameState.Attribute("Timer")?.Value ?? "0");
            levelTextBlock.Text = $"Level {level}";
            seconds.Text = timer.ToString();

            myGrid.Children.Clear();
            CreateButtonMatrix(rows, columns);

            IEnumerable<XElement> imagesXml = gameState.Element("Images").Elements("Image");
            IEnumerable<XElement> pairedButtonsXml = gameState.Element("ClickedButtons").Elements("Button");
            for (int i = 0; i < pairedButtonsXml.Count(); i++)
            {
                StringReader stringReader = new StringReader(pairedButtonsXml.ElementAt(i).ToString());
                XmlReader xmlReader = XmlReader.Create(stringReader);
                Button button = (Button)XamlReader.Load(xmlReader);

                StringReader stringReader2 = new StringReader(imagesXml.ElementAt(i).ToString());
                XmlReader xmlReader2 = XmlReader.Create(stringReader);
                Image image = (Image)XamlReader.Load(xmlReader);

                button.Content = image;
                pairedButtons.Add(button);
                button.IsEnabled = false;
                button.Opacity = 0.5;
            }

            MessageBox.Show("Game loaded successfully.");
        }
    }
}
