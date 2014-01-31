using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace LibEdward
{
   public class OutlineItem
   {
      private Document m_document;
      private Paragraph m_title;
      private Range m_range;
      private Range m_contentRange;
      private OutlineItem m_parent;
      private List<OutlineItem> m_children;
      private int m_startParagraphIndex;
      private int m_endParagraphIndex;

      public int Level
      {
         get
         {
            if (m_title == null)
            {
               return 0;
            }
            else
            {
               return (int) m_title.OutlineLevel;
            }
         }
      }

      public string Title
      {
         get
         {
            if (m_title == null)
            {
               return "NO TITLE";
            }
            else
            {
               return m_title.Range.Text.Trim();
            }
         }
         /*
         set
         {
            WdOutlineLevel levelBefore = m_title.OutlineLevel;
            m_title.Range.Text = value + "\r";
            Paragraph paragraph = m_range.Document.Paragraphs[m_startParagraphIndex];
            paragraph.OutlineLevel = levelBefore;
            m_title = paragraph;
         }
          */
      }
      public Range Range { get { return m_range; } }
      public OutlineItem Parent { get { return m_parent; } }
      public IList<OutlineItem> Children { get { return m_children; } }

      public Range ContentRange
      {
         get
         {
            if (m_contentRange == null)
            {
               if (m_title == null)
               {
                  return null;
               }
               if (m_children.Count == 0)
               {
                  if (m_title.Range.End == m_range.End)
                  {
                     return null;
                  }
                  else
                  {
                     m_contentRange = m_range.Document.Range(m_title.Range.End, m_range.End);
                  }
               }
               else
               {
                  if (m_title.Range.End == m_children[0].Range.Start)
                  {
                     return null;
                  }
                  else
                  {
                     m_contentRange = m_range.Document.Range(m_title.Range.End, m_children[0].Range.Start - 1);
                  }
               }
            }
            return m_contentRange;
         }
      }

      public Document Document
      {
         get
         {
            return m_range.Document;
         }
      }

      public string TextContent
      {
         get
         {
            Range cRange = this.ContentRange;
            if (cRange == null)
            {
               return String.Empty;
            }
            else
            {
               return cRange.Text;
            }
         }
      }

      public int PageNumber
      {
         get
         {
            if (m_title != null)
            {
               return Convert.ToInt32(m_title.Range.get_Information(WdInformation.wdActiveEndPageNumber));
            }
            else
            {
               return Convert.ToInt32(m_range.get_Information(WdInformation.wdActiveEndPageNumber));
            }
         }
      }

      internal OutlineItem(Document _document)
         : this(_document.Content, null, 0)
      {
         m_document = _document;
      }

      internal OutlineItem(Paragraph _title, OutlineItem _parent, int _startParagraphIndex)
         : this(_title.Range, _parent, _startParagraphIndex)
      {
         m_title = _title;
      }

      private OutlineItem(Range _range, OutlineItem _parent, int _startParagraphIndex)
      {
         m_document = null;
         m_title = null;
         m_range = _range;
         m_children = new List<OutlineItem>();
         m_parent = _parent;
         if (m_parent != null)
         {
            m_parent.m_children.Add(this);
         }
         m_startParagraphIndex = _startParagraphIndex;
         m_endParagraphIndex = _startParagraphIndex;
      }

      public void Cut()
      {
         m_range.Cut();
      }


      public void Promote()
      {
         if (m_title != null)
         {
            m_title.OutlinePromote();
         }
      }

      public void Demote()
      {
         if (m_title != null)
         {
            m_title.OutlineDemote();
         }
      }

      public string Path(string _seperator)
      {
         if (m_parent == null || m_parent.Parent == null)
         {
            return this.Title;
         }
         else
         {
            return m_parent.Path(_seperator) + _seperator + this.Title;
         }
      }

      public void AppendChild(string title)
      {
         int level = this.Level;
         int endBefore = m_range.End;
         if (endBefore == m_range.Document.Content.End)
         {
            m_range.InsertAfter("\r" + title + "\r");
         }
         else
         {
            m_range.InsertAfter(title + "\r");
         }
         Paragraph newParagraph = m_range.Document.Paragraphs[m_endParagraphIndex + 1];
         while (((int) newParagraph.OutlineLevel) != level + 1)
         {
            if (((int) newParagraph.OutlineLevel) > level)
            {
               newParagraph.OutlinePromote();
            }
            else
            {
               newParagraph.OutlineDemote();
            }
         }
      }

      public OutlineItem GetChild(string title)
      {
         foreach (OutlineItem item in m_children)
         {
            if (item.Title.Trim() == title)
            {
               return item;
            }
         }
         return null;
      }

      internal void UpdateEnd(int _newEnd, int _endParagraph)
      {
         m_range = m_range.Document.Range(m_range.Start, _newEnd);
         m_endParagraphIndex = _endParagraph;
      }

      public IEnumerable<Content> Content
      {
         get
         {
            Range cRange = this.ContentRange;
            if (cRange != null)
            {
               List<KeyValuePair<Range, Content>> usedRanges = new List<KeyValuePair<Range, Content>>();
               foreach (Table table in cRange.Tables)
               {
                  usedRanges.Add(new KeyValuePair<Range, Content>(table.Range, new TableContent(table)));
               }
               foreach (Shape shape in cRange.ShapeRange)
               {
                  InlineShape inlineShape = shape.ConvertToInlineShape();
                  usedRanges.Add(ProcessInlineShape(inlineShape));
               }
               foreach (InlineShape inlineShape in cRange.InlineShapes)
               {
                  usedRanges.Add(ProcessInlineShape(inlineShape));
               }

               foreach (Paragraph paragraph in cRange.ListParagraphs)
               {
                  object style = paragraph.get_Style();
                  string styleName = null;
                  if (style is Style)
                  {
                     styleName = (style as Style).NameLocal;
                  }
                  usedRanges.Add(new KeyValuePair<Range, Content>(paragraph.Range, new TextContent(paragraph.Range.Text, paragraph.Range.ListFormat.ListLevelNumber, paragraph.Range.ListFormat.ListType, styleName)));
               }

               if (usedRanges.Count > 0)
               {
                  usedRanges.Sort(new Comparison<KeyValuePair<Range, Content>>(SortRanges));

                  int currentEnd = cRange.Start;
                  foreach (KeyValuePair<Range, Content> item in usedRanges)
                  {
                     if (item.Key.Start != currentEnd)
                     {
                        Range textRange = item.Key.Document.Range(currentEnd, item.Key.Start);
                        string text = textRange.Text;
                        foreach (TextContent paragraph in ListParagraphsFor(textRange))
                        {
                           yield return paragraph;
                        }
                     }
                     yield return item.Value;
                     currentEnd = item.Key.End;
                  }
                  if (currentEnd != cRange.End && currentEnd < cRange.End)
                  {
                     foreach (TextContent paragraph in ListParagraphsFor(cRange.Document.Range(currentEnd, cRange.End)))
                     {
                        yield return paragraph;
                     }
                  }
               }
               else
               {
                  foreach (TextContent content in ListParagraphsFor(cRange))
                  {
                     yield return content;
                  }
               }
            }
         }
      }

      private static int SortRanges(KeyValuePair<Range, Content> a, KeyValuePair<Range, Content> b)
      {
         return a.Key.Start.CompareTo(b.Key.Start);
      }

      private static IEnumerable<TextContent> ListParagraphsFor(Range _range)
      {
         string text = _range.Text;
         if (text != null)
         {
            text = text.Trim('\r');
            string[] paragraphParts = text.Split('\r');
            int currentStart = _range.Start;
            foreach (string paragraphText in paragraphParts)
            {
               if (paragraphText == "/")
               {
                  yield return new TextContent("SeqZapManualGenerator: Unhandled figure/shape/???", "seqzap-manual-generator-error");
               }
               else
               {
                  Range paragraphRange = _range.Document.Range(currentStart, currentStart + paragraphText.Length);
                  object paragraphStyle = paragraphRange.get_Style();
                  string style = GetStyleName(paragraphStyle);

                  yield return new TextContent(paragraphText, style);

                  currentStart += paragraphText.Length + 1;
               }
            }
         }
      }

      private static string GetStyleName(object _style)
      {
         if (_style is Style)
         {
            return (_style as Style).NameLocal;
         }
         else
         {
            return null;
         }
      }

      private static KeyValuePair<Range, Content> ProcessInlineShape(InlineShape _shape)
      {
         float width = _shape.Width;
         float height = _shape.Height;
         string altText = "";
         string title = "";
         try
         {
            float scaleWidth = _shape.ScaleWidth;
            if (scaleWidth > 0.0)
            {
               //width *= (scaleWidth / 100.0f);
            }
         }
         catch
         {
         }
         try
         {
            float scaleHeight = _shape.ScaleHeight;
            if (scaleHeight > 0.0)
            {
               //height *= (scaleHeight / 100.0f);
            }
         }
         catch
         {
         }
         try
         {
            altText = _shape.AlternativeText;
         }
         catch
         {
         }
         try
         {
            title = _shape.Title;
         }
         catch
         {
         }
         PngImageContent imageContent = null;
         _shape.Range.CopyAsPicture();
         object pngData = Clipboard.GetData("PNG");
         if (pngData is System.IO.MemoryStream)
         {
            imageContent = new PngImageContent((System.IO.MemoryStream) pngData, altText, title, width, height);
         }
         else
         {
            if (Clipboard.ContainsData(DataFormats.EnhancedMetafile))
            {
               if (ClipboardFunctions.OpenClipboard(IntPtr.Zero))
               {
                  width *= 1.5f;
                  height *= 1.5f;
                  IntPtr data = ClipboardFunctions.GetClipboardData(DataFormats.GetFormat(DataFormats.EnhancedMetafile).Id);
                  System.Drawing.Imaging.Metafile metaFile = new System.Drawing.Imaging.Metafile(data, true);
                  ClipboardFunctions.CloseClipboard();
                  System.Drawing.Bitmap pngImage = new System.Drawing.Bitmap((int) width, (int) height);
                  using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(pngImage))
                  {
                     g.DrawImage(metaFile, new System.Drawing.Rectangle(0, 0, (int) width, (int) height));
                  }
                  metaFile.Dispose();
                  System.IO.MemoryStream pngDataStream = new System.IO.MemoryStream();
                  pngImage.Save(pngDataStream, System.Drawing.Imaging.ImageFormat.Png);
                  pngDataStream.Seek(0, System.IO.SeekOrigin.Begin);
                  imageContent = new PngImageContent(pngDataStream, altText, title, width, height);
                  pngImage.Dispose();
               }
            }
         }
         Clipboard.Clear();
         if (imageContent == null)
         {
            return new KeyValuePair<Range, Content>(_shape.Range, new TextContent(String.Format("INTERNAL ERROR: Could not convert shape to PNG image."), "seqzap-manual-generator-error"));
         }
         else
         {
            return new KeyValuePair<Range, Content>(_shape.Range, imageContent);
         }
      }

      public class ClipboardFunctions
      {
         [DllImport("user32.dll", EntryPoint = "OpenClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
         public static extern bool OpenClipboard(IntPtr hWnd);

         [DllImport("user32.dll", EntryPoint = "EmptyClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
         public static extern bool EmptyClipboard();

         [DllImport("user32.dll", EntryPoint = "SetClipboardData", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
         public static extern IntPtr SetClipboardData(int uFormat, IntPtr hWnd);

         [DllImport("user32.dll", EntryPoint = "CloseClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
         public static extern bool CloseClipboard();

         [DllImport("user32.dll", EntryPoint = "GetClipboardData", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
         public static extern IntPtr GetClipboardData(int uFormat);

         [DllImport("user32.dll", EntryPoint = "IsClipboardFormatAvailable", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
         public static extern short IsClipboardFormatAvailable(int uFormat);
      }
   }
}
