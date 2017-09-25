using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCOB_Tracker
{
    public class Descriptors
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string MembershipID { get; set; }
        public string CharacterID { get; set; }

        public string InstanceID { get; set; }
        public string ChildName { get; set; }

        public double GameStanding { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public string GameCompleted { get; set; }
        public double KDR { get; set; }
        public string WinLose { get; set; }
        public double TotalKillDistance { get; set; }
        public double AvgLifeSpan { get; set; }
        public int PrecisionKills { get; set; }
        public int LongestKillSpree { get; set; }
        public double LongestSingleLife { get; set; }
        public int OrbsDropped { get; set; }
        public double CombatRating { get; set; }
        public string BestWeapon { get; set; }
        public string FireTeamID { get; set; }
    }
}
