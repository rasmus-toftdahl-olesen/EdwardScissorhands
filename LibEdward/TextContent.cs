using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class TextContent : Content
   {
      private string m_text;
      private int m_listLevel;
      private WdListType m_listType;
      
      public string Text { get { return m_text; } }
      public override ContentType Type { get { return ContentType.Text; } }
      public int ListLevel { get { return m_listLevel; } }
      public bool NumberedList
      {
         get
         {
            switch (m_listType)
            {
               case WdListType.wdListListNumOnly:
               case WdListType.wdListMixedNumbering:
               case WdListType.wdListOutlineNumbering:
               case WdListType.wdListSimpleNumbering:
                  return true;

               default:
                  return false;
            }
         }
      }
      
      internal TextContent(string _text)
         : this(_text, 0, WdListType.wdListNoNumbering)
      {
      }
      
      internal TextContent(string _text, int _listLevel, WdListType _listType)
      {
         m_text = _text;
         m_listLevel = _listLevel;
         m_listType = _listType;
      }
   }
}
