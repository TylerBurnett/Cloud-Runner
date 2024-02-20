using Game.Global;
using UnityEngine;

namespace Game.InteractiveObjects
{
    /// <summary>
    /// Attachable behaviour that kills the player upon contact
    /// </summary>
    public class DeathObject : MonoBehaviour
    {
        protected void Awake()
        {            _ = gameObject.AddComponent<Rigidbody>();

            Material deathObjectMat = Resources.Load("DeathObject", typeof(Material)) as Material;
            gameObject.GetComponent<Renderer>().material = deathObjectMat;
        }

        protected void Update()
        {
            if (transform.position.y < -20)
            {
                Destroy(gameObject);
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                EventService<DeathObjectCollisionEvent>.Trigger();
            }
        }
    }
}




