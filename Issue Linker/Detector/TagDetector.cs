using System;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Shell;
using System.Threading.Tasks;
using Octokit;
using System.Collections.Generic;

namespace Issue_Linker
{
    internal sealed class TagDetector
    {
        /// <summary>
        /// The layer of the adornment.
        /// </summary>
        private readonly IAdornmentLayer layer;

        /// <summary>
        /// Text view where the adornment is created.
        /// </summary>
        private readonly IWpfTextView view;

        private TagDetectorTextViewCreationListener listener;

        internal TagDetectorTextViewCreationListener Listener { get => listener; set => listener = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDetector"/> class.
        /// </summary>
        /// <param name="view">Text view to create the adornment for</param>
        public TagDetector(IWpfTextView view, TagDetectorTextViewCreationListener listener)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            Listener = listener;

            this.layer = view.GetAdornmentLayer("TagDetector");

            this.view = view;
            this.view.LayoutChanged += this.OnLayoutChanged;
        }


        /// <summary>
        /// Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
        /// </summary>
        /// <remarks><para>This event is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
        /// <para>It is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition is called or in response to text or classification changes).</para>
        /// <para>It is also raised whenever the view scrolls horizontally or when its size changes.</para>
        /// </remarks>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await DetectionAsync(line);

                });

            }
        }
        /// <summary>
        /// Detects the pull request/issue by #number
        /// </summary>
        /// <param name="line">Line to add the adornments</param>
        private async System.Threading.Tasks.Task DetectionAsync(ITextViewLine line)
        {
            string text = "";
            for (int charIndex = line.Start; charIndex < line.End; charIndex++)
            {
                text += this.view.TextSnapshot[charIndex];
            }

            if (text.Contains("#") && text.Contains("//"))
            {
                // GitHub Regex
                Regex regex = new Regex(@"#\d+ ");
                Match match = regex.Match(text);
                if (match.Success)
                {
                    int number = Int32.Parse(match.Value.Split('#')[1]);

                    double x = line.Right;
                    double y = line.Top;
                    //if it exists, redraw
                    GitHubLink ghl;
                    try
                    {
                        ghl = (GitHubLink)Listener.Links.Find(m => m.TagNumber == number);
                        ghl.Redraw(x, y);
                    }
                    catch
                    {
                        ghl = new GitHubLink(number, view);
                        bool done = await ghl.CallAPIAsync();
                        ghl.CreateVisuals(x, y);
                        Listener.Links.Add(ghl);
                    }

                    return;
                }

                // jira regex
                regex = new Regex(@"#\w+-\d+ ");
                match = regex.Match(text);
                if (match.Success)
                {
                    // call jira api

                    return;
                }
            }

        }
    }
}
