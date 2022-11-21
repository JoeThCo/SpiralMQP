using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject PlayerObject;
    public float MovementSpeed;
    public Vector2 MovementDirection;
    public float DetectDistance;
    public float OuterDistance;
    public float InnerDistance;

    private SpriteRenderer Sprite;
    private Rigidbody2D EnemyRigidBody;
 

    private void Start(){
        Sprite = GetComponent<SpriteRenderer>();
        EnemyRigidBody = GetComponent<Rigidbody2D>();
        if(!PlayerObject){
            PlayerObject = GameObject.FindWithTag("Player");
        }
        UpdateSortingOrder();
    }

    private void Update(){
        float distance = Vector2.Distance(PlayerObject.transform.position, transform.position);
        if(distance > DetectDistance){
            MovementDirection = new Vector2();
        } else if(distance >= OuterDistance){
            MovementDirection = PlayerObject.transform.position - transform.position;
            MovementDirection.Normalize();
        } else if(distance <= InnerDistance){
            MovementDirection = PlayerObject.transform.position - transform.position;
            MovementDirection.Normalize();
            MovementDirection = -MovementDirection;
        } else {
            MovementDirection = new Vector2();
        }
        
        
        MoveEnemy();
    }

    private void UpdateSortingOrder(){
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y*100.0f);
        }
    }

    void MoveEnemy()
        {
            transform.position += (Vector3) MovementDirection * MovementSpeed * Time.deltaTime;
            UpdateSortingOrder();
        }
}