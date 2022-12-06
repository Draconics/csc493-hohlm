# Test Plan

## Summary
Testing of the main document editor is done via creating example files, and then saving and loading them.  If the document loads correctly, then there is assumed to be no errors.  If it does not load correctly, then the file is inspected via a standard text editor (Notepad++) to see if it has saved incorrectly.  If it has been saved correctly, then the error must be in the loading of the document.

## Key Test Cases
### 1: Blank Document
Tests requirement 1
Steps: Save a blank document, edit the file, then reload the blank document.  Test succeeds if all content added is removed.
### 2: Full Document
Tests requirement 1
Steps: Save a document with a title, summary, and body. Load an empty document, then reload the full document.
### 3: Saving a short document over a long document
Tests requirement 1
Steps: Put a significant amount of text into the body element, and then save the document. Remove multiple lines from the body, and then resave. Load an empty document, then reload the first document.

### 4: Links
Tests requirement 2
Steps: Link a document to another document. Click the link.
### 5: Link saving
Tests requirement 3
Steps: Link a document to another document. Save the document. Load an empty document, then reload the first document.

