using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibEdward
{
   public abstract class Content
   {
      internal Content()
      {
      }
      
      public abstract ContentType Type { get; }

      public TextContent AsText { get { return this as TextContent; } }
      public TableContent AsTable { get { return this as TableContent; } }
      public PngImageContent AsPngImage { get { return this as PngImageContent; } }
   }
}
