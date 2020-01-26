using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blamite.IO;
using Blamite.Serialization;

namespace Blamite.Blam.FirstGen.Structures
{
    class FirstGenTag : ITag
    {

        public FirstGenTag(StructureValueCollection values, FileSegmentGroup metaArea, Dictionary<int, ITagGroup> groupsById)
        {
            Load(values, metaArea, groupsById);
        }


        public ITagGroup Group { get; set; }

        public SegmentPointer MetaLocation { get; set; }

        public DatumIndex Index { get; private set; }

        public SegmentPointer FileNameOffset { get; set; }


        private void Load(StructureValueCollection values, FileSegmentGroup metaArea, Dictionary<int, ITagGroup> groupsById)
        {
            // Load the tag group by looking up the magic value that's stored
            var groupMagic = (int)values.GetInteger("tag group magic");
            if (groupMagic != -1)
                Group = groupsById[groupMagic];

            Index = new DatumIndex(values.GetInteger("datum index"));

            // TODO (Dragon): see about splitting the filenames into their own segment
            uint nameOffset = (uint)values.GetInteger("name offset");
            if (nameOffset > 0)
                FileNameOffset = SegmentPointer.FromPointer(nameOffset, metaArea);

            uint offset = (uint)values.GetInteger("offset");
            if (offset > 0)
                MetaLocation = SegmentPointer.FromPointer(offset, metaArea);

        }
    }
}
