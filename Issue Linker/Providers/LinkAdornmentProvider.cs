using Issue_Linker.Detector;
using Issue_Linker.Visuals;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue_Linker.Providers
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("text")]
    [ContentType("projection")]
    [TagType(typeof(IntraTextAdornmentTag))]
    class LinkAdornmentProvider: IViewTaggerProvider
    {
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
        [Import]
        internal IBufferTagAggregatorFactoryService BufferTagAggregatorFactoryService;
#pragma warning restore 649
        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (textView == null)
                throw new ArgumentNullException("textView");

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (buffer != textView.TextBuffer)
                return null;

            return LinkTagger.GetTagger(
                (IWpfTextView)textView,
                new Lazy<ITagAggregator<Tag>>(
                    () => BufferTagAggregatorFactoryService.CreateTagAggregator<Tag>(textView.TextBuffer)))
                as ITagger<T>;
        }
    }
}
