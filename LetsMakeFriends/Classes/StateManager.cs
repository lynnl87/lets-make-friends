﻿using LetsMakeFriends.Classes.Helpers;
using LetsMakeFriends.MVVM;
using LetsMakeFriends.MVVM.Model;
using LetsMakeFriends.MVVM.ViewModel;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Threading;
using static LetsMakeFriends.Classes.Helpers.Delegates;

namespace LetsMakeFriends.Classes
{
    /// <summary>
    /// Purpose of this is to help manager the state of the application.
    /// </summary>
    public class StateManager : ViewModelBase
    {
        /// <summary>
        /// Our http client.
        /// </summary>
        private readonly HttpClient s_client = new HttpClient();

        /// <summary>
        /// Private instance of our state manager class.
        /// </summary>
        private static StateManager s_instance;

        /// <summary>
        /// Contains an instance of our hotkey helper.
        /// </summary>
        private readonly HotkeyHelper s_hotkeyHelper;

        /// <summary>
        /// Contains our log model.
        /// </summary>
        private LogModel _logModel;

        /// <summary>
        /// Initializes an instance of the StateManager class.
        /// </summary>
        private StateManager()
        {
            _logModel = new LogModel();

            // Following check is put in place so the designer doesn't attempt to fire up an HttpClient.
            // without it, the designer will always crash.
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                return;
            }

            s_client = new HttpClient();
            s_client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Get our hotkeys setup.
            s_hotkeyHelper = new HotkeyHelper();
        }

        /// <summary>
        /// Gets the instance of the state manager class.
        /// </summary>
        public static StateManager Instance
        {
            get
            {
                return s_instance ?? (s_instance = new StateManager());
            }
        }

        /// <summary>
        /// Gets an instance of our http client.
        /// </summary>
        public HttpClient Client
        {
            get
            {
                return s_client;
            }
        }

        /// <summary>
        /// Handles getting our log model.
        /// </summary>
        public LogModel Log
        {
            get
            {
                return _logModel;
            }
            set
            {
                SetProperty(ref _logModel, value);
            }
        }

        /// <summary>
        /// Handles registering a key to a callback.
        /// </summary>
        /// <param name="keyValue">Key Value</param>
        /// <param name="modifiers">Ctrl/Alt/Shift modifiers.</param>
        /// <param name="callback">Function to be called on hotkey pressed.</param>
        public void RegisterKey(uint keyValue, uint modifiers, HotkeyPressedCallBack callback)
        {
            s_hotkeyHelper.RegisterHotKey(keyValue, modifiers, OnHotkeyPress);
        }

        /// <summary>
        /// Called when our hotkey is pressed.
        /// </summary>
        /// <param name="keyId">KeyID that was pressed.</param>
        private async void OnHotkeyPress(int keyId)
        {
            Tuple<string, string> results = await ApiOptionsViewModel.Instance.CallApi();
            Clipboard.SetText(results.Item2);
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                Instance.Log.AddLogLine(string.Format("{0} - {1}\n", results.Item1, results.Item2));
            }));
        }
    }
}
