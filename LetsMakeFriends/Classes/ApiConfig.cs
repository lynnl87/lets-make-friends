using LetsMakeFriends.MVVM.Model;
using LetsMakeFriends.MVVM.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace LetsMakeFriends.Classes
{
    /// <summary>
    /// Purpose of this class is to read in and parse a config file.
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>
        /// Handles parsing the config file.
        /// </summary>
        /// <param name="configLocation">Location of the config file.</param>
        public static void ParseConfig(string configLocation)
        {
            string data = File.ReadAllText(configLocation);
            ApiConfigFormat apiConfig = JsonConvert.DeserializeObject<ApiConfigFormat>(data);
            foreach (ApiConfigEntry entry in apiConfig.Apis)
            {
                ApiOptionsModel apiOptionsModel = new ApiOptionsModel()
                {
                    Name = entry.Name,
                    Endpoint = entry.Endpoint,
                    Description = entry.Description,
                    Format = entry.Format,
                    DesiredValue = entry.DesiredValue,
                    IsChecked = true
                };
                ApiOptionsViewModel.Instance.AddApiOption(apiOptionsModel);
            }
        }
    }

    /// <summary>
    /// Base class for the config.json file.
    /// </summary>
    public class ApiConfigFormat
    {
        /// <summary>
        /// An array of api configurations.
        /// </summary>
        public ApiConfigEntry[] Apis;
    }

    /// <summary>
    /// An api configuration.
    /// </summary>
    public class ApiConfigEntry
    {
        /// <summary>
        /// Display name of the API.
        /// </summary>
        public string Name;

        /// <summary>
        /// Description of the api.
        /// </summary>
        public string Description;

        /// <summary>
        /// Endpoint address of the api.
        /// </summary>
        public string Endpoint;

        /// <summary>
        /// The format of the API result.
        /// </summary>
        public JObject Format;

        /// <summary>
        /// Desired value from the api.
        /// </summary>
        public string DesiredValue;
    }
}
