using System;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Shell;
using System.Threading.Tasks;
using Octokit;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Issue_Linker.Visuals;
using IntraTextAdornmentSample;

namespace Issue_Linker
{
    internal sealed class TagDetector : RegexTagger<Tag>
    {

        public TagDetector(ITextBuffer buffer) : base(buffer, new[] { new Regex(@"#\d+ "), new Regex(@"#\w+-\d+ ") })
        {
        }

        protected override Tag TryCreateTagForMatch(Match match)
        {
            Link link = new GitHubLink(0);
            ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                link = await DetectionAsync(match);

            });

            return new Tag(link);
        }

        /// <summary>
        /// Detects the pull request/issue by #number
        /// </summary>
        /// <param name="line">Line to add the adornments</param>
        private async Task<Link> DetectionAsync(Match match)
        {
            if (match.Success)
            {
                if (!match.Value.Contains("-"))
                {
                    int number = Int32.Parse(match.Value.Split('#')[1]);
                    GitHubLink githubLink = new GitHubLink(number);
                    await githubLink.CallAPIAsync();
                    return githubLink;
                }
                else
                {
                    //todo jira
                    return null;
                }


                // call jira api

            }
            else return null;
        }

    }
}

