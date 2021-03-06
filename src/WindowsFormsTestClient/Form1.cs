﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using GPExtractor;
using ImageRenderer;
using Types;
using ImageLayout = GPExtractor.ImageLayout;

namespace WindowsFormsTestClient
{
    public partial class Form1 : Form
    {
        private void Do()
        {
            var filePath = @"../../../../gp/test.gp";

            var imagePaletteBytes = new Extractor().GetPaletteBytes(filePath);
            var extractResult = new Extractor().GetImagesFromOutput(filePath).ToList();
            
            IRenderer renderer = new BitmapRenderer();
            
            var paletteBytes = File.ReadAllBytes(@"../../../../palette/0/agew_1.pal");

            var rawParser = new RawParser();

            var colorCollection = rawParser.GetColorCollectionFromPalleteFile(paletteBytes);

            var imagePaletteColors = ImageGenerator.OffsetsToColors(imagePaletteBytes, colorCollection);

            var nationColorOffset = NationColorOffset.Blue;

            //Watch(extractResult);

            var bitmapList = DrawImage(extractResult, nationColorOffset, extractResult.Count, rawParser, renderer, imagePaletteColors, colorCollection);

            var itemsForListBox = Enumerable.Range(0, bitmapList.Count).Select(it => it.ToString()).ToList();

            listBox1.DataSource = itemsForListBox;

            listBox1.SelectedIndexChanged += (sender, args) =>
            {
                if ((sender as ListBox).SelectedIndex > -1)
                pictureBox2.Image = bitmapList[(sender as ListBox).SelectedIndex];
            };

            listBox1.ClearSelected();
            listBox1.SelectedIndex = 0;
        }
        
        private IList<Bitmap> DrawImage(IList<ImageLayout> extractResult, NationColorOffset nationColorOffset, int numberOfImages, RawParser rawParser, IRenderer renderer, Color[] imagePaletteArray,
            Color[] generalPalleteArray)
        {
            return Helper.WithMeasurement(
                () =>
                {
                    var bitMapCollection = new Collection<Bitmap>();

                    //var idsForProcessing = new[] { 0 };
                    var idsForProcessing = Enumerable.Range(1, numberOfImages - 1).ToList();

                    foreach (var imageNumber in idsForProcessing)
                    {
                        Debug.WriteLine("***************************** " + imageNumber);
                        Watch(extractResult[imageNumber].PartialLayouts.ToList());
                    }
                    
                    foreach (int i in idsForProcessing)
                    {
                        Helper.WithMeasurement(() =>
                        {
                            try
                            {
                                var bitMap = new Runner().Run(extractResult, nationColorOffset, i, rawParser, renderer, imagePaletteArray, generalPalleteArray);
                                bitMapCollection.Add(bitMap);
                            }
                            catch (Exception ex)
                            {

                            }
                        }, "Image", onFinish: elapsed => richTextBox1.Text += String.Format("[{0,4}] - {1:g}\n", i, elapsed));
                    }

                    return bitMapCollection;

                },
                name: "Run",
                onFinish: elapsed => richTextBox1.Text += String.Format("Finished in {0:c}\n ", elapsed));
                
        }

        private void Watch(List<ImageLayoutInfo> extractResult)
        {
            var headerBytes = extractResult.Select(it => it).ToList();

            foreach (var imageLayoutInfo in headerBytes)
            {
                Helper.DumpArray(imageLayoutInfo.HeaderBytes, (int)0);
            }
        }

        public Form1()
        {
            InitializeComponent();

            Load += (sender, args) => Do();
        }
    }
}
