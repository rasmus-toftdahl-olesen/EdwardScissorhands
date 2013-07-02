EdwardScissorhands
==================

Simple tool to append multiple .docx files into one.

__ It requires Microsoft Office to be installed. __

Edward takes a simple text file as input (.Edward file) and generates a .docx and a .pdf file by copying the content from the .docx files specified in the input file to the generated .docx file.

When it is done it updates all fields in the document so tables of contennts, figures, index are correct, then it saves the file and saves the file as a PDF file.

This is the test example .edward file which is included in this repo to test edward.

  # Title: Edward Scissorhands
  # Subject: Test 1
  # Author: CIM Software Testing A/S
  # Company: CIM Software Testing A/S
  # Style: template.dotx
  
  # Comments are allowed!
  
  Title.docx
  TOC.docx
  This should page page 1.docx
  This should page page 2.docx
  This should page page 3.docx

Command line arguments
----------------------
If you give edward a filename as the last argument he will open that file.

If you give edward --generate as the first argument, he will generate the file and close when he is done copy/pasting.

So to generate the above file you would have to say:

  EdwardScissorhands.exe --generate Test1.Edward
