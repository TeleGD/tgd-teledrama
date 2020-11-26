using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.UI;
using System;

public class PanelCaptcha : MonoBehaviour
{
    public List<Captcha> captchas = new List<Captcha>();
    private Captcha current;


    public GameObject panelImage;

    void Start()
    {
        restart();
    }

    public void checkButton(int button)
    {
        if (!current.getButton(button))
        {
            print("pas bon bouton");
            restart();
        }
        else
        {
            print("bon bouton");
            current.setButton(button);
            if (current.checkList())
            {
                print("tous les boutons ont bien été appuyé");
            }
        }
    }

    private void restart()
    {
        System.Random random = new System.Random();
        int numb = random.Next(0, captchas.Count);

        current = captchas[numb];
        current.reset();
        panelImage.gameObject.GetComponent<Image>().sprite = current.Image;

        
    }
}

[Serializable]
public class Captcha
{
    public Sprite Image;

    public List<bool> goodButton = new List<bool>();

    private List<bool> pushedButtons = new List<bool>();

    public Captcha()
    {
        for (int i = 0; i < 9; i++)
        {
            pushedButtons.Add(false);
        }
    }

    public void setButton(int index)
    {
        pushedButtons[index] = true;
    }

    public bool getButton(int index)
    {
        return (goodButton[index]);
    }

    public bool checkList()
    {
        for (int i = 0; i < pushedButtons.Count; i++)
        {
            if (pushedButtons[i] != goodButton[i]) return false;
        }
        return true;
    }

    public void reset()
    {
        for (int i = 0; i < pushedButtons.Count; i++)
        {
            pushedButtons[i] = false;
        }
    }
}