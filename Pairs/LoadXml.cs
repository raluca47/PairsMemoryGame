using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Pairs
{
    public class LoadXml
    {
        public ObservableCollection<Player> LoadUsersFromXml(string xmlFilePath)
        {
            ObservableCollection<Player> players = new ObservableCollection<Player>();

            XmlSerializer serializer = new XmlSerializer(typeof(Players));
            using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
            {
                Players playersList = (Players)serializer.Deserialize(fileStream);
                foreach (Player player in playersList.PlayersList)
                {
                    players.Add(player);
                }
            }

            return players;
        }
    }
}
