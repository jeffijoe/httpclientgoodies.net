// SkyClip
// - ProgressableStreamContent.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jeffijoe.HttpClientGoodies
{
    /// <summary>
    ///     The progressable stream content.
    /// </summary>
    public class ProgressableStreamContent : StreamContent
    {
        #region Constants

        /// <summary>
        ///     The default buffer size.
        /// </summary>
        private const int DefaultBufferSize = 4096;

        #endregion

        #region Fields

        /// <summary>
        ///     The buffer size.
        /// </summary>
        private readonly int bufferSize;

        /// <summary>
        ///     The progress.
        /// </summary>
        private readonly IProgress<UploadProgress> progress;

        /// <summary>
        ///     The stream to write.
        /// </summary>
        private readonly Stream streamToWrite;

        /// <summary>
        ///     The content consumed.
        /// </summary>
        private bool contentConsumed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressableStreamContent"/> class.
        /// </summary>
        /// <param name="streamToWrite">
        /// The stream to write.
        /// </param>
        /// <param name="downloader">
        /// The downloader.
        /// </param>
        public ProgressableStreamContent(Stream streamToWrite, IProgress<UploadProgress> downloader)
            : this(streamToWrite, DefaultBufferSize, downloader)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressableStreamContent"/> class.
        /// </summary>
        /// <param name="streamToWrite">
        /// The stream to write.
        /// </param>
        /// <param name="bufferSize">
        /// Size of the buffer.
        /// </param>
        /// <param name="progress">
        /// The progress.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Stream to write must not be null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Buffer size cannot be less than 0.
        /// </exception>
        public ProgressableStreamContent(Stream streamToWrite, int bufferSize, IProgress<UploadProgress> progress)
            : base(streamToWrite, bufferSize)
        {
            if (streamToWrite == null)
            {
                throw new ArgumentNullException("streamToWrite");
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            this.streamToWrite = streamToWrite;
            this.bufferSize = bufferSize;
            this.progress = progress;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Net.Http.HttpContent"/> and optionally disposes
        ///     of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to releases only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.streamToWrite.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Serialize the HTTP content to a stream as an asynchronous operation.
        /// </summary>
        /// <param name="stream">
        /// The target stream.
        /// </param>
        /// <param name="context">
        /// Information about the transport (channel binding token, for example). This parameter may be null.
        /// </param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task"/>.The task object representing the asynchronous operation.
        /// </returns>
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            this.PrepareContent();

            var buffer = new byte[this.bufferSize];
            var size = this.streamToWrite.Length;
            var uploaded = 0;

            using (this.streamToWrite)
            {
                while (true)
                {
                    var length = this.streamToWrite.Read(buffer, 0, buffer.Length);
                    if (length <= 0)
                    {
                        break;
                    }

                    uploaded += length;
                    this.progress.Report(new UploadProgress(uploaded, size));
                    await stream.WriteAsync(buffer, 0, length);
                }
            }
        }

        /// <summary>
        /// Determines whether the HTTP content has a valid length in bytes.
        /// </summary>
        /// <param name="length">
        /// The length in bytes of the HHTP content.
        /// </param>
        /// <returns>
        /// Returns <see cref="T:System.Boolean"/>.true if <paramref name="length"/> is a valid length; otherwise, false.
        /// </returns>
        protected override bool TryComputeLength(out long length)
        {
            length = this.streamToWrite.Length;
            return true;
        }

        /// <summary>
        ///     Prepares the content.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The stream has already been read.</exception>
        private void PrepareContent()
        {
            if (this.contentConsumed)
            {
                // If the content needs to be written to a target stream a 2nd time, then the stream must support
                // seeking (e.g. a FileStream), otherwise the stream can't be copied a second time to a target 
                // stream (e.g. a NetworkStream).
                if (this.streamToWrite.CanSeek)
                {
                    this.streamToWrite.Position = 0;
                }
                else
                {
                    throw new InvalidOperationException("The stream has already been read.");
                }
            }

            this.contentConsumed = true;
        }

        #endregion
    }
}