using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LibEdward;
using Microsoft.Office.Interop.Word;

namespace SeqZapManualGenerator
{
   public class Program
   {
      [STAThread]
      public static void Main(string[] args)
      {
         string inputFile = Path.GetFullPath(args[0]);
         try
         {

            OutlineItem document = Edward.LoadAndOutline(inputFile);
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            CsGenerator csGenerator = new CsGenerator();

            htmlGenerator.Generate(document, Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile)));

            csGenerator.ClassName = Path.GetFileNameWithoutExtension(inputFile).Replace(" ", "_");
            if (args.Length > 1)
            {
               csGenerator.Namespace = args[1];
            }
            csGenerator.Geneate(document, Path.Combine(Path.GetDirectoryName(inputFile), csGenerator.ClassName + ".cs"));
         }
         finally
         {
            Edward.StopWord();
         }
      }

      public static string GetTitle(OutlineItem _item)
      {
         return _item.Document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyTitle].Value;
      }

      public static string GetAuthor(OutlineItem _item)
      {
         return _item.Document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyAuthor].Value;
      }
   }
}
