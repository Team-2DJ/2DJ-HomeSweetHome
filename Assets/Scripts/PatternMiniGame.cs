using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternMiniGame : MonoBehaviour
{
    public static PatternMiniGame Instance;

    [Header("References")]
    [SerializeField] private Transform patternHolder;
    [SerializeField] private Image timerBar;

    private GameObject[] currentPatternData;
    private PatternTrigger patternTrigger;
    private int currentPatternIndex;

    private List<GameObject> patterns = new();

    public bool CanDraw { get; private set; }

    private float timer = 10f;

    #region Singleton
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public void Initialize(GameObject[] data, PatternTrigger reference)
    {
        CanDraw = true;
        currentPatternData = data;
        patternTrigger = reference;
        currentPatternIndex = 0;
        StartCoroutine(StartTimer());

        for (int i = 0; i < currentPatternData.Length; i++)
        {
           RectTransform go = Instantiate(data[i], Vector3.zero, Quaternion.identity, patternHolder.transform).GetComponent<RectTransform>();
           go.anchoredPosition3D = Vector3.zero;

           patterns.Add(go.gameObject);
        }

        ActivatePattern();
    }

    public void NextPattern()
    {
        if (!CanDraw)
            return;
        
        currentPatternIndex++;

        if (currentPatternIndex >= currentPatternData.Length)
        {
            Debug.Log("FINISHED");
            CanDraw = false;
            StopAllCoroutines();
            patternTrigger.Completed();
            PanelManager.Instance.ActivatePanel("Game UI");
            PlayerEvents.Instance.SetPlayerMovement(true);
            return;
        }

        ActivatePattern();
    }

    void ClearData()
    {

    }

    void ActivatePattern()
    {
        for (int i = 0; i < patterns.Count; i++)
            patterns[i].SetActive(i == currentPatternIndex);
    }

    IEnumerator StartTimer()
    {
        float currentTime = timer;
        
        while (currentTime > 0f)
        {
            Debug.Log(currentTime);
            yield return new WaitForSeconds(1f);
            currentTime--;
            timerBar.fillAmount = currentTime / timer;
        }

        Debug.Log("Time's Up!");
        CanDraw = false;
    }
}