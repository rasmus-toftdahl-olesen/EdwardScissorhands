using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibEdward
{
   public class TextContent : Content
   {
      private string m_text;

      public string Text { get { return m_text; } }
      public override ContentType Type { get { return ContentType.Text; } }
      
      internal TextContent(string _text)
      {
         m_text = _text;
      }
   }
}
