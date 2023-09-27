using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    GameObject[] generators;
    GameObject current_generator;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        generators = GameObject.FindGameObjectsWithTag("Generator");

        for (int i = 0; i < generators.Length; i++)
        {
            if (generators[i].GetComponent<GeneratorScript>().generatorNumber == 0)
            {
                current_generator = generators[i];
                generators[i].SetActive(true);
            }

            else
            {
                generators[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (current_generator.GetComponent<GeneratorScript>().beenTaken)
        {
            current_generator.SetActive(false);
            index++;

            if (index >= generators.Length)
                return;

            print("Started generator " + index);

            for (int i = 0; i < generators.Length; i++)
            {
                if (generators[i].GetComponent<GeneratorScript>().generatorNumber == index)
                {
                    current_generator = generators[i];
                    generators[i].SetActive(true);
                }
            }
        }
    }
}
