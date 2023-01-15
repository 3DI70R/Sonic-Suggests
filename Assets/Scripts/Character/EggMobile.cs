using UnityEditor;
using UnityEngine;

public class EggMobile : MonoBehaviour, IHittable {

    public enum WaypointAlogithm {
        Forward,
        Backward,
        Random
    }

    public GameObject[] waypoints;
    public GameObject eggmanModel;
    public LineRenderer lineRenderer;
    public WaypointAlogithm waypointAlogithm;
    public bool activated;
    public int health = 8;
    public float speed;
    public GameObject ballObject;
    private float lastHitTime;

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

            transform.position += Vector3.ClampMagnitude((currentWaypoint.transform.position
            - transform.position).normalized, Time.deltaTime * 5f * speed);
        } else {
            eggmanModel.SetActive(false);
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ballObject.transform.position);
    }

    public void OnHit(PlayerCharacter player) {

        if(Time.time > lastHitTime + 0.5) {
            health--;
            speed *= 1.1f;
            waypointAlogithm = (WaypointAlogithm) Random.Range(0, 3);
            lastHitTime = Time.time;
        }

        player.character.characterRigidbody.velocity = (player.transform.position - (transform.position + Vector3.up)).normalized * 10;


    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.CompareTag("Eggman")) {
            activated = true;
            Destroy(collider.gameObject);
        }
    }
}
