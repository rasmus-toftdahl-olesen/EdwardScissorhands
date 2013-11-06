﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class OutlineItem
   {
      private int m_level;
      private string m_title;
      private Range m_range;
      private OutlineItem m_parent;
      private List<OutlineItem> m_children;
      
      public int Level { get { return m_level; } }
      public string Title { get { return m_title; } }
      public Range Content { get { return m_range; } }
      public OutlineItem Parent { get { return m_parent; } }
      public IList<OutlineItem> Children { get { return m_children; } }

      public string TextContent { get { return m_range.Text; } }

      internal OutlineItem(int _level, string _title, Range _range, OutlineItem _parent)
      {
         m_level = _level;
         m_title = _title;
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
      
      internal void UpdateEnd ( int _newEnd )
      {
         m_range =  m_range.Document.Range(m_range.Start, _newEnd);
      }
   }
}
