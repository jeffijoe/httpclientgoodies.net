// SkyClip
// - UrlHelperTests.cs
// --------------------------------------------------------------------
// Author: Jeff Hansen <jeff@jeffijoe.com>
// Copyright (C) Jeff Hansen 2015. All rights reserved.

using Shouldly;

using Xunit;

namespace Jeffijoe.HttpClientGoodies.Tests
{
    /// <summary>
    ///     The url helper tests.
    /// </summary>
    public class UrlHelperTests
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Combines the returns correct urls.
        /// </summary>
        [Fact]
        public void CombineReturnsCorrectUrls()
        {
            UrlHelper.Combine("http://google.com", "api", "test", "/wee").ShouldBe("http://google.com/api/test/wee");
        }

        #endregion
    }
}