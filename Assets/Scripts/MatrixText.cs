using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixText : MonoBehaviour
{
    public Text text;
    public bool rebooting = false;
    public bool done = false;
    public bool booting = false;
    public int duration = 3;
    public GameObject button;

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            if (!rebooting)
            {
                if (text.text.Length <= 2048)
                    text.text += Random.Range(0, 999).ToString();
                else
                    text.text = "";
            }
            else if (!booting)
                text.text = "Rebooting ...";
        }
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(duration);
        booting = true;

        yield return new WaitForSeconds(1);
        text.text = "To run a command as administrator (user \"root\"), use \"sudo <command>\".\n";

        yield return new WaitForSeconds(1);
        text.text += "See \"man sudo_root\" for details.\n\n";

        yield return new WaitForSeconds(1);
        text.text += "TN/users/Patrick/ $";

        done = true;
        yield return new WaitForSeconds(1);
        MinigameManager.instance.WinGame();
    }

    public void Reboot()
    {
        rebooting = true;
        button.SetActive(false);
        StartCoroutine("StartTimer");
    }
    
}
