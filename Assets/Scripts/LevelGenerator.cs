using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public enum CubeSide
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] private GameObject m_sideCube;
    [SerializeField] private GameObject m_cubePrefab;
    //[SerializeField] private GameObject m_coinPrefab;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    public static LevelGenerator Instance { get; private set; }
    private GameObject m_cube;
    private GameObject m_levelPart;
    private int currentCubeSide = -1;

    private void Awake()
    {
        Instance = this;
        
       // GrowPool();
    }

    private void Start()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        for (int i = 0; i < 2000; i++)
        {
            m_cube = Instantiate(m_cubePrefab, new Vector3(i, 0, 0),Quaternion.Euler(new Vector3(90,0,90)));
            m_cube.transform.SetParent(transform);
            if (i % 2 == 0 && i>10)
            {
                #region BuildSideCube
                    var sideCube = Instantiate(m_sideCube, transform.position, Quaternion.identity);
                    var typeSideCube = (CubeSide)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CubeSide)).Length);
                    sideCube.transform.SetParent(m_cube.transform);
                    switch (typeSideCube)
                    {
                        case CubeSide.Left:
                            sideCube.transform.position = new Vector3(i, 0, 0.5f);
                            sideCube.transform.rotation = Quaternion.Euler(0, -90, 0);
                            break;
                        case CubeSide.Right:
                            sideCube.transform.position = new Vector3(i, 0, -0.5f);
                            sideCube.transform.rotation = Quaternion.Euler(0, 90, 0);

                            break;
                        case CubeSide.Up:
                            sideCube.transform.position = new Vector3(i, 0.5f, 0);
                            sideCube.transform.rotation = Quaternion.Euler(0, 0, 90);

                            break;
                        case CubeSide.Down:
                            sideCube.transform.position = new Vector3(i, -0.5f, 0);
                            sideCube.transform.rotation = Quaternion.Euler(0, 0, -90);
                            break;

                    }
                    sideCube.transform.localScale = new Vector3(UnityEngine.Random.Range(1, 10), 1, 1);
                #endregion
            }
        }
    }


    //private void GrowPool()
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        var instanceToAdd = Instantiate(m_levelPart);
    //        instanceToAdd.transform.SetParent(transform);
    //        AddToPool(instanceToAdd);
    //    }
    //}

    //private void AddToPool(GameObject instance)
    //{
    //    instance.SetActive(false);
    //    availableObjects.Enqueue(instance);
    //}

    //public GameObject GetFromPool()
    //{
    //    if (availableObjects.Count == 0)
    //        GrowPool();
    //    var instance = availableObjects.Dequeue();
    //    instance.SetActive(true);
    //    return instance;
    //}
}
