using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/InventoryBag_SO", order = 0)]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
}