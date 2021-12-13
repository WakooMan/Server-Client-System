using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    [Serializable]
    public class MyMessage
    {
        public string Message { get; set; }
        public int MNumber { get; set; }

        public MyMessage()
        { }

        public MyMessage(string mess,int n)
        {
            Message = mess;
            MNumber = n;
        }
    }
}
