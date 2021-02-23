using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{

	private Rigidbody rb;
	public Vector3 diceVelocity;
	private bool initialized = false;
	private GameManager _gameManager = default;
	private bool isDiceRolled;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	//When the dice is enable, restart booleans reponsible for the dice roll
    private void OnEnable()
    {
		isDiceRolled = false;
		initialized = false;
    }

	// Update is called once per frame
	void Update()
	{
		diceVelocity = rb.velocity;

		//When the dices are stationary and the gamestate is Battle state and when dice haven't been rolled yet this turn
		if (diceVelocity.magnitude == 0 && _gameManager.GetGameState() == 3 && !isDiceRolled)
		{
			isDiceRolled = true;
			
			StartCoroutine(WaitToStopCoroutine());

			//Physics values so the dice can roll by itself
			float dirX = Random.Range(0, 500);
			float dirY = Random.Range(0, 500);
			float dirZ = Random.Range(0, 500);
			transform.position = new Vector3(Random.Range(-4, 5), Random.Range(-49, -45), Random.Range(-4, 5));
			transform.rotation = Quaternion.identity;
			rb.AddForce(transform.up * 300);
			rb.AddTorque(dirX, dirY, dirZ);
		}

		//When game leave battle state, restore bool
		if (_gameManager.GetGameState() != 3)
		{
			initialized = false;
		}

		//If any dice fall off the DiceBox, return it back
		if (transform.position.y < -50f)
        {
			transform.position = new Vector3(Random.Range(-4, 5), Random.Range(-49, -45), Random.Range(-4, 5));
		}
	}


	//Verify if the dice is stationary so the result can be shown
	public bool IsDiceStationary()
	{
		return diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f && initialized;

	}

	//Coroutine so the DiceCHeckZone can wait all dices to stop rolling
	IEnumerator WaitToStopCoroutine()
    {
		yield return new WaitForSeconds(2);
		initialized = true;
	}

}
