using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibEdward
{
   public class TextPart
   {
      private string m_text;
      private TextPartType m_type;
      private Uri m_url;

      public string Text { get { return m_text; } }
      public TextPartType Type { get { return m_type; } }
      public Uri Url { get { return m_url; } }

      internal TextPart( string _text, TextPartType _type )
         : this( _text, _type, null )
      {
      }

      internal TextPart( string _text, TextPartType _type, Uri _url )
      {
         m_text = _text;
         m_type = _type;
         m_url = _url;
      }
   }
}
