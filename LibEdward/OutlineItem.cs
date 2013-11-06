using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class OutlineItem
   {
      private Document m_document;
      private int m_level;
      private Range m_titleRange;
      private Range m_range;
      private OutlineItem m_parent;
      private List<OutlineItem> m_children;

      public int Level { get { return m_level; } }
      public string Title
      {
         get
         {
            if (m_titleRange == null)
            {
               return m_document.Name;
            }
            else
            {
               return m_titleRange.Text;
            }
         }
         set
         {
            m_titleRange.Text = value;
         }
      }
      public Range Content { get { return m_range; } }
      public OutlineItem Parent { get { return m_parent; } }
      public IList<OutlineItem> Children { get { return m_children; } }

      public string TextContent { get { return m_range.Text; } }

      internal OutlineItem(Document _document)
         : this(0, null, null)
      {
         m_range = _document.Content;
         m_document = _document;
      }
      
      internal OutlineItem(int _level, Range _titleRange, OutlineItem _parent)
      {
         m_document = null;
         m_level = _level;
         m_titleRange = _titleRange;
         m_range = _titleRange;
         m_children = new List<OutlineItem>();
         m_parent = _parent;
         if (m_parent != null)
         {
            m_parent.m_children.Add(this);
         }
      }

      public void Cut()
      {
         m_range.Cut();
      }

      internal void UpdateEnd(int _newEnd)
      {
         m_range = m_range.Document.Range(m_range.Start, _newEnd);
      }
   }
}
