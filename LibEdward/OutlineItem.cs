using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class OutlineItem
   {
      private Document m_document;
      private Paragraph m_title;
      private Range m_range;
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
               return m_document.Name;
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
      public Range Content { get { return m_range; } }
      public OutlineItem Parent { get { return m_parent; } }
      public IList<OutlineItem> Children { get { return m_children; } }

      public string TextContent
      {
         get
         {
            if (m_children.Count == 0)
            {
               Range contentRange = m_range.Document.Range(m_title.Range.End, m_range.End);
               return contentRange.Text;
            }
            else
            {
               if (m_title.Range.End == m_children[0].Content.Start)
               {
                  return String.Empty;
               }
               else
               {
                  Range contentRange = m_range.Document.Range(m_title.Range.End, m_children[0].Content.Start - 1);
                  return contentRange.Text;
               }
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
   }
}
