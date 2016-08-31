using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Complier
{
    /// <summary>
    /// 模板信息
    /// </summary>
    public class Template
    {
        internal const string Key_ResourceValueXMLName = "ConfigResourceValue.xml";//配置资源文件
        internal const string Key_CurrentEnvironment = "Key_CurrentEnvironment";//当前环境关键字

        internal const string Key_RootNode = "ConfigResourceValueRoot";//XML文档的根节点
        internal const string Key_Environments = "Environments";//环境集合节点
        internal const string Key_Environment = "Environment";//环境集合子元素节点
        internal const string Key_ConfigResourceValues = "ConfigResourceValues";//配置资源值根的根节点
        

        private string projectPath;
        private string currentEnvironment;//当前编译的环境，这个是为基准
        public string CurrentEnvironment
        {
            get
            {
                return this.currentEnvironment;
            }
            private set
            {
                this.currentEnvironment = value;
            }
        }
        /// <summary>
        /// 获取当前编译环境模板
        /// </summary>
        /// <returns></returns>
        public TemplateEnvironment GetCurrentEnvironment()
        {
            return this.Environments.Where(currentEnv => currentEnv.EnvironmentName.Equals(this.CurrentEnvironment)).First();
        }
        private string environmentsFile;
        private List<TemplateEnvironment> Environments;
        public Template(string _projectPath,string _currentEnvironment)
        {
            this.projectPath = _projectPath;
            this.currentEnvironment = _currentEnvironment;
            this.environmentsFile = Path.Combine(this.projectPath, Template.Key_ResourceValueXMLName);
            Dictionary<string, string> defaultConfig = new Dictionary<string, string>()
            {
                {"Key_RootNode",Template.Key_RootNode},
                {"Key_Environments",Template.Key_Environments},
                {"Key_Environment",Template.Key_Environment},
                {"Key_ConfigResourceValues",Template.Key_ConfigResourceValues},
                {"Key_CurrentEnvironment",this.currentEnvironment}
            };
            this.Environments = TemplateEnvironment.GetEnvironments(this.environmentsFile, defaultConfig);
        }

        public ComplierMessage IsSame()
        {
            ComplierMessage result = new ComplierMessage(true);
            TemplateEnvironment currentEnvironmentTemplate = this.Environments.Where(template => template.EnvironmentName.Equals(this.currentEnvironment)).First();
            foreach (var environmentTemplate in Environments)
            {
                if (currentEnvironmentTemplate.Equals(environmentTemplate))
                    continue;
                else
                {
                    result.Success = false;
                    result.ComplierMsg.AddRange(this.PrintDifference(currentEnvironmentTemplate, environmentTemplate));
                }
            }
            return result;
        }
        /// <summary>
        /// 打印不同之处
        /// </summary>
        /// <param name="source">比较源</param>
        /// <param name="target">比较目标</param>
        private List<string> PrintDifference(TemplateEnvironment source, TemplateEnvironment target)
        {
            List<string> differenceContents = new List<string>();
            foreach(var sourceNodePair in source.NodePairs)
            {
                if (!target.NodePairs.ContainsKey(sourceNodePair.Key))
                {
                    string keyDifference = string.Format("{0}-{1}:{2}", source.EnvironmentName, target.EnvironmentName, sourceNodePair.Key);
                    differenceContents.Add(keyDifference);
                }
            }
            this.ResolveDifference(target, source, differenceContents);
            return differenceContents;
        }
        private void ResolveDifference(TemplateEnvironment target, TemplateEnvironment source, List<string> differenceContents)
        {
            foreach (var targetNodePair in target.NodePairs)
            {
                if (!source.NodePairs.ContainsKey(targetNodePair.Key))
                {
                    string keyDifference = string.Format("{0}-{1}:{2}", source.EnvironmentName, target.EnvironmentName, targetNodePair.Key);
                    if(differenceContents.Where(difference=>difference.Equals(keyDifference)).Count()==0)
                        differenceContents.Add(keyDifference);
                }
            }
        }
    }
}
