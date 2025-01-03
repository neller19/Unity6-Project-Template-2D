using System.Collections.Generic;
using UnityEngine;

public class DebugMessagePanel : Singleton<DebugMessagePanel>
{
    [SerializeField]
    DebugMessageLine linePrefab;

    Stack<DebugMessageLine> linePool = new Stack<DebugMessageLine>();

    [SerializeField]
    Transform activeContainer;
    [SerializeField]
    Transform inactiveContainer;

    protected override void OnAwake()
    {

        //set up some initial line writers
        for (int i = 0; i < 10; i++)
        {
            var line = Instantiate(linePrefab);
            ReturnDebugWriter(line);
        }
    }
    public DebugMessageLine GetDebugWriter()
    {
        if (linePool.Count <= 0)
        {
            linePool.Push(Instantiate(linePrefab, inactiveContainer));
        }

        var line = linePool.Pop();
        line.gameObject.SetActive(true);
        line.transform.localPosition = Vector3.zero;
        line.transform.localScale = Vector3.one;

        line.transform.SetParent(activeContainer, false);
        return line;
    }

    public void ReturnDebugWriter(DebugMessageLine line)
    {
        if (line == null) return;
        line.transform.SetParent(inactiveContainer);
        //line.transform.localScale = Vector3.one;
        //line.transform.localPosition = Vector3.zero;
        line.gameObject.SetActive(false);
        linePool.Push(line);
    }
}
