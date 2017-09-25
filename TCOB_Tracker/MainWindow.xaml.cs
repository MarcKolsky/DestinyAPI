using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace TCOB_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Descriptors> clanMembers;


        public MainWindow()
        {
            InitializeComponent();

            GetData();
        }




        private void GetData()
        {
            clanMembers = new List<Descriptors>();           ////  Creates object to store all clan member names

            #region  Get Destiny Member Data
            clanMembers = DestinyCalls.GetTCOBMemberNames();        ////  Get clan member names


            var updatedClanMembers = DestinyCalls.GetMembershipID(clanMembers);            ////  Gets updated list with membership IDs


            var fullClanMembers = DestinyCalls.GetCharacterID(updatedClanMembers);          ////  Gets updated list with character IDs
            #endregion

            



            List<Descriptors> instances = new List<Descriptors>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "[Customer API Key]");

                foreach (var member in fullClanMembers)
                {

                    /////   Get activity history for each clan member for Iron Banner
                    var response = client.GetAsync("https://www.bungie.net/Platform/Destiny/Stats/ActivityHistory/2/" + member.MembershipID + "/" + member.CharacterID + "/?mode=IronBanner&count=250").Result;
                    var characterResult = response.Content.ReadAsStringAsync().Result;            ////  Json read and converted into a string

                    

                    XmlDocument xml = (XmlDocument)JsonConvert.DeserializeXmlNode(characterResult, "XmlResult");           ////  Converts Json to XML
                    


                    foreach (XmlNode a in xml.DocumentElement.ChildNodes)
                    {
                        foreach (XmlNode b in a.ChildNodes)
                        {
                            foreach (XmlNode c in b.ChildNodes)
                            {
                                foreach (XmlNode d in c.ChildNodes)
                                {
                                    foreach (XmlNode e in d.ChildNodes)
                                    {

                                        instances.Add(new Descriptors() { DisplayName = member.DisplayName, MembershipID = member.MembershipID, CharacterID = member.CharacterID, ChildName = e.Name, InstanceID = e.InnerText });

                                    }
                                }
                            }
                        }
                    }
                }
            }





            var instanceList = instances.FindAll(x => x.ChildName == "instanceId");             ////  Retrieve all instance ID's from activity history for all members

            List<Descriptors> IronBannerResults = new List<Descriptors>();


            /////  Set date range for Iron Banner you want data for
            DateTime _startDate = Convert.ToDateTime("4/11/2017");
            DateTime _endDate = Convert.ToDateTime("4/17/2017");


            /////  Create connection to table on SQL Server
            SqlConnection tcobConnection = new SqlConnection("Data Source=[ Server Name/IP address ]; Initial Catalog=TCOBTracker;Integrated Security=True");
            tcobConnection.Open();


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "[ Customer API Key ]");




                foreach (var i in instanceList)
                {
                    try
                    {
                        ////  Retrieve Iron Banner stat information from Destiny servers per instance ID for each member
                        var response = client.GetAsync("https://www.bungie.net/Platform/Destiny/Stats/PostGameCarnageReport/" + i.InstanceID + "/").Result;
                        var postCarnageResult = response.Content.ReadAsStringAsync().Result;


                        dynamic _data = JsonConvert.DeserializeObject(postCarnageResult);

                        List<int> num = new List<int>();


                        string mode = _data.Response.data.activityDetails.mode.ToString();
                        DateTime period = Convert.ToDateTime(_data.Response.data.period.ToString());


                        ///////  Checking mode number
                        //if (period >= _startDate)
                        //{
                        //    System.Windows.Forms.MessageBox.Show(mode + "\n" + period);
                        //}

                        try
                        {

                            if (i.MembershipID == _data.Response.data.entries[0].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(0);
                            }
                            else if (i.MembershipID == _data.Response.data.entries[1].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(1);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[2].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(2);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[3].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(3);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[4].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(4);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[5].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(5);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[6].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(6);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[7].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(7);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[8].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(8);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[9].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(9);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[10].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(10);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[11].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(11);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[12].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(12);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[13].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(13);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[14].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(14);
                            }

                            else if (i.MembershipID == _data.Response.data.entries[15].player.destinyUserInfo.membershipId.ToString())
                            {
                                num.Add(15);
                            }
                        }
                        catch { }


                        if (mode == "19" && period >= _startDate && period <= _endDate)             //// Mode value can change depending on what even Iron Banner is each month
                        {

                            foreach (var n in num)
                            {
                                DateTime? gameDate = null;
                                int kills = 0;
                                int assists = 0;
                                int deaths = 0;
                                string gameCompleted = "";
                                decimal kdr = 0;
                                string winLose = "";
                                decimal totalKillDistance = 0;
                                decimal avgLifeSpan = 0;
                                int precisionKills = 0;
                                int longestKillSpree = 0;
                                decimal longestSingleLife = 0;
                                int orbsDropped = 0;
                                decimal combatRating = 0;
                                string bestWeapon = "";
                                string fireTeamID = "";





                                //////  Try Catch statement is used to catch any unintended exceptions should a null value be returned
                                try { gameDate = Convert.ToDateTime(_data.Response.data.period.ToString()); } catch { }
                                try { kills = Convert.ToInt32(_data.Response.data.entries[n].values.kills.basic.value.ToString()); } catch { }
                                try { assists = Convert.ToInt32(_data.Response.data.entries[n].values.assists.basic.value.ToString()); } catch { }
                                try { deaths = Convert.ToInt32(_data.Response.data.entries[n].values.deaths.basic.value.ToString()); } catch { }
                                try { gameCompleted = _data.Response.data.entries[n].values.completed.basic.displayValue.ToString(); } catch { }
                                try { kdr = Convert.ToDecimal(_data.Response.data.entries[n].values.killsDeathsRatio.basic.value.ToString()); } catch { }
                                try { winLose = _data.Response.data.entries[n].values.standing.basic.displayValue.ToString(); } catch { }
                                try { totalKillDistance = Convert.ToDecimal(_data.Response.data.entries[n].extended.values.totalKillDistance.basic.value.ToString()); } catch { }
                                try { avgLifeSpan = Convert.ToDecimal(_data.Response.data.entries[n].extended.values.averageLifespan.basic.value.ToString()); } catch { }
                                try { precisionKills = Convert.ToInt32(_data.Response.data.entries[n].extended.values.precisionKills.basic.value.ToString()); } catch { }
                                try { longestKillSpree = Convert.ToInt32(_data.Response.data.entries[n].extended.values.longestKillSpree.basic.value.ToString()); } catch { }
                                try { longestSingleLife = Convert.ToDecimal(_data.Response.data.entries[n].extended.values.longestSingleLife.basic.value.ToString()); } catch { }
                                try { orbsDropped = Convert.ToInt32(_data.Response.data.entries[n].extended.values.orbsDropped.basic.value.ToString()); } catch { }
                                try { combatRating = Convert.ToDecimal(_data.Response.data.entries[n].extended.values.combatRating.basic.value.ToString()); } catch { }
                                try { bestWeapon = _data.Response.data.entries[n].extended.values.weaponBestType.basic.displayValue.ToString(); } catch { }
                                try { fireTeamID = _data.Response.data.entries[n].extended.values.fireTeamId.basic.displayValue.ToString(); } catch { }




                                //// SQL command that actually updates the record.
                                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.IronBannerTracker2 (DisplayName, MembershipID, CharacterID, InstanceID, GameDate, Kills, Assists, Deaths, GameCompleted, KDR, WinLose, TotalKillDistance, AvgLifeSpan, PrecisionKills, LongestKillSpree, LongestSingleLife, OrbsDropped, CombatRating, BestWeapon, FireTeamID) values ('" + i.DisplayName + "', '" + i.MembershipID + "', '" + i.CharacterID + "', '" + i.InstanceID + "', '" + gameDate + "', " + kills + ", " + assists + ", " + deaths + ", '" + gameCompleted + "', " + kdr + ", '" + winLose + "', " + totalKillDistance + ", " + avgLifeSpan + ", " + precisionKills + ", " + longestKillSpree + ", " + longestSingleLife + ", " + orbsDropped + ", " + combatRating + ", '" + bestWeapon + "', '" + fireTeamID + "');", tcobConnection);
                                cmd.ExecuteNonQuery();
                                
                            }
                        }
                    }
                    catch { }
                }
            }


            tcobConnection.Close();             ////  Closes SQL connection after all data has been uploaded to the table
            _result.Text = "Completed!";
        }
    }
}
