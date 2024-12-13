using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Utilities
{
    using Elements;
    using UnityEditor.UIElements;
    using UnityEngine;

    public static class DSElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(this DSNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }

        public static Toggle CreateCheckbox(bool value = false, string label = null, EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            Toggle toggle = new Toggle()
            {
                label = label,
                value = value
            };

            if (onValueChanged != null)
            {
                toggle.RegisterValueChangedCallback(onValueChanged);
            }

            return toggle;
        }

        public static ObjectField CreateSpriteSelector(Sprite selectedSprite = null, string label = null, EventCallback<ChangeEvent<Object>> onValueChanged = null)
        {
            // Create the ObjectField
            ObjectField spriteSelector = new ObjectField()
            {
                label = label,
                value = selectedSprite,
                objectType = typeof(Sprite), // Restrict to Sprite assets
                allowSceneObjects = false    // Prevent selecting scene objects
            };

            // Register a callback for when the value changes
            if (onValueChanged != null)
            {
                spriteSelector.RegisterValueChangedCallback(onValueChanged);
            }

            return spriteSelector;
        }
    }
}