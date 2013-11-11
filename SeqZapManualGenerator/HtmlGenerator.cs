using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LibEdward;

namespace SeqZapManualGenerator
{
   public class HtmlGenerator
   {
      private List<string> m_externalStyles = new List<string>();

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

      public static string ToHtmlId(string value)
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

      public void Generate(OutlineItem document, string baseDirectory)
      {
         if (Directory.Exists(baseDirectory))
         {
            Directory.Delete(baseDirectory, true);
         }
         Directory.CreateDirectory(baseDirectory);

         //string title = document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyTitle].Value;
         //string author = document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyAuthor].Value;
         string title = "NO TITLE";
         string author = "NO AUTHOR";
         using (TextWriter tocWriter = new StreamWriter(Path.Combine(baseDirectory, "toc.html")))
         {
            HtmlStart(tocWriter, title + " - Table of contents", author);
            tocWriter.WriteLine("<ul>");
            foreach (OutlineItem child in document.Children)
            {
               GenerateItem(child, baseDirectory, tocWriter, "toc");
            }
            tocWriter.WriteLine("</ul>");
            HtmlEnd(tocWriter);
         }
      }

      private void GenerateItem(OutlineItem item, string baseDirectory, TextWriter tocWriter, string parentFilename)
      {
         string filename;
         string id;
         GenerateFilenameAndId(item, this.SmallestLevel, out filename, out id);
         
         using (TextWriter writer = new StreamWriter(Path.Combine(baseDirectory, filename + ".html")))
         {
            HtmlStart(writer, item.Title, "NO AUTHOR");
            writer.WriteLine("<div class=\"nav\">");
            writer.WriteLine("<a href=\"{0}.html\">Up</a>", parentFilename);
            writer.WriteLine("</div>");
            tocWriter.Write("<li><a href=\"{0}.html\">{1}</a>", filename, item.Title);
            HtmlItem(item, writer);
            if (item.Level < this.SmallestLevel)
            {
               tocWriter.WriteLine("<ul>");
               foreach (OutlineItem child in item.Children)
               {
                  GenerateItem(child, baseDirectory, tocWriter, filename);
               }
               tocWriter.WriteLine("</ul>");
            }
            else
            {
               foreach (OutlineItem child in item.Children)
               {
                  HtmlItem(child, writer);
               }
            }
            tocWriter.WriteLine("</li>");
            HtmlEnd(writer);
         }
      }

      private void HtmlItem(OutlineItem _item, TextWriter _writer)
      {
         string filename;
         string id;
         
         GenerateFilenameAndId(_item, this.SmallestLevel, out filename, out id);

         if (String.IsNullOrEmpty(id))
         {
            _writer.WriteLine("<h{0}>{1}</h{0}>", _item.Level, _item.Title);
         }
         else
         {
            _writer.WriteLine("<h{0} id=\"{2}\">{1}</h{0}>", _item.Level, _item.Title, id);
         }
         _writer.WriteLine(_item.TextContent);
      }

      /*
      foreach (Paragraph paragraph in document.Paragraphs)
      {
         int outlineLevel = (int) paragraph.OutlineLevel;
         if (outlineLevel == (int) WdOutlineLevel.wdOutlineLevelBodyText)
         {
            currentWriter.WriteLine("<p>{0}</p>", paragraph.Range.Text);
         }
         else
         {
      }
*/

      public static void GenerateFilenameAndId(OutlineItem _item, int smallestLevel, out string _filename, out string _id)
      {
         string filename = "";
         string id = "";
         int i = 0;
         LinkedList<string> stack = new LinkedList<string>();
         OutlineItem current = _item;
         while (current.Parent != null)
         {
            stack.AddFirst(HtmlGenerator.ToHtmlId(current.Title));
            current = current.Parent;
         }

         foreach (string stackItem in stack)
         {
            if (i < smallestLevel)
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
         _filename = filename;
         _id = id;
      }
   }
}
