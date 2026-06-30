using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 환율정보_및_로컬자산_관리프로그램_정승현
{
    public class ExchangeResponse
    {
        public string result { get; set; }

        public string base_code { get; set; }

        public Dictionary<string, double> rates { get; set; }
    }
}
