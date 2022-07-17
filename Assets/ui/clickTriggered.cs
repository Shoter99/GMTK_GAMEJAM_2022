using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class clickTriggered : MonoBehaviour
{
    public GameObject EventSystem = null;
    public bool isClicked;
    public UiManager receiver;

    
    void Start() 
    {
        receiver = EventSystem.GetComponent<UiManager>();
        this.GetComponent<Animator>().Play("unclicked");
        isClicked = false;


    }


    public void clicked(){
        Debug.Log(receiver.selectedCount);

        if (isClicked) {
            this.unclick();
        }
        else {
            if (receiver.selectedCount < 2) {
            this.GetComponent<Animator>().Play("clicked");
            this.isClicked = true;
            receiver.receiveClickedAction(this.gameObject);
            receiver.selectedCount++;

            }
        }
    }

    public void unclick(){
        if (isClicked){
            this.GetComponent<Animator>().Play("unclicked");
            receiver.selectedCount--;
            isClicked = false;
        }        
    }
}
