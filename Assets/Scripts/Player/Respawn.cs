//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Respawn : MonoBehaviour
//{
//    public bool activated = false;
//    private Transform fire;
//    private PlayerManager player;
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            player = collision.GetComponent<PlayerManager>();
//        }
//    }
//    private void Start()
//    {
//        fire = transform.GetChild(2);
//    }
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.DownArrow) && player != null)
//        {
//            activated = true;
//        }
//        if (activated)
//        {
//            fire.gameObject.SetActive(true);
//            fire.GetComponent<Animator>().Play("Idle");

//            if (player != null)
//            {
//                player.respawnPointScene = SceneManager.GetActiveScene().buildIndex;
//            }
//        }
//        if (!activated) 
//        {
//            fire.gameObject.SetActive(false);
//        }
//    }
//}
