using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SpawnSettings
{
	public bool active = true;
	public int spawn_count = 0;
	public int cluster_size = 1;
	public float delay = 0.0f;
	public float interval = 1.0f;
	public float time_since_spawn = 0.0f;
}

[Serializable]
public class HordeInflux
{
	public SpawnSettings basic   = new SpawnSettings();
	public SpawnSettings clingy  = new SpawnSettings();
	public SpawnSettings runner  = new SpawnSettings();
	public SpawnSettings thicc   = new SpawnSettings();
	public SpawnSettings amazon  = new SpawnSettings();
	public SpawnSettings yandere = new SpawnSettings();
} 

public class BattleManager : MonoBehaviour
{
	public static int level = 1;

	public List<Spawn> lst_hall_spawns = new List<Spawn>();

	public CharGirl prefab_girl_basic;
	public CharGirl prefab_girl_thicc;
	public CharGirl prefab_girl_amazon;

	public HordeInflux initial_influx;
	public List<HordeInflux> lst_surprise_influxes = new List<HordeInflux>();

	void Start()
	{
		ManagerMusic.Load();

		GenInfluxes();
	}
	
	void Update()
	{
		SpawnLogic();

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Debug.Log("Quitting...");
			
			Application.Quit();
		}
	}

	private void GenInfluxes()
	{
		initial_influx = new HordeInflux();

		initial_influx.basic.spawn_count = 4 + 4 * level;
		initial_influx.basic.cluster_size = 2 * level;

		initial_influx.thicc.spawn_count = Mathf.Max(0, 1 + level-2);
		initial_influx.thicc.cluster_size = (int)Mathf.Ceil(Mathf.Max(0, 1 + level-2) / 2.0f);

		initial_influx.amazon.spawn_count = 1 * Mathf.Max(0, 1 + level-4);
		initial_influx.amazon.cluster_size = (int)Mathf.Ceil(Mathf.Max(0, 1 + level-4) / 2.0f);

		// initial_influx.thicc.spawn_count = Mathf.Max(0, level-3);
		// initial_influx.thicc.cluster_size = 1;
		// if (level < 3)
		// 	initial_influx.thicc.delay = 3;
		// else
		// if (level < 6)
		// 	initial_influx.thicc.delay = 2;
		// else
		// if (level < 9)
		// 	initial_influx.thicc.delay = 1;
		// else
		// 	initial_influx.thicc.delay = 0;

		// initial_influx.amazon.spawn_count = Mathf.Max(0, level-5);
		// initial_influx.amazon.cluster_size = 1;
		// if (level < 5)
		// 	initial_influx.amazon.delay = 3;
		// else
		// if (level < 10)
		// 	initial_influx.amazon.delay = 2;
		// else
		// if (level < 15)
		// 	initial_influx.amazon.delay = 1;
		// else
		// 	initial_influx.amazon.delay = 0;

	}

	private void SpawnLogic()
	{
		SpawnPrefabLogic(prefab_girl_basic, initial_influx.basic, lst_hall_spawns);
		SpawnPrefabLogic(prefab_girl_thicc, initial_influx.thicc, lst_hall_spawns);
		SpawnPrefabLogic(prefab_girl_amazon, initial_influx.amazon, lst_hall_spawns);
	}

	private void SpawnPrefabLogic(CharGirl prefab, SpawnSettings settings, List<Spawn> lst_spawns)
	{
		if (settings.spawn_count > 0)
		{
			settings.time_since_spawn += Time.deltaTime;

			if (settings.time_since_spawn >= settings.delay)
			{
				// Reset the delay to next spawning.
				settings.time_since_spawn = 0.0f;
				settings.delay = settings.interval;

				// Decrement the spawn count.
				int count = Mathf.Min(settings.spawn_count, settings.cluster_size);
				settings.spawn_count -= count;

				// Loop until we've spawned this cluster.
				for (int index_spawn = 0; index_spawn < count; ++index_spawn)
				{
					Spawn spawn = lst_spawns[UnityEngine.Random.Range(0, lst_spawns.Count)];

					Vector3 spawn_pos = new Vector3
					(
						UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x),
						0.0f,
						UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z)
					);

					CharGirl girl = Instantiate(prefab);
					girl.transform.SetParent(transform);
					girl.transform.position = spawn_pos;
					girl.transform.rotation = Quaternion.LookRotation(-spawn_pos, Vector3.up);
				}
			}
		}
	}
};