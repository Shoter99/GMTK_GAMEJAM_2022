using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class clickTriggered : MonoBehaviour
{
    public GameObject EventSystem = null;
    public bool isClicked;

    
    void Start() 
    {
        this.GetComponent<Animator>().Play("unclicked");
        isClicked = false;


    }


    public void clicked(){
        if (UiManager.Instance.isSelected) return;
        Debug.Log(isClicked);
        if (isClicked) {
            this.unclick();
        }
        else {
            UiManager.Instance.isSelected = true;
            this.GetComponent<Animator>().Play("clicked");
            this.isClicked = true;
            UiManager.Instance.receiveClickedAction(this.gameObject);
        }
    }

    public void unclick(){
        //if (UiManager.Instance.isSelected) return;
        if (isClicked){
            this.GetComponent<Animator>().Play("unclicked");
            isClicked = false;
            UiManager.Instance.isSelected = false;
        }        
    }
}
