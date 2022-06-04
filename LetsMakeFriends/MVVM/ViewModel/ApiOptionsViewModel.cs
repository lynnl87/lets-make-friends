using LetsMakeFriends.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LetsMakeFriends.MVVM.ViewModel
{
    /// <summary>
    /// Purpose of this class is to be the inbetween of the UI and the API Options.
    /// </summary>
    public class ApiOptionsViewModel : ViewModelBase
    {
        /// <summary>
        /// Static instance of the class for use with singleton.
        /// </summary>
        private static ApiOptionsViewModel s_instance;

        /// <summary>
        /// List of api options we support.
        /// </summary>
        private readonly ObservableCollection<ApiOptionsModel> _apiOptions;

        /// <summary>
        /// Contains the currently selected item.
        /// </summary>
        private ApiOptionsModel _selectedItem;

        /// <summary>
        /// The generator for random numbers.
        /// </summary>
        private readonly Random _randomGenerator;

        /// <summary>
        /// Initializes an instance of the ApiOptionsViewModel class.
        /// </summary>
        private ApiOptionsViewModel()
        {
            _apiOptions = new ObservableCollection<ApiOptionsModel>();

            _randomGenerator = new Random();
        }

        /// <summary>
        /// Gets the static instance of the view model.
        /// </summary>
        public static ApiOptionsViewModel Instance
        {
            get
            {
                return s_instance ?? (s_instance = new ApiOptionsViewModel());
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item.
        /// </summary>
        public ApiOptionsModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        /// <summary>
        /// Gets the collection of api options
        /// </summary>
        public ObservableCollection<ApiOptionsModel> ApiOptions
        {
            get
            {
                return _apiOptions;
            }
        }

        /// <summary>
        /// Adds an option to the list.
        /// </summary>
        /// <param name="apiOptionsModel">An api options model to add.</param>
        public void AddApiOption(ApiOptionsModel apiOptionsModel)
        {
            _apiOptions.Add(apiOptionsModel);
        }

        /// <summary>
        /// Handles calling an API.
        /// </summary>
        /// <returns>The desired value.</returns>
        public async Task<Tuple<string, string>> CallApi()
        {
            string returnString = string.Empty;
            string returnName = string.Empty;
            var selectedList = ApiOptions.Where(x => x.IsChecked).ToList();
            if (selectedList.Count > 0)
            {
                ApiOptionsModel apiOptionsModel = selectedList[_randomGenerator.Next(selectedList.Count)];
                returnName = apiOptionsModel?.Name;
                returnString = await apiOptionsModel?.CallApi();
            }

            return new Tuple<string, string>(returnName, returnString);
        }
    }
}
