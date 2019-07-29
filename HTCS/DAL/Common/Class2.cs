using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{

    class Class3

    {

        public static Document insertDocumentAfterBookMark(Document mainDoc, Document tobeInserted, string bookmark)

        {

            // check maindoc

            if (mainDoc == null)

            {

                return null;

            }

            // check to be inserted doc

            if (tobeInserted == null)

            {

                return mainDoc;

            }

            DocumentBuilder mainDocBuilder = new DocumentBuilder(mainDoc);

            // check bookmark and then process

            if (bookmark != null && bookmark.Trim().Length > 0)

            {

                Bookmark bm = mainDoc.Range.Bookmarks[bookmark];

                if (bm != null)

                {

                    mainDocBuilder.MoveToBookmark(bookmark);

                    mainDocBuilder.Writeln();

                    Node insertAfterNode = mainDocBuilder.CurrentParagraph.PreviousSibling;

                    insertDocumentAfterNode(insertAfterNode, mainDoc, tobeInserted);

                }

            }

            else

            {

                // if bookmark is not provided, add the document at the end

                appendDoc(mainDoc, tobeInserted);

            }

            return mainDoc;

        }

        public static void insertDocumentAfterNode(Node insertAfterNode, Document mainDoc, Document srcDoc)

        {

            // Make sure that the node is either a pargraph or table.

            if ((insertAfterNode.NodeType != NodeType.Paragraph)

            & (insertAfterNode.NodeType != NodeType.Table))

                throw new Exception("The destination node should be either a paragraph or table.");

            //We will be inserting into the parent of the destination paragraph.

            CompositeNode dstStory = insertAfterNode.ParentNode;

            //Remove empty paragraphs from the end of document

            while (null != srcDoc.LastSection.Body.LastParagraph && !srcDoc.LastSection.Body.LastParagraph.HasChildNodes)

            {

                srcDoc.LastSection.Body.LastParagraph.Remove();

            }

            NodeImporter importer = new NodeImporter(srcDoc, mainDoc, ImportFormatMode.KeepSourceFormatting);

            //Loop through all sections in the source document.

            int sectCount = srcDoc.Sections.Count;

            for (int sectIndex = 0; sectIndex < sectCount; sectIndex++)

            {

                Section srcSection = srcDoc.Sections[sectIndex];

                //Loop through all block level nodes (paragraphs and tables) in the body of the section.

                int nodeCount = srcSection.Body.ChildNodes.Count;

                for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)

                {

                    Node srcNode = srcSection.Body.ChildNodes[nodeIndex];

                    Node newNode = importer.ImportNode(srcNode, true);

                    dstStory.InsertAfter(newNode, insertAfterNode);

                    insertAfterNode = newNode;

                }

            }

        }

      

        public static void appendDoc(Document dstDoc, Document srcDoc, bool includeSection)

        {

            // Loop through all sections in the source document.

            // Section nodes are immediate children of the Document node so we can

            // just enumerate the Document.

            if (includeSection)

            {

                foreach (Section srcSection in srcDoc.Sections)

                {

                    Node dstNode = dstDoc.ImportNode(srcSection, true, ImportFormatMode.UseDestinationStyles);

                    dstDoc.AppendChild(dstNode);

                }

            }

            else

            {

                //find the last paragraph of the last section

                Node node = dstDoc.LastSection.Body.LastParagraph;

                Node node1 = dstDoc.LastSection.Body.LastParagraph;
                if (node == null)

                {

                    node = new Paragraph(srcDoc);

                    dstDoc.LastSection.Body.AppendChild(node);

                }

                if ((node.NodeType != NodeType.Paragraph)

                & (node.NodeType != NodeType.Table))

                {

                    throw new Exception("Use appendDoc(dstDoc, srcDoc, true) instead of appendDoc(dstDoc, srcDoc, false)");

                }

                insertDocumentAfterNode(node, dstDoc, srcDoc);

            }

        }

        public static void appendDoc(Document dstDoc, Document srcDoc)

        {

            appendDoc(dstDoc, srcDoc, true);

        }

    }




}
