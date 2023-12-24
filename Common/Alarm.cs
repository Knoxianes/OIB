using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum UtilityLVL { Information, Warning, Error };
    public class Alarm
    {
        private DateTime dateTime;
        private string pname;
        private UtilityLVL utLVL;

        public Alarm()
        {
        }

        public Alarm(DateTime dateTime, string pname, UtilityLVL utLVL)
        {
            this.DateTime = dateTime;

            this.Pname = pname;
            this.UtLVL = utLVL;
        }

        public DateTime DateTime { get => dateTime; set => dateTime = value; }

        public string Pname { get => pname; set => pname = value; }
        public UtilityLVL UtLVL { get => utLVL; set => utLVL = value; }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}",dateTime,utLVL,pname);
        }
    }
}
