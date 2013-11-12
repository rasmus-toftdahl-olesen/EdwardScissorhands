using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LibEdward;

namespace SeqZapManualGenerator
{
   public class Program
   {
      [STAThread]
      public static void Main(string[] args)
      {
         //Application.EnableVisualStyles();
         //Application.SetCompatibleTextRenderingDefault(false);

         //Form form = new Form();
         //form.Load += new  EventHandler(delegate(object o, EventArgs a)
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
         //});
         //Application.Run(form);
      }
   }
}
