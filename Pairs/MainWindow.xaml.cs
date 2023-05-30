using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Pairs
{
    public partial class MainWindow : Window
    {
        ObservableCollection<BitmapImage> AvatarImages = new ObservableCollection<BitmapImage>
        {
                new BitmapImage(new Uri(@"/avatars/boy.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/cool.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/crocodile.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/demon.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/gentleman.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/grandfather.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/humorist.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/kangaroo.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/man.png", UriKind.Relative)),
                new BitmapImage(new Uri(@"/avatars/zombie.png", UriKind.Relative))
        };
        ObservableCollection<Player> playerList = new ObservableCollection<Player>();

        public void UpdatePlayerList(ObservableCollection<Player> updatedList)
        {
            playerList = updatedList;
            listView.ItemsSource = playerList;
        }

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("UserData.xml"))
            {
                CreateUserXML();
            }
            LoadXml loadXml = new LoadXml();
            playerList = loadXml.LoadUsersFromXml(@"UserData.xml");
            deleteUserButton.IsEnabled = false;
            playButton.IsEnabled = false;
            listView.ItemsSource = playerList;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listView.SelectedItem != null)
            {
                deleteUserButton.IsEnabled = true;
                playButton.IsEnabled = true;
                Player player = (Player)listView.SelectedItem;
                imageUser.Source = AvatarImages[player.ImageIndex];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Add User
        {
            this.Hide();
            NewUser newWindow = new NewUser(playerList, AvatarImages);
            newWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // Close Button
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) // Previous Button
        {
            int selectedIndex = listView.SelectedIndex;
            if(selectedIndex > 0)
            {
                listView.SelectedIndex = selectedIndex - 1;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) // Next Button
        {
            int selectedIndex = listView.SelectedIndex;
            if (selectedIndex < listView.Items.Count - 1)
            {
                listView.SelectedIndex = selectedIndex + 1;
            }
        }

        public void CreateUserXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("Players");
            xmlDoc.AppendChild(root);
            xmlDoc.Save(@"UserData.xml");
        }

        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"UserData.xml");
            XmlNode root = xmlDoc.DocumentElement;

            string usernameToDelete = ((Player)listView.SelectedItem).Name;
            XmlNode item = root.SelectSingleNode("//Player[username='" + usernameToDelete + "']");
            item.ParentNode.RemoveChild(item);
            xmlDoc.Save(@"UserData.xml");
            this.UpdatePlayerList(playerList);
            LoadXml loadXml = new LoadXml();
            playerList = loadXml.LoadUsersFromXml(@"UserData.xml");
            listView.ItemsSource = playerList;
        }

        private void playButton_Click(object sender, RoutedEventArgs e) 
        {
            this.Hide();
            Player player = (Player)listView.SelectedItem;
            PlayWindow playWindow = new PlayWindow(player);
            playWindow.Show();
        }
    }
}
