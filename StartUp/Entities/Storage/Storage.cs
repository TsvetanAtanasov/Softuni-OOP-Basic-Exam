namespace StorageMaster.Entities.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Vehicles;

    public abstract class Storage
    {
        private Vehicle[] garage;
        private List<Product> products;

        protected Storage(string name, int capacity, int garageSlots, IEnumerable<Vehicle> vehicles)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.GarageSlots = garageSlots;
            this.garage = new Vehicle[this.GarageSlots];
            this.products = new List<Product>();
            this.FillGarageWithInitialVehicles(vehicles);
        }

        public string Name { get; private set; }

        public int Capacity { get; private set; }

        public int GarageSlots { get; private set; }

        public IReadOnlyCollection<Product> Products => this.products.AsReadOnly();

        public IReadOnlyCollection<Vehicle> Garage => Array.AsReadOnly(this.garage);

        public bool IsFull => this.Products.Sum(p => p.Weight) >= this.Capacity;

        private void FillGarageWithInitialVehicles(IEnumerable<Vehicle> vehicles)
        {
            int index = 0;
            foreach (var vehicle in vehicles)
            {
                this.garage[index++] = vehicle;
            }
        }

        public Vehicle GetVehicle(int garageSlot)
        {
            if (garageSlot >= this.GarageSlots)
            {
                throw new InvalidOperationException("Invalid garage slot!");
            }

            var vehicle = this.garage[garageSlot];
            if (vehicle == null)
            {
                throw new InvalidOperationException("No vehicle in this garage slot!");
            }

            return vehicle;
        }

        public int SendVehicleTo(int garageSlot, Storage deliveryLocation)
        {
            var vehicle = GetVehicle(garageSlot);

            var deliveryGarageHasFreeSlot = deliveryLocation.Garage.Any(v => v == null);
            if (!deliveryGarageHasFreeSlot)
            {
                throw new InvalidOperationException("No room in garage!");
            }

            this.garage[garageSlot] = null;

            var addedSlot = deliveryLocation.AddVehicle(vehicle);
            return addedSlot;
        }

        public int UnloadVehicle(int garageSlot)
        {
            if (this.IsFull)
            {
                throw new InvalidOperationException("Storage is full!");
            }

            var vehicle = GetVehicle(garageSlot);

            var unloadedProductCount = 0;
            while (!vehicle.IsEmpty && !this.IsFull)
            {
                var crate = vehicle.Unload();
                this.products.Add(crate);

                unloadedProductCount++;
            }

            return unloadedProductCount;
        }

        private int AddVehicle(Vehicle vehicle)
        {
            var freeGarageSlotIndex = Array.IndexOf(this.garage, null);
            this.garage[freeGarageSlotIndex] = vehicle;

            return freeGarageSlotIndex;
        }

    }
}
