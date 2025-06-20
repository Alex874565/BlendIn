using System.Collections;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject offLayer;
    [SerializeField] private GameObject onLayer;
    [SerializeField] private GameObject offBackground;
    [SerializeField] private GameObject onBackground;
    [SerializeField] private TMPro.TMP_Text timer;
    [SerializeField] private GameObject particles;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private ColorType color;

    private bool isPressed = false;
    private static float timerValue = 0f;
    private float timerDuration = 5f; // Duration for the timer to count down

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(PressButton());
        }
    }

    public IEnumerator PressButton()
    {
        animator.Play("Button");
        particles.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        isPressed = true;
        offLayer.SetActive(false);
        onLayer.SetActive(true);
        offBackground.SetActive(false);
        onBackground.SetActive(true);
        //timer.gameObject.SetActive(true);
        //timer.text = timerValue.ToString("F1");
        if (timerValue > 0f)
        {
            timerValue = timerDuration;
            while (timerValue > 0f)
            {
                yield return new WaitForSeconds(1f);
                //timer.text = timerValue.ToString("F1");
            }
        }
        else
        {
            timerValue = timerDuration;
            while (timerValue > 0f)
            {
                yield return new WaitForSeconds(1f);
                timerValue -= 1;
                //timer.text = timerValue.ToString("F1");
            }
        }
        //timer.gameObject.SetActive(false);
        isPressed = false;
        offLayer.SetActive(true);
        onLayer.SetActive(false);
        offBackground.SetActive(true);
        onBackground.SetActive(false);
        animator.Play("ButtonRevers");
        yield return new WaitForSeconds(0.5f);
        particles.SetActive(true);
        playerBehaviour.LeaveButtonLayer(color);
    }
}
