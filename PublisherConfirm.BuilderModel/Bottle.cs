using System;
using System.Collections.Generic;
using System.Text;

namespace PublisherConfirm.BuilderModel
{
    public class Bottle : IPacking
    {
        public string Pack()
        {
            return "Bottle";
        }
    }
}
