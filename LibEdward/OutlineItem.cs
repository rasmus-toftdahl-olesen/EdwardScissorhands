using System;
using System.Collections.Generic;
using System.Text;
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
               /*
               dynamic d = m_document.BuiltInDocumentProperties;
               return d[WdBuiltInProperty.wdPropertyTitle].Value;
               */
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
                  m_contentRange = m_range.Document.Range(m_title.Range.End, m_range.End);
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
               
               foreach (InlineShape inlineShape in cRange.InlineShapes)
               {
                  inlineShape.Range.CopyAsPicture();
                  object pngData = Clipboard.GetData("PNG");
                  if (pngData is System.IO.MemoryStream)
                  {
                     usedRanges.Add(new KeyValuePair<Range, Content>(inlineShape.Range, new PngImageContent((System.IO.MemoryStream) pngData, inlineShape.AlternativeText, inlineShape.Title)));
                  }
                  else
                  {
                     usedRanges.Add(new KeyValuePair<Range, Content>(inlineShape.Range, new TextContent(String.Format("INTERNAL ERROR: Could not convert shape to PNG image."))));
                  }
                  Clipboard.Clear();
               }

               foreach (Paragraph paragraph in cRange.ListParagraphs)
               {
                  usedRanges.Add(new KeyValuePair<Range, Content>(paragraph.Range, new TextContent(paragraph.Range.Text, paragraph.Range.ListFormat.ListLevelNumber, paragraph.Range.ListFormat.ListType)));
               }

               if (usedRanges.Count > 0)
               {
                  usedRanges.Sort(new Comparison<KeyValuePair<Range, Content>>(SortRanges));

                  int currentEnd = cRange.Start;
                  foreach (KeyValuePair<Range, Content> item in usedRanges)
                  {
                     if (item.Key.Start != currentEnd)
                     {
                        yield return new TextContent(item.Key.Document.Range(currentEnd, item.Key.Start).Text);
                     }
                     yield return item.Value;
                     currentEnd = item.Key.End;
                  }
                  if (currentEnd != cRange.End)
                  {
                     yield return new TextContent(cRange.Document.Range(currentEnd, cRange.End).Text);
                  }
               }
               else
               {
                  yield return new TextContent(cRange.Text);
               }
            }
         }
      }

      private static int SortRanges(KeyValuePair<Range, Content> a, KeyValuePair<Range, Content> b)
      {
         return a.Key.Start.CompareTo(b.Key.Start);
      }
   }
}
