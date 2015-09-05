using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Types
{
    public static class MultiPictureElMerger
    {
        public static Collection<MultiPictureEl> MergeBlocks(this Collection<MultiPictureEl> elems)
        {
            return new Collection<MultiPictureEl>(elems.Select(MergeBlocks).ToList());
        }

        public static MultiPictureEl MergeBlocks(this MultiPictureEl elem)
        {
            var blocks = elem.Collection;

            if (blocks.Count < 2) return elem;

            var newElem = new MultiPictureEl
            {
                RowIndex = elem.RowIndex
            };

            var currentBlock = blocks[0];

            for (int i = 1; i < blocks.Count; i++)
            {
                var nextBlock = blocks[i];

                if (nextBlock.offsetx == 0)
                {
                    currentBlock.length += nextBlock.length;
                }
                else
                {
                    newElem.Collection.Add(new Block
                    {
                        length = currentBlock.length,
                        offsetx = currentBlock.offsetx
                    });

                    currentBlock = nextBlock;
                }
            }

            newElem.Collection.Add(new Block
            {
                length = currentBlock.length,
                offsetx = currentBlock.offsetx
            });

            return newElem;
        }
    }
}