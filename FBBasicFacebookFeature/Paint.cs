// -----------------------------------------------------------------------
// <copyright file="Paint.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace FBBasicFacebookFeature
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Paint
    {
        public Color LastColor { get; set; }

        public bool Paints { get; set; }

        public bool Brush { get; set; }

        public bool Pencil { get; set; }

        public bool Eraser { get; set; }

        public Graphics GraphicsPanel { get; set; }

        public Graphics GraphicsBmp { get; set; }

        public float WidthDraw { get; set; }

        public float HighDraw { get; set; }

        public SolidBrush SolidBrush { get; set; }

        public Bitmap Bmp { get; set; }

        public readonly string r_Path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;

        public Paint()
        {
            Paints = false;
            Brush = false;
            Pencil = false;
            Eraser = false;
            SolidBrush = new SolidBrush(Color.Black);
            WidthDraw = 3;
            HighDraw = 3;
            LastColor = Color.Black;
        }

        /// <summary>
        /// Sets each tool of its used
        /// </summary>
        /// <param name="i_Pencil">sets true or false if pencil used</param>
        /// <param name="i_Brush">sets true or false if brush used</param>
        /// <param name="i_Eraser">sets true or false if eraser used</param>
        public void ChangeTools(bool i_Pencil, bool i_Brush, bool i_Eraser)
        {
            Pencil = i_Pencil;
            Brush = i_Brush;
            Eraser = i_Eraser;
        }

        public byte[] ConvertBitMapToByteArray()
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(Bmp, typeof(byte[]));
        }
    }
}
