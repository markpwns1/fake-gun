using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CluesManager : MonoBehaviour
{
    public int numberOfClues;

    private Soundtrack st;

    private List<Clue> neededClues;
    private bool clueProcessed;
    private int lastCluesLeft;

    void Start()
    {
        clueProcessed = false;
        neededClues = new List<Clue>();
        st = FindObjectOfType<Soundtrack>();
    }

    void Update()
    {
        if (!clueProcessed)
        {
            Clue[] cluesInMap = FindObjectsOfType<Clue>();
            List<int> generatedNeededCluesNumbers = RandomNumberList(numberOfClues, 0, cluesInMap.Length - 1);
            for (int i = 0; i < numberOfClues; i++)
            {
                neededClues.Add(cluesInMap[generatedNeededCluesNumbers[i]]);
            }
            clueProcessed = true;
            foreach (Clue clue in cluesInMap)
            {
                if (!neededClues.Contains(clue))
                {
                    clue.Deactivate();
                }
            }
        }
        else {
            lastCluesLeft = neededClues.Count();
            neededClues = neededClues.Where(clue => clue != null).ToList();
            if (lastCluesLeft != neededClues.Count) {
                st.MusicChange();
            }
        }
    }

    List<int> RandomNumberList(int amount, int min, int max) {
        List<int> ret = new List<int>();
        while (ret.Count < amount) {
            int currentInt = Random.Range(min, max);
            if (!ret.Contains(currentInt)) {
                ret.Add(currentInt);
            }
        }
        return ret;
    }

    public int CluesLeft() {
        return neededClues.Count();
    }
}
