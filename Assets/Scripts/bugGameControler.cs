using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bugGameControler : MonoBehaviour
{

    public GameObject bug;
    public Sprite[] sprites;

    public bool jeufini=false;
    public int nbBug = 10;


    float maxx = 180;
    float maxy = 80;

    public float mindist = 1;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> pos = new List<Vector2>();
        for(int i=0; i<nbBug;i++){
            GameObject b = Instantiate(bug);
            b.transform.SetParent(this.transform);
            
            UnityEngine.UI.Image img = b.GetComponent<UnityEngine.UI.Image>();
            img.color = Random.ColorHSV();
            img.sprite = sprites[Random.Range(0,sprites.Length)];

            RectTransform rt = b.GetComponent<RectTransform>();

            Vector2 vec = new Vector2(0,0);

            
            bool foo = false;
            
            int antibloc = 0;

            while(!foo){
                antibloc++;
                vec = new Vector2(Random.Range(-maxx, maxx), Random.Range(-maxy, maxy));
                foo = true;
                Debug.Log("moo");
                for(int j=0;j<pos.Count;j++){
                    if(Vector2.Distance(pos[j], vec) < mindist){
                        Debug.Log("aggggg");
                        foo = false;
                        break;
                    }
                }

                if(antibloc > 30){ //Si après 30 essai on est tj bloqué, on arrête touuut
                    foo=true;
                }

            }

            pos.Add(vec);
            print(pos.Count);
            rt.anchoredPosition = vec;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void bugDestroyes(){
        if(transform.childCount <= 1){
            MinigameManager.instance.WinGame()
        }
        
    }

}
