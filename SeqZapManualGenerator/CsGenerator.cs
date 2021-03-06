﻿using System;
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

      public void Geneate( OutlineItem _document, string _outputFilename )
      {
         string title = Program.GetTitle( _document );
         string author = Program.GetAuthor( _document );

         using ( TextWriter csWriter = new StreamWriter( _outputFilename ) )
         {
            csWriter.WriteLine( "using Sequanto.Manual;" );
            csWriter.WriteLine( "using SeqZap.Manual;" );
            csWriter.WriteLine();
            csWriter.WriteLine( "namespace {0}", Namespace );
            csWriter.WriteLine( "{" );
            csWriter.WriteLine( "   public class {0} : IManual", ClassName );
            csWriter.WriteLine( "   {" );
            csWriter.WriteLine( "      private static readonly {0} s_instance = new {0} ();", ClassName );
            csWriter.WriteLine( "      private {0} ()", ClassName );
            csWriter.WriteLine( "      {" );
            csWriter.WriteLine( "      }" );
            csWriter.WriteLine( "      public static void Register ()", ClassName );
            csWriter.WriteLine( "      {" );
            csWriter.WriteLine( "         SeqZap.Base.ServiceManager.Get<IManualManager>().Register(s_instance);" );
            csWriter.WriteLine( "      }" );
            csWriter.WriteLine( "      public static {0} Instance {{ get {{ return s_instance; }} }}", ClassName );
            csWriter.WriteLine( "      public string Name {{ get {{ return \"{0}\"; }} }}", ClassName );
            csWriter.WriteLine( "      public string Title {{ get {{ return \"{0}\"; }} }}", title );
            csWriter.WriteLine( "      public string Author {{ get {{ return \"{0}\"; }} }}", author );
            csWriter.WriteLine( "      private static readonly Bookmark s_tableOfContents = new Bookmark( s_instance, \"toc\", \"\", 1 );" );
            csWriter.WriteLine( "      public Bookmark TableOfContents { get { return s_tableOfContents; } }" );

            List<KeyValuePair<string, Uri>> constUrls = new List<KeyValuePair<string, Uri>>();
            foreach ( OutlineItem child in _document.Children )
            {
               GenerateItem( child, csWriter, constUrls );
            }
            csWriter.WriteLine( "      public class Const" );
            csWriter.WriteLine( "      {" );
            foreach ( KeyValuePair<string, Uri> constUrl in constUrls )
            {
               csWriter.WriteLine( "         public const string {0} = \"{1}\";", constUrl.Key, constUrl.Value );
            }
            csWriter.WriteLine( "      }" );
            csWriter.WriteLine( "   }" );
            csWriter.WriteLine( "}" );
         }
      }

      private void GenerateItem( OutlineItem _item, TextWriter _writer, List<KeyValuePair<string, Uri>> _constUrls )
      {
         string filename;
         string id;
         string csharpName;
         HtmlGenerator.GenerateFilenameAndId( _item, this.SmallestLevel, out filename, out id, out csharpName );
         if ( filename.Length == 0 )
         {
            return;
         }

         UriBuilder builder = new UriBuilder( "seqzap-manual", ClassName, _item.PageNumber );
         builder.Path = filename;
         if ( _item.Level > this.SmallestLevel )
         {
            _writer.WriteLine( "      public static readonly Bookmark {0} = new Bookmark( s_instance, \"{1}\", \"{2}\", {3} );", csharpName, filename, id, _item.PageNumber );
            builder.Fragment = id;
            _constUrls.Add( new KeyValuePair<string, Uri>( csharpName, builder.Uri ) );
         }
         else
         {
            if ( _item.Title.Length > 0 )
            {
               _writer.WriteLine( "      public static readonly Bookmark {0} = new Bookmark( s_instance, \"{1}\", \"\", {2} );", csharpName, filename, _item.PageNumber );
               _constUrls.Add( new KeyValuePair<string, Uri>( csharpName, builder.Uri ) );
            }
         }
         foreach ( OutlineItem child in _item.Children )
         {
            GenerateItem( child, _writer, _constUrls );
         }
      }
   }
}
