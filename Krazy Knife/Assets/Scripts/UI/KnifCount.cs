using UnityEngine;
using UnityEngine.UI;

public class KnifCount : MonoBehaviour
{
    public Image[] knifs = new Image[20];
    private int maxKnifs;
    private int currentKnifs;
    public void SetMaxKnifs(int knifCount)
    {
        foreach (var item in knifs)
            item.color = Color.clear;
        if (knifCount <= knifs.Length)
        {
            maxKnifs = knifCount;
            currentKnifs = knifCount;
            for (int i = 0; i < maxKnifs; i++)
                knifs[i].color = Color.white;
        }
    }
    public void SpendKnif()
    {
        if (currentKnifs > 0)
        {
            knifs[currentKnifs - 1].color = Color.black;
            currentKnifs -= 1;
        }
    }
}
