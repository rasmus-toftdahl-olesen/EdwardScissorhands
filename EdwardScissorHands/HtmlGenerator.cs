using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace EdwardScissorhands
{
   public class HtmlGenerator
   {
      private List<string> m_externalStyles = new List<string>();

      public bool Enabled { get; set; }

      public int SmallestLevel { get; set; }

      public HtmlGenerator()
      {
         SmallestLevel = 3;
      }

      public void AddExternalStyle(string style)
      {
         m_externalStyles.Add(style);
      }

      private void HtmlStart(TextWriter writer, string title, string author)
      {
         writer.WriteLine("<!DOCTYPE html>");
         writer.WriteLine("<html>");
         writer.WriteLine("<head>");
         writer.WriteLine("<meta charset=\"utf-8\">");
         foreach (string externalCss in m_externalStyles)
         {
            writer.WriteLine("<style type=\"text/css\" src=\"{0}\">", externalCss);
         }
         writer.WriteLine("<title>{0}</title>", title);
         writer.WriteLine("</head>");
         writer.WriteLine("<body>");
      }

      private void HtmlEnd(TextWriter writer)
      {
         writer.WriteLine("</body>");
         writer.WriteLine("</html>");
      }

      private string ToHtmlId(string value)
      {
         StringBuilder ret = new StringBuilder();
         foreach (char c in value)
         {
            if (Char.IsLetterOrDigit(c))
            {
               ret.Append(Char.ToLowerInvariant(c));
            }
            else
            {
               switch (c)
               {
                  case '-':
                  case ' ':
                     ret.Append('-');
                     break;

                  default:
                     break;
               }
            }
         }
         return ret.ToString();
      }

      public void Generate(Document document, string baseDirectory)
      {
         if (Directory.Exists(baseDirectory))
         {
            Directory.Delete(baseDirectory, true);
         }
         Directory.CreateDirectory(baseDirectory);

         string title = document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyTitle].Value;
         string author = document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyAuthor].Value;
         List<string> outlineStack = new List<string>();
         string csClass = Path.GetFileName(baseDirectory);
         using (TextWriter csWriter = new StreamWriter(Path.Combine(baseDirectory, csClass + ".cs")))
         {
            csWriter.WriteLine("namespace EdwardScissorhands");
            csWriter.WriteLine("{");
            csWriter.WriteLine("   public class {0}", csClass);
            csWriter.WriteLine("   {");
            csWriter.WriteLine("      public struct Bookmark");
            csWriter.WriteLine("      {");
            csWriter.WriteLine("         private string m_filename, m_id;");
            csWriter.WriteLine("         public string Filename { get { return m_filename; } }");
            csWriter.WriteLine("         public string Id { get { return m_id; } }");
            csWriter.WriteLine("         protected Bookmark ( string _filename, string _id )");
            csWriter.WriteLine("         {");
            csWriter.WriteLine("            m_filename = _filename;");
            csWriter.WriteLine("            m_id = _id;");
            csWriter.WriteLine("         }");
            csWriter.WriteLine("      }");
            using (TextWriter allWriter = new StreamWriter(Path.Combine(baseDirectory, "all.html")))
            {
               HtmlStart(allWriter, title, author);
               using (TextWriter tocWriter = new StreamWriter(Path.Combine(baseDirectory, "toc.html")))
               {
                  HtmlStart(tocWriter, "Table of contents", author);
                  TextWriter currentWriter = new StreamWriter(Path.Combine(baseDirectory, "_start.html"));
                  foreach (Paragraph paragraph in document.Paragraphs)
                  {
                     int outlineLevel = (int) paragraph.OutlineLevel;
                     if (outlineLevel == (int) WdOutlineLevel.wdOutlineLevelBodyText)
                     {
                        allWriter.WriteLine("<p>{0}</p>", paragraph.Range.Text);
                        currentWriter.WriteLine("<p>{0}</p>", paragraph.Range.Text);
                     }
                     else
                     {
                        while (outlineStack.Count >= outlineLevel)
                        {
                           outlineStack.RemoveAt(outlineStack.Count - 1);
                        }
                        outlineStack.Add(ToHtmlId(paragraph.Range.Text));

                        string filename = "";
                        string id = "";
                        int i = 0;
                        foreach (string stackItem in outlineStack)
                        {
                           if (i < this.SmallestLevel)
                           {
                              if (filename.Length == 0)
                              {
                                 filename = stackItem;
                              }
                              else
                              {
                                 filename = filename + "-" + stackItem;
                              }
                           }
                           else
                           {
                              if (id.Length == 0)
                              {
                                 id = stackItem;
                              }
                              else
                              {
                                 id = id + "-" + stackItem;
                              }
                           }
                           i++;
                        }
                        if (outlineLevel > this.SmallestLevel)
                        {
                           allWriter.WriteLine("<h{0} id=\"{2}-{3}\">{1}</h{0}>", outlineLevel, paragraph.Range.Text, filename, id);
                           currentWriter.WriteLine("<h{0} id=\"{2}\">{1}</h{0}>", outlineLevel, paragraph.Range.Text, id);
                           csWriter.WriteLine("      public readonly Bookmark {0}_{1} = new Bookmark( \"{2}\", \"{3}\" );", filename.Replace('-', '_'), id.Replace('-', '_'), filename, id);
                        }
                        else
                        {
                           csWriter.WriteLine("      public readonly Bookmark {0} = new Bookmark( \"{1}\", \"\" );", filename.Replace('-', '_'), filename);
                           HtmlEnd(currentWriter);
                           currentWriter.Dispose();
                           currentWriter = new StreamWriter(Path.Combine(baseDirectory, filename + ".html"));
                           HtmlStart(currentWriter, paragraph.Range.Text, author);
                           allWriter.WriteLine("<h{0} id=\"{2}\">{1}</h{0}>", outlineLevel, paragraph.Range.Text, id);
                           currentWriter.WriteLine("<h{0} id=\"{2}\">{1}</h{0}>", outlineLevel, paragraph.Range.Text, id);
                           tocWriter.WriteLine("<h{0} id=\"{2}\"><a href=\"{2}.html\">{1}</a></h{0}>", outlineLevel, paragraph.Range.Text, id);
                        }
                     }
                  }
                  HtmlEnd(currentWriter);
                  currentWriter.Dispose();
                  HtmlEnd(tocWriter);
               }
               HtmlEnd(allWriter);
               csWriter.WriteLine("   }");
               csWriter.WriteLine("}");
            }
         }
      }
   }
}
