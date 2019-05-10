using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {

    //TODO rework when new items are introduced
    [System.Serializable]
	public class Inventory
    {
        public int maxWeight = 100;
        public int shaleAmount = 0;

        public Inventory(int maxWeight, int shaleAmount)
        {
            this.maxWeight = maxWeight;
            this.shaleAmount = shaleAmount;
        }

        public bool AddShale(int amount)
        {
            Debug.Log("Adding shale: " + amount);
            if(maxWeight >= shaleAmount + amount)
            {
                shaleAmount += amount;
                return true;
            } else
            {
                shaleAmount = maxWeight;
                return false;
            }
        }

        public bool IsFull
        {
            get {
                return shaleAmount == maxWeight;
            }
        }
    }

    public Inventory inventory;

    public Character(Inventory inventory)
    {
        this.inventory = inventory;
    }
}
