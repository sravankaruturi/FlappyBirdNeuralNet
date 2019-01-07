using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameManager gameManager;
    public NeuralNetworkTest net;


    [SerializeField]
    float jumpCoolDown;

    [SerializeField]
    float jumpTimer = 0;

    [SerializeField]
    float upForce;

    [SerializeField]
    float input_distance_y;

    [SerializeField]
    float distance_x;

    float lastDistanceMidY = 1.0f;

    [SerializeField]
    float[] outputsFromNet;

    [SerializeField]
    float score;

    [SerializeField]
    float NeuralNetOut;

    public float Score
    {
        get
        {
            return score;
        }
    }

    [SerializeField]
    bool initialized = false;

    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Init(NeuralNetworkTest _net)
    {
        this.net = _net;
        initialized = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (initialized)
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }

            if (this.alive)
            {
                this.score += Time.deltaTime;
            }

            jumpTimer -= Time.deltaTime;

            // We Get the nearest pipe to the right.
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");

            if (pipes.Length > 1)
            {
                GameObject closestPipeTop = pipes[0];
                GameObject closestPipeBottom = pipes[1];
                float distance = float.MaxValue;
                foreach (GameObject p in pipes)
                {
                    List<GameObject> passedPipes = new List<GameObject>();
                    if (p.transform.position.x > this.transform.position.x)
                    {
                        float tempDistanceX = p.transform.position.x - this.transform.position.x;
                        if (tempDistanceX < distance)
                        {
                            distance = tempDistanceX;
                            closestPipeTop = p;
                        }
                        else if (tempDistanceX == distance)
                        {
                            if (p.transform.position.y > closestPipeTop.transform.position.y)
                            {
                                closestPipeBottom = closestPipeTop;
                                closestPipeTop = p;
                            }
                            else
                            {
                                closestPipeBottom = p;
                            }
                        }
                    }
                    else
                    {
                        passedPipes.Add(p);
                    }

                    float ySum = 0;
                    int numPassed = 0;
                    foreach ( GameObject passedPipe in passedPipes)
                    {
                        ySum += passedPipe.transform.position.y;
                        numPassed++;
                    }
                    lastDistanceMidY = ySum / numPassed;
                }
                distance_x = distance;

                // Now that we have the pipes, compute the midpoint for them and the distance in thevrtical direction for that and the bird.
                float top_point_y = -closestPipeTop.GetComponent<BoxCollider2D>().size.y / 2 + closestPipeTop.transform.position.y;
                float bot_poitn_y = closestPipeBottom.GetComponent<BoxCollider2D>().size.y / 2 + closestPipeBottom.transform.position.y;

                float centre_point = (top_point_y + bot_poitn_y) / 2;
                this.input_distance_y = this.transform.position.y - centre_point;

                float[] inputs = { distance_x, input_distance_y, this.transform.position.y, this.GetComponent<Rigidbody2D>().velocity.y , lastDistanceMidY};
                outputsFromNet = this.net.FeedForward(inputs);

                NeuralNetOut = outputsFromNet[0];
                if (outputsFromNet[0] > 0.5f)
                {
                    Jump();
                }

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //gameManager.gameRunning = false;
        //gameManager.gameFinished = true;
        initialized = false;
        net.SetFitness(score);
        this.GetComponent<Rigidbody2D>().Sleep();
        this.alive = false;
    }

    public void Jump()
    {
        if (jumpTimer <= 0)
        {
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, upForce, 0));
            jumpTimer = jumpCoolDown;
        }

        if (!this.gameObject.GetComponent<Rigidbody2D>().IsAwake())
        {
            this.gameObject.GetComponent<Rigidbody2D>().WakeUp();
        }
    }
}
