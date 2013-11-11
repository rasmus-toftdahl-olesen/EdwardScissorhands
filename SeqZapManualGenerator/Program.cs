using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using LibEdward;

namespace SeqZapManualGenerator
{
   public class Program
   {
      public static void Main(string[] args)
      {
         string inputFile = Path.GetFullPath(args[0]);
         try
         {
            OutlineItem document = Edward.LoadAndOutline(inputFile);
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            CsGenerator csGenerator = new CsGenerator();

            htmlGenerator.Generate(document, Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile)));
            
            csGenerator.ClassName = Path.GetFileNameWithoutExtension(inputFile);
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
   }
}
