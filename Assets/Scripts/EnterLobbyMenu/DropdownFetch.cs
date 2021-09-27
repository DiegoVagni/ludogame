using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.UI;

public class DropdownFetch : MonoBehaviour
{
    private Dropdown gameObject;
    // Start is called before the first frame update
    void Awake() {
        gameObject = this.GetComponent<Dropdown>();
    }
    void Start()
    {
        //put id stanze;
        List<string> test = new List<string>() {
            "ciao",
            "sono",
            "una",
            "opzione"
        };
        gameObject.ClearOptions();
        gameObject.AddOptions(test);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
