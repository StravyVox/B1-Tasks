using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableObjectsClassLibrary;
namespace TableObjectsClassLibrary
{
    /// <summary>
    /// Class, that contains info about class and all Records attached to it
    /// </summary>
    public class ClassRecords
    {
        private ClassAddress classInfo;
        private List<Table> tables;

        public ClassAddress ClassInfo { get => classInfo; set => classInfo = value; }
        public List<Table> Tables { get => tables; set => tables = value; }
    }
}
