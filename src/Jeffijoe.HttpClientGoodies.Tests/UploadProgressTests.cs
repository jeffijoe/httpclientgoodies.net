// SkyClip
// - UploadProgressTests.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using Shouldly;

using Xunit;

namespace Jeffijoe.HttpClientGoodies.Tests
{
    /// <summary>
    ///     The upload progress tests.
    /// </summary>
    public class UploadProgressTests
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The verify constructor calculates correct percentage.
        /// </summary>
        [Fact]
        public void VerifyConstructorCalculatesCorrectPercentage()
        {
            var subject = new UploadProgress(100, 200);
            subject.ProgressPercentage.ShouldBe(50);
        }

        /// <summary>
        ///     Verifies the constructor does not touch percentage when there are no total bytes.
        /// </summary>
        [Fact]
        public void VerifyConstructorDoesNotTouchPercentageWhenThereAreNoTotalBytes()
        {
            var subject = new UploadProgress(100, null);
            subject.ProgressPercentage.ShouldBe(0);
        }

        #endregion
    }
}