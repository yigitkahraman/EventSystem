# EventSystem
 Generic Event System for Unity. Any 'object' can send and recieve events.
 
**Usage**
```cs
public class Listener : MonoBehaviour {
	private void OnEnable(){
		this.Subscribe("TestEvent", HandleEvent);
	}
	
	private void OnDisable(){
		this.Unsubscribe("TestEvent", HandleEvent);
	}
	
	private void HandleEvent(object args){
		Debug.Log((int)args);
	}
}

public class Invoker : MonoBehaviour {
	private void Start(){
		this.PostEvent("TestEvent", 13);
	}
}
```
