using Issue_Linker.Core;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue_Linker
{
    class GitHubLink : Link
    {
        public GitHubLink(int tagNumber) : base(tagNumber)
        {
            TagNumber = tagNumber;
        }

        public override async Task<bool> CallAPIAsync()
        {
            // call github api
            Tuple<Issue, PullRequest> result = await new GitHubAPI().GetObjectAsync(TagNumber);

            if (result.Item1.Id != 0)
            {
                this.State = result.Item1.State.StringValue;
                foreach (var label in result.Item1.Labels)
                {
                    this.Labels.Add(new Core.Label(label.Name, label.Color));
                }

                return true;
            }
            return false;
        }

    }
}
