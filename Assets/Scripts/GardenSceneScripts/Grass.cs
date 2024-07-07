using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private BaseFlower flowerParent;
    private Vector3 originalScale;
    private Vector3 originalPos;
    private float scaleFactor = 1.01f;
    private float finalYScale = 3f;

    private float cuttingTime = 0;
    private float cuttingTimeMax = .5f;
    private bool isCutting = false;

    private GameObject scissors;
    private Animator scissorsAnimator;

    private void Awake()
    {
        flowerParent = gameObject.GetComponentInParent<BaseFlower>();
        originalScale = transform.localScale;

        originalPos = transform.position;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (gameObject.activeSelf && flowerParent != null && flowerParent.IsWatering())
        {
            if (transform.localScale.y <= finalYScale)
            {
                Vector3 previousScale = transform.localScale;
                transform.localScale *= scaleFactor;
                float heightDifference = (transform.localScale.y - previousScale.y) / 8;
                transform.Translate(Vector3.up * heightDifference, Space.World);
            }       
        }

        // Get animator of scissors when scissors active
        if (scissorsAnimator == null)
        {
            scissors = GameObject.Find("Scissors");
            if (scissors != null)
            {
                scissorsAnimator = scissors.GetComponent<Animator>();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Scissors"))
        {
            if (Input.GetMouseButton(0))
            {
                isCutting = true;
                HandleCutting();

                PlayCuttingAnimation();
            }
            else
            {
                isCutting = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Scissors"))
        {
            isCutting = false;
            PlayCuttingAnimation();
        }
    }

    private void HandleCutting()
    {
        cuttingTime += Time.deltaTime;

        //Chỉ thực hiện phát một lần khi thực hiện cắt
        if (!AudioManager.Instance.GetSPXSource().isPlaying)
        {
            AudioManager.Instance.PlaySPF(AudioManager.Instance.cutting);
        }

        if (cuttingTime > cuttingTimeMax)
        {
            gameObject.SetActive(false);
            transform.position = originalPos;
            transform.localScale = originalScale;
            flowerParent.DecreaseGrassAount();
            cuttingTime = 0;
        }
    }

    private void PlayCuttingAnimation()
    {
        if (scissorsAnimator != null && scissors.activeSelf)
        {
            scissorsAnimator.SetBool("IsCutting", isCutting);
        }
    }

}
