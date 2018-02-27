using System;
using System.Collections.Generic;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace DCReleaseTools.Utils
{
    public class ProjectHelper
    {
        public static ITreeNavigator MoveToParentFolderOrProjectNode(ITreeNavigator currentNode, ITreeNavigator node)
        {
            while (node.MoveToParent())
            {
                var parentItem = currentNode.DataItem;
                if (parentItem is ProjectFolder ||
                    parentItem is Project)
                {
                    return node;
                }

                node = currentNode;
            }

            return null;
        }

        public static IEnumerable<ProjectFile> GetSiblings(ITreeNavigator[] currentNode)
        {
            //TODO: implement
            return null;
        }


    }
}
