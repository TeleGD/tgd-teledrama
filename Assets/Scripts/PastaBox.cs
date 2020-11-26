using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastaBox : MonoBehaviour
{

    public GameObject pastaboxIn;

    public void MovePastaBox()
    {
        pastaboxIn.SetActive(true);
        gameObject.SetActive(false);
    }

}
