using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private int harvestActionCount;

    private TileDetails tileDetails;
    public void ProcessToolAction(ItemDetails tool, TileDetails tile)
    {
        tileDetails = tile;
        //工具使用次数
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;

        //判断是否有动画 树木

        //点击计数器
        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;

            //播放粒子
            //播放声音
        }

        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPosition)
            {
                //生成农作物
                SpawnHarvestItems();
            }
        }
    }

    public void SpawnHarvestItems()
    {
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;

            if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
            {
                //代表只生成指定数量的
                amountToProduce = cropDetails.producedMinAmount[i];
            }
            else    //物品随机数量
            {
                amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
            }

            //执行生成指定数量的物品
            for (int j = 0; j < amountToProduce; j++)
            {
                if (cropDetails.generateAtPlayerPosition)
                {
                    EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                }
                else
                {
                    
                }
            }
        }

        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                
                EventHandler.CallRefreshCurrentMap();
            }
            else
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
                
                
            }
            Destroy(gameObject);
        }
        
    }
}
