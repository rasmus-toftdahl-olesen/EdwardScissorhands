using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

namespace EdwardScissorhands
{
   public partial class Form1 : Form
   {
      private string m_filename = Path.Combine(System.Environment.CurrentDirectory, "Untitled.Edward");
      private bool m_generate = false;

      public string Filename
      {
         get
         {
            return m_filename;
         }
         set
         {
            m_filename = Path.GetFullPath(value);
         }
      }

      public bool Generate
      {
         get
         {
            return m_generate;
         }
         set
         {
            m_generate = value;
         }
      }

      public Form1()
      {
         InitializeComponent();
      }

      protected override void OnLoad(EventArgs e)
      {
 	      base.OnLoad(e);

         this.Text += String.Format(" v. {0}", Program.VERSION);
         m_filenameTextbox.Text = m_filename;
         m_loadButton_Click(null, EventArgs.Empty);

         if (m_generate)
         {
            this.BeginInvoke(new MethodInvoker(GenerateAll));
         }
      }

      private void m_generateButton_Click(object sender, EventArgs e)
      {
         GenerateAll();
      }

      private void GenerateAll ()
      {
         this.Enabled = false;

         Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
         Document masterDoc = app.Documents.Add();
         object missing = System.Reflection.Missing.Value;
         try
         {
            string root = Path.GetDirectoryName(m_filename);
            string outputFilename = Path.Combine(root, Path.GetFileNameWithoutExtension(m_filename) + ".docx");
            masterDoc.SaveAs2(outputFilename);
            foreach (string rawLine in m_text.Text.Split('\n'))
            {
               string line = rawLine.Trim();
               if (line.Length > 0 )
               {
                  if (line[0] == '#')
                  {
                     string[] parts = line.Substring(1).Split(':', '=');
                     if (parts.Length == 2)
                     {
                        string key = parts[0].Trim().ToLowerInvariant();
                        string value = parts[1].Trim();

                        switch (key)
                        {
                           case "title":
                              masterDoc.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyTitle].Value = value;
                              break;

                           case "author":
                              masterDoc.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyAuthor].Value = value;
                              break;

                           case "company":
                              masterDoc.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyCompany].Value = value;
                              break;

                           case "subject":
                              masterDoc.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertySubject].Value = value;
                              break;

                           case "style":
                              string fullStyleName = Path.Combine(root, value);
                              if (File.Exists(fullStyleName))
                              {
                                 masterDoc.CopyStylesFromTemplate(fullStyleName);
                              }
                              else
                              {
                                 MessageBox.Show("Could not find style {0}", fullStyleName);
                              }
                              break;

                           default:
                              MessageBox.Show(String.Format("Unknown meta data: {0} = {1}", key, value));
                              break;
                        }
                     }
                  }
                  else
                  {
                     string fullPath = Path.Combine(root, line.Trim());
                     if (!File.Exists(fullPath))
                     {
                        MessageBox.Show("Could not find the file named " + fullPath);
                        return;
                     }
                     Document document = app.Documents.Open(fullPath);
                     document.Content.Copy();
                     app.Selection.Start = app.Selection.End;
                     masterDoc.Range(masterDoc.Content.End - 1, masterDoc.Content.End).Paste();
                     (document as _Document).Close();
                  }
               }
            }
            masterDoc.Save();

            foreach ( Field field in masterDoc.Fields )
            {
               field.Update();
            }
            masterDoc.Save();

            string pdfFilename = Path.Combine(Path.GetDirectoryName(m_filename), Path.GetFileNameWithoutExtension(m_filename) + ".pdf");
            masterDoc.SaveAs2(pdfFilename, WdSaveFormat.wdFormatPDF);
         }
         finally
         {
            (app as _Application).Quit(true, missing, missing);
            this.Enabled = true;
         }

         if (m_generate)
         {
            this.Close();
         }
      }

      private void m_saveButton_Click(object sender, EventArgs e)
      {
         File.WriteAllText(m_filename, m_text.Text);
      }

      private void m_loadButton_Click(object sender, EventArgs e)
      {
         m_text.Clear();
         if (File.Exists(m_filename))
         {
            m_text.Text = File.ReadAllText(m_filename);
         }
      }

      private void m_filenameTextbox_TextChanged(object sender, EventArgs e)
      {
         m_filename = m_filenameTextbox.Text;
      }
   }
}
