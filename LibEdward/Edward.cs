using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace LibEdward
{
   public class Edward
   {
      private static Application s_application;

      public static void StartWord()
      {
         if (s_application == null)
         {
            s_application = new Application();
         }
      }

      public static void CloseAll(bool saveChanges)
      {
         if (s_application != null)
         {
            foreach (Document doc in s_application.Documents)
            {
               (doc as _Document).Close(saveChanges);
            }
         }
      }

      public static void SaveAll()
      {
         foreach (Document doc in s_application.Documents)
         {
            (doc as _Document).Save();
         }
      }
      
      public static void StopWord()
      {
         if (s_application != null)
         {
            CloseAll(false);
            (s_application as _Application).Quit();
            s_application = null;
         }
      }

      public static OutlineItem LoadAndOutline(string _filename)
      {
         StartWord();
         return Outline(s_application.Documents.Open(_filename));
      }

      public static OutlineItem Create(string _filename)
      {
         StartWord();
         Document doc = s_application.Documents.Add();
         doc.SaveAs2(_filename);
         return Outline(doc);
      }

      public static OutlineItem Refresh(OutlineItem item)
      {
         return Outline(item.Content.Document);
      }
      
      public static OutlineItem Outline(Document document)
      {
         OutlineItem documentItem = new OutlineItem(document);
         List<OutlineItem> outlineStack = new List<OutlineItem>();
         outlineStack.Add(documentItem);
         int end = 0;
         int paragraphIndex = 0;
         foreach (Paragraph paragraph in document.Paragraphs)
         {
            int outlineLevel = (int) paragraph.OutlineLevel;
            if (outlineLevel == (int) WdOutlineLevel.wdOutlineLevelBodyText)
            {
               end = paragraph.Range.End;
            }
            else
            {
               while (outlineStack[outlineStack.Count - 1].Level >= outlineLevel)
               {
                  outlineStack[outlineStack.Count - 1].UpdateEnd(end, paragraphIndex);
                  outlineStack.RemoveAt(outlineStack.Count - 1);
               }
               outlineStack.Add(new OutlineItem(paragraph, outlineStack[outlineStack.Count - 1], paragraphIndex));
               end = paragraph.Range.End;
            }
            paragraphIndex++;
         }
         while (outlineStack.Count > 1)
         {
            outlineStack[outlineStack.Count - 1].UpdateEnd(end, paragraphIndex);
            outlineStack.RemoveAt(outlineStack.Count - 1);
         }
         documentItem.UpdateEnd(document.Content.End, document.Paragraphs.Count);
         return documentItem;
      }
   }
}
