using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddonFE.Models
{
  
   
    public class Modulo
    {
        public string type { get; set; }
        public string UniqueID { get; set; }
        public string String { get; set; }
    }

    public class Root
    {
        public List<Modulo> Modulo { get; set; }
    }
}
