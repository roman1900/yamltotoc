# YAMLtoTOC 

YAMLtoTOC is a table of contents generator for markdown documents.
## Usage
```
YAMLtoTOC is a table of contents generator for markdown documents
Usage: yamltotoc.exe [options]
 options:
     -a, --append                   Append the TOC to the end of the output file. Caution without append the output file is overwritten  
     -h, --help                     Print this help message
     -o, --output {filename}        Provide the file name to output the TOC. Defaults to readme.md
     -p, --path {directory path}    Provide the path to the markdown files to create the TOC from. Defaults to the current directory 
```
yamltotoc will iterate the directory looking for markdown files. (*.md) Every markdown file will be checked for a starting yaml section with the following keys.
Additional yaml keys are allowed and will be ignored.

```
---

title: title of doc

shortdescription: short description of the doc contents

---
```
The yaml section of each file will be collated into a markdown table and written to the output file.
