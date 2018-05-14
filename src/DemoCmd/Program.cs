using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDatumParser;

namespace DemoCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string gtxFilePath = @"C:\VDatum\ALFL_gom02_8301\mllw.gtx";
            GtxFile gtxFile = new GtxFileParser().Parse(gtxFilePath);
        }
    }
}
