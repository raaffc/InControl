using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace InControl
{
    abstract public class AbstractInputDeviceManager{
        protected List<IInputDevice> devices = new List<IInputDevice>();

        abstract public void Update(ulong updateTick, float deltaTime);
    }
}

