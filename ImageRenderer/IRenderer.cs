﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace ImageRenderer
{
    public interface IRenderer
    {
        Bitmap RenderBitmap(Collection<MultiPictureEl> piactureElements);
    }
}