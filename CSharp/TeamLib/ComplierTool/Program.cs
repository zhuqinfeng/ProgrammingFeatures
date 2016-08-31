using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complier;
using System.Text.RegularExpressions;
using System.IO;
namespace ComplierTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool success = true;
            try
            {
                Complier.Project project = new Complier.Project(args[0]);
                success = project.Compile();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                if (success)
                    Environment.Exit(0);
                else
                    Environment.Exit(1);
            }
        }
    }
}
