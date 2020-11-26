using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal.VR;
using UnityEngine.UI;

public class Microwave : MonoBehaviour
{
    public Slider timer;
    public Slider door;
    public Text information;

    public int duration = 60;

    public bool pastaIn = false;
    public bool running = false;
    public bool done = false;
    public bool doorClosed = false;
    public bool doorMoving = false;

    IEnumerator StartTimer()
    {
        int seconde = 0;
        while (seconde < duration)
        {
            // suspend execution for 1 seconds
            yield return new WaitForSeconds(1);
            seconde += 1;
            timer.value = seconde;
            int affichage = duration - seconde;
            information.text = "Please return in " + affichage.ToString() + " secondes ...";
        }
        running = false;
        done = true;
        information.text = "Open the door.";
    }

    IEnumerator DoorOpening()
    {
        doorMoving = true;
        int satut = 0;
        while (satut < 100)
        {
            // suspend execution for 1 seconds
            yield return new WaitForSeconds(1);
            satut += 20;
            door.value = 100 - satut;
        }
        doorClosed = false;
        doorMoving = false;
        information.text = "Enjoy your meal !";
    }

    IEnumerator DoorClosing()
    {
        doorMoving = true;
        int satut = 0;
        while (satut < 100)
        {
            // suspend execution for 1 seconds
            yield return new WaitForSeconds(1);
            satut += 20;
            door.value = satut;
        }
        doorClosed = true;
        doorMoving = false;
        information.text = "Start the microwave.";
    }

    public void MovePasta()
    {
        pastaIn = true;
        information.text = "Close the door.";

    }

    public void Door()
    {
        if (!doorMoving)
        {
            if (!running)
            {
                if (doorClosed & done & pastaIn)
                {
                    StartCoroutine("DoorOpening");
                    information.text = "Door Opening...";
                }
                else if (!doorClosed & pastaIn & !done)
                {
                    StartCoroutine("DoorClosing");
                    information.text = "Door Closing...";
                }
                else
                {
                    information.text = "Nope";
                }
            }
            else
            {
                information.text = "Currently running !";
            }
        }
        else
        {
            information.text = "Door moving !";
        }
    }

    public void StartMicrowave()
    {
        if (!done)
        {
            if (pastaIn)
            {
                if (doorClosed)
                {
                    running = true;
                    StartCoroutine("StartTimer");
                }
                else
                {
                    information.text = "Door not closed !";
                }
            }
            else
            {
                information.text = "Nothing in !";
            }
        }
        else
        {
            information.text = "Already run !";
        }
        
    }

}
