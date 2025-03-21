using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Scoring Settings")]
    public float pointsPerSecond = 10f;  // Points accumulated per second
    private float score = 0f;


    [SerializeField]
    float moveSpeed = 5f;

    Rigidbody2D rb;
    float horizontalInput;

    [SerializeField]
    float jumpDuration = 0.5f;

    [Header("UI Reference")]
    public TMP_Text scoreText;  // Reference to your TextMeshPro component
    public GameOverScript gameOverUI;

    private Vector3 originalScale;
    private bool isJumping = false;

    // Mouse-drag movement variables.
    private bool isDragging = false;
    private Vector3 dragStartMouseWorldPos;
    private Vector3 dragStartPlayerPos;

    private bool isGameActive = true;

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

        // Mouse drag movement (left/right)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPlayerPos = transform.position;
            // Convert the initial mouse screen position to world space.
            dragStartMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartMouseWorldPos.z = 0; // Ensure we're working in 2D (z = 0)
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            // Convert the current mouse position to world space.
            Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMouseWorldPos.z = 0;
            // Calculate how far the mouse has moved in the x direction.
            Vector3 delta = currentMouseWorldPos - dragStartMouseWorldPos;
            // Update player's position with only the x-axis change.
            Vector3 newPos = dragStartPlayerPos + new Vector3(delta.x, 0, 0);
            transform.position = newPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, 0);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(JumpEffect());
        }

    }

    public void EndGame()
    {
        isGameActive = false;
        Debug.Log("Final Score: " + score);
        Debug.Log("Game Over!");
        gameOverUI.DisplayMenu(Mathf.FloorToInt(score));
    }

    IEnumerator JumpEffect()
    {
        isJumping = true;
        float halfDuration = jumpDuration / 2f;
        float elapsed = 0f;

        // Scale up phase: interpolate from original scale to the jump scale
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.5f, t);
            yield return null;
        }

        // Scale down phase: interpolate back to the original scale
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(originalScale * 1.5f, originalScale, t);
            yield return null;
        }

        // Ensure the scale is reset
        transform.localScale = originalScale;
        isJumping = false;
    }

    public bool IsJumping { get { return isJumping; } }
}
