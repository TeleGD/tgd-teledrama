using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oscillo : MonoBehaviour
{
    public Image sine;
    public Image wanted;
    float t;

    public Slider f;
    public Vector2 f_range;
    public Slider an;
    public Vector2 an_range;
    public Slider a0;
    public Vector2 a0_range;

    bool[] bools = { false, false, false };

    float f_wanted;
    float an_wanted;
    float a0_wanted;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;

        f.maxValue = f_range.y;
        f.minValue = f_range.x;
        f_wanted = Random.Range(f_range.x, f_range.y);

        an.maxValue = an_range.y;
        an.minValue = an_range.x;
        an_wanted = Random.Range(an_range.x, an_range.y);

        a0.maxValue = a0_range.y;
        a0.minValue = a0_range.x;
        a0_wanted = Random.Range(a0_range.x, a0_range.y);

        wanted.rectTransform.localScale = new Vector3(f_wanted, an_wanted, 1);
        wanted.transform.localPosition = new Vector3(t, a0_wanted, 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        t = (t + speed) % 304;
        UpdateCurve();
        wanted.transform.localPosition = new Vector3(t, a0_wanted, 0);

        Vector3 error = new Vector3(Mathf.Abs(f_wanted - f.value), Mathf.Abs(an_wanted - an.value), Mathf.Abs(a0_wanted - a0.value));

        if (error.x < 0.01f)
        {
            bools[0] = true;
            f.interactable = false;
            Transform fill = f.transform.GetChild(1).GetChild(0);
            fill.GetComponent<Image>().color = new Color(0, 255, 0);
        }

        if (error.y < 0.01f)
        {
            bools[1] = true;
            an.interactable = false;
            Transform fill = an.transform.GetChild(1).GetChild(0);
            fill.GetComponent<Image>().color = new Color(0, 255, 0);
        }

        if (error.z < 5)
        {
            bools[2] = true;
            a0.interactable = false;
            Transform fill = a0.transform.GetChild(1).GetChild(0);
            fill.GetComponent<Image>().color = new Color(0, 255, 0);
        }

        if (bools[0] & bools[1] & bools[2])
        {
            Debug.Log("Done !");
        }
    }

    void UpdateCurve()
    {
        sine.rectTransform.localScale = new Vector3(f.value, an.value, 1);
        sine.transform.localPosition = new Vector3(t, a0.value, 0);
    }
}
