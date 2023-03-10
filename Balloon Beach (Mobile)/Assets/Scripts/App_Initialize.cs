using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class App_Initialize : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public GameObject inMenuUI;
    public GameObject inGameUI;
    public GameObject gameOverUI;
    public GameObject adButton;
    public GameObject heartUpButton;
    public GameObject player;
    public TextMeshProUGUI HeartUpText;
    [SerializeField] string _androidAdUnitId = "5115106";
    [SerializeField] string _iOSAdUnitId = "5115107";
    [SerializeField] string _adUnitId;

    bool hasGameStarted = false;
    bool hasSeenRewardedAd = false;
    bool scoreBoostedCoinValueFlag = false;
    int deathCounter = 0;
    int scoreBoostedCoinValueScore = 0;

    void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNIT_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        Shader.SetGlobalFloat("_Curvature", 2.0f);
        Shader.SetGlobalFloat("_Trimming", 0.1f);
        Application.targetFrameRate = 60;
        ColorSwitch(0, heartUpButton);

    }
    
    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        inMenuUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player.GetComponent<Score>().score != 0 && (player.GetComponent<Score>().score % 35) == 0 && !scoreBoostedCoinValueFlag)
        {
            deathCounter += 1;
            scoreBoostedCoinValueFlag = true;
            scoreBoostedCoinValueScore = player.GetComponent<Score>().score;
        }

        if (scoreBoostedCoinValueFlag && player.GetComponent<Score>().score > scoreBoostedCoinValueScore)
        {
            scoreBoostedCoinValueFlag = false;
        }
    }


    public void PlayButton()
    {
        if (hasGameStarted == false)
        {
            StartCoroutine(StartGame(0.0f));
        }
        else
        {
            StartCoroutine(StartGame(1.0f));
        }
        Advertisement.Load(_adUnitId, this);

    }

    public void Pausegame()
    {
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        hasGameStarted = true;
        inMenuUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    public void ShowAd()
    {

        //ColorSwitch(0);
        Advertisement.Show(_adUnitId, this);
    }

    private void ColorSwitch(int flip, GameObject heart)
    {
        if (flip == 0)
        {
            heart.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            heart.GetComponent<Button>().enabled = false;
        }
        else
        {
            heart.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            heart.GetComponent<Button>().enabled = true;
        }
    }

    public void RestartGame()
    {
        deathCounter = 0;
        SceneManager.LoadScene(0);//Loads scene 0
    }

    public void HeartUp()
    {
        player.GetComponent<Score>().coinCount -= (deathCounter * 10);
        StartCoroutine(StartGame(1.0f));
    }

    public void GameOver()
    {
        deathCounter += 1;
        if (player.GetComponent<Score>().coinCount >= 10) ColorSwitch(1, heartUpButton);
        else ColorSwitch(0, heartUpButton);

        HeartUpText.text = "Extra Life (Spend " + (deathCounter * 10).ToString() + " Coins)";
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        hasGameStarted = false;
        inMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
    }

    IEnumerator StartGame(float waitTime)
    {
        inMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }



    //
    //
    //
    //
    //FIX AD SYSTEM
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId))
        {
            //ColorSwitch(1);
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log("ads failed to load: " + error.ToString() + message);
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log("ads failed to show: " + adUnitId + " " + error.ToString() + message);
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if(adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            StartCoroutine(StartGame(1.0f));
            Advertisement.Load(_adUnitId, this);
        }
    }
}
