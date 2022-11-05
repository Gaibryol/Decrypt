using UnityEngine;
using System.Collections;

public class GameControllerFactory : MonoBehaviour
{
    [SerializeField] private SinglePlayerGameController singleController;
    [SerializeField] private MultiPlayerGameController multiController;

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
            yield return 0;
            GenerateSingle();
        } else if (GameManager.Instance.PlayMode == Constants.PlayMode.Multi)
        {
            Destroy(singleController);
            yield return 0;
            GenerateMulti();
        }
    }

    private void GenerateSingle()
    {
        singleController.enabled = true;
    }

    private void GenerateMulti()
    {
        multiController.enabled = true;
    }

 }
