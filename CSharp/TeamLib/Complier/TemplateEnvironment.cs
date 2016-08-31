using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Complier
{
    /// <summary>
    /// 模板中的环境类
    /// </summary>
    public class TemplateEnvironment
    {
        private string environmentName;
        /// <summary>
        /// 环境名称
        /// </summary>
        public string EnvironmentName
        {
            get
            {
                return this.environmentName;
            }
            private set
            {
                this.environmentName = value;
            }
        }
        private StringBuilder keyNodesStack;//节点堆栈
        public override int GetHashCode()
        {
            string keyNodes = this.keyNodesStack.ToString();
            return keyNodes.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (this.GetHashCode() == obj.GetHashCode())
                return true;
            return false;
        }
        private Dictionary<string, string> nodePairs;
        public Dictionary<string, string> NodePairs
        {
            get
            {
                return this.nodePairs;
            }
        }
        public TemplateEnvironment(string _environmentName)
        {
            this.environmentName = _environmentName;
            this.keyNodesStack = new StringBuilder();
            this.nodePairs = new Dictionary<string, string>();
        }
        private static XmlDocument templateXmlDocument;
        private static Dictionary<string, string> defaultConfig;
        static TemplateEnvironment()
        {
            templateXmlDocument = new XmlDocument();
        }
        /// <summary>
        /// 获取模板集合信息
        /// </summary>
        /// <returns></returns>
        public static List<TemplateEnvironment> GetEnvironments(string _templateFilePath, Dictionary<string, string> _defaultConfig)
        {
            templateXmlDocument.Load(_templateFilePath);
            defaultConfig = _defaultConfig;
            return GetTemplateInfoFromFile();
        }
        internal static List<TemplateEnvironment> GetTemplateInfoFromFile()
        {
            List<TemplateEnvironment> templateEnvironments = new List<TemplateEnvironment>();
            string environmentNodeSelect = string.Format("/{0}/{1}/{2}", Template.Key_RootNode, Template.Key_Environments, Template.Key_Environment);
            string resourceNodeSelect = string.Format("/{0}/{1}/{2}", Template.Key_RootNode, Template.Key_ConfigResourceValues, "{0}");
            XmlNodeList environmentNodes = templateXmlDocument.SelectNodes(environmentNodeSelect);
            foreach (XmlNode environmentNodeItem in environmentNodes)
            {
                TemplateEnvironment templateEnvironment = new TemplateEnvironment(environmentNodeItem.Attributes["Name"].Value.Trim());
                string resourceEnvironmentSelect = string.Format(resourceNodeSelect, templateEnvironment.environmentName);
                XmlNode resourceEnvironmentNode = templateXmlDocument.SelectSingleNode(resourceEnvironmentSelect);
                XmlNodeList resourceNodes = resourceEnvironmentNode.ChildNodes;
                foreach (XmlNode resourceNode in resourceNodes)
                {
                    templateEnvironment.keyNodesStack.Append(resourceNode.Name);
                    templateEnvironment.nodePairs.Add(resourceNode.Name, resourceNode.InnerText.Trim());
                }

                templateEnvironments.Add(templateEnvironment);
            }
            return templateEnvironments;
        }
    }
}
