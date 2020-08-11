﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
    /// 
    public class CodeBehind : SmartComponentCodeBehind
    {
        bool[] check = new bool[4];

        /// <summary>
        /// Called when the value of a dynamic property value has changed.
        /// </summary>
        /// <param name="component"> Component that owns the changed property. </param>
        /// <param name="changedProperty"> Changed property. </param>
        /// <param name="oldValue"> Previous value of the changed property. </param>
        public override void OnPropertyValueChanged(SmartComponent component, DynamicProperty changedProperty, Object oldValue)
        {

            switch (changedProperty.Name)
            {
                case "InputProp":
                    int bitVal = ToHexEncodedInt((double)changedProperty.Value);
               
                    component.IOSignals["GroupOutputMSB"].Value = (bitVal & 0xFF000000) >> 24;
                    component.IOSignals["GroupOutputB3"].Value = (bitVal & 0x00FF0000) >> 16;
                    component.IOSignals["GroupOutputB2"].Value = (bitVal & 0x0000FF00) >> 8;
                    component.IOSignals["GroupOutputLSB"].Value = (bitVal & 0x000000FF);
                    break;
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
                case "GroupInputMSB":
                    UpdateOutProp(component);
                    break;
                case "GroupInputB3":
                    UpdateOutProp(component);
                    break;
                case "GroupInputB2":
                    UpdateOutProp(component);
                    break;  
                case "GroupInputLSB":
                    UpdateOutProp(component);
                    break;
                default:
                    Logger.AddMessage(new LogMessage("not recognised IO signal change"));
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

        string ToHexString(double f)
        {
            var bytes = BitConverter.GetBytes(Convert.ToSingle(f));
            var i = BitConverter.ToInt32(bytes, 0);
            return "0x" + i.ToString("X8");

        }

        float FromHexString(string s)
        {
            var i = Convert.ToInt32(s, 16);
            var bytes = BitConverter.GetBytes(i);
            return BitConverter.ToSingle(bytes, 0);
        }


        // used to encode a REAL in an integer
        int ToHexEncodedInt(double f)
        {
            var bytes = BitConverter.GetBytes(Convert.ToSingle(f));
            return BitConverter.ToInt32(bytes, 0);
        }

        //  use to get a double from an encoded integer
        double FromHexString(int Real)
        {
            var bytes = BitConverter.GetBytes(Real);
            //int i2 = BitConverter.ToSingle(bytes, 0);
            return BitConverter.ToSingle(bytes, 0);
        }

        // converts 4 bytes into one long float value
        void UpdateOutProp(SmartComponent component)
        {
            int bitVal = (int)component.IOSignals["GroupInputLSB"].Value + ((int)component.IOSignals["GroupInputB2"].Value << 8) + ((int)component.IOSignals["GroupInputB3"].Value << 16) + ((int)component.IOSignals["GroupInputMSB"].Value << 24);
            component.Properties["OutputProp"].Value = FromHexString(bitVal);
        }
    }
}