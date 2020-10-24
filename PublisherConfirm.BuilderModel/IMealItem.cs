using System;

namespace PublisherConfirm.BuilderModel
{
    public interface IMealItem
    {
        public string Name { get; }
        public decimal Price { get; }
        public IPacking Packing();
    }
}
