using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawnController : MonoBehaviour
{
    public float Speed;
    private float ActualSpeed;
    public Animator anim;
    public Transform camera;
    public GameController game;
    public bool aiming;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        anim.SetFloat("Move", Mathf.Abs(movement.magnitude));
        if (movement != Vector3.zero)
        {
            if (!aiming)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized) * camera.rotation, 0.7F);
            }
        }
        if (aiming)
        {
            ActualSpeed = Speed / 2;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //Debug.DrawRay(transform.position, new Vector3(ray.direction.x * 100f,1f, ray.direction.z * 100f) , Color.white);
                if (Vector3.Distance(transform.position, hit.point) > 1)
                {
                    Vector3 aimDirection = new Vector3(hit.point.x, 1f, hit.point.z);

                    transform.LookAt(aimDirection);

                    GetComponent<LineRenderer>().enabled = true;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit shotHit2;

                    //Debug.DrawRay(transform.position, -(transform.position - (new Vector3(hit.point.x, 1f, hit.point.z))), Color.yellow);
                    Debug.Log("Atirei... " + new Vector3(hit.point.x, 1f, hit.point.z));
                    if (Physics.Raycast(transform.position, -(transform.position - (new Vector3(hit.point.x, 1f, hit.point.z))), out shotHit2,100f))
                    {
                        Debug.Log("Verificando... ");
                        if (shotHit2.transform.GetComponent<EnemyController>())
                        {
                            Debug.Log("Acertou " + shotHit2.transform.name);
                            GameController.UnitTakeDamage(GetComponent<UnitCombatController>(), shotHit2.transform.GetComponent<UnitCombatController>());
                        }
                    }
                }
            }
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }

        ActualSpeed = Speed;
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (anim.GetBool("Crouch"))
            {
                
                anim.SetBool("Crouch", false);
            }
            else
            {
                
                anim.SetBool("Crouch", true);
            }
        }

        if (anim.GetBool("Crouch"))
        {
            ActualSpeed = Speed / 2;
        }
        
        transform.Translate(movement * ActualSpeed * Time.deltaTime, camera);


        if (Input.GetMouseButtonDown(1)){
            anim.SetBool("Aiming", true);

            aiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            aiming = false;
            anim.SetBool("Aiming", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Loot"))
        {
            if (other.gameObject.GetComponent<LootController>().Lootable)
            {
                other.gameObject.GetComponent<LootController>().ActivateCanvas();
                if (Input.GetKeyDown("e"))
                {
                    game.SetCurrentLoot(other.gameObject.GetComponent<LootController>());
                    other.gameObject.GetComponent<LootController>().GenerateLoot();
                }
            }
        }
        if (other.CompareTag("MainHall"))
        {
            other.gameObject.GetComponent<MainHallController>().ActivateCanvas();
            if (Input.GetKeyDown("e"))
            {
                other.gameObject.GetComponent<MainHallController>().ActivateManaging();
            }
        }
        if (other.CompareTag("Warehouse"))
        {
            other.gameObject.GetComponent<WarehouseController>().ActivateCanvas();
            if (Input.GetKeyDown("e"))
            {
                other.gameObject.GetComponent<WarehouseController>().ActivateManaging();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Loot"))
        {
            other.gameObject.GetComponent<LootController>().DesactivateCanvas();
            
        }
        if (other.CompareTag("MainHall"))
        {
            other.gameObject.GetComponent<MainHallController>().DesactivateCanvas();

        }
        if (other.CompareTag("Warehouse"))
        {
            other.gameObject.GetComponent<WarehouseController>().DesactivateCanvas();

        }
    }
}
