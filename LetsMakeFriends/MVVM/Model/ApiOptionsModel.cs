using LetsMakeFriends.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LetsMakeFriends.MVVM.Model
{
    /// <summary>
    /// Purpose of this class is to be an inbetween for the ui and backend options.
    /// </summary>
    public class ApiOptionsModel : ViewModelBase
    {
        /// <summary>
        /// Name of the api.
        /// </summary>
        private string _name;

        /// <summary>
        /// Endpoint address.
        /// </summary>
        private string _endpoint;

        /// <summary>
        /// Description of the api.
        /// </summary>
        public string _description;

        /// <summary>
        /// The format of the result.
        /// </summary>
        private JObject _format;

        /// <summary>
        /// The desired value to get from the format.
        /// </summary>
        private string _desiredValue;

        /// <summary>
        /// A value indicating if the UI is checked for us.
        /// </summary>
        public bool _isChecked;

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        /// <summary>
        /// Gets or sets the endpoint address.
        /// </summary>
        public string Endpoint
        {
            get
            {
                return _endpoint;
            }
            set
            {
                SetProperty(ref _endpoint, value);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetProperty(ref _description, value);
            }
        }

        /// <summary>
        /// Gets or sets the desired format.
        /// </summary>
        public JObject Format
        {
            get
            {
                return _format;
            }
            set
            {
                SetProperty(ref _format, value);
            }
        }

        /// <summary>
        /// Gets or sets the desired value.
        /// </summary>
        public string DesiredValue
        {
            get
            {
                return _desiredValue;
            }
            set
            {
                SetProperty(ref _desiredValue, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not we are checked on the ui side.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                SetProperty(ref _isChecked, value);
            }
        }

        /// <summary>
        /// Handles calling this api.
        /// </summary>
        /// <returns>The desired value.</returns>
        public async Task<string> CallApi()
        {
            string returnValue = string.Empty;
            string[] desiredValue = _desiredValue.Split('.');
            try
            {
                HttpResponseMessage response = await StateManager.Instance.Client.GetAsync(_endpoint);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    object tempObject = JsonConvert.DeserializeObject(data);
                    JToken finalPart = null;
                    if (typeof(JArray) == tempObject.GetType())
                    {
                        finalPart = ((JArray)tempObject)[0];
                    }
                    else if (typeof(JObject) == tempObject.GetType())
                    {
                        finalPart = (JToken)tempObject;
                    }

                    foreach (var part in desiredValue)
                    {
                        finalPart = finalPart[part];
                    }

                    returnValue = finalPart.ToString();
                }
            }
            catch (Exception e)
            {
                returnValue = e.ToString();
            }

            return returnValue;
        }
    }
}
