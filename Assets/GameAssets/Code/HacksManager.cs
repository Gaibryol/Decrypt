using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HacksManager : MonoBehaviour
{

    public static HacksManager Instance { get; private set; }
    public List<CombinedHack> ActivatedHacks; //hack[0] is a positive hack & hacks[1] is a negative hack
    public int ShowAmount;
    public Camera mainCamera;
    private GraphicRaycaster raycaster;

    private List<Hack> allPositiveHacks;
    private List<Hack> allNegativeHacks;
    private List<Hack> sp_PositiveHacks;
    private List<Hack> sp_NegativeHacks;
    private List<Hack> mp_PositiveHacks;
    private List<Hack> mp_NegativeHacks;

    private Queue<Hack> queueHacks;
    private List<Hack> applyHacks;
    private Hack updateHacks;
    private Hack rightClickHack;
    
	[SerializeField] private GameController gameController;
	[SerializeField] private GameUIController gameUIController;


    private void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
        }
        InitVariables();
    }

    public void InitVariables()
    {
        allPositiveHacks = new List<Hack>(){new PositiveHack0(), new PositiveHack1(), new PositiveHack2(), new PositiveHack3(), 
                                            new PositiveHack4(), new PositiveHack5(), new PositiveHack6(), new PositiveHack7(), 
                                            new PositiveHack8()};
        allNegativeHacks = new List<Hack>(){new NegativeHack0(), new NegativeHack1(), new NegativeHack2(), new NegativeHack3(), 
                                            new NegativeHack4(),new NegativeHack5()};

        sp_PositiveHacks = new List<Hack>();
        sp_NegativeHacks = new List<Hack>();


        mp_PositiveHacks = new List<Hack>();
        mp_NegativeHacks = new List<Hack>();

        foreach(Hack hack in allPositiveHacks){
            if(hack.GetGameMode() == Constants.GameMode.SinglePlayer)
                sp_PositiveHacks.Add(hack);
            else if(hack.GetGameMode() == Constants.GameMode.Multiplayer)
                mp_PositiveHacks.Add(hack);
            else
            {
                sp_PositiveHacks.Add(hack);
                mp_PositiveHacks.Add(hack);
            }
        }

        foreach(Hack hack in allNegativeHacks){
            if(hack.GetGameMode() == Constants.GameMode.SinglePlayer)
                sp_NegativeHacks.Add(hack);
            else if(hack.GetGameMode() == Constants.GameMode.Multiplayer)
                mp_NegativeHacks.Add(hack);
            else
            {
                sp_NegativeHacks.Add(hack);
                mp_NegativeHacks.Add(hack);
            }
        }

        ShowAmount = 0;
        ActivatedHacks = new List<CombinedHack>();
        applyHacks = new List<Hack>();
        rightClickHack = null;
        updateHacks = null;
        queueHacks = new Queue<Hack>();
        raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
    }
    private void Update()
    {
        //Check if the left Mouse button is clicked
         if (rightClickHack != null && Input.GetKeyDown(KeyCode.Mouse1))
         {
             //Set up the new Pointer Event
             PointerEventData pointerData = new PointerEventData(EventSystem.current);
             List<RaycastResult> results = new List<RaycastResult>();
 
             //Raycast using the Graphics Raycaster and mouse click position
             pointerData.position = Input.mousePosition;
             this.raycaster.Raycast(pointerData, results);
 
             //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
             foreach (RaycastResult result in results)
             {
                if(result.gameObject.name == "Word(Clone)" && result.gameObject.GetComponent<Word>().IsInteractable && rightClickHack.GetRightClickType() == Constants.RightClickTarget.Word)
                    rightClickHack.RightClick(result.gameObject);
             }
            if(rightClickHack.GetRightClickType() == Constants.RightClickTarget.All)
                rightClickHack.RightClick(null);
         }
         if(updateHacks != null)
            updateHacks.Update();
    }

    public void AddCombinedHack(CombinedHack combinedHack)
    {
        Hack positiveHack = combinedHack.GetPositiveHack();
        Hack negativeHack = combinedHack.GetNegativeHack();
        sp_PositiveHacks.Remove(positiveHack);
        sp_NegativeHacks.Remove(negativeHack);

        
        for(int i = sp_PositiveHacks.Count -1; i >= 0; i--)
        {
            Hack tempHack = sp_PositiveHacks[i];
            if(tempHack.GetEffectType() == positiveHack.GetEffectType() || tempHack.GetEffectType() == negativeHack.GetEffectType())
                sp_PositiveHacks.Remove(tempHack);
        }

        for(int i = sp_NegativeHacks.Count -1; i >= 0; i--)
        {
            Hack tempHack = sp_NegativeHacks[i];
            if(tempHack.GetEffectType() == negativeHack.GetEffectType() || tempHack.GetEffectType() == positiveHack.GetEffectType())
                sp_NegativeHacks.Remove(tempHack);
        }

        negativeHack.Initialize();
        positiveHack.Initialize();

        ActivatedHacks.Add(combinedHack);
	}

    public List<CombinedHack> GenerateCombinedHacks()
    {
        bool running = true;
        Hack positiveHack1 = null;
        Hack positiveHack2 = null;
        Hack negativeHack1 = null;
        Hack negativeHack2 = null;

        while(running)
        {
            positiveHack1 = sp_PositiveHacks[Random.Range(0,sp_PositiveHacks.Count)];
            positiveHack2 = sp_PositiveHacks[Random.Range(0,sp_PositiveHacks.Count)];
            negativeHack1 = sp_NegativeHacks[Random.Range(0,sp_NegativeHacks.Count)];
            negativeHack2 = sp_NegativeHacks[Random.Range(0,sp_NegativeHacks.Count)];
            if(positiveHack1 != positiveHack2 && negativeHack1 != negativeHack2 && negativeHack1.GetEffectType() != positiveHack1.GetEffectType() && negativeHack2.GetEffectType() != positiveHack2.GetEffectType())
			{
				running = false;
			}
        }
        return new List<CombinedHack>(){new CombinedHack(positiveHack1, negativeHack1), new CombinedHack(positiveHack2, negativeHack2)};
    }
    public void ApplyCombinedHack(GameObject wordGameObject){
        foreach(Hack hack in applyHacks){
            hack.Apply(wordGameObject);
        }
    }

    public Hack DequeueHack(){
        if(queueHacks.Count != 0)
            return queueHacks.Dequeue();
        else
            return null;
    }
    public void QueueHack(Hack hack){
        queueHacks.Enqueue(hack);
    }

    public void AddApplyHack(Hack hack){
        applyHacks.Add(hack);
    }
    public void SetUpdateHack(Hack hack){
        updateHacks = hack;
    }
    public void SetRightClick(Hack hack){
        rightClickHack = hack;
    }
}
