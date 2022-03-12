using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

public class UIManager_Game : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField] Image crosshair;
    [SerializeField] Image hitCrosshair;
    [SerializeField] Image itemFrame;
    [SerializeField] Slider playerHP_Bar;
    [SerializeField] TextMeshProUGUI playerHP;
    [SerializeField] TextMeshProUGUI Ammo;
    [SerializeField] TextMeshProUGUI Reload;
    [SerializeField] TextMeshProUGUI enegyCoreCount;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float duration = 1;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject option;
    [SerializeField] GameObject gameStart;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject result;
    [SerializeField] GameObject itemPopUp;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] TextMeshProUGUI resultValue;
    [SerializeField] weaponFire fire;
    
    CanvasGroup gameStartPanel;
    CanvasGroup pausePanel;
    CanvasGroup optionPanel;
    CanvasGroup resultPanel;
    CanvasGroup gameOverPanel;
    CanvasGroup itemPopUpPanel;
    float time = 0;
    float sec = 0;
    int min = 0;
    int hour = 0;
        // pause.SetActive(false);
        // option.SetActive(false);
        // result.SetActive(false);
        // gameStart.SetActvive(false);
        // gameOver.SetActive(false);
        // itemPopUp.SetActive(false);
    void Awake()
    {
        // Initalize

        gameStartPanel = gameStart.GetComponent<CanvasGroup>();
        gameOverPanel = gameOver.GetComponent<CanvasGroup>();
        pausePanel = pause.GetComponent<CanvasGroup>();
        resultPanel = result.GetComponent<CanvasGroup>();
        optionPanel = option.GetComponent<CanvasGroup>();
        itemPopUpPanel = itemPopUp.GetComponent<CanvasGroup>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pausePanel.alpha = 0;
        optionPanel.alpha = 0;
        resultPanel.alpha = 0;
        gameOverPanel.alpha = 0;
        gameStartPanel.alpha = 0;
        itemPopUpPanel.alpha = 0;
        crosshair.color = new Color(1,1,1,1);
        hitCrosshair.color = new Color32(255,0,0,0);

        pause.SetActive(false);
        option.SetActive(false);
        result.SetActive(false);
        gameOver.SetActive(false);
        itemPopUp.SetActive(false);

        gameStartPanel.alpha = 1;
        gameStart.SetActive(true);

        InputController.Instance.input.SwitchCurrentActionMap("UI");
        playerHP_Bar.maxValue = GameManager.Instance.maxHP;
    }

    async void Start()
    {
        await UniTask.Delay(1000);
        countDown.text = "3";

        await UniTask.Delay(1000);
        countDown.text = "2";

        await UniTask.Delay(1000);
        countDown.text = "1";

        await UniTask.Delay(1000);
        countDown.text = "Game Start!";

        await UniTask.Delay(1000);
        gameStartPanel.DOFade(0,duration)
        .OnComplete(() => gameStart.SetActive(false));

        InputController.Instance.input.SwitchCurrentActionMap("Game");
        GameManager.Instance.startTimer = true;
    }

    private void Update()
    {   
        Debug.Log((int)itemPopUpPanel.alpha);


        playerHP_Bar.value = GameManager.Instance.playerHP;
        playerHP.text = $"{GameManager.Instance.playerHP.ToString()} / {GameManager.Instance.maxHP.ToString()}";
        Ammo.text = $"Ammo: {fire.ammo.ToString()}/{fire.maxAmmo.ToString()}";
        Reload.text = $"Reload: {fire.nowReloadTime}";
        // enegyCoreCount.text = $"EnegyCore: {GameManager.Instance.dropItemSize.ToString()}";

        // Timer
        if (GameManager.Instance.startTimer) 
        {
            time += Time.deltaTime;
            sec = time % 60;
            min = (int)time / 60 % 60;
            hour = (int)time / 60 / 60;
        }
        timer.text = $"{hour.ToString("D2")} : {min.ToString("D2")} : {sec.ToString("F2")}";

        if (GameManager.Instance.playerHP <= 0) OnGameOver();
    }

    public void CrossHairFade(bool IN,float duration)
    {
        if (IN) crosshair.DOFade(1,duration);
        else crosshair.DOFade(0,duration);
    }

    public void HitCrossHairFade(bool IN,bool enableSemiAuto,float duration)
    {
        if (IN) hitCrosshair.DOFade(1,duration)
        .OnComplete(() => 
        {
            if (enableSemiAuto) hitCrosshair.DOFade(0,duration);
        });
        else hitCrosshair.DOFade(0,duration);
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene(0);
    }

    public void ToPause()
    {
        gameOver.SetActive(false);
        gameStart.SetActive(false);
        result.SetActive(false);
        option.SetActive(false);
        itemPopUp.SetActive(false);

        if (option.activeInHierarchy) 
        {
            optionPanel.DOFade(0,duration)
            .OnComplete(() => option.SetActive(false));
        }
        pause.SetActive(true);
        pausePanel.DOFade(1,duration);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToGame()
    {
        gameOver.SetActive(false);
        gameStart.SetActive(false);
        result.SetActive(false);
        option.SetActive(false);
        
        if (pausePanel.alpha > 0)
        {
            Debug.Log("ToGame(): pausePanel.alpha > 0");
            pausePanel.DOFade(0,duration)
            .OnComplete(() => pause.SetActive(false));
        }
        else if (itemPopUpPanel.alpha > 0)
        {
            Debug.Log("ToGame(): itenPanel.alpha > 0");
            itemPopUpPanel.DOFade(0,duration)
            .OnComplete(() => itemPopUp.SetActive(false));
        }
        
        InputController.Instance.input.SwitchCurrentActionMap("Game");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToOptions()
    {
        pausePanel.DOFade(0,duration)
        .OnComplete(() => 
        {
            pause.SetActive(false);
            option.SetActive(true);
            optionPanel.DOFade(1,duration);
        });

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnResult()
    {
        gameOver.SetActive(false);
        gameStart.SetActive(false);
        pause.SetActive(false);
        option.SetActive(false);
        InputController.Instance.input.SwitchCurrentActionMap("UI");

        resultValue.text = $"Time: {hour.ToString("D2")} : {min.ToString("D2")} : {sec.ToString("F2")}\nEnegyCoreDUMMY: {/*GameManager.Instance.dropItemSize*/1+1}\nKill: Not supported.";
        result.SetActive(true);
        resultPanel.DOFade(1,duration);
    }

    void OnGameOver()
    {
        gameStart.SetActive(false);
        pause.SetActive(false);
        option.SetActive(false);
        result.SetActive(false);
        InputController.Instance.input.SwitchCurrentActionMap("UI");
        GameManager.Instance.startTimer = false;

        gameOver.SetActive(true);
        gameOverPanel.DOFade(1,duration);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.startTimer = false;
    }

    public void OnItemPop(bool Up, Sprite item)
    {
        pause.SetActive(false);
        option.SetActive(false);
        result.SetActive(false);
        gameOver.SetActive(false);
        gameStart.SetActive(false);
        itemPopUp.SetActive(true);
        
        Cursor.visible = Up ? true : false;
        Cursor.lockState = Up ? CursorLockMode.None : CursorLockMode.Locked;

        if (Up) 
        {
            itemFrame.sprite = item;
            itemPopUpPanel.DOFade(1,duration)
            .OnComplete(() => InputController.Instance.input.SwitchCurrentActionMap("UI"));
        }
        else 
        {
            itemPopUpPanel.DOFade(0,duration)
            .OnComplete(() => InputController.Instance.input.SwitchCurrentActionMap("Game"));
        }
    }
}
