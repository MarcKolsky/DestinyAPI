using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCOB_Tracker
{
    public class DestinyCalls
    {

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///         All catch statements were used for testing, and left empty at runtime as no exception handling was needed for this simple task      ////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        public static List<Descriptors> GetTCOBMemberNames()
        {
            List<Descriptors> Names = new List<Descriptors>();              /// Stores usernames retrieved from Destiny servers

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "[Customer API Key]");                ////  Adds additional criteria to 

                var responseClanNames = client.GetAsync("http://www.bungie.net/Platform/Group/1552163/MembersV3/?lc=en&fmt=true&lcin=true&currentPage=1").Result;       /////   Connects to Destiny servers and retrieves all usernames of members of the TCOB clan.

                var contentClanNames = responseClanNames.Content.ReadAsStringAsync().Result;            ////  Json read and converted into a string
                dynamic _data = JsonConvert.DeserializeObject(contentClanNames);

                for (int f = 0; f < 50; f++)
                {
                    try
                    {
                        string name = null;

                        try { name = _data.Response.results[f].user.psnDisplayName.ToString(); } catch { }
                        if (name == null)
                            try { name = _data.Response.results[f].user.displayName.ToString(); } catch { }
                        

                        Names.Add(new Descriptors() { DisplayName = name });
                    }
                    catch {  }
                }
            }

            return Names;
        }






        public static List<Descriptors> GetMembershipID(List<Descriptors> clan)
        {
            List<Descriptors> updated = new List<Descriptors>();           ////  List to be updated with membership IDs


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "[Customer API Key]");

                foreach (var member in clan)
                {
                    if (member.DisplayName != null)
                    {
                        Uri uriString = null;

                        
                        try { uriString = uriString = new Uri("http://www.bungie.net/Platform/Destiny/SearchDestinyPlayer/2/" + member.DisplayName + "/"); } catch { }          /// Uses usernames queried from the Destiny servers to add membership ID to retrieve additional information

                        if (uriString != null)
                        {
                            try
                            {
                                var responseMemberNameSearch = client.GetAsync(uriString).Result;               /////   Get membershipID for each clan member
                                var contentMemberID = responseMemberNameSearch.Content.ReadAsStringAsync().Result;

                                dynamic _memberID = JsonConvert.DeserializeObject(contentMemberID);                                


                                updated.Add(new Descriptors() { DisplayName = member.DisplayName, MembershipID = _memberID.Response[0].membershipId.ToString() });
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("No MemberID:\n\n" + member.DisplayName);
                            }
                        }
                    }
                }
            }

            return updated;
        }






        public static List<Descriptors> GetCharacterID(List<Descriptors> clan)
        {
            List<Descriptors> updated = new List<Descriptors>();           ////  List to be updated with character IDs


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", "[Customer API Key]");

                foreach (var member in clan)
                {
                    var response = client.GetAsync("http://www.bungie.net/Platform/Destiny/2/Account/" + member.MembershipID + "/").Result;             /////   Get characterID for each clan member
                    var characterResult = response.Content.ReadAsStringAsync().Result;            ////  Json read and converted into a string
                    
                    dynamic _data = JsonConvert.DeserializeObject(characterResult);

                    
                    try
                    {
                        string _charID1 = _data.Response.data.characters[0].characterBase.characterId;
                        updated.Add(new Descriptors() { DisplayName = member.DisplayName, MembershipID = member.MembershipID, CharacterID = _charID1 });
                    }
                    catch { }

                    try
                    {
                        string _charID2 = _data.Response.data.characters[1].characterBase.characterId;
                        updated.Add(new Descriptors() { DisplayName = member.DisplayName, MembershipID = member.MembershipID, CharacterID = _charID2 });
                    }
                    catch { }

                    try
                    {
                        string _charID3 = _data.Response.data.characters[2].characterBase.characterId;
                        updated.Add(new Descriptors() { DisplayName = member.DisplayName, MembershipID = member.MembershipID, CharacterID = _charID3 });
                    }
                    catch { }
                }
            }


            return updated;
        }
    }
}
