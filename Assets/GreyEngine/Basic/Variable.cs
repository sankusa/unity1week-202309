using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Variable {
    public string name;
    public string typeName;
    public string valueString;
    
    public Variable(string name, string typeName, string valueString) {
        this.name = name;
        this.typeName = typeName;
        this.valueString = valueString;
    }
}
