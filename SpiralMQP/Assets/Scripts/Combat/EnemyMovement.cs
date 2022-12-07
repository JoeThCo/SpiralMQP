using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject PlayerObject;

    [Space(10)]

    public float MovementSpeed;
    public float BattleMovementSpeed;
    public float RestMovementSpeed;

    [Space(10)]

    public Vector2 MovementDirection;
    
    [Space(10)]

    public float DetectDistance;
    public float OuterDistance;
    public float InnerDistance;

    [Space(10)]

    public LayerMask AvoidLayers;
    public float AvoidDistance;

    public float NoiseValue;
    public float MovementFrequency = 0.5f;

    private Vector3 InitPoint;
    private SpriteRenderer Sprite;
    private Rigidbody2D EnemyRigidBody;
    private Collider2D EnemyCollider;


    private void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        EnemyRigidBody = GetComponent<Rigidbody2D>();
        EnemyCollider = GetComponent<Collider2D>();
        MovementSpeed = RestMovementSpeed;
        InitPoint = transform.position;

        if (!PlayerObject)
        {
            PlayerObject = GameObject.FindWithTag("Player");
        }

        NoiseValue = Random.Range(0.0f, 10.0f);
        UpdateSortingOrder();
    }

    private void Update()
    {
        NoiseValue += Time.deltaTime * MovementFrequency;
        float distance = Vector2.Distance(PlayerObject.transform.position, transform.position);
        // Outside detection range, rest mode
        if (distance > DetectDistance)
        {
            MovementDirection.x = Mathf.PerlinNoise(NoiseValue, 0.5f) - 0.5f;
            MovementDirection.y = Mathf.PerlinNoise(0.5f, NoiseValue) - 0.5f;
            MovementDirection.Normalize();

            // Getting back to initial point
            if (Vector2.Distance(InitPoint, transform.position) > 2)
            {
                Vector2 direction = InitPoint - transform.position;
                MovementDirection += direction / 4;
                MovementDirection.Normalize();
            }
            MovementSpeed = RestMovementSpeed;
        }

        // Within detection range, enter battle mode
        else
        {
            if (distance >= OuterDistance)
            {
                MovementDirection = PlayerObject.transform.position - transform.position;
                MovementDirection.Normalize();
            }
            else if (distance <= InnerDistance)
            {
                MovementDirection = PlayerObject.transform.position - transform.position;
                MovementDirection.Normalize();
                MovementDirection = -MovementDirection;
            }
            else
            {
                MovementDirection.x = Mathf.PerlinNoise(NoiseValue, 0.5f) - 0.5f;
                MovementDirection.y = Mathf.PerlinNoise(0.5f, NoiseValue) - 0.5f;
                Vector2 direction = PlayerObject.transform.position - transform.position;
                direction.Normalize();
                MovementDirection += (direction) / (3 * (OuterDistance - distance));
                MovementDirection += (direction) / (3 * (InnerDistance - distance));
                MovementDirection.Normalize();
            }
            MovementSpeed = BattleMovementSpeed;
        }

        // Avoiding walls, dodging bullets, etc
        Collider2D[] avoidObjects = Physics2D.OverlapCircleAll(transform.position, AvoidDistance, AvoidLayers);
        foreach (Collider2D avoidObject in avoidObjects)
        {
            if (avoidObject != EnemyCollider)
            {
                Vector2 direction = (avoidObject.gameObject.transform.position - gameObject.transform.position);
                direction.Normalize();
                MovementDirection -= direction * (1.0f - Physics2D.Distance(EnemyCollider, avoidObject).distance / AvoidDistance);
            }
        }

        // Move Enemy
        MovementDirection.Normalize();
        MoveEnemy();
    }

    private void UpdateSortingOrder()
    {
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y * 100.0f);
        }
    }

    void MoveEnemy()
    {
        transform.position += (Vector3)MovementDirection * MovementSpeed * Time.deltaTime;
        UpdateSortingOrder();
    }
}