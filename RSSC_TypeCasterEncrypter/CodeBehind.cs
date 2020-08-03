﻿using System;
using System.Collections.Generic;
using System.Text;

using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;

namespace RSSC_TypeCasterEncrypter
{
    /// <summary>
    /// Code-behind class for the RSSC_TypeCasterEncrypter Smart Component.
    /// </summary>
    /// <remarks>
    /// The code-behind class should be seen as a service provider used by the 
    /// Smart Component runtime. Only one instance of the code-behind class
    /// is created, regardless of how many instances there are of the associated
    /// Smart Component.
    /// Therefore, the code-behind class should not store any state information.
    /// Instead, use the SmartComponent.StateCache collection.
    /// </remarks>
    public class CodeBehind : SmartComponentCodeBehind
    {
        /// <summary>
        /// Called when the value of a dynamic property value has changed.
        /// </summary>
        /// <param name="component"> Component that owns the changed property. </param>
        /// <param name="changedProperty"> Changed property. </param>
        /// <param name="oldValue"> Previous value of the changed property. </param>
        public override void OnPropertyValueChanged(SmartComponent component, DynamicProperty changedProperty, Object oldValue)
        {
            double GrabTheData; 
            
            if (changedProperty.Name == "SampleProperty")
            {
                GrabTheData = (double)changedProperty.Value;

                component.IOSignals["GroupOutput"].Value = changedProperty.Value;
                Logger.AddMessage(new LogMessage("input changed to: "+GrabTheData.ToString()));
            }
            Logger.AddMessage(new LogMessage("Executed OnPropertyValueChanged"));
        }

        /// <summary>
        /// Called when the value of an I/O signal value has changed.
        /// </summary>
        /// <param name="component"> Component that owns the changed signal. </param>
        /// <param name="changedSignal"> Changed signal. </param>
        public override void OnIOSignalValueChanged(SmartComponent component, IOSignal changedSignal)
        {
            switch (changedSignal.Name)
            {
                case "DigitalInput":
                    component.IOSignals["DigitalOutput"].Value = changedSignal.Value;
                    Logger.AddMessage(new LogMessage("Someone just pressed DigitalInput!"));
                    break;
                default:
                    Logger.AddMessage(new LogMessage("not recognised"));
                    break;
            }
            Logger.AddMessage(new LogMessage("finished OnIOSignalValueChanged"));
        }

        /// <summary>
        /// Called during simulation.
        /// </summary>
        /// <param name="component"> Simulated component. </param>
        /// <param name="simulationTime"> Time (in ms) for the current simulation step. </param>
        /// <param name="previousTime"> Time (in ms) for the previous simulation step. </param>
        /// <remarks>
        /// For this method to be called, the component must be marked with
        /// simulate="true" in the xml file.
        /// </remarks>
        public override void OnSimulationStep(SmartComponent component, double simulationTime, double previousTime)
        {
        }
    }
}