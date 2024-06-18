using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyState
{
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public float drag;
    public float angularDrag;
    public float mass;

    public Vector3 position;
    public Quaternion rotation;
	public RigidbodyState(Rigidbody2D rb)
	{
		velocity = rb.velocity;
		drag = rb.drag;
		angularDrag = rb.angularDrag;
		mass = rb.mass;
	}

	public void SetRigidbody(Rigidbody2D rb)
	{
		rb.velocity = velocity;
		rb.drag = drag;
		rb.angularDrag = angularDrag;
		rb.mass = mass;
	}
}

public class PlayerRecorder : MonoBehaviour
{
    public Queue<Snapshot> snapshots = new Queue<Snapshot>();

    public Player player;
    public Rigidbody2D rb;

    public float firstTime;

    public List<Snapshot.SnapAction> queuedActions = new List<Snapshot.SnapAction>();

    public bool recordOn;

    private Vector2 startLocation;
    public GameObject playerGhost;

    void Start()
    {
        firstTime = Time.time;

        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();

        startLocation = transform.position;
    }

    void FixedUpdate()
    {
        if (recordOn && player && rb)
        {
            snapshots.Enqueue(new Snapshot(player.transform.position, Input.mousePosition, rb, queuedActions, Time.time - firstTime));
            queuedActions.Clear();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            recordOn = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            recordOn = false;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObject ghost = Instantiate(playerGhost, startLocation, Quaternion.identity);
            Queue<Snapshot> newSnaps = new Queue<Snapshot>();
            foreach (Snapshot snap in snapshots)
            {
                newSnaps.Enqueue(snap.CopySelf());
            }
            ghost.GetComponent<PlayerGhost>().snapshots = newSnaps;
        }
    }

    public void FlushRecordings()
    {
        firstTime = Time.time;
        snapshots.Clear();

        startLocation = transform.position;
    }
}