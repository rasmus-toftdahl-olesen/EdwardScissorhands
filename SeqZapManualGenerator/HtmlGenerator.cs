using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LibEdward;

namespace SeqZapManualGenerator
{
   public class HtmlGenerator
   {
      public int SmallestLevel { get; set; }

      public HtmlGenerator()
      {
         SmallestLevel = 3;
      }

      private void HtmlStart(TextWriter writer, string title, string author)
      {
         writer.WriteLine("<!DOCTYPE html>");
         writer.WriteLine("<html>");
         writer.WriteLine("<head>");
         writer.WriteLine("<meta charset=\"utf-8\">");
         writer.WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\">");
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
         File.WriteAllText(Path.Combine(baseDirectory, "style.css"), Properties.Resources.style);

         using (TextWriter tocWriter = new StreamWriter(Path.Combine(baseDirectory, "toc.html")))
         {
            HtmlStart(tocWriter, title + " - Table of contents", author);
            tocWriter.WriteLine("<ul>");
            foreach (OutlineItem child in document.Children)
            {
               GenerateItem(child, baseDirectory, tocWriter, null, "toc");
            }
            tocWriter.WriteLine("</ul>");
            HtmlEnd(tocWriter);
         }
      }

      private void GenerateItem(OutlineItem item, string baseDirectory, TextWriter tocWriter,  TextWriter parentWriter, string parentFilename)
      {
         string filename;
         string id;
         LinkedList<KeyValuePair<string, OutlineItem>> heritage = GetHeritage(item);
         GenerateFilenameAndId(heritage, this.SmallestLevel, out filename, out id);
         if (filename.Length == 0)
         {
            return;
         }

         using (TextWriter writer = new StreamWriter(Path.Combine(baseDirectory, filename + ".html")))
         {
            HtmlStart(writer, item.Title, "NO AUTHOR");
            writer.WriteLine("<div class=\"nav\">");
            string fullname = null;
            foreach (KeyValuePair<string, OutlineItem> heritageItem in heritage)
            {
               if (fullname == null)
               {
                  fullname = heritageItem.Key;
               }
               else
               {
                  writer.Write(" &raquo; ");
                  fullname = fullname + "-" + heritageItem.Key;
               }
               if (heritageItem.Value != null)
               {
                  writer.Write("<a href=\"{0}.html\">{1}</a>", fullname, heritageItem.Value.Title);
               }
            }
            writer.WriteLine();
            writer.WriteLine("<span class=\"relational\">");
            int indexOfItem = item.Parent.Children.IndexOf(item);
            if (indexOfItem == 0)
            {
               writer.WriteLine("Prev");
            }
            else
            {
               string prevFilename;
               string prevId;
               OutlineItem prev = item.Parent.Children[indexOfItem - 1];
               GenerateFilenameAndId(prev, this.SmallestLevel, out prevFilename, out prevId);
               writer.WriteLine("<a href=\"{0}.html\">Prev</a>", prevFilename);
            }
            writer.WriteLine("<a href=\"{0}.html\"> Up </a>", parentFilename);
            if (indexOfItem == item.Parent.Children.Count - 1)
            {
               writer.WriteLine("Next", parentFilename);
            }
            else
            {
               string nextFilename;
               string nextId;
               OutlineItem next = item.Parent.Children[indexOfItem + 1];
               GenerateFilenameAndId(next, this.SmallestLevel, out nextFilename, out nextId);
               writer.WriteLine("<a href=\"{0}.html\">Next</a>", nextFilename);
            }
            writer.WriteLine("</span>");
            writer.WriteLine("</div>");
            if (parentWriter != null)
            {
               parentWriter.Write("<li><a href=\"{0}.html\">{1}</a>", filename, item.Title);
            }
            tocWriter.Write("<li><a href=\"{0}.html\">{1}</a>", filename, item.Title);
            HtmlItem(item, writer);
            if (item.Level < this.SmallestLevel)
            {
               writer.WriteLine("<div class=\"children\">");
               writer.WriteLine("<ul>");
               tocWriter.WriteLine("<ul>");
               foreach (OutlineItem child in item.Children)
               {
                  GenerateItem(child, baseDirectory, tocWriter, writer, filename);
               }
               tocWriter.WriteLine("</ul>");
               writer.WriteLine("</ul>");
               writer.WriteLine("</div");
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
         //LinkedList<string> stack = GetHeritage(_item);
         GenerateFilenameAndId(_item, this.SmallestLevel, out filename, out id);

         if (String.IsNullOrEmpty(id))
         {
            _writer.WriteLine("<h{0}>{1}</h{0}>", _item.Level, _item.Title);
         }
         else
         {
            _writer.WriteLine("<h{0} id=\"{2}\">{1}</h{0}>", _item.Level, _item.Title, id);
         }
         _writer.WriteLine("<div class=\"content\">");
         _writer.WriteLine(_item.TextContent);
         _writer.WriteLine("</div");
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
         GenerateFilenameAndId(GetHeritage(_item), smallestLevel, out _filename, out _id);
      }

      private static void GenerateFilenameAndId(LinkedList<KeyValuePair<string, OutlineItem>> _stack, int smallestLevel, out string _filename, out string _id)
      {
         string filename = "";
         string id = "";
         int i = 0;

         i = 0;
         foreach (KeyValuePair<string, OutlineItem> stackItem in _stack)
         {
            if (i < smallestLevel)
            {
               if (filename.Length == 0)
               {
                  filename = stackItem.Key;
               }
               else
               {
                  filename = filename + "-" + stackItem.Key;
               }
            }
            else
            {
               if (id.Length == 0)
               {
                  id = stackItem.Key;
               }
               else
               {
                  id = id + "-" + stackItem.Key;
               }
            }
            i++;
         }
         _filename = filename;
         _id = id;
      }

      private static LinkedList<KeyValuePair<string, OutlineItem>> GetHeritage(OutlineItem _item)
      {
         LinkedList<KeyValuePair<string, OutlineItem>> stack = new LinkedList<KeyValuePair<string, OutlineItem>>();
         OutlineItem current = _item;
         while (current.Parent != null)
         {
            stack.AddFirst(new KeyValuePair<string, OutlineItem>(HtmlGenerator.ToHtmlId(current.Title), current));
            for (int i = current.Parent.Level ; i < current.Level - 1 ; i++)
            {
               stack.AddFirst(new KeyValuePair<string, OutlineItem>(String.Format("MISSING-H{0}", i + 1), null));
            }
            current = current.Parent;
         }
         return stack;
      }
   }
}
