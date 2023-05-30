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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Pairs
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        Player player;
        public Statistics(Player player)
        {
            InitializeComponent();
            this.player = player;
        }

        private void AllStatisticsButton(object sender, RoutedEventArgs e)
        {
            XDocument xmlDoc = XDocument.Load("UserData.xml");
            IEnumerable<XElement> playerElements = xmlDoc.Descendants("Player");
            StringBuilder output = new StringBuilder();

            foreach(XElement playerElement in playerElements)
            {
                string username = playerElement.Element("username").Value;
                int playedGames = (int)playerElement.Element("playedGames");
                int wonGames = (int)playerElement.Element("wonGames");

                output.AppendLine($"Username: {username} | Played Games: {playedGames} | Won Games: {wonGames}\n");
            }

            statisticsTextBlock.Text = output.ToString();
        }

        private void MyStatisticsButton(object sender, RoutedEventArgs e)
        {
            int playedGames, wonGames;
            string username;
            XDocument xmlDoc = XDocument.Load("UserData.xml");
            XElement currentPlayerElement = xmlDoc.Descendants("Player").Where(p => (string)p.Element("username") == player.Name).FirstOrDefault();
            playedGames = int.Parse(currentPlayerElement.Element("playedGames").Value);
            username = currentPlayerElement.Element("username").Value;
            wonGames = int.Parse(currentPlayerElement.Element("wonGames").Value);
            statisticsTextBlock.Text = "Username: " + username + " | Games Played: " + playedGames + " | Games Won: " + wonGames;
        }
    }
}
