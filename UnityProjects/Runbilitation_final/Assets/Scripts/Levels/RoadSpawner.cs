using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
	public GameObject defaultPrefab;
	public GameObject curvingLeftPrefab;
	public GameObject curvingRightPrefab;
	public GameObject endingPrefab;
	public GameObject[] prefabs;


	public Transform character;
	public int displayedPrefabs = 8;
	public float changeDirectionProbability = 0.1f;

	public int levelLenght = 100;
	private int levelPrefabCounter = 0;

	private List<GameObject> activePrefabs;
	private float prefabsZSize;
	private float destroyDistance;
	private int lastPrefabIndex = 0;
	private Vector3 lastPrefabSpawnPosition;

	// array with probabilities to change a certain direction
	private float[] directionProbabilities;
	private float direction = 0;
	private float oldChangeDirection = 0;


	void Start()
	{
		activePrefabs = new List<GameObject>();
		prefabsZSize = prefabs[0].GetComponent<Renderer>().bounds.size.z;
		lastPrefabSpawnPosition = new Vector3(0, 0, -prefabsZSize);
		destroyDistance = prefabsZSize * 2;

		// 3 elements
		directionProbabilities = new float[3];
		// first element is the probability to mantain a certain direction
		directionProbabilities[0] = 1 - changeDirectionProbability;
		// second and third elements are the probability to turn left or right
		// the sum of all elements as to be 1
		directionProbabilities[1] = directionProbabilities[2] = changeDirectionProbability / 2;


		//Debug.Log("Destroy distance: " + destroyDistance);
		//Debug.Log("Prefab z size = " + prefabsZSize);

		// Initial creation, the first prefab is repeated 4 times.
		for (int i = 0; i < displayedPrefabs; i++)
		{
			if (i < 2)
				Spawn(0, true);
			else
				Spawn();
		}
	}


	void Update()
	{
		if (Math.Abs(Vector3.Distance(character.position, activePrefabs[0].transform.position)) > destroyDistance)  // se distanza tra ultimo blocco generato e personaggio > soglia
		{
			if (levelPrefabCounter < levelLenght - 1)
			{
				Spawn();    // genero blocco
				DeletePrefab();       // distruggo il più vecchio

			}
			else
			{
				if (levelPrefabCounter == levelLenght - 1)
					Spawn(3, true);
			}
		}
	}


	void Spawn(int specialPrefab = -1, bool fixedDirection = false)
	{
		GameObject newPrefab;
		float newChangeDirection = 0;
		Vector3 spawnDirection = Vector3.forward;
		float spawnYRotation = 0f;

		// decide first based on old direction and update the second time
		if (direction == 0)
		{
			spawnDirection = Vector3.forward;
			spawnYRotation = 0f;
		}
		else if (direction == 1)
		{
			spawnDirection = Vector3.left;
			spawnYRotation = -90f;
		}
		else if (direction == 2)
		{
			spawnDirection = Vector3.right;
			spawnYRotation = 90f;
		}


		if (!fixedDirection)
		{
			newChangeDirection = ChooseDirection(directionProbabilities);
			//Debug.Log("Sorted direction: " + newChangeDirection);
		}

		// decide if change direction or not
		if (newChangeDirection != oldChangeDirection)
		{
			if (newChangeDirection > 0)	// actually change direction only if sorted 1 or 2
			{
				// if new choice
				oldChangeDirection = newChangeDirection;
				// but if new choice of turning left or right
				//Debug.Log("I'll change direction");
				if (direction == 0)
				{
					direction = newChangeDirection;
				}
				else if (direction == 1)
				{
					direction = 0;
				}
				else if (direction == 2)
				{
					direction = 0;
				}
				specialPrefab = (int)newChangeDirection;
			}
		}


		// decide which prefab to create
		if (specialPrefab == -1)
			newPrefab = Instantiate(prefabs[GetRandomPrefabIndex()] as GameObject);
		else
		{
			if (specialPrefab == 1)
			{
				newPrefab = Instantiate(curvingLeftPrefab as GameObject);
			}
			else if (specialPrefab == 2)
			{
				newPrefab = Instantiate(curvingRightPrefab as GameObject);
			}
			else if (specialPrefab == 3)
			{
				newPrefab = Instantiate(endingPrefab as GameObject);
			}
			else
			{
				newPrefab = Instantiate(defaultPrefab as GameObject);
			}
		}

		newPrefab.transform.SetParent(this.transform);
		lastPrefabSpawnPosition = newPrefab.transform.position =
			lastPrefabSpawnPosition + spawnDirection * prefabsZSize;

		newPrefab.transform.Rotate(0f, spawnYRotation, 0f, Space.Self);

		activePrefabs.Add(newPrefab);

		levelPrefabCounter++;
	}


	void DeletePrefab()
	{
		Destroy(activePrefabs[0]);
		activePrefabs.RemoveAt(0);
	}


	int GetRandomPrefabIndex()
	{
		if (prefabs.Length <= 1)
			return 0;

		int randomIndex = lastPrefabIndex;
		while (randomIndex == lastPrefabIndex)
		{
			randomIndex = UnityEngine.Random.Range(0, prefabs.Length);
		}

		lastPrefabIndex = randomIndex;
		return randomIndex;
	}


	float ChooseDirection(float[] probs)
	{
		float total = 0;

		foreach (float elem in probs)
		{
			total += elem;
		}

		float randomPoint = UnityEngine.Random.value * total;

		for (int i = 0; i < probs.Length; i++)
		{
			if (randomPoint < probs[i])
			{
				return i;
			}
			else
			{
				randomPoint -= probs[i];
			}
		}
		return 0;
	}
}
