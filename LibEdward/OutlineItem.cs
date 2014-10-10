using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace LibEdward
{
   public class OutlineItem
   {
      private Document m_document;
      private Paragraph m_title;
      private Range m_range;
      private Range m_contentRange;
      private OutlineItem m_parent;
      private List<OutlineItem> m_children;
      private int m_startParagraphIndex;
      private int m_endParagraphIndex;

      public int Level
      {
         get
         {
            if ( m_title == null )
            {
               return 0;
            }
            else
            {
               return (int) m_title.OutlineLevel;
            }
         }
      }

      public string Title
      {
         get
         {
            if ( m_title == null )
            {
               return "NO TITLE";
            }
            else
            {
               return m_title.Range.Text.Trim();
            }
         }
         /*
         set
         {
            WdOutlineLevel levelBefore = m_title.OutlineLevel;
            m_title.Range.Text = value + "\r";
            Paragraph paragraph = m_range.Document.Paragraphs[m_startParagraphIndex];
            paragraph.OutlineLevel = levelBefore;
            m_title = paragraph;
         }
          */
      }
      public Range Range { get { return m_range; } }
      public OutlineItem Parent { get { return m_parent; } }
      public IList<OutlineItem> Children { get { return m_children; } }

      public Range ContentRange
      {
         get
         {
            if ( m_contentRange == null )
            {
               if ( m_title == null )
               {
                  return null;
               }
               if ( m_children.Count == 0 )
               {
                  if ( m_title.Range.End == m_range.End )
                  {
                     return null;
                  }
                  else
                  {
                     m_contentRange = m_range.Document.Range( m_title.Range.End, m_range.End );
                  }
               }
               else
               {
                  if ( m_title.Range.End == m_children[0].Range.Start )
                  {
                     return null;
                  }
                  else
                  {
                     m_contentRange = m_range.Document.Range( m_title.Range.End, m_children[0].Range.Start - 1 );
                  }
               }
            }
            return m_contentRange;
         }
      }

      public Document Document
      {
         get
         {
            return m_range.Document;
         }
      }

      public string TextContent
      {
         get
         {
            Range cRange = this.ContentRange;
            if ( cRange == null )
            {
               return String.Empty;
            }
            else
            {
               return cRange.Text;
            }
         }
      }

      public int PageNumber
      {
         get
         {
            if ( m_title != null )
            {
               return Convert.ToInt32( m_title.Range.get_Information( WdInformation.wdActiveEndPageNumber ) );
            }
            else
            {
               return Convert.ToInt32( m_range.get_Information( WdInformation.wdActiveEndPageNumber ) );
            }
         }
      }

      internal OutlineItem( Document _document )
         : this( _document.Content, null, 0 )
      {
         m_document = _document;
      }

      internal OutlineItem( Paragraph _title, OutlineItem _parent, int _startParagraphIndex )
         : this( _title.Range, _parent, _startParagraphIndex )
      {
         m_title = _title;
      }

      private OutlineItem( Range _range, OutlineItem _parent, int _startParagraphIndex )
      {
         m_document = null;
         m_title = null;
         m_range = _range;
         m_children = new List<OutlineItem>();
         m_parent = _parent;
         if ( m_parent != null )
         {
            m_parent.m_children.Add( this );
         }
         m_startParagraphIndex = _startParagraphIndex;
         m_endParagraphIndex = _startParagraphIndex;
      }

      public void Cut()
      {
         m_range.Cut();
      }


      public void Promote()
      {
         if ( m_title != null )
         {
            m_title.OutlinePromote();
         }
      }

      public void Demote()
      {
         if ( m_title != null )
         {
            m_title.OutlineDemote();
         }
      }

      public string Path( string _seperator )
      {
         if ( m_parent == null || m_parent.Parent == null )
         {
            return this.Title;
         }
         else
         {
            return m_parent.Path( _seperator ) + _seperator + this.Title;
         }
      }

      public void AppendChild( string title )
      {
         int level = this.Level;
         int endBefore = m_range.End;
         if ( endBefore == m_range.Document.Content.End )
         {
            m_range.InsertAfter( "\r" + title + "\r" );
         }
         else
         {
            m_range.InsertAfter( title + "\r" );
         }
         Paragraph newParagraph = m_range.Document.Paragraphs[m_endParagraphIndex + 1];
         while ( ( (int) newParagraph.OutlineLevel ) != level + 1 )
         {
            if ( ( (int) newParagraph.OutlineLevel ) > level )
            {
               newParagraph.OutlinePromote();
            }
            else
            {
               newParagraph.OutlineDemote();
            }
         }
      }

      public OutlineItem GetChild( string title )
      {
         foreach ( OutlineItem item in m_children )
         {
            if ( item.Title.Trim() == title )
            {
               return item;
            }
         }
         return null;
      }

      internal void UpdateEnd( int _newEnd, int _endParagraph )
      {
         m_range = m_range.Document.Range( m_range.Start, _newEnd );
         m_endParagraphIndex = _endParagraph;
      }

      public IEnumerable<Content> Content
      {
         get
         {
            Range cRange = this.ContentRange;
            if ( cRange != null )
            {
               List<KeyValuePair<Range, Content>> usedRanges = new List<KeyValuePair<Range, Content>>();
               foreach ( Table table in cRange.Tables )
               {
                  usedRanges.Add( new KeyValuePair<Range, Content>( table.Range, new TableContent( table ) ) );
               }

               foreach ( Shape shape in cRange.ShapeRange )
               {
                  InlineShape inlineShape = shape.ConvertToInlineShape();
                  usedRanges.Add( ProcessInlineShape( inlineShape ) );
               }

               foreach ( InlineShape inlineShape in cRange.InlineShapes )
               {
                  usedRanges.Add( ProcessInlineShape( inlineShape ) );
               }

               foreach ( Paragraph paragraph in cRange.ListParagraphs )
               {
                  object style = paragraph.get_Style();
                  string styleName = null;
                  if ( style is Style )
                  {
                     styleName = ( style as Style ).NameLocal;
                  }
                  string paragraphText = paragraph.Range.Text;
                  paragraphText = paragraphText.Replace( "\v", "" );
                  usedRanges.Add( new KeyValuePair<Range, Content>( paragraph.Range, new TextContent( paragraphText, paragraph.Range.ListFormat.ListLevelNumber, paragraph.Range.ListFormat.ListType, styleName, ListParts( paragraph ) ) ) );
               }

               if ( usedRanges.Count > 0 )
               {
                  foreach ( Paragraph paragraph in cRange.Paragraphs )
                  {
                     string paragraphText = paragraph.Range.Text.Trim();
                     if ( paragraph.Range.InlineShapes.Count > 0 )
                     {
                        if ( paragraphText != FIGURE_MARKER && paragraphText != EMBEDDED_OBJECT_MARKER )
                        {
                           usedRanges.Add( new KeyValuePair<Range, Content>( paragraph.Range, new TextContent( String.Format( "INLINE SHAPE WITH TEXT: This inline shape also contains text - this is not currently supported by Edward.{0}The paragraph with the troublesome text reads:{0}{0}{1}", Environment.NewLine, paragraphText ), Edward.ERROR_STYLE ) ) );
                        }
                     }
                     else if ( paragraph.Range.ShapeRange.Count > 0 )
                     {
                        if ( paragraphText != "/" )
                        {
                           usedRanges.Add( new KeyValuePair<Range, Content>( paragraph.Range, new TextContent( String.Format( "SHAPE WITH TEXT: This shape also contains text - this is not currently supported by Edward.{0}The paragraph with the troublesome text reads:{0}{0}{1}", Environment.NewLine, paragraphText ), Edward.ERROR_STYLE ) ) );
                        }
                     }
                     else if ( paragraph.Range.ListFormat != null && paragraph.Range.ListFormat.ListType != WdListType.wdListNoNumbering )
                     {

                     }
                     else if ( paragraph.Range.Tables.Count > 0 )
                     {
                     }
                     else
                     {
                        object paragraphStyle = paragraph.get_Style();
                        string style = GetStyleName( paragraphStyle );
                        usedRanges.Add( new KeyValuePair<Range, Content>( paragraph.Range, new TextContent( paragraphText, style, ListParts( paragraph ) ) ) );
                     }
                  }
                  usedRanges.Sort( new Comparison<KeyValuePair<Range, Content>>( SortRanges ) );

                  int currentEnd = cRange.Start;
                  foreach ( KeyValuePair<Range, Content> item in usedRanges )
                  {
                     if ( item.Key.Start > currentEnd )
                     {
                        Range textRange = item.Key.Document.Range( currentEnd, item.Key.Start );
                        string text = textRange.Text.Trim();
                        if ( text.Length > 0 )
                        {
                           yield return new TextContent( "ERROR: Unhandled text - this should not happen, please report this bug at https://github.com/rasmus-toftdahl-olesen/EdwardScissorhands/issues with the problematic .docx file attached.", Edward.ERROR_STYLE );
                        }
                     }
                     yield return item.Value;
                     currentEnd = item.Key.End;
                  }
                  if ( currentEnd != cRange.End && currentEnd < cRange.End )
                  {
                     string text = cRange.Document.Range( currentEnd, cRange.End ).Text.Trim();
                     if ( text.Length > 0 )
                     {
                        yield return new TextContent( "ERROR: Unhandled text - this should not happen, please report this bug at https://github.com/rasmus-toftdahl-olesen/EdwardScissorhands/issues with the problematic .docx file attached.", Edward.ERROR_STYLE );
                     }
                  }
               }
               else
               {
                  foreach ( TextContent content in ListParagraphsFor( cRange ) )
                  {
                     yield return content;
                  }
               }
            }
         }
      }

      private const string FIGURE_MARKER = "/";
      private const string EMBEDDED_OBJECT_MARKER = "\u0001";

      private static int SortRanges( KeyValuePair<Range, Content> a, KeyValuePair<Range, Content> b )
      {
         return a.Key.Start.CompareTo( b.Key.Start );
      }

      private static IEnumerable<TextContent> ListParagraphsFor( Range _range )
      {
         foreach ( Paragraph paragraph in _range.Paragraphs )
         {
            string paragraphText = paragraph.Range.Text.Trim();
            if ( paragraphText == "/" )
            {
               // This is a figure/shape/something and should have been handled earlier.
            }
            else if ( paragraphText.Length == 0 )
            {
               // Skip empty paragraphs.
            }
            else
            {
               object paragraphStyle = paragraph.get_Style();
               string style = GetStyleName( paragraphStyle );
               yield return new TextContent( paragraphText, style, ListParts( paragraph ) );
            }
         }
      }

      private static TextPart[] ListParts( Paragraph _paragraph )
      {
         if ( _paragraph.Range.InlineShapes.Count > 0 )
         {
            return new TextPart[] { new TextPart( "ERROR: Inline-shape in paragraph - this is not supported yet. Please make sure that you have a paragraph end before and after the inline-shape.!", TextPartType.Error ) };
         }
         else if ( _paragraph.Range.ShapeRange.Count > 0 )
         {
            return new TextPart[] { new TextPart( "ERROR: Shape in paragraph - this is not supported yet. Please make sure that you have a paragraph end before and after the shape.!", TextPartType.Error ) };
         }
         else
         {
            Hyperlinks links = _paragraph.Range.Hyperlinks;
            if ( links.Count == 0 )
            {
               return new TextPart[] { new TextPart( _paragraph.Range.Text.TrimEnd( '\r' ), TextPartType.Plain ) };
            }
            else
            {
               int i = _paragraph.Range.Start;
               List<TextPart> parts = new List<TextPart>();
               foreach ( Hyperlink link in links )
               {
                  if ( i != link.Range.Start )
                  {
                     parts.Add( new TextPart( _paragraph.Range.Document.Range( i, link.Range.Start ).Text.TrimEnd( '\r' ), TextPartType.Plain ) );
                  }
                  parts.Add( new TextPart( link.TextToDisplay, TextPartType.Hyperlink, link.Address ) );
                  i = link.Range.End;
               }
               if ( i != _paragraph.Range.End )
               {
                  parts.Add( new TextPart( _paragraph.Range.Document.Range( i, _paragraph.Range.End ).Text.TrimEnd( '\r' ), TextPartType.Plain ) );
               }
               return parts.ToArray();
            }
         }
      }

      private static string GetStyleName( object _style )
      {
         if ( _style is Style )
         {
            return ( _style as Style ).NameLocal;
         }
         else
         {
            return null;
         }
      }

      private static KeyValuePair<Range, Content> ProcessInlineShape( InlineShape _shape )
      {
         float width = _shape.Width;
         float height = _shape.Height;
         string altText = "";
         string title = "";
         try
         {
            float scaleWidth = _shape.ScaleWidth;
            if ( scaleWidth > 0.0 )
            {
               //width *= (scaleWidth / 100.0f);
            }
         }
         catch
         {
         }
         try
         {
            float scaleHeight = _shape.ScaleHeight;
            if ( scaleHeight > 0.0 )
            {
               //height *= (scaleHeight / 100.0f);
            }
         }
         catch
         {
         }
         try
         {
            altText = _shape.AlternativeText;
         }
         catch
         {
         }
         try
         {
            title = _shape.Title;
         }
         catch
         {
         }
         PngImageContent imageContent = null;
         _shape.Range.CopyAsPicture();
         object pngData = Clipboard.GetData( "PNG" );
         if ( pngData is System.IO.MemoryStream )
         {
            imageContent = new PngImageContent( (System.IO.MemoryStream) pngData, altText, title, width, height );
         }
         else
         {
            if ( Clipboard.ContainsData( DataFormats.EnhancedMetafile ) )
            {
               if ( ClipboardFunctions.OpenClipboard( IntPtr.Zero ) )
               {
                  width *= 1.5f;
                  height *= 1.5f;
                  IntPtr data = ClipboardFunctions.GetClipboardData( DataFormats.GetFormat( DataFormats.EnhancedMetafile ).Id );
                  System.Drawing.Imaging.Metafile metaFile = new System.Drawing.Imaging.Metafile( data, true );
                  ClipboardFunctions.CloseClipboard();
                  System.Drawing.Bitmap pngImage = new System.Drawing.Bitmap( (int) width, (int) height );
                  using ( System.Drawing.Graphics g = System.Drawing.Graphics.FromImage( pngImage ) )
                  {
                     g.DrawImage( metaFile, new System.Drawing.Rectangle( 0, 0, (int) width, (int) height ) );
                  }
                  metaFile.Dispose();
                  System.IO.MemoryStream pngDataStream = new System.IO.MemoryStream();
                  pngImage.Save( pngDataStream, System.Drawing.Imaging.ImageFormat.Png );
                  pngDataStream.Seek( 0, System.IO.SeekOrigin.Begin );
                  imageContent = new PngImageContent( pngDataStream, altText, title, width, height );
                  pngImage.Dispose();
               }
            }
         }
         Clipboard.Clear();
         if ( imageContent == null )
         {
            return new KeyValuePair<Range, Content>( _shape.Range, new TextContent( String.Format( "INTERNAL ERROR: Could not convert shape to PNG image." ), Edward.ERROR_STYLE ) );
         }
         else
         {
            return new KeyValuePair<Range, Content>( _shape.Range, imageContent );
         }
      }

      public class ClipboardFunctions
      {
         [DllImport( "user32.dll", EntryPoint = "OpenClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall )]
         public static extern bool OpenClipboard( IntPtr hWnd );

         [DllImport( "user32.dll", EntryPoint = "EmptyClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall )]
         public static extern bool EmptyClipboard();

         [DllImport( "user32.dll", EntryPoint = "SetClipboardData", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall )]
         public static extern IntPtr SetClipboardData( int uFormat, IntPtr hWnd );

         [DllImport( "user32.dll", EntryPoint = "CloseClipboard", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall )]
         public static extern bool CloseClipboard();

         [DllImport( "user32.dll", EntryPoint = "GetClipboardData", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall )]
         public static extern IntPtr GetClipboardData( int uFormat );

         [DllImport( "user32.dll", EntryPoint = "IsClipboardFormatAvailable", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall )]
         public static extern short IsClipboardFormatAvailable( int uFormat );
      }
   }
}
