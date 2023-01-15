using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class EggMobile : MonoBehaviour, IHittable {

    public enum WaypointAlogithm {
        Forward = 0,
        Backward = 1,
        Random = 2
    }

    public GameObject[] waypoints;
    public GameObject eggmanModel;
    public LineRenderer lineRenderer;
    public WaypointAlogithm waypointAlogithm;
    public AudioSource source;
    public AudioClip deadSound;
    public AudioClip hitSound;
    public CinemachineVirtualCamera bossBattleCamera;
    public AudioClip awakeSound;
    public AudioClip[] swearSound;
    public GameObject explosionPrefab;
    public AudioClip endMusic;
    public PlayableDirector endCutsceneDirector;
    public bool activated;
    public int health = 8;
    public float speed;
    public GameObject ballObject;
    private float lastHitTime;
    private bool dead;

    private int currentWaypointIndex;

    private void Update() {
        if(activated) {
            eggmanModel.SetActive(true);
            var currentWaypoint = waypoints[currentWaypointIndex];

            if(Vector3.Distance(currentWaypoint.transform.position, transform.position) < 2) {
                switch (waypointAlogithm) {
                    case WaypointAlogithm.Forward:
                    {
                        currentWaypointIndex++;

                        if(currentWaypointIndex >= waypoints.Length) {
                            currentWaypointIndex = 0;
                        }

                        break;
                    }
                    case WaypointAlogithm.Backward:
                    {
                        currentWaypointIndex--;

                        if(currentWaypointIndex < 0) {
                            currentWaypointIndex = waypoints.Length - 1;
                        }

                        break;
                    }
                    case WaypointAlogithm.Random:
                    {
                        currentWaypointIndex = Random.Range(0, waypoints.Length);
                        break;
                    }
                }
            }

            if(!dead)
            {
                transform.position += Vector3.ClampMagnitude((currentWaypoint.transform.position
                                                              - transform.position).normalized, Time.deltaTime * 5f * speed);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(
                    currentWaypoint.transform.position
                    - transform.position), Time.deltaTime * 10);
            }
        } else {
            eggmanModel.SetActive(false);
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ballObject.transform.position);
    }

    public void OnHit(PlayerCharacter player) {

    }

    private void OnDead()
    {
        dead = true;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        endCutsceneDirector.Play();
        
        MusicManager.Instance.PlayMusic(endMusic)
            .SetLooping(true)
            .TransitionCrossFade(3f)
            .ReplaceEverything(true)
            .Start();
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Eggman")) {
            activated = true;
            Destroy(collider.gameObject);
            source.PlayOneShot(awakeSound);
            bossBattleCamera.Priority = 100;
        } 
        else if (collider.GetComponent<PlayerCharacter>())
        {
            var player = collider.GetComponent<PlayerCharacter>();
            
            if(Time.time > lastHitTime + 0.5) {
                health--;
                speed *= 1.1f;
                waypointAlogithm = (WaypointAlogithm) Random.Range(0, 3);
                lastHitTime = Time.time;
            
                source.PlayOneShot(hitSound);

                if (health == 0)
                {
                    source.PlayOneShot(deadSound);
                    OnDead();
                }
                else
                {
                    source.PlayOneShot(swearSound[Random.Range(0, swearSound.Length)]);
                }
            
            }

            player.character.characterRigidbody.velocity = (player.transform.position - (transform.position + Vector3.up)).normalized * 10;
        }
    }
}
