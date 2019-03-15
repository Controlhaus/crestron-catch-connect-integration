using System;
using System.Text;
using System.Collections.Generic;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Catch_Connect_Crestron_Library
{
    public class ChangeEventArgs : EventArgs
    {
        public SimplSharpString key { get; set; }
        public ushort digitalValue { get; set; }
        public ushort analogValue { get; set; }
        public SimplSharpString stringValue { get; set; }
        public ChangeEventArgs() { }
    }

    public class DigitalEventHandler
    {
        public delegate void DigitalHandler(SimplSharpString key, ushort value);
        public DigitalHandler OnDigitalEvent { set; get; }
        string key;

        public DigitalEventHandler()
        {
            CatchConnect.digitalEvent += new CatchConnect.DigitalEventHandler(CatchConnect_digitalEvent);
        }

        public void Initialize(SimplSharpString key)
        {
            this.key = key.ToString();
        }

        void CatchConnect_digitalEvent(object sender, ChangeEventArgs args)
        {
            if (args.key.ToString().Equals(this.key))
            {
                OnDigitalEvent(args.key, args.digitalValue);
            }
        }
    }

    public class DigitalPulseEventHandler
    {
        public delegate void DigitalPulseHandler(SimplSharpString key);
        public DigitalPulseHandler OnDigitalPulseEvent { set; get; }
        string key;

        public DigitalPulseEventHandler()
        {
            CatchConnect.digitalPulseEvent += new CatchConnect.DigitalPulseEventHandler(CatchConnect_digitalPulseEvent);
        }

        public void Initialize(SimplSharpString key)
        {
            this.key = key.ToString();
        }

        void CatchConnect_digitalPulseEvent(object sender, ChangeEventArgs args)
        {
            if (args.key.ToString().Equals(this.key))
            {
                OnDigitalPulseEvent(args.key);
            }
        }
    }

    public class AnalogEventHandler
    {
        public delegate void AnalogHandler(SimplSharpString key, ushort value);
        public AnalogHandler OnAnalogEvent { set; get; }
        string key;

        public AnalogEventHandler()
        {
            CatchConnect.analogEvent += new CatchConnect.AnalogEventHandler(CatchConnect_analogEvent);
        }

        public void Initialize(SimplSharpString key)
        {
            this.key = key.ToString();
        }

        void CatchConnect_analogEvent(object sender, ChangeEventArgs args)
        {
            if (args.key.ToString().Equals(this.key))
            {
                OnAnalogEvent(args.key, args.analogValue);
            }
        }
    }

    public class SerialEventHandler
    {
        public delegate void SerialHandler(SimplSharpString key, SimplSharpString value);
        public SerialHandler OnSerialEvent { set; get; }
        string key;

        public SerialEventHandler()
        {
            CatchConnect.serialEvent += new CatchConnect.SerialEventHandler(CatchConnect_serialEvent);
        }

        public void Initialize(SimplSharpString key)
        {
            this.key = key.ToString();
        }

        void CatchConnect_serialEvent(object sender, ChangeEventArgs args)
        {
            if (args.key.ToString().Equals(this.key))
            {
                OnSerialEvent(args.key, args.stringValue);
            }
        }
    }

    public static class CatchConnect
    {
        public static ChangeEventArgs changeEventArgs = new ChangeEventArgs();

        /* Digitals */
        public static List<SimplSharpString> digitals = new List<SimplSharpString>();
        public delegate void DigitalEventHandler(object sender, ChangeEventArgs args);
        public static event DigitalEventHandler digitalEvent;
        public static void onDigitalEvent(object sender, ChangeEventArgs args)
        {
            if (digitalEvent != null)
            {
                digitalEvent(sender, args);
            }
        }
        public delegate void DigitalPulseEventHandler(object sender, ChangeEventArgs args);
        public static event DigitalPulseEventHandler digitalPulseEvent;
        public static void onDigitalPulseEvent(object sender, ChangeEventArgs args)
        {
            if (digitalPulseEvent != null)
            {
                digitalPulseEvent(sender, args);
            }
        }

        /* Analogs */
        public static List<SimplSharpString> analogs = new List<SimplSharpString>();
        public delegate void AnalogEventHandler(object sender, ChangeEventArgs args);
        public static event AnalogEventHandler analogEvent;
        public static void onAnalogEvent(object sender, ChangeEventArgs args)
        {
            if (analogEvent != null)
            {
                analogEvent(sender, args);
            }
        }

        /* Serials */
        public static List<SimplSharpString> serials = new List<SimplSharpString>();
        public delegate void SerialEventHandler(object sender, ChangeEventArgs args);
        public static event SerialEventHandler serialEvent;
        public static void onSerialEvent(object sender, ChangeEventArgs args)
        {
            if (serialEvent != null)
            {
                serialEvent(sender, args);
            }
        }
        
        /* Service */
        public delegate void ServiceHandler(ushort online, ushort statusValue, SimplSharpString statusString);
        public static ServiceHandler OnServiceEvent { set; get; }
        public static ushort serviceStatusValue = 0;
        private static TCPServer tcpServer;
        public static uint debug = 0;

        
        /* Digitals */
        public static void InitializeDigital(SimplSharpString key)
        {
            digitals.Add(key);
        }

        public static void SetDigitalValue(SimplSharpString key, ushort value)
        {
            try
            {
                string msg = "digital;" + key.ToString() + ";" + value.ToString();
                SendDataAsync(msg);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Catch Connect SetDigitalValue Exception: " + e.Message);
            }
        }

        /* Analogs */
        public static void InitializeAnalog(SimplSharpString key)
        {
            analogs.Add(key);
        }

        public static void SetAnalogValue(SimplSharpString key, ushort value)
        {
            try
            {
                string msg = "analog;" + key.ToString() + ";" + value.ToString();
                SendDataAsync(msg);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Catch Connect SetAnalogValue Exception: " + e.Message);
            }
        }

        /* Serials */
        public static void InitializeSerial(SimplSharpString key)
        {
            serials.Add(key);
        }

        public static void SetSerialValue(SimplSharpString key, SimplSharpString value)
        {
            try
            {
                string msg = "serial;" + key.ToString() + ";" + value.ToString();
                SendDataAsync(msg);
            }
            catch (Exception e)
            {
                ErrorLog.Error("Catch Connect SetSerialValue Exception: " + e.Message);
            }
        }

        /* Service */
        public static void InitializeService(ushort port)
        {
            if (debug > 0)
            {
                CrestronConsole.Print("\n Catch Connect InitializeService with port: " + port.ToString());
            }

            CreateCsvFile();

            tcpServer = new TCPServer((int)port, 1);
            tcpServer.SocketStatusChange += new TCPServerSocketStatusChangeEventHandler(tcpServer_SocketStatusChange);
            StartListening();            
        }

        private static void CreateCsvFile()
        {
            string data = "Message Type,Name,Message,Append Special Character,Is Hexadecimal,Is Regular Expression\r\n";
            foreach (var digital in digitals)
            {
                string message = "digital;" + digital.ToString() + ";#PAYLOAD#";
                data = data + "command," + digital.ToString() + " Set Value," + message + ",,FALSE,FALSE\r\n";

                message = "digital;" + digital.ToString() + ";1";
                data = data + "command," + digital.ToString() + " On," + message + ",,FALSE,FALSE\r\n";

                message = "digital;" + digital.ToString() + ";0";
                data = data + "command," + digital.ToString() + " Off," + message + ",,FALSE,FALSE\r\n";

                message = "digital;" + digital.ToString() + ";pulse";
                data = data + "command," + digital.ToString() + " Pulse," + message + ",,FALSE,FALSE\r\n";

                message = "^digital;" + digital.ToString() + ";(.+)$";
                data = data + "response," + digital.ToString() + " Value," + message + ",,FALSE,TRUE\r\n";

                message = "digital;" + digital.ToString() + ";1";
                data = data + "response," + digital.ToString() + " Is On," + message + ",,FALSE,TRUE\r\n";

                message = "digital;" + digital.ToString() + ";0";
                data = data + "response," + digital.ToString() + " Is Off," + message + ",,FALSE,TRUE\r\n";
            }
            foreach (var analog in analogs)
            {
                string message = "analog;" + analog.ToString() + ";#PAYLOAD#";
                data = data + "command," + analog.ToString() + "," + message + ",,FALSE,FALSE\r\n";
                message = "^analog;" + analog.ToString() + ";(.+)$";
                data = data + "response," + analog.ToString() + "," + message + ",,FALSE,TRUE\r\n";
            }
            foreach (var serial in serials)
            {
                string message = "serial;" + serial.ToString() + ";#PAYLOAD#";
                data = data + "command," + serial.ToString() + "," + message + ",,FALSE,FALSE\r\n";
                message = "^serial;" + serial.ToString() + ";(.+)$";
                data = data + "response," + serial.ToString() + "," + message + ",,FALSE,TRUE\r\n";
            }
            digitals = null;
            analogs = null;
            serials = null;

            try
            {                
                string filePath = string.Format("NVRAM\\{0}-CatchConnect.csv", InitialParametersClass.ProgramIDTag);
                using (FileStream fileHandle = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    fileHandle.Write(data, Encoding.ASCII);
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("\n Catch Connect CreateCsvFile Write Exception: " + e.Message);
            }
        }

        private static void StartListening() 
        {
            try
            {
                SocketErrorCodes resultCodes = tcpServer.WaitForConnectionAsync("0.0.0.0", OnTCPServerClientConnectCallback);
                ErrorLog.Error("\n  Catch Connect InitializeService WaitForConnectionAsync resultCodes: " + resultCodes);
                if (debug > 0)
                {
                    CrestronConsole.Print("\n Catch Connect InitializeService WaitForConnectionAsync resultCodes: " + resultCodes.ToString());
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("\n Catch Connect InitializeService Exception: " + e.Message);
            }
        }


        private static void tcpServer_SocketStatusChange(TCPServer myTCPServer, uint clientIndex, SocketStatus serverSocketStatus)
        {
            if (debug > 0)
            {
                CrestronConsole.Print("\n Catch Connect SocketStatusChange serverSocketStatus: {0} for index: {1}", serverSocketStatus.ToString(), clientIndex.ToString());
            }
            
            ushort online = 0;
            switch(serverSocketStatus){
                case SocketStatus.SOCKET_STATUS_CONNECTED:
                    online = 1;
                    break;
                case SocketStatus.SOCKET_STATUS_NO_CONNECT:
                    online = 0;
                    myTCPServer.DisconnectAll();
                    StartListening();
                    break;
                default:
                    break;
            }
            OnServiceEvent(online, (ushort)serverSocketStatus, serverSocketStatus.ToString());            
        }

        private static void OnTCPServerClientConnectCallback(TCPServer myTCPServer, uint clientIndex)
        {
            if (debug > 0)
            {
                CrestronConsole.Print("\n Catch Connect OnTCPServerClientConnectCallback index: " + clientIndex.ToString());
            }
            //Listen for data from the connected client
            if (myTCPServer.ClientConnected(clientIndex))
            {
                SocketErrorCodes resultCodes = myTCPServer.ReceiveDataAsync(clientIndex, OnTCPReceiveCallback);
                if (debug > 0)
                {
                    CrestronConsole.Print("\n Catch Connect OnTCPServerClientConnectCallback ReceiveDataAsync resultCodes: " + resultCodes.ToString());
                }
            }
            else
            {
                if (debug > 0)
                {
                    CrestronConsole.Print("\n Catch Connect OnTCPServerClientConnectCallback client not connected at: " + clientIndex);
                }
            }

            //Listen for other connections
            if (myTCPServer.MaxNumberOfClientSupported > myTCPServer.NumberOfClientsConnected)
            { 
                try
                {
                    SocketErrorCodes connectionResultCodes = myTCPServer.WaitForConnectionAsync("0.0.0.0", OnTCPServerClientConnectCallback);
                    if (debug > 0)
                    {
                        CrestronConsole.Print("\n Catch Connect TCPServer  OnTCPServerClientConnectCallback WaitForConnectionAsync resultCodes: " + connectionResultCodes.ToString());
                    }
                }
                catch (Exception e)
                {
                    ErrorLog.Error("\n Catch Connect OnTCPServerClientConnectCallback WaitForConnectionAsync Exception: " + e.Message);
                }
            }
        }

        private static void OnTCPReceiveCallback(TCPServer myTCPServer, uint clientIndex, int numberOfBytesReceived)
        {
            if (debug > 0)                    
                CrestronConsole.Print("\n Catch Connect OnTCPReceiveCallback clientIndex: " + clientIndex);

            if (myTCPServer != null && clientIndex > 0 && numberOfBytesReceived > 0)
            {
                try
                {
                    byte[] data = myTCPServer.GetIncomingDataBufferForSpecificClient(clientIndex);
                    string receiveBuffer = Encoding.UTF8.GetString(data, 0, numberOfBytesReceived);
                    parseResponse(receiveBuffer);
                }
                catch (Exception e)
                {
                    ErrorLog.Error("\n Catch Connect OnTCPReceiveCallback Exception: " + e.Message);
                }
            }
            //Start listening for new messages
            if (myTCPServer != null && clientIndex > 0)
            {
                try
                {
                    SocketErrorCodes resultCodes = myTCPServer.ReceiveDataAsync(clientIndex, OnTCPReceiveCallback);
                    if (debug > 0)
                    {
                        CrestronConsole.Print("\n Catch Connect OnTCPReceiveCallback ReceiveDataAsync resultCodes: " + resultCodes.ToString());
                    }
                }
                catch (Exception e)
                {
                    ErrorLog.Error("\n Catch Connect OnTCPReceiveCallback ReceiveDataAsync Exception: " + e.Message);
                }
            }
        }

        private static void parseResponse(string receiveBuffer)
        {
            string[] messageArray = receiveBuffer.Split(';');
            if (messageArray.Length == 3)
            {
                string type = messageArray[0];
                SimplSharpString key = messageArray[1];
                string value = messageArray[2];
                ChangeEventArgs changeEventArgs = new ChangeEventArgs();
                changeEventArgs.key = key;
                if (value != "#PAYLOAD#")
                {
                    switch (type)
                    {
                        case "digital":
                            if (value == "pulse")
                                onDigitalPulseEvent(null, changeEventArgs);
                            else
                                changeEventArgs.digitalValue = ushort.Parse(value);
                            onDigitalEvent(null, changeEventArgs);
                            break;
                        case "analog":
                            changeEventArgs.analogValue = ushort.Parse(value);
                            onAnalogEvent(null, changeEventArgs);
                            break;
                        case "serial":
                            changeEventArgs.stringValue = value;
                            onSerialEvent(null, changeEventArgs);
                            break;
                        default:
                            ErrorLog.Error("\n Catch Connect could not parse message: " + receiveBuffer);
                            break;
                    }
                }
                else 
                {
                    ErrorLog.Error("\n Catch Connect ignoring message with uninitialzed payload: " + receiveBuffer);
                }
            }
        }

        private static void SendDataAsync(string dataToSend)
        {
            try
            {
                byte[] SendData = System.Text.Encoding.ASCII.GetBytes(dataToSend);
                for (uint index = 1; index <= tcpServer.NumberOfClientsConnected; index++)
                {
                    if (debug > 0)
                    {
                        CrestronConsole.Print("\n Catch Connect SendDataAsync attempt for index: " + index);
                    }
                    if (tcpServer.ClientConnected(index))
                    {
                        SocketErrorCodes resultCodes = tcpServer.SendDataAsync(index, SendData, SendData.Length, OnTCPServerSendCallback);
                        if (debug > 0)
                        {
                            CrestronConsole.Print("\n Catch Connect SendDataAsync resultCodes: " + resultCodes.ToString());
                        }
                    }
                    else
                    {
                        CrestronConsole.Print("\n Catch Connect SendDataAsync client not connected at index: " + index);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLog.Error("error = " + e.Message);
            }
        }

        private static void OnTCPServerSendCallback(TCPServer myTCPServer, uint clientIndex, int numberOfBytesSent)
        {
            if (debug > 0)
            {
                CrestronConsole.Print("\n Catch Connect OnTCPServerSendCallback clientIndex: {0}, numberOfBytesSent: {1}", clientIndex, numberOfBytesSent);
            }
        } 

    }
}
