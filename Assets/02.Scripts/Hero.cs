using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField]
    float speed;

    Rigidbody2D rBody;
    Animator animator;

    bool canTalk;
    float horizontal;

    bool isInteracting;
    Npc lastNpc;

    public static Hero Instance;

    void Awake()
    {
        Instance = this;

        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // block movement if player is interacting
        if (isInteracting) return;

        // move
        horizontal = Input.GetAxisRaw("Horizontal");

        // face forward
        if (horizontal != 0)
            transform.forward = new Vector3(0, 0, horizontal);

        // talk interaction
        if (canTalk && Input.GetButtonDown("Interact"))
        {
            TalkPanel.Instance.Hide();
            DialoguePanel.Instance.Show(lastNpc.dialogue);
            horizontal = 0;
            isInteracting = true;
        }
    }

    public void InteractFinished()
    {
        isInteracting = false;
        TalkPanel.Instance.Show();
        canTalk = true;
    }

    private void FixedUpdate()
    {
        // move player
        rBody.velocity = new Vector2(horizontal * speed, rBody.velocity.y);

        // animate player
        animator.SetFloat("Horizontal", horizontal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            lastNpc = collision.gameObject.GetComponent<Npc>();
            TalkPanel.Instance.Show();
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            lastNpc = null;
            TalkPanel.Instance.Hide();
            canTalk = false;
        }
    }
}

