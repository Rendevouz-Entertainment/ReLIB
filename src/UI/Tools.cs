using UnityEngine;
using UnityEngine.UIElements;

namespace ReLIB.UI
{

    public static class Tools
    {
        static dynamic getDocumentByID(string ID)
        {
            return GameObject.Find("GameManager/Default Game Instance(Clone)/UI Manager(Clone)/Main Canvas").GetComponent<UIDocument>().rootVisualElement.Q(ID);
        }
    }
    
}