using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Model
{
    [Serializable]
    public class TestModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public DateTime? Brith { get; set; }

        public decimal? Money { get; set; }
    }
}
