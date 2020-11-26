using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bugController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public void clic(){
    
    Destroy(this.gameObject);
    this.transform.parent.gameObject.GetComponent<bugGameControler>().bugDestroyes();
}


}
