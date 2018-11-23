namespace StorageMaster.Entities.Vehicles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public abstract class Vehicle
    {
        private List<Product> products;

        protected Vehicle(int capacity)
        {
            this.Capacity = capacity;
            this.products = new List<Product>();
        }

        public int Capacity { get; private set; }

        public IReadOnlyCollection<Product> Trunk => this.products.AsReadOnly();
        //TODO field?
        public bool IsFull => this.Trunk.Sum(p => p.Weight) >= this.Capacity;

        public bool IsEmpty => this.Trunk.Count == 0;

        public void LoadProduct(Product product)
        {
            if (this.IsFull)
            {
                throw new InvalidOperationException("Vehicle is full!");
            }
            this.products.Add(product);
        }

        public Product Unload()
        {
            if (this.Trunk.Count == 0)
            {
                throw new InvalidOperationException("No products left in vehicle!");
            }
            var elem = this.products[this.products.Count - 1];
            this.products.Remove(elem);
            return elem;
        }
    }
}
