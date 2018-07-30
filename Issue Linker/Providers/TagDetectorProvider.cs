using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Issue_Linker.Detector;
using Issue_Linker.Visuals;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Issue_Linker
{
    /// <summary>
    /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
    /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
    /// </summary>
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(Tag))]
    internal sealed class TagDetectorProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            return buffer.Properties.GetOrCreateSingletonProperty(() => new TagDetector(buffer)) as ITagger<T>;
        }
    }
}
