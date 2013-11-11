using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using LibEdward;

namespace SeqZapManualGenerator
{
   public class CsGenerator
   {
      public int SmallestLevel { get; set; }
      public string Namespace { get; set; }
      public string ClassName { get; set; }

      public CsGenerator()
      {
         SmallestLevel = 3;
         Namespace = "EdwardScissorhands";
         ClassName = "Manual";
      }

      public void Geneate(OutlineItem _document, string _outputFilename)
      {
         string title = "NO TITLE";
         string author = "NO AUTHOR";

         using (TextWriter csWriter = new StreamWriter(_outputFilename))
         {
            csWriter.WriteLine("using SeqZap.Manual;");
            csWriter.WriteLine();
            csWriter.WriteLine("namespace {0}", Namespace);
            csWriter.WriteLine("{");
            csWriter.WriteLine("   public class {0} : ISeqZapManual", ClassName);
            csWriter.WriteLine("   {");
            csWriter.WriteLine("      private static readonly {0} s_instance = new {0} ();", ClassName);
            csWriter.WriteLine("      private {0} ()", ClassName);
            csWriter.WriteLine("      {");
            csWriter.WriteLine("      }");
            csWriter.WriteLine("      public static void Register ()", ClassName);
            csWriter.WriteLine("      {");
            csWriter.WriteLine("         SeqZap.Base.ServiceManager.Get<IManualManager>().Register(s_instance);");
            csWriter.WriteLine("      }");
            csWriter.WriteLine("      public static {0} Instance {{ get {{ return s_instance; }} }}", ClassName);
            csWriter.WriteLine("      public string Name {{ get {{ return \"{0}\"; }} }}", ClassName);
            csWriter.WriteLine("      public string Title {{ get {{ return \"{0}\"; }} }}", title);
            csWriter.WriteLine("      public string Author {{ get {{ return \"{0}\"; }} }}", author);
            foreach (OutlineItem child in _document.Children)
            {
               GenerateItem(child, csWriter);
            }
            csWriter.WriteLine("   }");
            csWriter.WriteLine("}");
         }
      }

      private void GenerateItem(OutlineItem _item, TextWriter _writer)
      {
         string filename;
         string id;
         HtmlGenerator.GenerateFilenameAndId(_item, this.SmallestLevel, out filename, out id);
         if (filename.Length == 0)
         {
            return;
         }
         
         if (_item.Level > this.SmallestLevel)
         {
            _writer.WriteLine("      public static readonly Bookmark {0}_{1} = new Bookmark( s_instance, \"{2}\", \"{3}\" );", Capitalize(filename.Replace('-', '_')), Capitalize(id.Replace('-', '_')), filename, id);
         }
         else
         {
            if (_item.Title.Length > 0)
            {
               _writer.WriteLine("      public static readonly Bookmark {0} = new Bookmark( s_instance, \"{1}\", \"\" );", Capitalize(filename.Replace('-', '_')), filename);
            }
         }
         foreach (OutlineItem child in _item.Children)
         {
            GenerateItem(child, _writer);
         }
      }

      private static string Capitalize(string _filename)
      {
         if (Char.IsUpper(_filename[0]))
         {
            return _filename;
         }
         else
         {
            return Char.ToUpper(_filename[0]) + _filename.Substring(1);
         }
      }
   }
}
