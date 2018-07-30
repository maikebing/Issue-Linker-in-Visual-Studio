#define HIDING_TEXT
using IntraTextAdornmentSample;
using Issue_Linker.Core;
using Issue_Linker.Visuals;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue_Linker.Detector
{
    class LinkTagger
#if HIDING_TEXT
        : IntraTextAdornmentTagTransformer<Tag, LinkAdornment>
#else
        : IntraTextAdornmentTagger<Tag, LinkAdornment>
#endif
    {
        internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<Tag>> tagger)
        {
            return view.Properties.GetOrCreateSingletonProperty<LinkTagger>(
                () => new LinkTagger(view, tagger.Value));
        }

#if HIDING_TEXT
        private LinkTagger(IWpfTextView view, ITagAggregator<Tag> tagger)
            : base(view, tagger)
        {
        }

        public override void Dispose()
        {
            base.view.Properties.RemoveProperty(typeof(LinkTagger));
        }
#else
        private ITagAggregator<Tag> tagger;

        private LinkTagger(IWpfTextView view, ITagAggregator<Tag> colorTagger)
            : base(view)
        {
            this.tagger = colorTagger;
        }

        public void Dispose()
        {
            tagger.Dispose();

            view.Properties.RemoveProperty(typeof(LinkTagger));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, Tag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;

            ITextSnapshot snapshot = spans[0].Snapshot;

            var tags = tagger.GetTags(spans);

            foreach (IMappingTagSpan<Tag> dataTagSpan in tags)
            {
                NormalizedSnapshotSpanCollection colorTagSpans = dataTagSpan.Span.GetSpans(snapshot);

                // Ignore data tags that are split by projection.
                // This is theoretically possible but unlikely in current scenarios.
                if (colorTagSpans.Count != 1)
                    continue;

                SnapshotSpan adornmentSpan = new SnapshotSpan(colorTagSpans[0].Start, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
            }
        }
#endif

        protected override LinkAdornment CreateAdornment(Tag dataTag, SnapshotSpan span)
        {
            return new LinkAdornment(dataTag);
        }

        protected override bool UpdateAdornment(LinkAdornment adornment, Tag dataTag)
        {
            adornment.Update(dataTag);
            return true;
        }

    }
}
