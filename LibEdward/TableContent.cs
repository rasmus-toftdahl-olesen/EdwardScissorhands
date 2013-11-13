using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class TableContent : Content
   {
      private Table m_table;
      
      public override ContentType Type { get { return ContentType.Table; } }

      public int Columns { get { return m_table.Columns.Count; } }
      public int Rows { get { return m_table.Rows.Count; } }
      
      internal TableContent(Table _table)
      {
         m_table = _table;
      }

      public string CellText(int _row, int _column)
      {
         string text = m_table.Cell(_row + 1, _column + 1).Range.Text;
         return text.TrimEnd('\r', '\a');
      }
   }
}
