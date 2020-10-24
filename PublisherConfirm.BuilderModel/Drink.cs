using System;
using System.Collections.Generic;
using System.Text;

namespace PublisherConfirm.BuilderModel
{
    public abstract class Drink : IMealItem
    {
        public abstract string Name { get;}
        public abstract decimal Price { get; }
        public IPacking Packing() => new Bottle();
    }
}
