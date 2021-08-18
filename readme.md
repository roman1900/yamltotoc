# YAMLtoTOC 

YAMLtoTOC is a table of contents generator for markdown documents.
## Usage

yamltotoc will iterate the current directory looking for markdown files. (*.md) Every markdown files will be checked for a starting yaml section ie.

`---`

title: title of doc

shortdescription: short description of the doc contents

`---`

The yaml section of each file will be collated into a markdown table and written to readme.md. Additional yaml keys will be ignored.
