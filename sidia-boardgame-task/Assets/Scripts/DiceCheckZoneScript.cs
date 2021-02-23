using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour
{

	private bool dicesStationary;

	private GameObject[] p1Dices;
	private GameObject[] p2Dices;
	private UIManager _uiManager = default;

	private void Start()
	{
		p1Dices = GameObject.FindGameObjectsWithTag("P1Dice");
		p2Dices = GameObject.FindGameObjectsWithTag("P2Dice");
		_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//Verify if all dices are stationary so the results can be shown
		dicesStationary = AreAllDicesStationaries();
	}

	void OnTriggerStay(Collider col)
	{

		//if all dices are stationary
		if (dicesStationary)
		{
			int id = col.gameObject.GetComponent<SideCheckScript>().id;

			//Populate the result of each dice based on the opposite side that collide to the boxdice collider
			switch (col.gameObject.name)
			{
				case "SideCheck1":
					UIManager.diceResults[id] = 6;
					break;
				case "SideCheck2":
					UIManager.diceResults[id] = 5;
					break;
				case "SideCheck3":
					UIManager.diceResults[id] = 4;
					break;
				case "SideCheck4":
					UIManager.diceResults[id] = 3;
					break;
				case "SideCheck5":
					UIManager.diceResults[id] = 2;
					break;
				case "SideCheck6":
					UIManager.diceResults[id] = 1;
					break;
			}

			//Trigger battle results
			_uiManager.ShowBattleResult();
		}
	}

	public bool AreAllDicesStationaries()
	{

		return p1Dices[0].gameObject.GetComponent<DiceScript>().IsDiceStationary() && p1Dices[1].gameObject.GetComponent<DiceScript>().IsDiceStationary() && p1Dices[2].gameObject.GetComponent<DiceScript>().IsDiceStationary()
			&& p2Dices[0].gameObject.GetComponent<DiceScript>().IsDiceStationary() && p2Dices[1].gameObject.GetComponent<DiceScript>().IsDiceStationary() && p2Dices[2].gameObject.GetComponent<DiceScript>().IsDiceStationary();


	}
}