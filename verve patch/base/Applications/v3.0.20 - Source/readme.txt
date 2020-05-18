FileExplorer is released under MIT license, it's less strict (thus compatitable with) 
than LGPL / MS-PL license.

***** FileExplorer3 *****

FileExplorer3 is an WPF based control that emulate most aspect of Shell Explorer, 
you can use it to display shell objects or any hierarchical data.  

You can use it as a standalone window, as a FilePicker (Open or Save), FolderPicker, 
or embed it as a part of your UI. 

The project source code is hosted on Codeplex
http://fileexplorer.codeplex.com/

Documentation can be found in
http://www.codeproject.com/Articles/770837/WPF-x-FileExplorer-x-MVVM

If you want to compile FileExplorer3 yourself, please noted that TestApp.WPF require online 
api keys to access online contents, they cant be include in the source code, 
please find more information here.
http://www.codeproject.com/Articles/770837/WPF-x-FileExplorer-x-MVVM#Compilingthesourcecode

***** FileExplorer2 *****

FileExplorer2 is written using Cinch v2 MVVM framework.  
FileExplorer2 includes Breadcrumb, FolderTree, FileList, Toolbar and Statusbar (in addition to DirectoryTree and FileList). 

* Please noted that this version is no longer supported * 
1. Please noted that this project is under MIT license (was released as LGPL license).
2. Documentation please find in doc\FileExplorer2.html, there are a number of diagram which is useful to understand the component as well.
3. Demo is attached, in src\test\integration\Explorer2TestProj\bin\Debug\Explorer2TestProj.exe
4. Before you compile, you have to remove the "Sign the assembly" in QuickZip.UserControls and QuickZip.UserControls.Explorer,
they are signed because it's required for shell extension, you can create your own signature file if you like.

***** FileExplorer1 *****

FileExplorer controls included DirectoryTree and FileList, using Model-View-ViewModel (MVVM) pattern and Cinch framework.

* Please noted that this version is no longer supported * 
Please find the source code and documentation in CodeProject:
http://www.codeproject.com/Articles/78517/WPF-x-FileExplorer-x-MVVM