using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Complier
{
    /// <summary>
    /// 项目工程
    /// </summary>
    public class Project
    {
        private string projectPath;
        private const string Key_CurrentEnvironment = "CurrentEnvironment";
        private string currentEnvironment;//当前编译环境
        private Configs projectConfigs;
        private Template projectTemplate;

        public Project(string _projectPath)
        {
            this.projectPath = _projectPath;
            this.currentEnvironment = ConfigurationManager.AppSettings[Project.Key_CurrentEnvironment].Trim();
            this.projectTemplate = new Template(this.projectPath, this.currentEnvironment);
            this.projectConfigs = new Configs(this.projectPath,this.projectTemplate.GetCurrentEnvironment());
        }
        /// <summary>
        /// 进行项目编译前的编译准备工作
        /// </summary>
        /// <returns></returns>
        public bool Compile()
        {
            bool successCompile = true;
            var projectTemplateCheck=this.projectTemplate.IsSame();

            if (projectTemplateCheck.Success)
            {
                var configsCheck = this.projectConfigs.IsSuitClone();
                var compileCheck = projectTemplateCheck + configsCheck;
                if (compileCheck.Success)
                    this.projectConfigs.CreateConfigFiles();
                else
                {
                    successCompile = false;
                    compileCheck.PrintMessage();
                }
            }
            else
            {
                successCompile = false;
                projectTemplateCheck.PrintMessage();
            }
            return successCompile;
        }
    }
}
