using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CluesManager : MonoBehaviour
{
    public int numberOfClues;
    private List<Clue> neededClues;
    private bool clueProcessed;

    void Start()
    {
        clueProcessed = false;
        neededClues = new List<Clue>();
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
            neededClues = neededClues.Where(clue => clue != null).ToList();
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
