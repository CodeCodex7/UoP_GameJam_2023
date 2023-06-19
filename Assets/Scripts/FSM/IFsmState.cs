using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFsmState
{
  
    abstract void OnEnter();
    
    abstract void InState();
    
    abstract void OnExit();
    

}
