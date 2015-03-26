using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibEdward
{
   public class DocRoot
   {
      private OutlineItem m_root;

      public DocRoot(OutlineItem _root)
      {
         m_root = _root;
      }

      public OutlineItem Get(params string[] _path)
      {
         OutlineItem parent = m_root;
         foreach (string pathItem in _path)
         {
            OutlineItem item = parent.GetChild(pathItem);
            if (item == null)
            {
               parent.AppendChild(pathItem);
               m_root = Edward.Refresh( m_root );
               return Get(_path);
            }
            parent = item;
         }
         return parent;
      }

      public void Refresh ()
      {
         m_root = Edward.Refresh( m_root );
      }
   }
}
