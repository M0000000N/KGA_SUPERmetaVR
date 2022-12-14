using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class LoginManager : OnlyOneSceneSingleton<LoginManager>
{
    private string userID;
    public string UserID { get { return userID; } set { userID = value; } }

    [SerializeField] private Material newSkybox;
    public Material NewSkybox { get { return newSkybox; } set { newSkybox = value; } }

    [SerializeField] private NPC_CommunicationManager npc_CommunicationManager;
    public NPC_CommunicationManager NPC_CommunicationManager { get { return npc_CommunicationManager; } set { npc_CommunicationManager = value; } }

    [Header("?α???")]
    [SerializeField] private GameObject joinCanvas;
    public GameObject JoinCanvas { get { return joinCanvas; } set { joinCanvas = value; } }
    [SerializeField] private GameObject createCanvas;
    public GameObject CreateCanvas { get { return createCanvas; } set { createCanvas = value; } }
    [SerializeField] private GameObject findCanvas;
    public GameObject FindCanvas { get { return findCanvas; } set { findCanvas = value; } }
    [SerializeField] private GameObject changePWCanvas;
    public GameObject ChangePWCanvas { get { return changePWCanvas; } set { changePWCanvas = value; } }

    [SerializeField] private GameObject incorrectIDPopupCanvas;
    public GameObject IncorrectIDPopupCanvas { get { return incorrectIDPopupCanvas; } set { incorrectIDPopupCanvas = value; } }
    [SerializeField] private GameObject incorrectPWPopupCanvas;
    public GameObject IncorrectPWPopupCanvas { get { return incorrectPWPopupCanvas; } set { incorrectPWPopupCanvas  = value; } }
    [SerializeField] private GameObject duplicateIDPopupCanvas;
    public GameObject DuplicateIDPopupCanvas { get { return duplicateIDPopupCanvas; } set { duplicateIDPopupCanvas = value; } }
    [SerializeField] private GameObject checkInfomationPopupCanvas;
    public GameObject CheckInfomationPopupCanvas { get { return checkInfomationPopupCanvas; } set { checkInfomationPopupCanvas = value; } }
    [SerializeField] private GameObject changePWPopupCanvas;
    public GameObject ChangePWPopupCanvas { get { return changePWPopupCanvas; } set { changePWPopupCanvas = value; } }
    [SerializeField] private GameObject findIDPopupCanvas;
    public GameObject FindIDPopupCanvas { get { return findIDPopupCanvas; } set { findIDPopupCanvas = value; } }
    [SerializeField] private GameObject signUpCompletePopupCanvas;
    public GameObject SignUpCompletePopupCanvas { get { return signUpCompletePopupCanvas; } set { signUpCompletePopupCanvas = value; } }
    [SerializeField] private GameObject theIDCanBeUsedPopupCanvas;
    public GameObject TheIDCanBeUsedPopupCanvas { get { return theIDCanBeUsedPopupCanvas; } set { theIDCanBeUsedPopupCanvas = value; } }

    [Header("Ʃ?丮??")]
    [SerializeField] private GameObject createNickName;
    public GameObject CreateNickName { get { return createNickName; } set { createNickName = value; } }
    [SerializeField] private GameObject nopCreateNickNamePopup;
    public GameObject NopCreateNickNamePopup { get { return nopCreateNickNamePopup; } set { nopCreateNickNamePopup = value; } }
    [SerializeField] private GameObject selectCharacter;
    public GameObject SelectCharacter { get { return selectCharacter; } set { selectCharacter = value; } }
    [SerializeField] private GameObject remindSelectPopup;
    public GameObject RemindSelectPopup { get { return remindSelectPopup; } set { remindSelectPopup = value; } }

    [Header("?˾? ????Ʈ")]

    [SerializeField] private GameObject[] canvasList;
    [SerializeField] private GameObject[] popupCanvasList;



    private bool isStartCoroutine;
    private bool isStartButtonUICoroutine;

    public void InitializeUI()
    {
        for (int i = 0; i < canvasList.Length; i++)
        {
            canvasList[i].SetActive(false);
        }
    }

    public void InitializePopupUI()
    {
        for (int i = 0; i < popupCanvasList.Length; i++)
        {
            popupCanvasList[i].GetComponent<CanvasGroup>().alpha = 0;
            popupCanvasList[i].SetActive(false);
        }
    }

    private void Start()
    {
        isStartCoroutine = false;
        isStartButtonUICoroutine = false;
        InitializePopupUI();
        SetUICanvas(JoinCanvas);
        SoundManager.Instance.PlayBGM("Login_bgm.mp3");
    }

    public void SetUICanvas(GameObject _canvas)
    {
        InitializeUI();
        _canvas.SetActive(true);
    }

    public void SetPopupUICanvas(GameObject _canvas)
    {
        if(isStartCoroutine == false)
        {
            SoundManager.Instance.PlaySE("popup_error.ogg");
            isStartCoroutine = true;
            StopCoroutine(SetPopupUICanvasCoroutine(_canvas));
            StartCoroutine(SetPopupUICanvasCoroutine(_canvas));
        }
    }

    IEnumerator SetPopupUICanvasCoroutine(GameObject _canvas)
    {
        _canvas.SetActive(true);

        CanvasGroup canvasGroup = _canvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSecondsRealtime(0.02f); // (1 : 0.2 = 0.1 : x) => (0.02 = x)
            // 0.2?? ???̵???
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSecondsRealtime(0.5f); // 0.5?? ????

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSecondsRealtime(0.03f); // (1 : 0.3 = 0.1 : x) => (0.03 = x)
            // 0.3?? ???̵??ƿ?
        }
        canvasGroup.alpha = 0;
        _canvas.SetActive(false);
        isStartCoroutine = false;
    }

    public void SetPopupButtonUICanvas(GameObject _canvas)
    {
        if (isStartButtonUICoroutine == false)
        {
            isStartButtonUICoroutine = true;
            StopCoroutine(SetPopupButtonUICanvasCoroutine(_canvas));
            StartCoroutine(SetPopupButtonUICanvasCoroutine(_canvas));
        }
    }

    IEnumerator SetPopupButtonUICanvasCoroutine(GameObject _canvas)
    {
        _canvas.SetActive(true);
        CanvasGroup canvasGroup = _canvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSecondsRealtime(0.02f); // (1 : 0.2 = 0.1 : x) => (0.02 = x)
            // 0.2?? ???̵???
        }
        canvasGroup.alpha = 1;
        isStartButtonUICoroutine = false;
    }

}
