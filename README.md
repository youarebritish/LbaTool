LbaTool
========
A tool for compiling and decompiling Fox Engine lba files, which store locators and locator metadata.

Requirements
--------
```
Microsoft .NET Framework 4.5 
```

Usage
--------
```
Drag and drop one or more .lba files onto the .exe to unpack them to .xml files. Do the same with .xml files to repack them into .lba format.

If a file called "lba dictionary.txt" is in the same directory as the .exe, it will hash each line and use it as a lookup table for hashes in the locators. Matched strings will be output to a file called "lba hash matches.txt".