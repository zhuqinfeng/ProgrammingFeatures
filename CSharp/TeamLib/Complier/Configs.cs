using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Complier
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Configs
    {
        internal const string Key_DefaultConfigsTplDictionary = "Configs";
        internal const string Key_TemplateFileExtension = "tpl";
        internal const string Key_SearchTplPattern = "*.tpl"; 

        private string projectPath;
        private TemplateEnvironment currentEnvironment;
        /// <summary>
        /// 当前编译环境
        /// </summary>
        public TemplateEnvironment CurrentEnvironment
        {
            get { return currentEnvironment; }
            private set { currentEnvironment = value; }
        }
        private List<TplFile> TplFiles;
        public Configs(string _projectPath,TemplateEnvironment _currentEnvironment)
        {
            this.projectPath = _projectPath;
            this.currentEnvironment = _currentEnvironment;
            this.TplFiles = TplFile.GetTplFiles(this.projectPath);
        }
        public ComplierMessage IsSuitClone()
        {
            ComplierMessage result = new ComplierMessage(true);
            Dictionary<string, string> replaceSigns = new Dictionary<string, string>();
            foreach (var tplfile in this.TplFiles)
            {
                foreach (var sign in tplfile.ReplaceSign)
                {
                    if (replaceSigns.ContainsKey(sign.Key))
                        continue;
                    replaceSigns.Add(sign.Key,sign.Value);
                }
            }
            foreach (var replaceSign in replaceSigns)
            {
                if (this.currentEnvironment.NodePairs.ContainsKey(replaceSign.Value))
                    continue;
                else
                {
                    result.Success = false;
                    result.ComplierMsg.Add(string.Format("{0}:缺少节点{1},资源标识{2}", this.currentEnvironment.EnvironmentName, replaceSign.Value, replaceSign.Key));
                }
            }
            return result;
        }
        /// <summary>
        /// 创建配置文件
        /// </summary>
        public void CreateConfigFiles()
        {
            foreach (var tplFile in this.TplFiles)
            {
                tplFile.CloneFile(this.currentEnvironment);
            }
        }
    }
    public class TplFile
    {
        internal const string regexPattern = @"\$\S+\$";
        private string fileDirectory;
        public string FileDirectory
        {
            get { return fileDirectory; }
        }
        private string fileName;
        public string FileName
        {
            get { return fileName; }
        }
        private string fileContent;
        public string FileContent
        {
            get { return fileContent; }
        }
        private Dictionary<string, string> replaceSign;
        public Dictionary<string,string> ReplaceSign
        {
            get { return replaceSign; }
        }

        public TplFile(string _projectPath, string _fileName)
        {
            this.fileDirectory = Path.Combine(_projectPath, Configs.Key_DefaultConfigsTplDictionary);
            this.fileName = _fileName;
            string filePath = Path.Combine(this.fileDirectory, this.fileName);
            this.fileContent = File.ReadAllText(filePath);
            this.replaceSign = TplFile.TakeReplaceSignAndNodeName(this.fileContent);
        }
        private static Dictionary<string, string> TakeReplaceSignAndNodeName(string _fileContent)
        {
            Dictionary<string, string> replcaceSign = new Dictionary<string, string>();
            Regex regex = new Regex(TplFile.regexPattern);
            MatchCollection matchCollection = regex.Matches(_fileContent);
            foreach (Match matchItem in matchCollection)
            {
                string mvalue = matchItem.Value.Trim('$');
                string mkey = matchItem.Value;
                if (replcaceSign.ContainsKey(mkey))
                    continue;
                replcaceSign.Add(mkey, mvalue);
            }
            return replcaceSign;
        }
        internal static List<TplFile> GetTplFiles(string _projectPath)
        {
            List<TplFile> tplFiles = new List<TplFile>();
            string tplPath = Path.Combine(_projectPath, Configs.Key_DefaultConfigsTplDictionary);
            string[] tplFilesPath = Directory.GetFiles(tplPath, Configs.Key_SearchTplPattern);
            foreach (string tplFilePath in tplFilesPath)
            {
                var tplFileSplits=tplFilePath.Split('\\');
                string fileName = tplFileSplits[tplFileSplits.Length - 1];
                TplFile tplFile = new TplFile(_projectPath, fileName);
                tplFiles.Add(tplFile);
            }
            return tplFiles;
        }
        /// <summary>
        /// 根据模板创建一个真实的文件出来
        /// </summary>
        public void CloneFile(TemplateEnvironment _currentEnvironment)
        {
            foreach (var replaceSignItem in this.replaceSign)
            {
                this.fileContent=this.fileContent.Replace(replaceSignItem.Key, _currentEnvironment.NodePairs[replaceSignItem.Value]);
            }

            string fileExtension=string.Format(".{0}", Configs.Key_TemplateFileExtension);
            string newFileName = this.fileName.Replace(fileExtension,"");
            string newFilePath= Path.Combine(this.fileDirectory,newFileName);
            File.WriteAllText(newFilePath, this.fileContent);
        }
    }
}
