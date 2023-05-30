using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Pairs
{
    public class Player
    {
        private string username;
        private int imageIndex;
        private int playedGames;
        private int wonGames;

        [XmlElement("playedGames")]
        public int PlayedGames
        {
            get { return playedGames; }
            set { playedGames = value; }
        }

        [XmlElement("wonGames")]
        public int WonGames
        {
            get { return wonGames; }
            set { wonGames = value; }
        }

        [XmlElement("username")]
        public string Name
        {
            get { return username; }
            set { username = value; }
        }

        [XmlElement("imageIndex")]
        public int ImageIndex
        { 
            get { return imageIndex; }
            set { imageIndex = value; }
        }

        public Player(){ }

        public Player(string username, int image)
        {
            this.username = username;
            this.imageIndex = image;
        }
    }
}
