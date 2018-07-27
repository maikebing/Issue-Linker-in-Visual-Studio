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
        public GitHubLink(int tagNumber, IWpfTextView view) : base(tagNumber, view)
        {
            TagNumber = tagNumber;
            View = view;
        }

        public override async Task<bool> CallAPIAsync()
        {
            // call github api
            Tuple<Issue, PullRequest> result = await new GitHubAPI().GetObjectAsync(TagNumber);
            //Do graphics with the result
            CreateVisuals(X, Y);

            return true;
        }

        public override void CreateVisuals(double x, double y)
        {
           Visuals.Add(new CreateVisuals(View,x,y));
       
        }

        public override void Redraw(double x, double y)
        {
            foreach (var visual in Visuals)
            {
                visual.Redraw(x, y);
            }
        }
    }
}
