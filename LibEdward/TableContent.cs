using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class TableContent : Content
   {
      public class Row
      {
         private Microsoft.Office.Interop.Word.Row m_row;
         private int m_numberOfColumnsAccordingToTable;
         private bool m_applyHeadingStyle;
         
         internal Row(Microsoft.Office.Interop.Word.Row _row, int _numberOfColumnsAccordingToTable, bool _applyHeadingStyle)
         {
            m_row = _row;
            m_numberOfColumnsAccordingToTable = _numberOfColumnsAccordingToTable;
            m_applyHeadingStyle = _applyHeadingStyle;
         }

         public bool IsHeading
         {
            get
            {
               return m_row.HeadingFormat == 1 || m_applyHeadingStyle;
            }
         }

         public IEnumerable<Cell> Cells
         {
            get
            {
               Microsoft.Office.Interop.Word.Cell cell = m_row.Cells[1];
               while (cell != null && cell.RowIndex == m_row.Index)
               {
                  yield return new Cell(cell, m_numberOfColumnsAccordingToTable);
                  cell = cell.Next;
               }
            }
         }
      }

      public class Cell
      {
         private Microsoft.Office.Interop.Word.Cell m_cell;
         private int m_numberOfColumnsAccordingToTable;

         internal Cell(Microsoft.Office.Interop.Word.Cell _cell, int _numberOfColumnsAccordingToTable)
         {
            m_cell = _cell;
            m_numberOfColumnsAccordingToTable = _numberOfColumnsAccordingToTable;
         }

         public string Text
         {
            get
            {
               return m_cell.Range.Text.TrimEnd('\r', '\a');
            }
         }

         public int ColumnSpan
         {
            get
            {
               int myIndex = m_cell.ColumnIndex;
               Microsoft.Office.Interop.Word.Cell nextCell = m_cell.Next;
               if (nextCell == null || nextCell.ColumnIndex == myIndex + 1)
               {
                  return 1;
               }
               else if (nextCell.RowIndex != m_cell.RowIndex)
               {
                  // If we are the last cell of the row, but out index is still smaller than the number of columns, then this
                  // cell should span the rest of the columns
                  if (m_cell.ColumnIndex != m_numberOfColumnsAccordingToTable)
                  {
                     return m_numberOfColumnsAccordingToTable - m_cell.ColumnIndex + 1;
                  }
                  else
                  {
                     return 1;
                  }
               }
               else
               {
                  return nextCell.ColumnIndex - myIndex;
               }
            }
         }
      }

      private Table m_table;

      public override ContentType Type { get { return ContentType.Table; } }

      public int NumberOfColumns { get { return m_table.Columns.Count; } }
      public int NumberOfRows { get { return m_table.Rows.Count; } }
      public bool IsUniform { get { return m_table.Uniform; } }
      public IEnumerable<Row> Rows
      {
         get
         {
            foreach (Microsoft.Office.Interop.Word.Row row in m_table.Rows)
            {
               yield return new Row(row, NumberOfColumns, row.Index == 1 && m_table.ApplyStyleHeadingRows);
            }
         }
      }

      internal TableContent(Table _table)
      {
         m_table = _table;
      }
   }
}
