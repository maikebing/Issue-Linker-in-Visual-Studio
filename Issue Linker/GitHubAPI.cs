using Octokit;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace Issue_Linker
{
    class GitHubAPI
    {
        //TODO: GET NAME OF THE GIT REPOSITORY
        GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("Issue-Linker-in-Visual-Studio"));

        public async Task<Tuple<Issue, PullRequest>> GetObjectAsync(int number)
        {

            //string solutionName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            string repositoryName = "Issue-Linker-in-Visual-Studio";
            //TODO: Get the repository from git
            var repo = await gitHubClient.Repository.Get("AndreiMaga", repositoryName);

            var issue = await GetIssueAsync(repo.Id, number);
            var pullRequest = await GetPullRequestAsync(repo.Id, number);
            return new Tuple<Issue, PullRequest>(issue, pullRequest);
        }

        private async Task<Issue> GetIssueAsync(long id, int number)
        {
            try
            {
                return await gitHubClient.Issue.Get(id, number);
            }
            catch
            {
                return new Issue();
            }

        }

        private async Task<PullRequest> GetPullRequestAsync(long id, int number)
        {
            try
            {
                return await gitHubClient.PullRequest.Get(id, number);
            }
            catch
            {
                return new PullRequest();
            }
        }
    }
}
