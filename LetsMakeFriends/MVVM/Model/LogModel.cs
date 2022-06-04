namespace LetsMakeFriends.MVVM.Model
{
    /// <summary>
    /// Class to interface with a log on the ui portion.
    /// </summary>
    public class LogModel : ViewModelBase
    {
        /// <summary>
        /// Contains the current log lines.
        /// </summary>
        private string _logText;

        /// <summary>
        /// Initializes an instance of LogModel class.
        /// </summary>
        public LogModel()
        {
        }

        /// <summary>
        /// Gets or sets the log text.
        /// </summary>
        public string LogText
        {
            get
            {
                return _logText;
            }
            set
            {
                SetProperty(ref _logText, value);
            }
        }

        /// <summary>
        /// Handles adding a line of text to the log.
        /// </summary>
        /// <param name="logText">The text to log</param>
        public void AddLogLine(string logText)
        {
            LogText += string.Format("{0}\n", logText);
        }
    }
}
