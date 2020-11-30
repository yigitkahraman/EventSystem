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
**Custom Arguments**
```cs
public struct OnTestEvent {
	public readonly string name;
	public readonly int value;
	
	public OnTestEvent(string name, int value){
		this.name = name;
		this.value = value;
	}
}

public class Listener : MonoBehaviour {
	private void OnEnable(){
		this.Subscribe("TestEvent", HandleEvent);
	}
	
	private void OnDisable(){
		this.Unsubscribe("TestEvent", HandleEvent);
	}
	
	private void HandleEvent(object args){
		if(!(args is OnTestEvent arguments) return;
		Debug.LogFormat("Name: {0}, Value: {1}", arguments.name, arguments.value);
	}
}

public class Invoker : MonoBehaviour {
	private void Start(){
		this.PostEvent("TestEvent", new OnTestEvent("Test", 13));
	}
}
```
