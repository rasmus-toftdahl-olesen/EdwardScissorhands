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
      public string BaseDirectory { get; set; }

      public HtmlGenerator()
      {
         SmallestLevel = 3;
      }

      private void HtmlStart(TextWriter writer, string title, string author, int pageNumber)
      {
         writer.WriteLine("<!DOCTYPE html>");
         writer.WriteLine("<html>");
         writer.WriteLine("<head>");
         writer.WriteLine("<meta charset=\"utf-8\">");
         writer.WriteLine("<meta name=\"generator\" content=\"SeqZap Manual Generator v. {0}\">", EdwardVersion.VERSION);
         writer.WriteLine("<meta name=\"author\" content=\"{0}\">", author);
         writer.WriteLine("<meta name=\"seqzap-page-number\" content=\"{0}\">", pageNumber);
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

      public void Generate(OutlineItem document, string _baseDirectory)
      {
         BaseDirectory = _baseDirectory;

         if (Directory.Exists(BaseDirectory))
         {
            Directory.Delete(BaseDirectory, true);
         }
         Directory.CreateDirectory(BaseDirectory);

         string title = Program.GetTitle(document);
         string author = Program.GetAuthor(document);
         File.WriteAllText(Path.Combine(BaseDirectory, "style.css"), Properties.Resources.style);

         using (TextWriter tocWriter = new StreamWriter(Path.Combine(BaseDirectory, "toc.html")))
         {
            //HtmlStart(tocWriter, title + " - Table of contents", author);
            HtmlStart(tocWriter, "Table of contents", author, -1);
            tocWriter.WriteLine("<ul>");
            foreach (OutlineItem child in document.Children)
            {
               GenerateItem(child, tocWriter, null, "toc", author);
            }
            tocWriter.WriteLine("</ul>");
            HtmlEnd(tocWriter);
         }
      }

      private void GenerateItem(OutlineItem item, TextWriter tocWriter, TextWriter parentWriter, string parentFilename, string author)
      {
         string filename;
         string id;
         string csharpName;
         LinkedList<KeyValuePair<string, OutlineItem>> heritage = GetHeritage(item);
         GenerateFilenameAndId(heritage, this.SmallestLevel, out filename, out id, out csharpName);
         if (filename.Length == 0)
         {
            return;
         }

         using (TextWriter writer = new StreamWriter(Path.Combine(BaseDirectory, filename + ".html")))
         {
            HtmlStart(writer, item.Title, author, item.PageNumber);
            writer.WriteLine("<div class=\"nav\">");
            string fullname = null;
            writer.Write("<a href=\"toc.html\">TOC</a>");
            foreach (KeyValuePair<string, OutlineItem> heritageItem in heritage)
            {
               writer.Write(" &raquo; ");
               if (fullname == null)
               {
                  fullname = heritageItem.Key;
               }
               else
               {
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
            HtmlItem(item, writer, item.Level);
            if (item.Level < this.SmallestLevel)
            {
               writer.WriteLine("<div class=\"children\">");
               writer.WriteLine("<ul>");
               tocWriter.WriteLine("<ul>");
               foreach (OutlineItem child in item.Children)
               {
                  GenerateItem(child, tocWriter, writer, filename, author);
               }
               tocWriter.WriteLine("</ul>");
               writer.WriteLine("</ul>");
               writer.WriteLine("</div>");
            }
            else
            {
               foreach (OutlineItem child in item.Children)
               {
                  HtmlItemRecursively(child, writer, item.Level);
               }
            }
            tocWriter.WriteLine("</li>");
            HtmlEnd(writer);
         }
      }

      private void HtmlItemRecursively(OutlineItem _item, TextWriter _writer, int _topLevel)
      {
         HtmlItem(_item, _writer, _topLevel);
         foreach (OutlineItem child in _item.Children)
         {
            HtmlItemRecursively(child, _writer, _topLevel);
         }
      }

      private void HtmlItem(OutlineItem _item, TextWriter _writer, int _topLevel)
      {
         string filename;
         string id;
         //LinkedList<string> stack = GetHeritage(_item);
         GenerateFilenameAndId(_item, this.SmallestLevel, out filename, out id);

         if (String.IsNullOrEmpty(id))
         {
            _writer.WriteLine("<h{0}>{1}</h{0}>", _item.Level - _topLevel + 1, _item.Title);
         }
         else
         {
            _writer.WriteLine("<h{0} id=\"{2}\">{1}</h{0}>", _item.Level - _topLevel + 1, _item.Title, id);
         }
         _writer.WriteLine("<div class=\"content\">");
         List<string> listLevels = new List<string>();
         foreach (Content content in _item.Content)
         {
            try
            {
               switch (content.Type)
               {
                  case ContentType.Text:
                     {
                        TextContent text = content.AsText;
                        while (listLevels.Count != text.ListLevel)
                        {
                           if (listLevels.Count > text.ListLevel)
                           {
                              _writer.WriteLine("</{0}>", listLevels[listLevels.Count - 1]);
                              listLevels.RemoveAt(listLevels.Count - 1);
                           }
                           else
                           {
                              if (text.NumberedList)
                              {
                                 listLevels.Add("ol");
                              }
                              else
                              {
                                 listLevels.Add("ul");
                              }
                              _writer.WriteLine("<{0}>", listLevels[listLevels.Count - 1]);
                           }
                        }
                        if (listLevels.Count == 0)
                        {
                           switch (text.Style)
                           {
                              case "Code":
                                 _writer.WriteLine("<code>{0}</code>", HtmlEscape(text.Text));
                                 break;

                              default:
                                 _writer.WriteLine("<p>{0}</p>", HtmlEscape(text.Text));
                                 break;
                           }
                        }
                        else
                        {
                           _writer.WriteLine("<li>{0}</li>", HtmlEscape(text.Text));
                        }
                     }
                     break;

                  case ContentType.ImagePng:
                     {
                        PngImageContent image = content.AsPngImage.Scale(0.8f);
                        string imageFileName = SaveImage(image);
                        _writer.WriteLine("<div class=\"figure\">");
                        _writer.WriteLine("<img src=\"{0}\" alt=\"{1}\" style=\"width: {2}pt; height: {3}pt;\" />", imageFileName, image.AltText, image.Width, image.Height);
                        if (!String.IsNullOrEmpty(image.Title))
                        {
                           _writer.WriteLine("<div class=\"figcaption\">{0}</div>", image.Title);
                        }
                        _writer.WriteLine("</div>");
                     }
                     break;

                  case ContentType.Table:
                     {
                        _writer.WriteLine("<table>");
                        TableContent table = content.AsTable;
                        foreach (TableContent.Row row in table.Rows)
                        {
                           string tag = "td";
                           if (row.IsHeading)
                           {
                              tag = "th";
                           }
                           _writer.Write("  <tr>");
                           foreach (TableContent.Cell cell in row.Cells)
                           {
                              int colSpan = cell.ColumnSpan;
                              string text = HtmlEscape(cell.Text);
                              if (colSpan > 1)
                              {
                                 _writer.Write("<{0} colspan=\"{2}\">{1}</{0}>", tag, text, colSpan);
                              }
                              else
                              {
                                 _writer.Write("<{0}>{1}</{0}>", tag, text);
                              }
                           }
                           _writer.WriteLine("</tr>");
                        }
                        _writer.WriteLine("</table>");
                     }
                     break;
               }
            }
            catch (Exception ex)
            {
               _writer.WriteLine("<div class=\"seqzap-manual-generator-error\">");
               _writer.WriteLine("<h1>While trying to generate {0}</h1>", content.Type);
               _writer.WriteLine("<h2>Message</h2>");
               _writer.WriteLine("<p>{0}</p>", ex.Message);
               _writer.WriteLine("<h2>Stacktrace</h2>");
               _writer.WriteLine("<pre>{0}</pre>", ex.StackTrace);
               _writer.WriteLine("</div>");
            }
         }
         _writer.WriteLine("</div>");
      }

      public static void GenerateFilenameAndId(OutlineItem _item, int smallestLevel, out string _filename, out string _id)
      {
         string csharpName;
         GenerateFilenameAndId(GetHeritage(_item), smallestLevel, out _filename, out _id, out csharpName);
      }

      public static void GenerateFilenameAndId(OutlineItem _item, int smallestLevel, out string _filename, out string _id, out string _csharpName)
      {
         GenerateFilenameAndId(GetHeritage(_item), smallestLevel, out _filename, out _id, out _csharpName);
      }

      private static void GenerateFilenameAndId(LinkedList<KeyValuePair<string, OutlineItem>> _stack, int smallestLevel, out string _filename, out string _id, out string _csharpName)
      {
         string filename = "";
         string id = "";
         string csharpName = "";
         int i = 0;

         i = 0;
         foreach (KeyValuePair<string, OutlineItem> stackItem in _stack)
         {
            if (i < smallestLevel)
            {
               if (filename.Length == 0)
               {
                  filename = stackItem.Key;
                  csharpName = Capitalize(stackItem.Key);
               }
               else
               {
                  filename = filename + "-" + stackItem.Key;
                  csharpName = csharpName + "_" + Capitalize(stackItem.Key);
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
               csharpName = csharpName + "_" + Capitalize(stackItem.Key);
            }
            i++;
         }
         _filename = filename;
         _id = id;
         _csharpName = RemoveCharsAndCapitalize(csharpName, '-');
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

      private static string Capitalize(string _filename)
      {
         if (_filename.Length == 0)
         {
            return _filename;
         }

         if (Char.IsUpper(_filename[0]))
         {
            return _filename;
         }
         else
         {
            return Char.ToUpper(_filename[0]) + _filename.Substring(1);
         }
      }

      private static string RemoveCharsAndCapitalize(string s, params char[] chars)
      {
         bool capitalizeNext = false;
         StringBuilder ret = new StringBuilder(s.Length);
         foreach (char c in s)
         {
            if (Array.IndexOf(chars, c) == -1)
            {
               if (capitalizeNext)
               {
                  ret.Append(Char.ToUpper(c));
                  capitalizeNext = false;
               }
               else
               {
                  ret.Append(c);
               }
            }
            else
            {
               capitalizeNext = true;
            }
         }
         return ret.ToString();
      }

      private int m_nextImageIndex = 0;
      private string SaveImage(PngImageContent _image)
      {
         string imageDir = Path.Combine(BaseDirectory, "images");
         if (!Directory.Exists(imageDir))
         {
            Directory.CreateDirectory(imageDir);
         }
         int imageIndex = m_nextImageIndex++;
         string imageFilename = String.Format("image{0:000000}.png", imageIndex);
         string fullImageFilename = Path.Combine(imageDir, imageFilename);
         using (FileStream stream = new FileStream(fullImageFilename, FileMode.Create))
         {
            _image.Write(stream);
         }
         return "images/" + imageFilename;
      }

      private static string HtmlEscape(string _text)
      {
         return _text.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quote;");
      }
   }
}
