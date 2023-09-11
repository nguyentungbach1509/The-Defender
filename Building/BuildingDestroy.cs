using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDestroy : MonoBehaviour
{
    public bool DestroyBuilding { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        DestroyBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopDestroy()
    {
        DestroyBuilding = true;
        gameObject.SetActive(false);
    }
}
