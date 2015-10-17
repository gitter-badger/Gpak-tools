﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPExtractor;
using Types;

namespace ImageRenderer
{
    public class Runner
    {
        public void Run(ExtractorResult extractResult, RawParser rawParser, IRenderer renderer, List<Color> imagePaletteColors)
        {
            var imageView = new ImageView(600, 600);

            Helper.WithMeasurement(renderer.SetupCanvas, name: "SetupCanvas");

            var imageGenerator = new ImageGenerator();

            foreach (var layout in extractResult.LayoutCollection.Take(2))
            {
                int offset = 0;

                ImageLayoutInfo layout1 = layout;

                var firstPartBlocks = Helper.WithMeasurement(() =>
                {
                    var rawFirstPartBlocks = rawParser.ParseRawBlockGroups(layout1.Bytes, out offset);
                    return rawFirstPartBlocks.ConvertToAbsoluteCoordinatesBlocks();

                }, "firstPartBlocks");

                var secondPartBlocks = Helper.WithMeasurement(() => rawParser.GetRawColorBlocks(layout1.Bytes, offset, (int)layout1.GlobalByteOffsetStart + offset + layout1.HeaderBytes.Length), "secondPartBlocks");

                Helper.WithMeasurement(() => imageGenerator.RenderCounterBlocksOnBitmap(imageView, firstPartBlocks, secondPartBlocks, layout1, imagePaletteColors), "RenderCounterBlocksOnBitmap");

                renderer.RenderImage(imageView);
            }
        }
    }
}