using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickerClass
{
    public int stickerPrefabIndex;
    public Vector2 position;
    public GameObject stickerObject;

    public StickerClass()
    {
        stickerPrefabIndex = 0;
        position = Vector2.zero;
    }

}
