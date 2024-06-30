using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private PlanetGravity currentPlanetGravity;
    public float currentWalkSpeed;
    public float currentJumpForce;
    [SerializeField] private float baseWalkSpeed = 20f;
    [SerializeField] private float baseJumpForce = 20f;
    private float jumpButtonHoldTime = 0f;
    [SerializeField] private float maxJumpButtonHoldTime = 2f;
    [SerializeField] private float jumpForceIncreaseMultiplier = 1.4f;

    private Rigidbody2D rb;
    public bool IsInOrbit { get; private set; }
    private GameObject currentPlanet = null;
    private Vector3 spawnPoint;
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip crystalSound;
    public bool leftButtonPressed = false;
    private bool rightButtonPressed = false;
    private bool jumpButtonPressed = false;
    private bool isJumping = false;
    private bool hasCrystal = false;
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject loseScreen;
    public GameObject winScreen;
    public Slider jumpBar;
    private float lastRestartTime = 0f;
    public float restartCooldown = 2f;
    public GameObject leftButton;
    public GameObject rightButton;
    public float zoomSpeed = 0.5f;
    public float minZoom = 1f;
    public float maxZoom = 5f;
    private Camera mainCamera;
    private bool pinchZoomEnabled = true;

    void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the object.");
        }

        currentHealth = maxHealth;
        UpdateHealthUI();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.8f;
        IsInOrbit = false;
        currentWalkSpeed = baseWalkSpeed;
        currentJumpForce = baseJumpForce;
        spawnPoint = transform.position;

    }

    void Update()
    {
        if (pinchZoomEnabled)
        {
            PinchZoom();
        }
        if (winScreen.activeSelf || loseScreen.activeSelf)
        {
            // Eğer kazanma veya kaybetme ekranı aktifse, düğmeleri ve kenar çubuğunu deaktive et
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            Debug.Log("False");

        }
        else
        {
            Debug.Log("True");
            leftButton.SetActive(true);
            rightButton.SetActive(true);

        }
        if (isJumping)
        {

            jumpButtonHoldTime += Time.deltaTime;
            jumpButtonHoldTime = Mathf.Min(jumpButtonHoldTime, maxJumpButtonHoldTime);
            jumpBar.value = jumpButtonHoldTime / maxJumpButtonHoldTime;
        }

        if (IsInOrbit && currentPlanet != null)
        {
            float gravityMagnitude = Mathf.Abs(currentPlanetGravity.gravity);
            float adjustedWalkSpeed = currentWalkSpeed * (1 - gravityMagnitude / 100);
            adjustedWalkSpeed = Mathf.Max(1, adjustedWalkSpeed);


            float input = Input.GetAxis("Horizontal") + (leftButtonPressed ? -1 : 0) + (rightButtonPressed ? 1 : 0);
            if (Mathf.Abs(input) > 0)
            {
                RotateAroundPlanet(input, adjustedWalkSpeed);
            }

            if (Input.GetKey(KeyCode.Space) || jumpButtonPressed)
            {
                if (!isJumping)
                {
                    isJumping = true;
                    jumpButtonHoldTime = 0;


                }
                else
                {

                    jumpButtonHoldTime += Time.deltaTime;
                    jumpButtonHoldTime = Mathf.Min(jumpButtonHoldTime, maxJumpButtonHoldTime);
                }
            }


            if (Input.GetKeyUp(KeyCode.Space) || (!Input.GetKey(KeyCode.Space) && isJumping && !jumpButtonPressed))
            {
                JumpOutOfOrbit();
                isJumping = false;
                jumpButtonHoldTime = 0f;
                jumpBar.value = 0;

            }
        }
    }
    void PinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Zoom(deltaMagnitudeDiff * zoomSpeed);
        }
    }
    void Zoom(float deltaMagnitudeDiff)
    {
        mainCamera.orthographicSize += deltaMagnitudeDiff;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
    }
    public void DieAndRespawn()
    {
        currentHealth--;
        UpdateHealthUI();
        audioSource.PlayOneShot(deathSound);
        if (currentHealth <= 0)
        {
            LoseGame();
        }
        else
        {
            transform.position = spawnPoint;
            rb.velocity = Vector2.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
    void LoseGame()
    {

        loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdateSpeedAndJumpBasedOnGravity()
    {
        if (currentPlanetGravity != null)
        {
            float gravityMagnitude = Mathf.Abs(currentPlanetGravity.gravity);


            currentWalkSpeed = baseWalkSpeed * (1 - gravityMagnitude / 50);
            currentJumpForce = baseJumpForce * (1 - gravityMagnitude / 50);


            currentWalkSpeed = Mathf.Max(1, currentWalkSpeed);
            currentJumpForce = Mathf.Max(1, currentJumpForce);
        }
    }

    public void PressLeftButton() { leftButtonPressed = true; pinchZoomEnabled = false; }
    public void ReleaseLeftButton() { leftButtonPressed = false; pinchZoomEnabled = true; }
    public void PressRightButton() { rightButtonPressed = true; pinchZoomEnabled = false; }
    public void ReleaseRightButton() { rightButtonPressed = false; pinchZoomEnabled = true; }
    public void PressJumpButton()
    {
        isJumping = true;
        jumpButtonPressed = true;
        pinchZoomEnabled = false;
    }

    public void ReleaseJumpButton()
    {
        if (!leftButtonPressed && !rightButtonPressed)
        {
            pinchZoomEnabled = true;
        }

        if (isJumping)
        {
            jumpBar.value = 0;
            JumpOutOfOrbit();
            isJumping = false;
            jumpButtonHoldTime = 0f;
            jumpButtonPressed = false;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void PressRestartButton()
    {
        if (Time.time - lastRestartTime >= restartCooldown)
        {
            rb.velocity = Vector3.zero;
            transform.position = spawnPoint;

            lastRestartTime = Time.time;
            transform.rotation = Quaternion.identity;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Crystal"))
        {
            audioSource.PlayOneShot(crystalSound);
            Debug.Log("Tebrikler! Kristali topladınız!");
            hasCrystal = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Alien"))
        {

            DieAndRespawn();
        }
        if (other.gameObject.CompareTag("Planet"))
        {
            IsInOrbit = true;
            currentPlanet = other.gameObject;
            rb.gravityScale = 0;
            currentPlanetGravity = currentPlanet.GetComponent<PlanetGravity>();

            UpdateSpeedAndJumpBasedOnGravity();
        }
        else if (other.gameObject.CompareTag("LethalPlanet"))
        {
            DieAndRespawn();


        }
        if (other.gameObject.CompareTag("Earth") && hasCrystal)
        {
            IsInOrbit = true;
            currentPlanet = other.gameObject;
            rb.gravityScale = 0;
            currentPlanetGravity = currentPlanet.GetComponent<PlanetGravity>();
            WinGame();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentPlanet)
        {
            IsInOrbit = false;
            currentPlanet = null;
            rb.gravityScale = 1;
            currentPlanetGravity = null;


            currentWalkSpeed = baseWalkSpeed;
            currentJumpForce = baseJumpForce;
        }
    }
    public void GoToNextLevel()
    {
        LevelManager.UnlockNextLevel();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void JumpOutOfOrbit()
    {
        if (currentPlanetGravity != null)
        {
            audioSource.PlayOneShot(jumpSound);
            float gravityMagnitude = Mathf.Abs(currentPlanetGravity.gravity);

            float adjustedJumpForce = currentJumpForce + jumpButtonHoldTime / maxJumpButtonHoldTime * jumpForceIncreaseMultiplier * currentJumpForce;
            adjustedJumpForce = Mathf.Max(1, adjustedJumpForce);

            IsInOrbit = false;
            rb.AddForce(transform.up * adjustedJumpForce, ForceMode2D.Impulse);
        }
    }
    public void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu");
    }
    void WinGame()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    void RotateAroundPlanet(float input, float adjustedWalkSpeed)
    {
        float direction = input > 0 ? 1f : -1f;
        Vector3 planetCenter = currentPlanet.transform.position;

        transform.RotateAround(planetCenter, Vector3.forward, -adjustedWalkSpeed * direction * Time.deltaTime);

        Vector3 toCenter = (planetCenter - transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, -toCenter);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }
}
