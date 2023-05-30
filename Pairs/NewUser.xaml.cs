using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Pairs
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        ObservableCollection<BitmapImage> AvatarImages { get; set; }
        ObservableCollection<Player> _players;

        int currentIndex = 0;

        public NewUser(ObservableCollection<Player> playersList, ObservableCollection<BitmapImage> avatarsList)
        {
            InitializeComponent();
            _players = playersList;
            AvatarImages = avatarsList;
            avatarImage.Source = AvatarImages[currentIndex];
        }

        public void UpdateAvatarList(ObservableCollection<BitmapImage> updatedList)
        {
            AvatarImages = updatedList;
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Sign up
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"UserData.xml");
            bool isOk = true;
            XmlNodeList playerNodes = xmlDoc.SelectNodes("//Player");
            foreach(XmlNode playerNode in playerNodes)
            {
                XmlNode usernameNode = playerNode.SelectSingleNode("username");
                if(usernameNode.InnerText == usernameBox.Text)
                {
                    isOk = false;
                    break;
                }
            }
            if (isOk)
            {
                Player newPlayer = new Player();
                newPlayer.Name = usernameBox.Text;
                newPlayer.ImageIndex = currentIndex;
                newPlayer.PlayedGames = 0;
                newPlayer.WonGames = 0;
                _players.Add(newPlayer);
                AddUserToXml(newPlayer.Name, currentIndex);
                MainWindow mainWindow = new MainWindow();
                mainWindow.UpdatePlayerList(_players);
                this.Close();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Username already exists.", "Error", MessageBoxButton.OK);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Previous
        {
            if(currentIndex > 0)
            {
                currentIndex--;
                avatarImage.Source = AvatarImages[currentIndex];
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) // Next
        {
            if(currentIndex < AvatarImages.Count() - 1)
            {
                currentIndex++;
                avatarImage.Source = AvatarImages[currentIndex];
            }
        }

        public void AddUserToXml(string username, int imageIndex)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"UserData.xml");

            XmlElement userElement = xmlDoc.CreateElement("Player");

            XmlElement usernameElement = xmlDoc.CreateElement("username");
            usernameElement.InnerText = username;
            userElement.AppendChild(usernameElement);

            XmlElement imageElement = xmlDoc.CreateElement("imageIndex");
            imageElement.InnerText = imageIndex.ToString();
            userElement.AppendChild(imageElement);

            XmlElement playedGames = xmlDoc.CreateElement("playedGames");
            playedGames.InnerText = "0";
            userElement.AppendChild(playedGames);

            XmlElement wonGames = xmlDoc.CreateElement("wonGames");
            wonGames.InnerText = "0";
            userElement.AppendChild(wonGames);

            XmlElement rootElement = xmlDoc.DocumentElement;
            rootElement.AppendChild(userElement);

            xmlDoc.Save(@"UserData.xml");
        }
    }
}
