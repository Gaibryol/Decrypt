using UnityEngine;
using System.Collections;

public class GameControllerFactory : MonoBehaviour
{
    [SerializeField] private SinglePlayerGameController singleController;
    [SerializeField] private MultiPlayerGameController multiController;

    [SerializeField] private GameObject HacksParent;
    [SerializeField] private HacksManager HacksManager;

    private void Awake()
    {
        StartCoroutine(GetGameController());    
    }

    private IEnumerator GetGameController()
    {
        if (GameManager.Instance.PlayMode == Constants.PlayMode.Single)
        {
            // Destory happens in next game loop. Wait for destory first
            Destroy(multiController);
            //Destroy(multiHacksManager);
            yield return 0;
            GenerateSingle();
        } else if (GameManager.Instance.PlayMode == Constants.PlayMode.Multi)
        {
            Destroy(singleController);
            if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.BR)
            {
                Destroy(HacksManager);
            }
            yield return 0;
            GenerateMulti();

        }
    }

    private void GenerateSingle()
    {
        singleController.enabled = true;
        //singleHacksManager.enabled = true;
    }

    private void GenerateMulti()
    {
        multiController.enabled = true;
        if (GameManager.Instance.GamePrefs.GameType == Constants.GameType.BR)
        {
            HacksParent.AddComponent<BattleRoyalHacksManager>();
        }
        //multiHacksManager.enabled = true;
    }

 }
