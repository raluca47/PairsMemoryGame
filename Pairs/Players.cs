using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pairs
{
    [XmlRoot("Players")]
    public class Players
    {
        [XmlElement("Player")]
        public List<Player> PlayersList { get; set; }
    }
}
