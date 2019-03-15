using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;
using Catch_Connect_Crestron_Library;

namespace UserModule_CATCH_CONNECT_SERIAL
{
    public class UserModuleClass_CATCH_CONNECT_SERIAL : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.StringInput IN;
        Crestron.Logos.SplusObjects.StringOutput OUT;
        Catch_Connect_Crestron_Library.SerialEventHandler SERIALEVENTS;
        StringParameter FRIENDLYNAME;
        object IN_OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                 CatchConnect.SetSerialValue(  FRIENDLYNAME  .ToSimplSharpString() ,  IN .ToSimplSharpString() )  ;  
 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    public void CALLBACKEVENTHANDLER ( SimplSharpString KEY , SimplSharpString VALUE ) 
        { 
        try
        {
            SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
            
            __context__.SourceCodeLine = 142;
            OUT  .UpdateValue ( VALUE  .ToString()  ) ; 
            
            
        }
        finally { ObjectFinallyHandler(); }
        }
        
    public override object FunctionMain (  object __obj__ ) 
        { 
        try
        {
            SplusExecutionContext __context__ = SplusFunctionMainStartCode();
            
            __context__.SourceCodeLine = 151;
            WaitForInitializationComplete ( ) ; 
            __context__.SourceCodeLine = 152;
             CatchConnect.InitializeSerial(  FRIENDLYNAME  .ToSimplSharpString() )  ;  
 
            __context__.SourceCodeLine = 153;
            SERIALEVENTS . Initialize ( FRIENDLYNAME  .ToSimplSharpString()) ; 
            __context__.SourceCodeLine = 154;
            // RegisterDelegate( SERIALEVENTS , ONSERIALEVENT , CALLBACKEVENTHANDLER ) 
            SERIALEVENTS .OnSerialEvent  = CALLBACKEVENTHANDLER; ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler(); }
        return __obj__;
        }
        
    
    public override void LogosSplusInitialize()
    {
        SocketInfo __socketinfo__ = new SocketInfo( 1, this );
        InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
        _SplusNVRAM = new SplusNVRAM( this );
        
        IN = new Crestron.Logos.SplusObjects.StringInput( IN__AnalogSerialInput__, 24, this );
        m_StringInputList.Add( IN__AnalogSerialInput__, IN );
        
        OUT = new Crestron.Logos.SplusObjects.StringOutput( OUT__AnalogSerialOutput__, this );
        m_StringOutputList.Add( OUT__AnalogSerialOutput__, OUT );
        
        FRIENDLYNAME = new StringParameter( FRIENDLYNAME__Parameter__, this );
        m_ParameterList.Add( FRIENDLYNAME__Parameter__, FRIENDLYNAME );
        
        
        IN.OnSerialChange.Add( new InputChangeHandlerWrapper( IN_OnChange_0, false ) );
        
        _SplusNVRAM.PopulateCustomAttributeList( true );
        
        NVRAM = _SplusNVRAM;
        
    }
    
    public override void LogosSimplSharpInitialize()
    {
        SERIALEVENTS  = new Catch_Connect_Crestron_Library.SerialEventHandler();
        
        
    }
    
    public UserModuleClass_CATCH_CONNECT_SERIAL ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
    
    
    
    
    const uint IN__AnalogSerialInput__ = 0;
    const uint OUT__AnalogSerialOutput__ = 0;
    const uint FRIENDLYNAME__Parameter__ = 10;
    
    [SplusStructAttribute(-1, true, false)]
    public class SplusNVRAM : SplusStructureBase
    {
    
        public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
        
        
    }
    
    SplusNVRAM _SplusNVRAM = null;
    
    public class __CEvent__ : CEvent
    {
        public __CEvent__() {}
        public void Close() { base.Close(); }
        public int Reset() { return base.Reset() ? 1 : 0; }
        public int Set() { return base.Set() ? 1 : 0; }
        public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
    }
    public class __CMutex__ : CMutex
    {
        public __CMutex__() {}
        public void Close() { base.Close(); }
        public void ReleaseMutex() { base.ReleaseMutex(); }
        public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
    }
     public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
