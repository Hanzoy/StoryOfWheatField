using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        public Item bounceItemPrefab;
        
        private Transform itemParent;

        private Transform PlayerTransform => FindObjectOfType<Player>().transform;
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            EventHandler.DropItemEvent -= OnDropItemEvent;
        }

        private void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
        {
            if (itemType == ItemType.Seed)
            {
                return;
            }
            var item = Instantiate(bounceItemPrefab, PlayerTransform.position, Quaternion.identity, itemParent);
            item.itemID = ID;
            var dir = (pos - PlayerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(pos, dir);
        }

        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
        }

        private void OnAfterSceneLoadedEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItems();
        }

        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }
        
        /// <summary>
        /// ????????????????????????Item
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };

                currentSceneItems.Add(sceneItem);
            }

            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //?????????????????????item????????????
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else    //??????????????????
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }


        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                    //??????
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}
