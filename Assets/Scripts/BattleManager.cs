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

	}

	private void SpawnLogic()
	{
		SpawnPrefabLogic(prefab_girl_basic, initial_influx.basic, lst_hall_spawns);
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

					CharGirl girl = Instantiate(prefab_girl_basic);
					girl.transform.SetParent(transform);
					girl.transform.position = spawn_pos;
					girl.transform.rotation = Quaternion.LookRotation(-spawn_pos, Vector3.up);
				}
			}
		}
	}
};