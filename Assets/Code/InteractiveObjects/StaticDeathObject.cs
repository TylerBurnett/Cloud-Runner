using Game.Global;
using UnityEngine;

namespace Game.InteractiveObjects
{
    /// <summary>
    /// Attachable behaviour that kills the player upon contact
    /// </summary>
    public class StaticDeathObject : MonoBehaviour
    {
        // Start is called before the first frame update
        protected void Awake()
        {
            Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.freezeRotation = false;

            Material deathObjectMat = Resources.Load("Materials/DeathObject", typeof(Material)) as Material;
            gameObject.GetComponent<Renderer>().material = deathObjectMat;
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




