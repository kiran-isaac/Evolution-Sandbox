using TMPro;
using UnityEngine;

namespace Simulation
{
    public class CreateFlags : MonoBehaviour
    {
        public GameObject flagPrefab;



        public void GenerateFlags()
        {
            while (transform.childCount != 0)
            {
                foreach (Transform child in transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            for (int x = (int)-transform.localScale.x / 2; x < (int)transform.localScale.x / 2; x += 5)
            {
                GameObject flag = Instantiate(flagPrefab, new Vector3(x, 0f, 0f), Quaternion.identity);
                flag.transform.parent = transform;
                GameObject dist = flag.transform.GetChild(2).gameObject;
                TextMeshPro text = dist.GetComponent<TextMeshPro>();
                text.SetText(x.ToString() + "m");
            }
        }
    }
}
