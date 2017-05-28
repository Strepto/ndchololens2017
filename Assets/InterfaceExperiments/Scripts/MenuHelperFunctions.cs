using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHelperFunctions : MonoBehaviour {
    public GameObject HelpPane;
    public GameObject MenuExtension;
    public GameObject ValveInfo;
    public GameObject ShowHelpOverThisObject;
    public GameObject ShowMenuUnderThisObject;
    public GameObject ShowValveInfoUnderThisObject;

    private GameObject _valveInfoObj;
    private GameObject _helpPaneObj;
    private GameObject _menuExtensionObj;

    public void ResetDynamic()
    {
        var parent = GameObject.Find("_Dynamic").transform;
        var children = new List<GameObject>();
        foreach(Transform child in parent)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
    }

    public void EnableHelpOverGameObject(bool enable)
    {
        if (enable)
        {
            _helpPaneObj = InstantiateHere(HelpPane, ShowHelpOverThisObject);
        }
        else
        {
            if (_helpPaneObj != null)
            {
                _helpPaneObj.GetComponent<Animator>().SetTrigger("Collapse");
            }
        }
    }

    public void EnableGravityOnDynamics(bool enable)
    {
        foreach(Rigidbody body in GameObject.Find("_Dynamic").GetComponentsInChildren<Rigidbody>())
        {
            body.useGravity = enable;
        }
    }

    public void EnableMenuUnderGameObject(bool enable)
    {
        if (enable)
        {
            _menuExtensionObj = InstantiateHere(MenuExtension ,ShowMenuUnderThisObject);
        }
        else
        {
            if (_menuExtensionObj != null)
            {
                _menuExtensionObj.GetComponent<Animator>().SetTrigger("Collapse");
            }
        }
    }


    public void ExpandValveInfo(bool enable)
    {
        if (enable)
        {
            _valveInfoObj = InstantiateHere(ValveInfo, ShowValveInfoUnderThisObject);
        }
        else
        {
            if (_valveInfoObj != null)
            {
                _valveInfoObj.GetComponent<Animator>().SetTrigger("Collapse");
            }
        }
    }

    public GameObject InstantiateHere(GameObject sourceObject, GameObject parentObject)
    {
        return Instantiate(sourceObject, parentObject.transform, false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
