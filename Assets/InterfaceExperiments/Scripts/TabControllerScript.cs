using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TabControllerScript : MonoBehaviour {
    public List<TabAndEvents> TabsAndEvents;
    public GameObject currentTab;
    
    [System.Serializable]
    public class TabAndEvents
    {
        public GameObject TabButton;
        public void OnTabSelected(bool boolz)
        {
            if (boolz == true)
            {
                OnTabSelectedEvent.Invoke(boolz);
                TabControllerEvent.Invoke(TabButton);
            }
            
        }
        public TabularButton.BooleanUnityEvent OnTabSelectedEvent;
        public delegate void TabControllerEventDelegate(GameObject obj);
        public event TabControllerEventDelegate TabControllerEvent;
    }

    public void TabStateController(GameObject tabObjectSelected)
    {
        foreach(TabAndEvents tabEvents in TabsAndEvents.Where(x => x.TabButton != tabObjectSelected))
        {
            TabularButton tabButScript = tabEvents.TabButton.GetComponent<TabularButton>();
            if (tabButScript.Checked)
            {
                tabButScript.Uncheck();
            }
        }
        currentTab = tabObjectSelected;
        //tabObjectSelected.GetComponent<TabularButton>().SetChecked(true);
    }

	// Use this for initialization
	void Start () {

        foreach (TabAndEvents tabEvents in TabsAndEvents)
        {
            tabEvents.TabButton.GetComponent<TabularButton>().ButtonStateCheckedListener.AddListener(tabEvents.OnTabSelected);
            tabEvents.TabControllerEvent += TabStateController;
        }
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update () {
		
	}
}
