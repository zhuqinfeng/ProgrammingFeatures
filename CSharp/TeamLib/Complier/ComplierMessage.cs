using System;
using System.Collections.Generic;
using System.Text;

namespace Complier
{
    /// <summary>
    /// 编译过程中产生的信息
    /// </summary>
    public class ComplierMessage
    {
        private bool success;
        public bool Success
        {
            get { return success; }
            set{success=value;}
        }
        private List<string> complierMsg;
        public List<string> ComplierMsg
        {
            get{return complierMsg;}
        }
        public ComplierMessage()
        {
            this.complierMsg = new List<string>();
        }
        public ComplierMessage(bool _success)
        {
            this.complierMsg = new List<string>();
            this.success = _success;
        }
        public static ComplierMessage operator +(ComplierMessage a,ComplierMessage b)
        {
            ComplierMessage newComplierMessage = new ComplierMessage(a.success && b.success);
            newComplierMessage.complierMsg.AddRange(a.complierMsg);
            newComplierMessage.complierMsg.AddRange(b.complierMsg);
            return newComplierMessage;
        }
        public void PrintMessage()
        {
            foreach (var msg in this.complierMsg)
            {
                System.Console.WriteLine(msg);
            }
        }
    }
}
