using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        ItemFader[] faders = col.GetComponentsInChildren<ItemFader>();
        if (faders.Length > 0)
        {
            foreach (var itemFader in faders)
            {
                itemFader.FadeOn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        if (faders.Length > 0)
        {
            foreach (var itemFader in faders)
            {
                itemFader.FadeIn();
            }
        }
    }
}