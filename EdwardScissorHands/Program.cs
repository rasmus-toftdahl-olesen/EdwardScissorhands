using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EdwardScissorhands
{
   public static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      public static void Main(string[] args )
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Form1 form = new Form1();
         if ( args.Length > 0 )
         {
            form.Filename = args[args.Length - 1];
            if (args.Length > 1)
            {
               if (args[0] == "--generate")
               {
                  form.Generate = true;
               }
            }
         }
         Application.Run(form);
      }
   }
}
