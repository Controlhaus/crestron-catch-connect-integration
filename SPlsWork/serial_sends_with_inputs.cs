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

namespace UserModule_SERIAL_SENDS_WITH_INPUTS
{
    public class UserModuleClass_SERIAL_SENDS_WITH_INPUTS : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> SEND;
        InOutArray<Crestron.Logos.SplusObjects.StringInput> TEXT_IN__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput TEXT_OUT__DOLLAR__;
        object SEND_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 70;
                MakeString ( TEXT_OUT__DOLLAR__ , "{0}", TEXT_IN__DOLLAR__ [ Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ] ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    
    public override void LogosSplusInitialize()
    {
        _SplusNVRAM = new SplusNVRAM( this );
        
        SEND = new InOutArray<DigitalInput>( 65, this );
        for( uint i = 0; i < 65; i++ )
        {
            SEND[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( SEND__DigitalInput__ + i, SEND__DigitalInput__, this );
            m_DigitalInputList.Add( SEND__DigitalInput__ + i, SEND[i+1] );
        }
        
        TEXT_IN__DOLLAR__ = new InOutArray<StringInput>( 100, this );
        for( uint i = 0; i < 100; i++ )
        {
            TEXT_IN__DOLLAR__[i+1] = new Crestron.Logos.SplusObjects.StringInput( TEXT_IN__DOLLAR____AnalogSerialInput__ + i, TEXT_IN__DOLLAR____AnalogSerialInput__, 65, this );
            m_StringInputList.Add( TEXT_IN__DOLLAR____AnalogSerialInput__ + i, TEXT_IN__DOLLAR__[i+1] );
        }
        
        TEXT_OUT__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( TEXT_OUT__DOLLAR____AnalogSerialOutput__, this );
        m_StringOutputList.Add( TEXT_OUT__DOLLAR____AnalogSerialOutput__, TEXT_OUT__DOLLAR__ );
        
        
        for( uint i = 0; i < 65; i++ )
            SEND[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( SEND_OnPush_0, false ) );
            
        
        _SplusNVRAM.PopulateCustomAttributeList( true );
        
        NVRAM = _SplusNVRAM;
        
    }
    
    public override void LogosSimplSharpInitialize()
    {
        
        
    }
    
    public UserModuleClass_SERIAL_SENDS_WITH_INPUTS ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
    
    
    
    
    const uint SEND__DigitalInput__ = 0;
    const uint TEXT_IN__DOLLAR____AnalogSerialInput__ = 0;
    const uint TEXT_OUT__DOLLAR____AnalogSerialOutput__ = 0;
    
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
