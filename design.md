# Design Document
## Main Window
  Sets initilization values
## Methods 
### Load_File()
  Code for loading a file. Calls Generate_Link() for each link saved in the file
### Generate_Link()
  Adds a new link to a page
## Click Events
### OpenButton_Click()
  Opens a file picker, then feeds that file to Load_File()
### SaveButton_Click()
  Saves current file
### LinkButton_Click()
  Opens a file picker, then feeds that file to Generate_Link()
### Link_Click()
  Feeds the file associated with the link to Load_File()
### BoldButton_Click()
  Formats selected text bold
### ItalicButton_Click()
  Formats selected text italic
### UnderlineButton_Click()
  Formats selecteed text underlined
