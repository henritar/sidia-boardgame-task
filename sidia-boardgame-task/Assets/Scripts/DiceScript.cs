using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{

	private Rigidbody rb;
	public Vector3 diceVelocity;
	private bool initialized = false;
	private GameManager _gameManager = default;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    private void OnEnable()
    {
		initialized = false;
    }

    // Update is called once per frame
    void Update()
	{
		diceVelocity = rb.velocity;

		if (Input.GetKeyDown(KeyCode.Space) && diceVelocity.magnitude == 0 && _gameManager.GetGameState() == 3)
		{
			StartCoroutine(WaitToStopCoroutine());

			float dirX = Random.Range(0, 500);
			float dirY = Random.Range(0, 500);
			float dirZ = Random.Range(0, 500);
			transform.position = new Vector3 (Random.Range(-4, 5), Random.Range(-49, -46), Random.Range(-4, 5));
			transform.rotation = Quaternion.identity;
			rb.AddForce(transform.up * 200);
			rb.AddTorque(dirX, dirY, dirZ);
		}
		if(_gameManager.GetGameState() == 1)
        {
			initialized = false;
        }
	}

	public bool IsDiceStationary()
	{
		return diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f && initialized;

	}

	IEnumerator WaitToStopCoroutine()
    {
		yield return new WaitForSeconds(2);
		initialized = true;
	}

}
