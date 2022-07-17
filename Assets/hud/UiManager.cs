using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    public int health = 10;
    public int minValue = 1;
    // order of dices: melee, ranged, reposition, heal, defense
    public GameObject[] diceParents = {};
    public GameObject hpBar;
    public int[] diceCounts = new int[5];
    public Animator[,] diceAnims = new Animator[5,3];
    public int[] selected = {7,7};
    public bool isSelected = false;


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < hpBar.transform.childCount; j++)
            {
            hpBar.transform.GetChild(j).gameObject.SetActive(false);
            }
            diceCounts[i] = diceParents[i].transform.childCount;
            for (int j = 0; j < diceCounts[i]; j++)
            {
                diceAnims[i,j] = diceParents[i].transform.GetChild(j).GetChild(0).gameObject.GetComponent<Animator>();
            }
            int [,]  values = {{1,0,0},{2,0,0},{2,0,0},{3,0,0},{7,0,0}};
            updateHealth(15);
        }
    }
    
//reroll takes [5,3] array of numbers to show on dices
    public void reroll(int [,] diceValues){

        removeSelected();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < diceCounts[i]; j++)
            {
                diceParents[i].transform.GetChild(j).GetChild(0).GetComponent<Animator>().Play(diceValues[i,j].ToString());
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < diceCounts[i]; j++)
            {
                Animator anim = diceParents[i].transform.GetChild(j).GetComponent<Animator>();
                anim.Play("revealDice");
            }
        }
    }

    public void add_dice(int diceType)
    {
        diceCounts[diceType]++;
        int diceIndex = diceCounts[diceType] - 1;
        GameObject newDice = Instantiate(diceParents[diceType].transform.GetChild(0).gameObject, diceParents[diceType].transform);
        newDice.transform.localPosition = new Vector3(0,-100 * diceCounts[diceType],0);
        newDice.transform.localScale = new Vector3(1,1,1);
        diceAnims[diceType,diceIndex] =  newDice.GetComponent<Animator>();


    }

    public void receiveClickedAction(GameObject icon){
        for (int i = 0; i<5; i++){
            if (icon == diceParents[i])
            {
                SetAction(i);
                break;
            }
            //here two actions are selected, stored in variable selected as numbers 0-4 respectively with order of which icons are displayed from left to right
        }
    }

    private void SetAction(int i)
    {
        Debug.Log("Test");
        switch (i)
        {
            case 0:
                Debug.Log(i);
                Player.Instance.actionTaken = "Melee";
                break;
            case 1:
                Debug.Log(i);
                Player.Instance.actionTaken = "Fire";
                break;
            case 2:
                Debug.Log(i);
                Player.Instance.actionTaken = "Move";
                break;
            case 3:
                Debug.Log(i);
                Player.Instance.actionTaken = "Heal";
                break;
            case 4:
                Debug.Log(i);
                Player.Instance.actionTaken = "Defend";
                break;
        }
    }

    public void removeSelected(){
        for(int i = 0; i<5; i++){
            diceParents[i].GetComponent<clickTriggered>().unclick();
        }
        Debug.Log(diceParents[2]);
        
    }
    private void LateUpdate()
    {
        if(Player.Instance.actionTaken == "None")
        {
            removeSelected();
        }
    }
    public void updateHealth(int health)
    {
        health = 20 - health;
        //unhide all children of hpbar
        for (int i = 0; i<hpBar.transform.childCount ; i++)
        {  
            Debug.Log(i);
            if (i<health){
                hpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else{
                hpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
