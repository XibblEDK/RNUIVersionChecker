using Rage;
using LSPD_First_Response.Mod.API;
using Octokit;
using System.Diagnostics;

namespace RNUIVersionChecker
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            List<Version> version = CheckRNUINewerVersionAsync();
            if (version != null )
            {

                if (version[0] > version[1])
                {
                    Game.LogTrivial("Clients RNUI version is NOT UP-TO-DATE! Please download and install the latest release.");
                }
                else if (version[0] < version[1])
                {
                    Game.LogTrivial("Clients RNUI version is HIGHER than latest RNUI release?");
                }
                else if (version[0] == version[1])
                {
                    Game.LogTrivial("Clients RNUI version is UP-TO-DATE!");
                }
            }
        }

        public override void Finally()
        {

        }

        public List<Version> CheckRNUINewerVersionAsync()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("SomeName"));
            Task<Release> release = client.Repository.Release.GetLatest("alexguirre", "RAGENativeUI");

            List<Version> versions = new List<Version>();
            Version latestGithubVersion = new Version(release.Result.TagName);
            var userDirectory = Directory.GetCurrentDirectory();
            var exists = File.Exists(userDirectory + @"\RAGENativeUI.dll");
            if (exists)
            {
                Game.LogTrivial("Found RAGENativeUI.dll");
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(userDirectory + @"\RAGENativeUI.dll");
                versions.Add(latestGithubVersion);
                versions.Add(Version.Parse(fileVersionInfo.ProductVersion));
                return versions;
            }
            else
            {
                Game.LogTrivial("Couldn't find RAGENativeUI.dll in GTAV folder. Please make sure the .dll file is inside the GTAV folder!");
                return null;
            }
        }
    }
}