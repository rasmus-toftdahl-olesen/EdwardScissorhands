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
               return m_title.Range.Text;
            }
         }
         set
         {
            m_title.Range.Text = value;
         }
      }
      public Range Content { get { return m_range; } }
      public OutlineItem Parent { get { return m_parent; } }
      public IList<OutlineItem> Children { get { return m_children; } }
      
      public string TextContent { get { return m_range.Text; } }

      internal OutlineItem(Document _document)
         : this(_document.Content, null)
      {
         m_document = _document;
      }
      internal OutlineItem(Paragraph _title, OutlineItem _parent)
         : this(_title.Range, _parent)
      {
         m_title = _title;
      }
      
      private OutlineItem(Range _range, OutlineItem _parent)
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

      internal void UpdateEnd(int _newEnd)
      {
         m_range = m_range.Document.Range(m_range.Start, _newEnd);
      }
   }
}
