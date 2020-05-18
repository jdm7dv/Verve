-----------------------------------------------

Source Code Bundle for 

Expert F# (Apress)
Don Syme, Adam Granicz, Antonio Cisternino
ISBN-10: 1-59059-850-4

Publication Date: December 3, 2007
Source Bundle Date: November 14, 2007

-----------------------------------------------

ABOUT THE SOURCE CODE

   This source code bundle contains all the source code in the book.  Sources in
   each chapter are placed in separate directories, such as "Chapter02", and
   within those each example is placed in separate directories, such as "Example03".

   In the chapter directories you will find a README.txt file describing the
   examples in that chapter, giving

    * the short summary of the example (or the Listing label)
    * the page number in the book

   There are four types of examples:

    * an executable script ("script.fsx")
    * a single-module application ("main.fs")
    * a larger application consisting of several modules
    * ASP.NET web sites

   The following sections tells you how you can use/run/compile these examples.


HOW TO USE THE SOURCE CODE - EXECUTABLE SCRIPTS

   Most examples are F# scripts contained in a file called "script.fsx".  You can
   run these scripts using F# Interactive, or open them in Visual Studio, highlight
   code as you progress through the text, and press Alt+Enter to execute it in the
   F# Interactive plug-in.  This gives you fine control over running these scripts,
   giving you the ability to experiment with the code on the fly.


HOW TO USE THE SOURCE CODE - SINGLE-MODULE APPLICATIONS

   These are stand-alone applications that can be compiled using fsc.exe:

      fsc.exe -o app.exe main.fs

   You can run the resulting executable:

      app.exe

   
HOW TO USE THE SOURCE CODE - PROJECTS WITH MULTIPLE FILES

   These projects include a build.bat file that performs all the steps in
   creating the main executable from the various source files; e.g. it builds
   lexers and parsers from their definitions, etc.  You can clean the generated
   files using

      build.bat clean


HOW TO USE THE SOURCE CODE - ASP.NET WEBSITES

   The easiest way to test these examples is to use Visual Studio or Visual Web
   Developer, open the example directory as a website and use the built-in web
   server to serve the pages (press F5).

   Alternatively, you can follow the instructions in Chapter 14 to serve these
   examples out of IIS.

