// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Xml;

namespace MBF.PerfTests.Util
{

    /// <summary>
    /// This class contains the all the xml related functions.
    /// </summary>
    internal class XmlUtility
    {
        XmlDocument m_xmlDoc;

        /// <summary>
        /// Constructor which sets the config file.
        /// </summary>
        /// <param name="xmlFilePath">Config file path.</param>
        internal XmlUtility(string xmlFilePath)
        {
            m_xmlDoc = new XmlDocument();
            m_xmlDoc.Load(xmlFilePath);
        }

        /// <summary>
        /// Returns the Text value for the nodes specified from the configuration file.
        /// </summary>
        /// <param name="parentNode">Name of the Parent node.</param>
        /// <param name="nodeName">Name of the node, from which the text value to be read.</param>
        /// <returns>Text with in the node.</returns>
        internal string GetTextValue(string parentNode, string nodeName)
        {
            XmlNode actualNode = GetNode(parentNode, nodeName);
            return actualNode.InnerText;
        }

        /// <summary>
        /// Returns the node from the configuration file.
        /// </summary>
        /// <param name="parentNode">Name of the Parent node</param>
        /// <param name="nodeName">Name of the node, which needs to be returned</param>
        /// <returns>Xml node.</returns>
        internal XmlNode GetNode(string parentNode, string nodeName)
        {
            XmlNode lst = null;

            lst = m_xmlDoc.ChildNodes[1];
            string xPath = string.Concat("/AutomationTest/", parentNode, "/", nodeName);

            XmlNode childNode = lst.SelectSingleNode(xPath);

            if (null == childNode)
                throw new XmlException(string.Format(null, "Could not find the Xpath '{0}'", xPath));

            return childNode;
        }
    }
}
