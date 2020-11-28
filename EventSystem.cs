using System.Collections.Generic;
using Handler = System.Action<object>;
using SenderTable = System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<System.Action<object>>>;

public class EventSystem {
    private Dictionary<string, SenderTable> _table = new Dictionary<string, SenderTable>();
    private HashSet<List<Handler>> _invoking = new HashSet<List<Handler>>();

    public readonly static EventSystem instance = new EventSystem();
    private EventSystem() { }

    public void Subscribe(string eventName, Handler handler) {
        Subscribe(eventName, handler, null);
    }

    public void Subscribe(string eventName, Handler handler, object sender) {
        if(handler == null) return;
        if(string.IsNullOrEmpty(eventName)) return;
        if(!_table.ContainsKey(eventName)) _table.Add(eventName, new SenderTable());
        var subTable = _table[eventName];
        var key = sender ?? this;
        if(!subTable.ContainsKey(key)) subTable.Add(key, new List<Handler>());
        var list = subTable[key];
        if(list.Contains(handler)) return;
        if(_invoking.Contains(list)) subTable[key] = list = new List<Handler>(list);
        list.Add(handler);
    }

    public void Unsubscribe(string eventName, Handler handler) {
        Unsubscribe(eventName, handler, null);
    }

    public void Unsubscribe(string eventName, Handler handler, object sender) {
        if(handler == null) return;
        if(string.IsNullOrEmpty(eventName)) return;
        if(!_table.ContainsKey(eventName)) return;
        var subTable = _table[eventName];
        var key = sender ?? this;
        if(!subTable.ContainsKey(key)) return;
        var list = subTable[key];
        var index = list.IndexOf(handler);
        if(index == -1) return;
        if(_invoking.Contains(list)) subTable[key] = list = new List<Handler>(list);
        list.RemoveAt(index);
    }

    public void PostEvent(string eventName, object args) {
        PostEvent(eventName, args, null);
    }

    public void PostEvent(string eventName, object args, object sender) {
        if(string.IsNullOrEmpty(eventName)) return;
        if(!_table.ContainsKey(eventName)) return;
        var subTable = _table[eventName];
        if(sender != null && subTable.ContainsKey(sender)) {
            var handlers = subTable[sender];
            _invoking.Add(handlers);
            for(int i = 0; i < handlers.Count; i++) {
                handlers[i](args);
            }
            _invoking.Remove(handlers);
        }
        if(subTable.ContainsKey(this)) {
            var handlers = subTable[this];
            _invoking.Add(handlers);
            for(int i = 0; i < handlers.Count; i++) {
                handlers[i](args);
            }
            _invoking.Remove(handlers);
        }
    }
}

public static class EventSystemExtensions {
    public static void PostEvent(this object obj, string eventName) {
        EventSystem.instance.PostEvent(eventName, null);
    }

    public static void PostEvent(this object obj, string eventName, object args) {
        EventSystem.instance.PostEvent(eventName, args, null);
    }

    public static void Subscribe(this object obj, string eventName, Handler handler) {
        EventSystem.instance.Subscribe(eventName, handler, null);
    }

    public static void Unsubscribe(this object obj, string eventName, Handler handler) {
        EventSystem.instance.Unsubscribe(eventName, handler, null);
    }
}
