using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace LibEdward
{
   public class PngImageContent : Content
   {
      private byte[] m_imageData;
      private string m_altText;
      private string m_title;
      private float m_width;
      private float m_height;

      public override ContentType Type { get { return ContentType.ImagePng; } }
      public string AltText { get { return m_altText; } }
      public string Title { get { return m_title; } }

      /// <summary>
      /// Return the width of the image in points (pt).
      /// </summary>
      public float Width { get { return m_width; } }

      /// <summary>
      /// Return the height of the image in points (pt).
      /// </summary>
      public float Height { get { return m_height; } }

      internal PngImageContent(MemoryStream _dataStream, string _altText, string _title, float _width, float _height)
      {
         m_imageData = new byte[_dataStream.Length];
         _dataStream.Read(m_imageData, 0, m_imageData.Length);
         m_altText = _altText;
         m_title = _title;
         m_width = _width;
         m_height = _height;
      }

      public void Write(Stream _destination)
      {
         _destination.Write(m_imageData, 0, m_imageData.Length);
      }

      public PngImageContent Scale(float _percentage)
      {

         using (MemoryStream inStream = new MemoryStream(m_imageData))
         {
            using (Image oldImage = Image.FromStream(inStream))
            {
               using (Image resizedImage = ImageUtilities.ResizeImage(oldImage, (int) (oldImage.Width * _percentage), (int) (oldImage.Height * _percentage)))
               {
                  using (MemoryStream outStream = new MemoryStream())
                  {
                     resizedImage.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
                     outStream.Seek(0, SeekOrigin.Begin);
                     return new PngImageContent(outStream, m_altText, m_title, m_width * _percentage, m_height * _percentage);
                  }
               }
            }
         }
      }
   }
}
