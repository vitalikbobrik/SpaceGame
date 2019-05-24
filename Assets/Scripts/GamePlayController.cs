using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private GameObject m_levelArt;
    [SerializeField] private GameObject m_deathPanel;
    [SerializeField] private float m_speed = 5;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_highScoreText;
    [SerializeField] private TextMeshProUGUI m_endLevelScoreText;
    private float m_score = 0;
    private float m_deathTimer = 0;
    private bool isRotating = false;
    private bool isGameOver = false;
    private Animator anim;
    

    private void Start()
    {
        m_deathPanel.SetActive(false);
        isGameOver = false;
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (!isGameOver)
        {
            CheckInput();
        }
        CheckObstacles();
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (!isGameOver)
        {
            m_score += Time.deltaTime * 10;
            m_scoreText.text = "Score: " + (int)m_score;
        }
    }

    private void CheckObstacles()
    {
        RaycastHit hit;
        //check walls
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f))
        {
            //move player forward(speed increases at every 100 scorepoints)
            transform.Translate(Vector3.forward * (m_speed+m_score/100) * Time.deltaTime);
            anim.SetInteger("AnimationPar", 1);
            m_deathTimer = 0; 
        }
        else
        {
            //if player is behind the wall more than 0.2sec , lose screeen will appear
            m_deathTimer += Time.deltaTime;
            anim.SetInteger("AnimationPar", 0); 
            if (m_deathTimer > 0.2f)
            {
                isGameOver = true;
                m_deathPanel.SetActive(true);
                m_endLevelScoreText.text = "Your score: " + (int)m_score;
                if (PlayerPrefs.GetInt("Score") < (int)m_score)
                {
                    PlayerPrefs.SetInt("Score", (int)m_score);
                }
                m_highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("Score");
            }
        }
    }

    private void CheckInput()
    {
        if (Input.GetMouseButton(0) && !isRotating)
        {
            StartCoroutine(Rotate(Vector3.right, -90, 0.3f));
        }
        if (Input.GetMouseButton(1) && !isRotating)
        {
            StartCoroutine(Rotate(Vector3.right, 90, 0.3f));
        }
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        //rotating walls
        isRotating = true;
        Quaternion from = m_levelArt.transform.rotation;
        Quaternion to = m_levelArt.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            m_levelArt.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        m_levelArt.transform.rotation = to;
        isRotating = false;
    }
}
