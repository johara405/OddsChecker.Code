using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsChecker
{
    public class Selection
    {
        protected int id;
        protected String name;
        protected String odds;
        protected int result;


        public Selection()
        {
            
        }

        public int ID
        {
            get { return id;}
            set { id = value;}
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Odds
        {
            get { return odds; }
            set { odds = value; }
        }

        public int Result
        {
            get { return result; }
            set { result = value; }
        }
        
        
    }
}
