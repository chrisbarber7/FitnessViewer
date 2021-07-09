#region Copyright (C) 2014 Sascha Simon

//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see http://www.gnu.org/licenses/.
//
//  Visit the official homepage at http://www.sascha-simon.com

#endregion

using System;
using System.Timers;
using Strava.Authentication;
using Strava.Clients;

namespace Strava.Upload
{
    /// <summary>
    /// This class is used to check the status of an upload. You can subscribe to the UploadChecked event which is raised 
    /// whenever the status was checked. You can then read the current status of the upload.
    /// </summary>
    public class UploadStatusCheck
    {
        private readonly Timer _timer;
        private CheckStatus _currentStatus;
        private readonly string _token;
        private readonly string _uploadId;

        /// <summary>
        /// Indicates whether the activity is processed.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return _currentStatus == CheckStatus.Finished;
            }
        }

        /// <summary>
        /// This attribute contains details about the error.
        /// </summary>
        public string ErrorMessage { get; set; }

        #region Events

        /// <summary>
        /// UploadChecked is raised whenever the status of an upload was checked and a response was received.
        /// </summary>
        public event EventHandler<UploadStatusCheckedEventArgs> UploadChecked;

        /// <summary>
        /// ActivityReady is raised whenever the activity was successfully uploaded to and processed by Strava.
        /// </summary>
        public event EventHandler ActivityReady;

        /// <summary>
        /// ActivityProcessing is raised whenever the activity is being processed by Strava.
        /// </summary>
        public event EventHandler ActivityProcessing;

        /// <summary>
        /// Error is raised whenever an error has occured.
        /// </summary>
        public event EventHandler Error;

        #endregion

        /// <summary>
        /// Initializes a new instance of the UploadStatusCheck class.
        /// </summary>
        public UploadStatusCheck(string accessToken, string uploadId)
        {
            _timer = new Timer(1000D);
            _timer.Elapsed += TimerTick;

            _token = accessToken;
            _uploadId = uploadId;
        }

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            StaticAuthentication auth = new StaticAuthentication(_token);
            StravaClient client = new StravaClient(auth);

            UploadStatus status = client.Uploads.CheckUploadStatus(_uploadId);

            switch (status.Status)
            {
                case "Your activity is still being processed.":
                    if (UploadChecked != null)
                    {
                        UploadChecked(this, new UploadStatusCheckedEventArgs(CurrentUploadStatus.Processing));
                    }
                    if (ActivityProcessing != null)
                    {
                        ActivityProcessing(this, EventArgs.Empty);
                    }
                    break;

                case "The created activity has been deleted.":
                    if (UploadChecked != null)
                    {
                        UploadChecked(this, new UploadStatusCheckedEventArgs(CurrentUploadStatus.Deleted));
                    }
                    Finish();
                    break;

                case "There was an error processing your activity.":
                    if (UploadChecked != null)
                    {
                        UploadChecked(this, new UploadStatusCheckedEventArgs(CurrentUploadStatus.Error));
                    }
                    ErrorMessage = status.Error;
                    if (Error != null)
                    {
                        Error(this, EventArgs.Empty);
                    }
                    Finish();
                    break;

                case "Your activity is ready.":
                    if (UploadChecked != null)
                    {
                        UploadChecked(this, new UploadStatusCheckedEventArgs(CurrentUploadStatus.Ready));
                    }
                    if (ActivityReady != null)
                    {
                        ActivityReady(this, EventArgs.Empty);
                    }
                    Finish();
                    break;
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (!IsFinished)
            {                
                _timer.Start();
                _currentStatus = CheckStatus.Busy;
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _currentStatus = CheckStatus.Idle;
        }

        private void Finish()
        {
            _currentStatus = CheckStatus.Finished;
            _timer.Stop();
        }
    }
}
