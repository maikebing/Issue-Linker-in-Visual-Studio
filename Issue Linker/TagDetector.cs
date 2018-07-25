using System;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Shell;
using System.Threading.Tasks;
using Octokit;

namespace Issue_Linker
{
    /// <summary>
    /// TagDetector places red boxes behind all the "a"s in the editor window
    /// </summary>
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

        /// <summary>
        /// Adornment brush.
        /// </summary>
        private readonly Brush brush;

        /// <summary>
        /// Adornment pen.
        /// </summary>
        private readonly Pen pen;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDetector"/> class.
        /// </summary>
        /// <param name="view">Text view to create the adornment for</param>
        public TagDetector(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            this.layer = view.GetAdornmentLayer("TagDetector");

            this.view = view;
            this.view.LayoutChanged += this.OnLayoutChanged;

            // Create the pen and brush to color the box behind the a's
            this.brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xff));
            this.brush.Freeze();

            var penBrush = new SolidColorBrush(Colors.Red);
            penBrush.Freeze();
            this.pen = new Pen(penBrush, 0.5);
            this.pen.Freeze();
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
            IWpfTextViewLineCollection textViewLines = this.view.TextViewLines;

            // Loop through each character, and place a box around any 'a'
            //for (int charIndex = line.Start; charIndex < line.End; charIndex++)
            var snap = this.view.TextSnapshot;
            foreach (var l in this.view.TextSnapshot.Lines)
            {
                string text = l.GetText();
                if (text.Contains("#"))
                {
                    // github regex
                    Regex regex = new Regex(@"#\d+ ");
                    Match match = regex.Match(text);
                    if (match.Success)
                    {
                        int number = Int32.Parse(match.Value.Split('#')[1]);
                        // call github api
                        Console.WriteLine("Calling the GitHub API, it might take a second");
                        Tuple<Issue, PullRequest> result = await new GitHubAPI().GetObjectAsync(number);
                        //Do graphics with the 
                        

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
}
