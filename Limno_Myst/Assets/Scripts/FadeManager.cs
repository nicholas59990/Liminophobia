using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }
    [SerializeField] private Image fadeImage;
    private Animator animator;
    private bool isTransitioning;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;
        }
    }

    public void Transition(Action onComplete = null)
    {
        if (isTransitioning)
        {
            return;
        }
        StartCoroutine(TransitionRoutine(onComplete));
    }

    private IEnumerator TransitionRoutine(Action onComplete)
    {
        isTransitioning = true;
        fadeImage.raycastTarget = true;
        animator.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1f);
        //onBlack?.Invoke();
        Debug.Log("FadeOut");
        animator.SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(1f);
        fadeImage.raycastTarget = false;
        isTransitioning = false;
        onComplete?.Invoke();
    }

    public IEnumerator FadeToBlack()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1.2f);
    }
 
    public IEnumerator FadeFromBlack()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(1.2f);
    }
}
