using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibEdward
{
   public class PngImageContent : Content
   {
      private byte[] m_imageData;
      private string m_altText;
      private string m_title;
      
      public override ContentType Type { get { return ContentType.ImagePng; } }
      public string AltText { get { return m_altText; } }
      public string Title { get { return m_title; } }

      internal PngImageContent(MemoryStream _dataStream, string _altText, string _title)
      {
         m_imageData = new byte[_dataStream.Length];
         _dataStream.Read(m_imageData, 0, m_imageData.Length);
         m_altText = _altText;
         m_title = _title;
      }

      public void Write(Stream _destination)
      {
         _destination.Write(m_imageData, 0, m_imageData.Length);
      }
   }
}
