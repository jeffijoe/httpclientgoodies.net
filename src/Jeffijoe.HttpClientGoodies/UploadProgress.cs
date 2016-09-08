// SkyClip
// - UploadProgress.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.
namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     The upload progress.
    /// </summary>
    public class UploadProgress
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadProgress"/> class.
        /// </summary>
        /// <param name="bytesTransfered">
        /// The bytes transfered.
        /// </param>
        /// <param name="totalBytes">
        /// The total bytes.
        /// </param>
        public UploadProgress(long bytesTransfered, long? totalBytes)
        {
            this.BytesTransfered = bytesTransfered;
            this.TotalBytes = totalBytes;
            if (totalBytes.HasValue)
            {
                this.ProgressPercentage = (int)((float)bytesTransfered / totalBytes.Value * 100);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the bytes transfered.
        /// </summary>
        public long BytesTransfered { get; private set; }

        /// <summary>
        ///     Gets the progress percentage.
        /// </summary>
        public int ProgressPercentage { get; private set; }

        /// <summary>
        ///     Gets the total bytes.
        /// </summary>
        public long? TotalBytes { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}% ({1} / {2})", this.ProgressPercentage, this.BytesTransfered, this.TotalBytes);
        }

        #endregion
    }
}