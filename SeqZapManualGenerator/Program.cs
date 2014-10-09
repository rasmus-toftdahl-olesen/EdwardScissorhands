using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using LibEdward;
using Microsoft.Office.Interop.Word;

namespace SeqZapManualGenerator
{
   public class Program
   {
      [STAThread]
      public static void Main( string[] args )
      {
         string inputFile = Path.GetFullPath( args[0] );
         try
         {

            OutlineItem document = Edward.LoadAndOutline( inputFile );
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            CsGenerator csGenerator = new CsGenerator();

            htmlGenerator.Generate( document, Path.Combine( Path.GetDirectoryName( inputFile ), Path.GetFileNameWithoutExtension( inputFile ) ) );

            csGenerator.ClassName = Path.GetFileNameWithoutExtension( inputFile ).Replace( " ", "_" );
            if ( args.Length > 1 )
            {
               csGenerator.Namespace = args[1];
            }
            csGenerator.Geneate( document, Path.Combine( Path.GetDirectoryName( inputFile ), csGenerator.ClassName + ".cs" ) );

            if ( htmlGenerator.HasErrors )
            {
               string errorFilename = Path.GetTempFileName() + ".html";
               using ( TextWriter writer = new StreamWriter( errorFilename ) )
               {
                  writer.WriteLine( "<!html>" );
                  writer.WriteLine( "<html><body>" );
                  writer.WriteLine( "<h1>Errors were reported during generating</h1>" );
                  writer.WriteLine( "<ul>" );
                  foreach ( HtmlError error in htmlGenerator.Errors )
                  {
                     writer.WriteLine( "<li>In <a href=\"file:///{0}\">{0}</a><br /><pre>{0}</pre></li>", HtmlGenerator.HtmlEscape( error.FileName ), HtmlGenerator.HtmlEscape( error.Text ) );
                  }
                  writer.WriteLine( "</ul>" );
                  writer.WriteLine( "</body></html>" );
               }
               System.Diagnostics.Process.Start( errorFilename );
            }
         }
         finally
         {
            Edward.StopWord();
         }
      }

      public static string GetTitle( OutlineItem _item )
      {
         return _item.Document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyTitle].Value;
      }

      public static string GetAuthor( OutlineItem _item )
      {
         return _item.Document.BuiltInDocumentProperties[WdBuiltInProperty.wdPropertyAuthor].Value;
      }
   }
}
