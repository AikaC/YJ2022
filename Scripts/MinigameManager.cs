using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public GameObject GameScreen;
    public AudioSource audioFound;

    [SerializeField]
    private List<HiddenObjectData> hiddenObjectsList;
    private int totalHiddenObjectsFound = 0;

    //Game Manager
    private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GM.GameStatus() == true)
        {
            MouseObject();
        }
    }

    // Hidden Object Game - mouse find object
    public void MouseObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.zero);

            if (hit && hit.collider != null)
            {
                //play audio
                audioFound.Play();
                // Set Object Inactive On Click
                hit.collider.gameObject.SetActive(false);
                totalHiddenObjectsFound++;

                if (totalHiddenObjectsFound >= 4)
                {
                    // End of Minigame
                    GameScreen.SetActive(false);
                }
            }
        }
    }
}

// Hidden Objects Minigame
[System.Serializable]
public class HiddenObjectData
{
    public GameObject hiddenObject;
    public bool makeHidden = false;
}
