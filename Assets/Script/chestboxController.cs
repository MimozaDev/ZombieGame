using UnityEngine;
using DG.Tweening;

public class chestboxController : MonoBehaviour
{
    [SerializeField] float duration = 0.4f;
    [SerializeField] CanvasGroup excute;
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] GameObject bandagePrefab;
    [SerializeField] GameObject enegyCorePrefab;
    [SerializeField] GameObject chestboxTop;
    Animator animator;
    float openDuration = 2;
    AnimatorStateInfo animationInfo;
    
    void Start()
    {
        excute.alpha = 0;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            excute.DOFade(1,duration);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            excute.DOFade(0,duration);
            chestboxTop.transform.DORotate(new Vector3(0,180,0),openDuration);
        }
    }

    public void Open()
    {  
        chestboxTop.transform.DORotate(new Vector3(0,180,-80),openDuration)
        .OnComplete(() => 
        {
            Instantiate(RandomPicker(),Vector3.up + gameObject.transform.position,Quaternion.identity);
        });
    }

    GameObject RandomPicker()
    {
        var randomValue = (int)Random.Range(0,3);
        switch (randomValue)
        {
            case 0:
                return ammoPrefab;
            case 1:
                return bandagePrefab;
            case 2:
                return enegyCorePrefab;
            default:
                return ammoPrefab;
        }
    }
}