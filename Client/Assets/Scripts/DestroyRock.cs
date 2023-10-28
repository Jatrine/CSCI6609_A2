using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRock : MonoBehaviour
{
    static public bool selecting = false;

    [SerializeField]
    List<GameObject> rock;
    [SerializeField]
    List<Transform> rockFracture;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    AudioSource sfx;

    float destroySpeed = -5f;
    RaycastHit hit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.touches[0].position);
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Rock"))
            {
                for (int index = 0; index < rock.Count; index++)
                {
                    if (hit.transform.name == rock[index].name)
                        SelectRock(index);
                }
            }
        }

        if (Input.acceleration.y < destroySpeed)
        {
            ReplaceRock();
        }

        selecting = false;
        for (int index = 0; index < rock.Count; index++)
        {
            if (rock[index].GetComponent<Outline>().enabled == true && rock[index].activeInHierarchy)
                selecting = true;
        }
    }

    void SelectRock(int index)
    {
        if (rock[index].GetComponent<Outline>().enabled == false)
        {
            hit.transform.gameObject.GetComponent<Outline>().enabled = true;
            NetworkManager.UserAction = "Select rock";
        }
        else if (rock[index].GetComponent<Outline>().enabled == true)
        {
            hit.transform.gameObject.GetComponent<Outline>().enabled = false;
            NetworkManager.UserAction = "Deselect rock";
        }
    }


    void ReplaceRock()
    {
        NetworkManager.UserAction = "Destroy rock";
        for (int index = 0; index < rock.Count; index++)
        {
            if (rock[index].GetComponent<Outline>().enabled == true && rock[index].activeInHierarchy)
            {
                rock[index].SetActive(false);
                sfx.Play();
                Instantiate(rockFracture[index], rock[index].transform.position, rock[index].transform.rotation, gameObject.transform);
            }
        }
    }
}
