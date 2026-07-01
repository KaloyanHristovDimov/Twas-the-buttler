using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupScript : MonoBehaviour
{
    [SerializeField] private GameObject spriteParent;
    [SerializeField] private GameObject tagParent;
    [SerializeField] private Image spriteLocation;
    [SerializeField] private TextMeshProUGUI tagLocation;
    [SerializeField] private float popupTime = .5f;
    private bool buzy;


    public void PopupClues(List<ClueData> listOfClueData)
    {
        foreach (ClueData data in listOfClueData)
        {
            PopupSprite(data.clueSprite);
            foreach (ClueData.StringTag tag in data.asosiatedStringTags)
            {
                PopupTag(tag.ToString());
            }
        }
    }

    
    public void StartPopupCoroutine(List<ClueData> listOfClueData)
    {
        StartCoroutine(PopupCluesZ(listOfClueData));
    }

    IEnumerator PopupCluesZ(List<ClueData> listOfClueData)
    {
        foreach (ClueData data in listOfClueData)
        {
            PopupSprite(data.clueSprite);
            yield return new WaitForSeconds(popupTime);
            SetFalse();
            foreach (ClueData.StringTag tag in data.asosiatedStringTags)
            {
                PopupTag(tag.ToString());
                yield return new WaitForSeconds(popupTime);
                SetFalse();
            }
        }
    }

    private void SetFalse()
    {
        spriteParent.SetActive(false);
        tagParent.SetActive(false);
    }

    private void PopupSprite(Sprite sprite)
    {
        spriteParent.SetActive(true);
        spriteLocation.sprite = sprite;
    }

    private void PopupTag(string tag)
    {
        tagParent.SetActive(true);
        tagLocation.text = tag;
    }


}
