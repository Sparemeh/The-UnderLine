using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Scoring Settings")]
    public float pointsPerSecond = 10f; 
    float score = 0f;

    // Movement Variables
    [Header("Movement Settings")]
    [SerializeField]
    float moveSpeed = 5f;
    Rigidbody2D rb;
    float horizontalInput;
    Vector3 originalScale;
    bool isJumping = false;

    [SerializeField]
    float jumpDuration = 0.5f;

    [Header("UI Reference")]
    public TMP_Text scoreText;  
    public GameOverScript gameOverUI;
    public TMP_Text effectsText;

    // Mouse-drag movement variables.
    bool isDragging = false;
    Vector3 dragStartMouseWorldPos;
    Vector3 dragStartPlayerPos;

    bool isGameActive = true;
    bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        // Increase points if game is active
        if (isGameActive)
        {
            score += pointsPerSecond * Time.deltaTime;

            // Display score on UI
            scoreText.text = "Score: " + Mathf.FloorToInt(score);
        }

        // MOUSE DRAG INPUT

        // Store mouse position on mouse down
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPlayerPos = transform.position;
            dragStartMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartMouseWorldPos.z = 0; 
        }

        // While mouse down, update position
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;
            Vector3 delta = currentMouseWorldPos - dragStartMouseWorldPos;
            Vector3 newPos = dragStartPlayerPos + new Vector3(delta.x, 0, 0);
            transform.position = newPos;
        }

        // stop update position when mouse up
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }


        // LEFT RIGHT KEYBOARD INPUT
        horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, 0);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(JumpEffect());
        }
    }

    // handles ending the game
    public void EndGame()
    {
        isGameActive = false;
        Debug.Log("Final Score: " + score);
        Debug.Log("Game Over!");
        gameOverUI.DisplayMenu(Mathf.FloorToInt(score));
    }

    #region Player Effects

    // handles applying invincibility effects, called from powerup scripts
    public void ApplyInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        float timeRemaining = duration;

        // Update text to show remaining duration.
        while (timeRemaining > 0)
        {
            effectsText.text = string.Format("Invincible: {0:0.0}s", timeRemaining);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        effectsText.text = "";
        isInvincible = false;
    }

    // handles applying shrink effects, called from powerup scripts
    public void ApplyShrink(float duration, float shrinkMultiplier)
    {
        StartCoroutine(ShrinkCoroutine(duration, shrinkMultiplier));
    }

    private IEnumerator ShrinkCoroutine(float duration, float shrinkMultiplier)
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * shrinkMultiplier;
        float timeRemaining = duration;

        // Update text to show remaining duration.
        while (timeRemaining > 0)
        {
            effectsText.text = string.Format("Shrunk: {0:0.0}s", timeRemaining);
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        effectsText.text = "";
        transform.localScale = originalScale;
    }

    // Handles jump effect, scales up and down the player sprite when jumping
    IEnumerator JumpEffect()
    {
        isJumping = true;
        float halfDuration = jumpDuration / 2f;
        float elapsed = 0f;

        // Scale up 
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.5f, t);
            yield return null;
        }

        // Scale down 
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale * 1.5f, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
        isJumping = false;
    }
    #endregion

    #region Getter and Setters
    public bool IsJumping { get { return isJumping; } }

    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; }}
    #endregion
}
