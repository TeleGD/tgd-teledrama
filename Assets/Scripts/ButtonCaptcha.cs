using UnityEngine;
using UnityEngine.UI;

public class ButtonCaptcha : MonoBehaviour
{
    // Start is called before the first frame update
    public int number;
    public Button button;
    public GameObject panel;
    
    void Start()
    {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(taskOnClick);
    }

    public void taskOnClick()
    {
        print(number.ToString() + " appuyé");
        panel.gameObject.GetComponent<PanelCaptcha>().checkButton(number);
    }
}
