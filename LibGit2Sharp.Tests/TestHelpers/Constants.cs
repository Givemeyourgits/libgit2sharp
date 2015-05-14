﻿using System;
using System.Diagnostics;
using System.IO;

namespace LibGit2Sharp.Tests.TestHelpers
{
    public static class Constants
    {
        public static readonly string TemporaryReposPath = BuildPath();
        public const string UnknownSha = "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef";
        public static readonly Identity Identity = new Identity("A. U. Thor", "thor@valhalla.asgard.com");
        public static readonly Signature Signature = new Signature(Identity, new DateTimeOffset(2011, 06, 16, 10, 58, 27, TimeSpan.FromHours(2)));
        public static readonly Signature Signature2 = new Signature("nulltoken", "emeric.fermas@gmail.com", DateTimeOffset.Parse("Wed, Dec 14 2011 08:29:03 +0100"));

        // Populate these to turn on live credential tests:  set the
        // PrivateRepoUrl to the URL of a repository that requires
        // authentication. Define PrivateRepoCredentials to return an instance of
        // UsernamePasswordCredentials (for HTTP Basic authentication) or
        // DefaultCredentials (for NTLM/Negotiate authentication).
        //
        // For example:
        // public const string PrivateRepoUrl = "https://github.com/username/PrivateRepo";
        // ... return new UsernamePasswordCredentials { Username = "username", Password = "swordfish" };
        //
        // Or:
        // public const string PrivateRepoUrl = "https://tfs.contoso.com/tfs/DefaultCollection/project/_git/project";
        // ... return new DefaultCredentials();

        public const string PrivateRepoUrl = "";

        public static Credentials PrivateRepoCredentials(string url, string usernameFromUrl,
                                                         SupportedCredentialTypes types)
        {
            return null;
        }

        public static string BuildPath()
        {
            string tempPath = null;

            var unixPath = Type.GetType("Mono.Unix.UnixPath, Mono.Posix, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");

            if (unixPath != null)
            {
                // We're running on Mono/*nix. Let's unwrap the path
                tempPath = (string)unixPath.InvokeMember("GetCompleteRealPath",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy |
                    System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public,
                    null, unixPath, new object[] { Path.GetTempPath() });
            }
            else
            {
                const string LibGit2TestPath = "LibGit2TestPath";
                // We're running on .Net/Windows
                if (Environment.GetEnvironmentVariables().Contains(LibGit2TestPath))
                {
                    Trace.TraceInformation("{0} environment variable detected", LibGit2TestPath);
                    tempPath = Environment.GetEnvironmentVariables()[LibGit2TestPath] as String;
                }

                if (String.IsNullOrWhiteSpace(tempPath) || !Directory.Exists(tempPath))
                {
                    Trace.TraceInformation("Using default test path value");
                    tempPath = Path.GetTempPath();
                }
            }

            string testWorkingDirectory = Path.Combine(tempPath, "LibGit2Sharp-TestRepos");
            Trace.TraceInformation("Test working directory set to '{0}'", testWorkingDirectory);
            return testWorkingDirectory;
        }
    }
}
