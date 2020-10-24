using System;
using System.Collections.Generic;
using System.Text;

namespace PublisherConfirm.BuilderModel
{
    public abstract class Burger : IMealItem
    {
        public abstract string Name { get; }
        public abstract decimal Price { get; }
        public IPacking Packing() => new Wrapper();
    }
}
