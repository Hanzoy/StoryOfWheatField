using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CropDataList_SO", menuName = "Crop/CropDataList", order = 0)]
public class CropDataList_SO : ScriptableObject
{
    public List<CropDetails> cropDetailsList;
}

