using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LetsMakeFriends.Classes.Helpers
{
    /// <summary>
    /// Class to represent a return value from the advice api.
    /// </summary>
    public class AdviceResult
    {
        public Advice Slip;
    }

    /// <summary>
    /// Internal portion of an AdviceResult.
    /// </summary>
    public class Advice
    {
        /// <summary>
        /// Id of the advice message.
        /// </summary>
        public int Id;

        /// <summary>
        /// The actual advice as a string.
        /// </summary>
        /// <remarks>We cant have the field named the same as the class. Using a jsonproperty to help with parsing.</remarks>
        [JsonProperty("advice")]
        public string AdviceMessage;
    }

    /// <summary>
    /// Purpose of this class is to interface with the web api.
    /// </summary>
    public class AdviceHelper
    {
        /// <summary>
        /// An instance of an HTTP Client.
        /// </summary>
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Static initializer of an AdviceHelper class.
        /// </summary>
        static AdviceHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Call to get advice!
        /// </summary>
        /// <returns>An instance of the AdviceResult class.</returns>
        public static async Task<AdviceResult> GetAdviceAsync()
        {
            AdviceResult advice = null;
            HttpResponseMessage response = await client.GetAsync("https://api.adviceslip.com/advice");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                advice = JsonConvert.DeserializeObject<AdviceResult>(data);
            }

            return advice;
        }
    }
}
