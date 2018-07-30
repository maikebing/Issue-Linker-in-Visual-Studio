using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue_Linker.Visuals
{
    class Tag : ITag
    {

        public Tag(Link link)
        {
            Link = link;
        }

        internal readonly Link Link;
    }
}
