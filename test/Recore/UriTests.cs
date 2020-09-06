using System.Collections.Generic;
using System.Runtime.InteropServices;

using Xunit;

namespace Recore.Tests
{
    public class UriTests
    {
        private static readonly bool s_isWindowsSystem = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        // From https://github.com/dotnet/runtime/blob/6072e4d3a7a2a1493f514cdf4be75a3d56580e84/src/libraries/System.Runtime/tests/System/Uri.CreateUriTests.cs#L13
        public static IEnumerable<object[]> Combine_AbsoluteUri_TestData()
        {
            yield return new object[] { "http://host/?query#fragment", "path", "http://host/path" };
            yield return new object[] { "http://hostold/path1", "//host", "http://host/" };
            yield return new object[] { "http://host/", "", "http://host/" };

            // Query
            yield return new object[] { "http://host", "?query", "http://host/?query" };
            yield return new object[] { "http://host?queryold", "?query", "http://host/?query" };
            yield return new object[] { "http://host#fragment", "?query", "http://host/?query" };
            yield return new object[] { "http://host", " \t \r \n   ?query  \t \r \n  ", "http://host/?query" };

            // Fragment
            yield return new object[] { "http://host", "#fragment", "http://host/#fragment" };
            yield return new object[] { "http://host#fragmentold", "#fragment", "http://host/#fragment" };
            yield return new object[] { "http://host?query", "#fragment", "http://host/?query#fragment" };
            yield return new object[] { "http://host", " \t \r \n   #fragment  \t \r \n  ", "http://host/#fragment" };

            // Path
            yield return new object[] { "http://host/", "path1/page?query=value#fragment", "http://host/path1/page?query=value#fragment" };
            yield return new object[] { "http://host/", "C:/x", "http://host/C:/x" };

            // Explicit windows drive file
            yield return new object[] { "file:///D:/abc", "C:/x", "file:///C:/x" };
            yield return new object[] { "D:/abc", "C:/x", "file:///C:/x" };
            yield return new object[] { "file:///C:/", "/path", "file:///C:/path" };
            yield return new object[] { "file:///C:/", @"\path", "file:///C:/path" };
            yield return new object[] { "file:///C:/pathold", "/path", "file:///C:/path" };
            yield return new object[] { "file:///C:/pathold", @"\path", "file:///C:/path" };
            yield return new object[] { "file:///C:/pathold", "path", "file:///C:/path" };
            yield return new object[] { "file:///C:/", "/", "file:///C:/" };
            yield return new object[] { "file:///C:/", @"\", "file:///C:/" };

            // Implicit windows drive file
            yield return new object[] { "C:/", "/path", "file:///C:/path" };
            yield return new object[] { "C:/", @"\path", "file:///C:/path" };
            yield return new object[] { "C:/pathold", "/path", "file:///C:/path" };
            yield return new object[] { "C:/pathold", @"\path", "file:///C:/path" };
            yield return new object[] { "C:/pathold", "path", "file:///C:/path" };
            yield return new object[] { "C:/", "/", "file:///C:/" };
            yield return new object[] { "C:/", @"\", "file:///C:/" };

            // Unix style path
            yield return new object[] { "file:///pathold/", "/path", "file:///path" };
            yield return new object[] { "file:///pathold/", "path", "file:///pathold/path" };
            yield return new object[] { "file:///", "/path", "file:///path" };
            yield return new object[] { "file:///", "path", "file:///path" };

            // UNC
            if (s_isWindowsSystem) // Unc can only start with '/' on Windows
            {
                yield return new object[] { @"\\servernameold\path1", "//servername", @"\\servername" };
            }
            yield return new object[] { @"\\servernameold\path1", @"\\servername", @"\\servername" };
            yield return new object[] { @"\\servername\path1", "/path", @"\\servername\path1\path" };
            yield return new object[] { @"\\servername\path1", @"\path", @"\\servername\path1\path" };
            yield return new object[] { @"\\servername\path1\path2", @"\path", @"\\servername\path1\path" };
            yield return new object[] { @"\\servername\pathold", "path", @"\\servername\path" };
            yield return new object[] { @"file://\\servername/path1", "/path", "file://servername/path1/path" };
            yield return new object[] { @"\\servername\path1", "?query", @"\\servername/?query" };

            // Unix path
            if (!s_isWindowsSystem)
            {
                // Implicit file
                yield return new object[] { "/path1", "/path", "/path" };
                yield return new object[] { "/path1/path2", "/path", "/path" };
                yield return new object[] { "/pathold", "path", "/path" };
            }

            // IPv6
            yield return new object[] { "http://[::1]", "/path", "http://[::1]/path" };
            yield return new object[] { "http://[::1]", @"\path", "http://[::1]/path" };
            yield return new object[] { "http://[::1]", @"path", "http://[::1]/path" };
            yield return new object[] { "http://[::1]:90", "/path", "http://[::1]:90/path" };
            yield return new object[] { @"\\[::1]/", "path", @"\\[::1]/path" };

            // Unknown
            yield return new object[] { "unknown:", "C:/x", "unknown:/C:/x" };
            yield return new object[] { "unknown:", "//host/path?query#fragment", "unknown://host/path?query#fragment" };
            yield return new object[] { "unknown:", "path", "unknown:path" };
            yield return new object[] { "unknown:pathold", "path", "unknown:path" };

            // Telnet
            yield return new object[] { "telnet://username:password@host:10", "path", "telnet://username:password@host:10/path" };
        }

        [Theory]
        [MemberData(nameof(Combine_AbsoluteUri_TestData))]
        public void Combine(string uriString1, string uriString2, string expectedUriString)
        {
            var baseUri = new AbsoluteUri(uriString1);
            var expected = new AbsoluteUri(expectedUriString);

            var uri = baseUri.Combine(uriString2);
            Assert.Equal(expected, uri);
        }
    }
}