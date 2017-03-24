/*FloorControllerCsharp.cs
 * 
 * Reads serial data from the COM port and refine it to be used in game.
  8  1
7      2
6      3
  5  4
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Floor
{
	public class FloorControllerCSharp : MonoBehaviour
	{
		private static FloorData _floorData;
		private static FloorSize _floorSize;

		public static bool startReadingData = true;
		private const Int16 _StandardMessageLength = 17;
		private byte[] _serialBuffer;
		private byte[] _tempIncompleteMessageBuffer;
		private int _tempIncompleteMessageLength = 0;
		private SerialPort _serialPort;
		private Thread _readSerialThread;

		private volatile bool _stopThread = false;

      //  public static volatile bool startReadingData = true;

        private volatile Queue _queue = Queue.Synchronized (new Queue ());

		/// <summary>
		/// Returns state of given petal index (1 to 8) if last value from device was >= StepValue
		/// careful! - min index is 1, not 0
		/// </summary>
		public bool IsPetalStepped (int petalIndex)
		{
            switch (petalIndex)
            {
                case 1:
                    return FloorInput.t1 >= StepValue;
                case 2:
                    return FloorInput.t2 >= StepValue;
                case 3:
                    return FloorInput.t3 >= StepValue;
                case 4:
                    return FloorInput.t4 >= StepValue;
                case 5:
                    return FloorInput.t5 >= StepValue;
                case 6:
                    return FloorInput.t6 >= StepValue;
                case 7:
                    return FloorInput.t7 >= StepValue;
                case 8:
                    return FloorInput.t8 >= StepValue;

                default:
                    Debug.LogWarning("FloorControllerCSharp.IsPetalStepped(): invalid petal index " + petalIndex);
                    return false;
            }
        }

		//This method returns the check on petal that has reached threshold of StompValue
		public bool IsPetalStomped (int petalIndex)
		{
            switch (petalIndex)
            {
                case 1:
                    return FloorInput.t1 >= StompValue;
                case 2:
                    return FloorInput.t2 >= StompValue;
                case 3:
                    return FloorInput.t3 >= StompValue;
                case 4:
                    return FloorInput.t4 >= StompValue;
                case 5:
                    return FloorInput.t5 >= StompValue;
                case 6:
                    return FloorInput.t6 >= StompValue;
                case 7:
                    return FloorInput.t7 >= StompValue;
                case 8:
                    return FloorInput.t8 >= StompValue;

                default:
                    Debug.LogWarning("FloorControllerCSharp.IsPetalStomped(): invalid petal index " + petalIndex);
                    return false;
            }
        }

		public struct FloorData
		{
			public int x, y;
			public int t1, t2, t3, t4, t5, t6, t7, t8;
		};

		public struct FloorSize
		{
			public int rows;
			public int cols;
		};

		public FloorData FloorInput {
			get { return _floorData; }
			set { }
		}

		public FloorSize floorSize {
			get { return _floorSize; }
			set { }
		}

		/// <summary>
		/// this value is needed to trigger button
		/// </summary>
		public int StepValue {
			get { return 13 + 127; }
			private set { }
		}

		/// <summary>
		/// this value is the threshhold to measure a stomping gesture
		/// </summary>
		public int StompValue {
			get { return 23 + 127; }
			private set { }
		}

		#region FLOOR_DATA

		// Use this for initialization
		void Start ()
		{
			_floorData = new FloorData ();
			_floorSize = new FloorSize ();

			//For small Sensfloor
			_floorSize.rows = 2;
			_floorSize.cols = 4;

			_serialPort = new SerialPort ();
			_serialPort.PortName = "COM3";
			_serialPort.BaudRate = 115200;
			_serialPort.DataBits = 8;
			_serialPort.Parity = Parity.None;
			_serialPort.StopBits = StopBits.One;

			try {
				_serialPort.Open ();

				_serialBuffer = new byte[_StandardMessageLength];
				_floorData = new FloorData ();
                Debug.Log("StartREADING = " + startReadingData);
				_readSerialThread = new Thread (Run);
			} catch (Exception e) {
				//Debug.LogException(e);
			}

			if (_serialPort.IsOpen) {
				_readSerialThread.Start ();
			}
		}

		public void Run ()
		{
            //Not working as expected ?
            if (!startReadingData)
                return;

            Action startReading = null;
                startReading = delegate
                {
                    _serialPort.BaseStream.BeginRead(_serialBuffer, 0, _serialBuffer.Length,
                        delegate (IAsyncResult ar)
                        {
                            try
                            {
                                int actualLength = _serialPort.BaseStream.EndRead(ar);
                                byte[] received = new byte[actualLength];
                                Buffer.BlockCopy(_serialBuffer, 0, received, 0, actualLength);
                                // If we have received an incomplete message, we store it temporarily and try
                                // to recover it with the next read.
                                /*
                            if (actualLength < _StandardMessageLength)
                            {
                                // We store an incomplete message
                                if (_tempIncompleteMessageLength == 0)
                                {
                                    _tempIncompleteMessageBuffer = received;
                                    _tempIncompleteMessageLength = actualLength;
                                }
                                // We try to recover a message by putting two incomplete messages together
                                else if ((_tempIncompleteMessageLength + actualLength) == _StandardMessageLength)
                                {
                                    byte[] recoveredMessage = new byte[_StandardMessageLength];
                                    Buffer.BlockCopy(_tempIncompleteMessageBuffer, 0, recoveredMessage, 0, _tempIncompleteMessageLength);
                                    Buffer.BlockCopy(received, 0, recoveredMessage, _tempIncompleteMessageLength + 1, actualLength);
                                    received = recoveredMessage;
                                    _tempIncompleteMessageLength = 0;
                                }
                            }*/

                                if (isFloorMessageValid(received))
                                {
                                    //Debug.Log(BitConverter.ToString(received));
                                    FloorData floorData = new FloorData();

                                    //Exchanged index values for x and y (as x is rows and y is cols)
                                    floorData.x = BitConverter.ToInt32(new byte[] { received[3], 0, 0, 0 }, 0);
                                    floorData.y = BitConverter.ToInt32(new byte[] { received[2], 0, 0, 0 }, 0);
                                    floorData.t1 = BitConverter.ToInt32(new byte[] { received[8], 0, 0, 0 }, 0);
                                    floorData.t2 = BitConverter.ToInt32(new byte[] { received[9], 0, 0, 0 }, 0);
                                    floorData.t3 = BitConverter.ToInt32(new byte[] { received[10], 0, 0, 0 }, 0);
                                    floorData.t4 = BitConverter.ToInt32(new byte[] { received[11], 0, 0, 0 }, 0);
                                    floorData.t5 = BitConverter.ToInt32(new byte[] { received[12], 0, 0, 0 }, 0);
                                    floorData.t6 = BitConverter.ToInt32(new byte[] { received[13], 0, 0, 0 }, 0);
                                    floorData.t7 = BitConverter.ToInt32(new byte[] { received[14], 0, 0, 0 }, 0);
                                    floorData.t8 = BitConverter.ToInt32(new byte[] { received[15], 0, 0, 0 }, 0);

                                    PrintFloorDataToConsole(floorData);

                                    _floorData = floorData;
                                }
                            }
                            catch (IOException e)
                            {
                                Debug.LogException(e);
                            }
                        }, null);

                    //if (!startReadingData)
                    //    return;

                    if (_stopThread)
                        return;
                    startReading();
                };
          
                startReading();
        }

		public static bool isFloorMessageValid (byte[] message)
		{
			if (message.Length < _StandardMessageLength - 1)
				return false;
			//if (message[0] != 0xFD)
			//return false;
			if (message [2] == 0x80 && message [4] == 0x00)
				return false;
			if (message [6] != 0x11)
				return false;
			return true;
		}

		public static void ResetData ()
		{
			_floorData.x = 0;
			_floorData.y = 0;
			_floorData.t1 = _floorData.t2 = _floorData.t3 = _floorData.t4 = _floorData.t5 = _floorData.t6 = _floorData.t7 = _floorData.t8 = 0;
		}

		void OnDisable ()
		{
            //startReadingData = false;
			_stopThread = true;

			if (_readSerialThread != null)
				_readSerialThread.Join (100);
			if (_serialPort != null && _serialPort.IsOpen) {
				_serialPort.Close ();
			}
		}

		public static bool GetStatusOfReadingData ()
		{
			return startReadingData;
		}

		private void PrintFloorDataToConsole (FloorData data)
		{
			Debug.Log ("Floor data - X: " + data.x +
			" Y: " + _floorData.y +
			" T1: " + _floorData.t1 +
			" T2: " + _floorData.t2 +
			" T3: " + _floorData.t3 +
			" T4: " + _floorData.t4 +
			" T5: " + _floorData.t5 +
			" T6: " + _floorData.t6 +
			" T7: " + _floorData.t7 +
			" T8: " + _floorData.t8);
		}

		#endregion
	}
}
