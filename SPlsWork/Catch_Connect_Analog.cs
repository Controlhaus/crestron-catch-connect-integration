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

namespace UserModule_CATCH_CONNECT_ANALOG
{
    public class UserModuleClass_CATCH_CONNECT_ANALOG : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.AnalogInput IN;
        Crestron.Logos.SplusObjects.AnalogOutput OUT;
        Catch_Connect_Crestron_Library.AnalogEventHandler ANALOGEVENTS;
        StringParameter FRIENDLYNAME;
        object IN_OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                 CatchConnect.SetAnalogValue(  FRIENDLYNAME  .ToSimplSharpString() , (ushort)( IN  .UshortValue ) )  ;  
 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    public void CALLBACKEVENTHANDLER ( SimplSharpString KEY , ushort VALUE ) 
        { 
        try
        {
            SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
            
            __context__.SourceCodeLine = 142;
            OUT  .Value = (ushort) ( VALUE ) ; 
            
            
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
             CatchConnect.InitializeAnalog(  FRIENDLYNAME  .ToSimplSharpString() )  ;  
 
            __context__.SourceCodeLine = 153;
            ANALOGEVENTS . Initialize ( FRIENDLYNAME  .ToSimplSharpString()) ; 
            __context__.SourceCodeLine = 154;
            // RegisterDelegate( ANALOGEVENTS , ONANALOGEVENT , CALLBACKEVENTHANDLER ) 
            ANALOGEVENTS .OnAnalogEvent  = CALLBACKEVENTHANDLER; ; 
            
            
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
        
        IN = new Crestron.Logos.SplusObjects.AnalogInput( IN__AnalogSerialInput__, this );
        m_AnalogInputList.Add( IN__AnalogSerialInput__, IN );
        
        OUT = new Crestron.Logos.SplusObjects.AnalogOutput( OUT__AnalogSerialOutput__, this );
        m_AnalogOutputList.Add( OUT__AnalogSerialOutput__, OUT );
        
        FRIENDLYNAME = new StringParameter( FRIENDLYNAME__Parameter__, this );
        m_ParameterList.Add( FRIENDLYNAME__Parameter__, FRIENDLYNAME );
        
        
        IN.OnAnalogChange.Add( new InputChangeHandlerWrapper( IN_OnChange_0, false ) );
        
        _SplusNVRAM.PopulateCustomAttributeList( true );
        
        NVRAM = _SplusNVRAM;
        
    }
    
    public override void LogosSimplSharpInitialize()
    {
        ANALOGEVENTS  = new Catch_Connect_Crestron_Library.AnalogEventHandler();
        
        
    }
    
    public UserModuleClass_CATCH_CONNECT_ANALOG ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
    
    
    
    
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
